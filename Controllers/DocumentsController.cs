using Microsoft.AspNetCore.Mvc;
using EasySearchApi.Models.DTOs;
using EasySearchApi.Repository.IRepositories;
using Microsoft.AspNetCore.Authorization;

namespace EasySearchApi.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentRepository _documentRepository;

        public DocumentsController(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }


        [HttpGet("list/{DictionaryId}")]
        public async Task<ActionResult<IEnumerable<Document>>> GetDocuments(int DictionaryId)
        {
            return await _documentRepository.GetDocuments(DictionaryId);
        }

        [HttpGet("{DocumentId}")]
        public async Task<ActionResult<Document>> GetDocument(int DocumentId)
        {
            return await _documentRepository.GetDocument(DocumentId);
        }

        [HttpPut("{id}")]
        public async Task<bool> PutDocument(int id, Document document)
        {
            return await _documentRepository.Update(document);
        }

        [HttpPost]
        public async Task<int> PostDocument(CreateDocumentDTO request)
        {
            return await _documentRepository.PostDocument(request);
        }

        [HttpDelete("{id}")]
        public async Task<bool> DeleteDocument(int id)
        {
            return await _documentRepository.Delete(id);
        }

        [HttpGet("DocumentSimilarity/{Id1}/{Id2}")]
        public async Task<ActionResult<double>> DocumentSimilarity(int Id1, int Id2)
        {
            return await _documentRepository.DocumentSimilarity(Id1, Id2);
        }

        [HttpGet("similarDocuments/{number}/{id}")]
        public async Task<IEnumerable<WithSimilarityModel>> MostSimilarDocuments(int number, int id)
        {
            return await _documentRepository.MostSimilarDocuments(number, id);
        }

        [HttpGet("search/{dictionaryId}/{text}")]
        public async Task<IEnumerable<WithSimilarityModel>> Search(int dictionaryId,string text)
        {
            return await _documentRepository.Search(dictionaryId,text);
        }

        [HttpGet("query/{dictionaryId}/{fieldIndex}/{query}/{length}")]
        public async Task<IEnumerable<Document>> QueryField(int dictionaryId,int fieldIndex,string query,int length)
        {
            return await _documentRepository.QueryField(dictionaryId,fieldIndex,query,length);
        }

        [HttpPut("changeDocumentDictionary")]
        public async Task<bool> ChangeDocumentDictionary(int document_id, int dictionary_id)
        {
            return await _documentRepository.ChangeDocumentDictionary(document_id, dictionary_id);
        }

        [HttpPut("changeDocumentTitle")]
        public async Task<bool> ChangeDocumentTitle(int document_id, string title)
        {
            return await _documentRepository.ChangeDocumentTitle(document_id, title);
        }

        [HttpPut("changeDocumentRawDocument")]
        public async Task<bool> ChangeDocumentRawDocument(editContentDTO request)
        {
            return await _documentRepository.ChangeDocumentRawDocument(request.id, request.text);
        }

        [HttpPut("editDocumentField")]
        public async Task<bool> EditDocumentField(int document_id, int index, string text)
        {
            return await _documentRepository.EditDocumentField(document_id, index, text);
        }
    }
}



