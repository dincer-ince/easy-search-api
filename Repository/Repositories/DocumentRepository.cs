using EasySearchApi.Base;
using EasySearchApi.Helpers;
using EasySearchApi.Models;
using EasySearchApi.Models.DTOs;
using EasySearchApi.Repository.IRepositories;
using EasySearchApi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EasySearchApi.Repository.Repositories
{
    public class DocumentRepository : GenericRepository<Document>, IDocumentRepository
    {

        public DocumentRepository(DataContext context, UserManager<User> userManager) : base(context, userManager)
        {
            
        }

        public async Task<List<Document>> GetDocuments(int DictionaryId)
        {
            var documents = await _context.Documents.AsNoTracking().Where(x=>x.DictionaryId==DictionaryId).ToListAsync();
            
            return documents;
        }

        public new async Task<bool> Delete(int documentId)
        {
            var document = await _context.Documents.Include(x=>x.Dictionary).Where(x=>x.Id==documentId).SingleAsync();
            document.Dictionary.NumberOfDocuments--;
            document.Dictionary.TotalNumberOfWords-=document.NumberOfWords;
            _context.Documents.Remove(document);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ActionResult<Document>> GetDocument(int DocumentId)
        {
            return await _context.Documents.AsNoTracking().Where(c => c.Id == DocumentId).FirstOrDefaultAsync();
        }

        public async Task<int> PostDocument(CreateDocumentDTO request)
        {
            var dictionaryId = request.DictionaryID;
            var rawDocument = request.RawDocument;

            var dictionary = await _context.Dictionaries.FindAsync(dictionaryId);

            //Prepare document
            var document = new Document()
            {
                Title = request.Title,
                RawDocument = rawDocument,
                DictionaryId = dictionaryId,
                Dictionary = dictionary,
                Words = new List<DocumentWord>(),
                ExtraFields =request.extraFields
                
            };
            _context.Documents.Add(document);

            createDocument(document);

            dictionary.NumberOfDocuments++;
            dictionary.TotalNumberOfWords += document.NumberOfWords;

            return await _context.SaveChangesAsync();
        }

        public async Task<ActionResult<double>> DocumentSimilarity(int Id1, int Id2)
        {
            var doc1Nullable = _context.Documents.Find(Id1);
            var doc2Nullable = _context.Documents.Find(Id2);

            if (doc1Nullable == null)
            {
                doc1Nullable = _context.Documents.Local.Where(x => x.Id == Id1).FirstOrDefault();

                if (doc1Nullable == null) return 0;
            }
            if (doc2Nullable == null)
            {
                doc2Nullable = _context.Documents.Local.Where(x => x.Id == Id2).FirstOrDefault();

                if (doc2Nullable == null) return 0;
            }

            Document doc1 = doc1Nullable;
            Document doc2 = doc2Nullable;

            if (doc1.DictionaryId != doc2.DictionaryId)
            {
                return -1;
            }


            var words = _context.Words.Include(w => w.Documents).ThenInclude(w => w.Document)
                .Where(w => w.Documents.
                            Where(d => d.Document.DictionaryId == doc1.DictionaryId).Any())
                .ToList();

            double[] doc1Vector = VectorizationHelper.TfIdfTransform(doc1, words);
            double[] doc2Vector = VectorizationHelper.TfIdfTransform(doc2, words);

            var similarityResult = CosineSimilarityHelper.CosineSimilarity(doc1Vector, doc2Vector);


            return similarityResult;
        }

        public async Task<IEnumerable<WithSimilarityModel>> MostSimilarDocuments(int number, int id)
        {
            var docNullable = _context.Documents.Find(id);


            if (docNullable == null)
            {
                docNullable = _context.Documents.Local.Where(x => x.Id == id).First();
            }

            Document doc = docNullable;

            var documentsCompared = await _context.Documents.Where(x => x.DictionaryId == doc.DictionaryId && x.Id != doc.Id).Select(x => x.Id).ToArrayAsync();

            var results = new List<KeyValuePair<int, double>>();
            for (int i = 0; i < documentsCompared.Length; i++)
            {
                var test = await DocumentSimilarity(id, documentsCompared[i]);
                //var testest = test.Result as OkObjectResult;

                double result = (double)test.Value;
                if (result > 0) results.Add(new KeyValuePair<int, double>(documentsCompared[i], result));
            }

            var list = results.OrderByDescending(x => x.Value).Take(number).ToArray();

            var documents = new List<Document>();

            for (int i = 0; i < list.Length; i++)
            {
                var document = _context.Documents.AsNoTracking().Where(x => x.Id == list[i].Key).FirstOrDefault();
                documents.Add(document);
            }

            var last = documents.Select(x => new
            {
                id = x.Id,
                title = x.Title,
                post = x.RawDocument,
                numberOfWords = x.NumberOfWords,
            }).ToList();

            var withSimilarity = from a in list
                                 join b in last on a.Key equals b.id
                                 select new WithSimilarityModel()
                                 {
                                     id = b.id,
                                     title = b.title,
                                     rawDocument = b.post,
                                     numberOfWords = b.numberOfWords,
                                     similarity = a.Value
                                 };


            //var documents = await _context.Documents.AsNoTracking().Where(x => list.Contains(x.Id)).Select(x => new
            //{
            //    id = x.Id,
            //    title = x.Title,
            //    post = x.rawDocument,
            //    numberOfWords = x.numberOfWords
            //}).ToListAsync();

            return withSimilarity;
        }

        public async Task<IEnumerable<WithSimilarityModel>> Search(int dictionaryId,string text)
        {


            var dictionary = await _context.Dictionaries.FindAsync(dictionaryId);

            switch (dictionary.preferredSearch)
            {
                case (int) SearchTechniqueEnum.ExactSearch:
                    return await exactSearch(dictionaryId, text);

                case (int)SearchTechniqueEnum.VectorizedSearch:
                    var document = new Document()
                    {
                        Title = "search",
                        RawDocument = text,
                        DictionaryId = dictionaryId,
                        Dictionary = dictionary,
                        Words = new List<DocumentWord>()
                    };

                    _context.Documents.Add(document);
                    createDocument(document);

                    var result = await MostSimilarDocuments(5, document.Id);
                    return result;
                default:
                    return await exactSearch(dictionaryId, text);

            }
            
        }

        public async Task<IEnumerable<Document>> QueryField(int dictionaryId, int fieldIndex, string query, int length)
        {
            var documents = await _context.Documents.Where(x=>x.DictionaryId==dictionaryId && x.ExtraFields[fieldIndex]==query).Take(length).ToListAsync();
            return documents;
        }

        // private methods

        private Document createDocument(Document document)
        {
            string[] tokenized = PreProcessService.NormalizeTokenizeStop(document);
            var wordsInDoc = PreProcessService.LemmatizeWords(tokenized);


            var wordDb = _context.Words.ToList();

            foreach (string word in wordsInDoc)
            {
                //var wordInDb = await _context.Words.FirstOrDefaultAsync(x => x.term.Equals(word));
                var wordInDb = wordDb.FirstOrDefault(x => x.Term.Equals(word));
                if (wordInDb == null)
                {
                    wordInDb = new Word();
                    wordInDb.Term = word;
                    wordInDb.Documents = new List<DocumentWord>();
                    _context.Words.Add(wordInDb);
                    wordDb.Add(wordInDb);
                }
                var docWord = document.Words.FirstOrDefault(x => x.Word.Id == wordInDb.Id);
                if (docWord == null)
                {
                    docWord = new DocumentWord()
                    {
                        Word = wordInDb,
                        Document = document
                    };
                    document.Words.Add(docWord);
                }
                docWord.Count++;

            }

            document.NumberOfWords = wordsInDoc.Count;

            return document;
        }

        public async Task<bool> ChangeDocumentDictionary(int document_id, int dictionary_id)
        {
            var document = await GetById(document_id);
            document.DictionaryId = dictionary_id;
            return await Update(document);
        }

        public async Task<bool> ChangeDocumentTitle(int document_id, string title)
        {
            var document = await GetById(document_id);
            document.Title = title;
            return await Update(document);
        }

        public async Task<bool> ChangeDocumentRawDocument(int document_id, string raw_document)
        {
            var document = await GetById(document_id);
            document.RawDocument = raw_document;
            document.Words = new List<DocumentWord>();
             var words=await _context.DocumentWord.Where(x => x.DocumentId == document_id).ToListAsync();
            _context.RemoveRange(words);
            await _context.SaveChangesAsync();
            createDocument(document);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditDocumentField(int document_id, int index, string text)
        {
            var document = await GetById(document_id);
            while (index >= document.ExtraFields.Length)
            {
                document.ExtraFields= document.ExtraFields.Append("").ToArray();
            }
            document.ExtraFields[index] = text;
            return await Update(document);
        }

        public async Task<List<WithSimilarityModel>> exactSearch(int dictionaryId,string query)
        {
            string[] tokenized = PreProcessService.NormalizeTokenizeStop(new Document { RawDocument=query});
            var wordsInQuery = PreProcessService.LemmatizeWords(tokenized).Distinct().ToList();
            if(wordsInQuery is null)
            {
                return new List<WithSimilarityModel>();
            }

            var WordsWithDocuments = await _context.Words.Where(x=> wordsInQuery.Contains(x.Term))
                .Select(t=>t.Documents.Select(x=>x.Document).Where(y => y.DictionaryId == dictionaryId).Distinct())
                .ToListAsync();
            if(WordsWithDocuments.Count ==0 || wordsInQuery.Count() != WordsWithDocuments.Count) return new List<WithSimilarityModel>();

            List<Document> documents = WordsWithDocuments[0].ToList();
            for(int i=1; i<WordsWithDocuments.Count; i++)
            {
                var nestedlist = documents.Where(x => WordsWithDocuments[i].Select(a => a.Id).Contains(x.Id)).Distinct().ToList();
                if (nestedlist.Count == 0) return new List<WithSimilarityModel>();
                documents=nestedlist;
            }

            return documents.Select(x=>new WithSimilarityModel { id=x.Id,title=x.Title,rawDocument=x.RawDocument,numberOfWords=x.NumberOfWords,similarity=1}).ToList();

        }
    }
}
