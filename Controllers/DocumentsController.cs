using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EasySearchApi.Data;
using EasySearchApi.Models;
using EasySearchApi.Models.DTOs;
using System.Diagnostics;
using EasySearchApi.Services;
using Microsoft.Identity.Client;
using EasySearchApi.Helpers;
using Microsoft.AspNetCore.Http.HttpResults;
using Azure.Core;

namespace EasySearchApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly DataContext _context;

        public DocumentsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Documents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CreateDocumentDTO>>> GetDocuments()
        {
            if (_context.Documents == null)
            {
                return NotFound();
            }
            var documents = await _context.Documents.AsNoTracking().ToListAsync();
            List<CreateDocumentDTO> result = new List<CreateDocumentDTO>();
            documents.ForEach(document =>
            {
                result.Add(new CreateDocumentDTO { DictionaryID = document.dictionaryId, RawDocument = document.rawDocument });
            });
            return result;
        }

        // GET: api/Documents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Document>> GetDocument(int id)
        {
            if (_context.Documents == null)
            {
                return NotFound();
            }
            var document = await _context.Documents.AsNoTracking().Include(c => c.words).ThenInclude(docWord => docWord.word).Where(c => c.Id == id).FirstOrDefaultAsync();


            if (document == null)
            {
                return NotFound();
            }



            return document;
        }

        // PUT: api/Documents/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDocument(int id, Document document)
        {
            if (id != document.Id)
            {
                return BadRequest();
            }

            _context.Entry(document).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocumentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Documents
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Document>> PostDocument(CreateDocumentDTO request)
        {
            //TODO : Move logic from controller to repositories
            var dictionaryId = request.DictionaryID;
            var rawDocument = request.RawDocument;


            //Check prereqs ----
            if (_context.Documents == null)
            {
                return Problem("Entity set 'DataContext.Documents'  is null.");
            }

            var dictionary = await _context.Dictionaries.FindAsync(dictionaryId);
            if (dictionary == null)
            {
                return Problem("No dictionary");
            }
            //------
            
            //Prepare document
            var document = new Document()
            {
                Title=request.Title,
                rawDocument = rawDocument,
                dictionaryId = dictionaryId,
                dictionary = dictionary,
                words = new List<DocumentWord>()
            };

            createDocument(document);

            dictionary.NumberOfDocuments++;
            dictionary.totalNumberOfWords += document.numberOfWords;

            await _context.SaveChangesAsync();

            

            return CreatedAtAction("GetDocument", new { id = document.Id }, document); ;
        }

        private Document createDocument(Document document)
        {
            string[] tokenized = PreProcessService.NormalizeTokenizeStop(document);
            var wordsInDoc = PreProcessService.LemmatizeWords(tokenized);


            _context.Documents.Add(document);
            

            var wordDb = _context.Words.ToList();

            foreach (string word in wordsInDoc)
            {
                //var wordInDb = await _context.Words.FirstOrDefaultAsync(x => x.term.Equals(word));
                var wordInDb = wordDb.FirstOrDefault(x => x.term.Equals(word));
                if (wordInDb == null)
                {
                    wordInDb = new Word();
                    wordInDb.term = word;
                    wordInDb.documents = new List<DocumentWord>();
                    _context.Words.Add(wordInDb);
                    wordDb.Add(wordInDb);
                }
                var docWord = document.words.FirstOrDefault(x => x.word.Id == wordInDb.Id);
                if (docWord == null)
                {
                    docWord = new DocumentWord()
                    {
                        word = wordInDb,
                        document = document
                    };
                    document.words.Add(docWord);
                }
                docWord.count++;

            }

            document.numberOfWords = wordsInDoc.Count;



            return document;
        }

        [HttpGet("/DocumentSimilarity/{Id1}/{Id2}")]
        public async Task<ActionResult<double>> DocumentSimilarity(int Id1,int Id2)
        {
            var doc1Nullable =  _context.Documents.Find(Id1);
            var doc2Nullable =  _context.Documents.Find(Id2);

            if (doc1Nullable == null)
            {
                doc1Nullable = _context.Documents.Local.Where(x => x.Id == Id1).FirstOrDefault();

                if (doc1Nullable == null) return Problem("Document doesn't exist.");
            }
            if (doc2Nullable == null)
            { 
                doc2Nullable = _context.Documents.Local.Where(x => x.Id == Id2).FirstOrDefault();

                if (doc2Nullable == null) return Problem("Document doesn't exist.");
            }

            Document doc1 = doc1Nullable;
            Document doc2 = doc2Nullable;

            if (doc1.dictionaryId!= doc2.dictionaryId) 
            {
                return Problem("Documents are in different dictionaries.");
            }


            var words = _context.Words.Include(w => w.documents).ThenInclude(w => w.document)
                .Where(w => w.documents.
                            Where(d => d.document.dictionaryId == doc1.dictionaryId).Any())
                .ToList();

            double[] doc1Vector = VectorizationHelper.TfIdfTransform(doc1,words);
            double[] doc2Vector = VectorizationHelper.TfIdfTransform(doc2,words);

            var similarityResult = CosineSimilarityHelper.CosineSimilarity(doc1Vector, doc2Vector);


            return Ok(similarityResult);
        }

        [HttpGet("/similarDocuments/{number}/{id}")] 
        public async Task<IActionResult> MostSimilarDocuments(int number, int id)
        {
            var docNullable = _context.Documents.Find(id);


            if (docNullable == null)
            {
                docNullable = _context.Documents.Local.Where(x => x.Id == id).First();
                if(docNullable == null) return Problem("Document Not Found");
            }
            
            Document doc = docNullable;

            var documentsCompared= await _context.Documents.Where(x=> x.dictionaryId == doc.dictionaryId && x.Id!=doc.Id).Select(x=>x.Id).ToArrayAsync();

            var results = new List<KeyValuePair<int, double>>();
            for(int i = 0; i < documentsCompared.Length; i++)
            {
                var test = await DocumentSimilarity(id, documentsCompared[i]);
                var testest = test.Result as OkObjectResult;
                
                double result = (double)testest.Value;
                if (result > 0) results.Add(new KeyValuePair<int, double>(documentsCompared[i], result));
            }

            var list = results.OrderByDescending(x => x.Value).Take(number).Select(x=>x.Key).ToArray();

            var documents = new List<Document>();

            for (int i = 0; i < list.Length; i++)
            {
                var document = _context.Documents.AsNoTracking().Where(x => x.Id == list[i]).FirstOrDefault();
                documents.Add(document);
            }
            var last = documents.Select(x => new
            {
                id = x.Id,
                title = x.Title,
                post = x.rawDocument,
                numberOfWords = x.numberOfWords,
            }).ToList();


            //var documents = await _context.Documents.AsNoTracking().Where(x => list.Contains(x.Id)).Select(x => new
            //{
            //    id = x.Id,
            //    title = x.Title,
            //    post = x.rawDocument,
            //    numberOfWords = x.numberOfWords
            //}).ToListAsync();

            return Ok(last);
        }

        [HttpGet("search/{text}")]
        public async Task<IActionResult> Search(string text)
        {
            //Check prereqs ----
            if (_context.Documents == null)
            {
                return Problem("Entity set 'DataContext.Documents'  is null.");
            }

            var dictionary = await _context.Dictionaries.FindAsync(1);
            if (dictionary == null)
            {
                return Problem("No dictionary");
            }
            //------

            //Prepare document
            var document = new Document()
            {
                Title = "search",
                rawDocument = text,
                dictionaryId = 1,
                dictionary = dictionary,
                words = new List<DocumentWord>()
            };

            createDocument(document);

            var result = await MostSimilarDocuments(5, document.Id);

            return result;
        }

        // DELETE: api/Documents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument(int id)
        {
            if (_context.Documents == null)
            {
                return NotFound();
            }
            var document = await _context.Documents.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }

            _context.Documents.Remove(document);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DocumentExists(int id)
        {
            return (_context.Documents?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        


        
        [HttpGet("posts")]
        public async Task<IActionResult> GetPosts()
        {
            if (_context.Documents == null)
            {
                return NotFound();
            }
            var documents = await _context.Documents.AsNoTracking().Select(x =>new 
            { 
                id = x.Id,
                title = x.Title
            }).ToListAsync();
            
            return Ok(documents);
        }

        [HttpGet("post/{id}")]
        public async Task<IActionResult> GetPost(int id)
        {
            if (_context.Documents == null)
            {
                return NotFound();
            }
            var documents = await _context.Documents.AsNoTracking().Where(x=>x.Id==id).Select(x => new
            {
                id=x.Id,
                title = x.Title,
                post = x.rawDocument,
                numberOfWords = x.numberOfWords
            }).FirstOrDefaultAsync();

            return Ok(documents);
        }
    }
}



