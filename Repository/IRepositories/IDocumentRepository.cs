using EasySearchApi.Base;
using EasySearchApi.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EasySearchApi.Repository.IRepositories
{
    public interface IDocumentRepository : IGenericRepository<Document>
    {
        Task<List<Document>> GetDocuments(int DictionaryId);
        Task<ActionResult<Document>> GetDocument(int DocumentId);
        Task<int> PostDocument(CreateDocumentDTO request);
        Task<ActionResult<double>> DocumentSimilarity(int Id1, int Id2);
        Task<IEnumerable<WithSimilarityModel>> MostSimilarDocuments(int number, int id);
        Task<IEnumerable<WithSimilarityModel>> Search(int dictionaryId,string text);
        Task<IEnumerable<Document>> QueryField(int dictionaryId, int fieldIndex, string query, int length);
        Task<bool> ChangeDocumentDictionary(int document_id, int dictionary_id);
        Task<bool> ChangeDocumentTitle(int document_id, string title);
        Task<bool> ChangeDocumentRawDocument(int document_id, string raw_document);
        Task<bool> EditDocumentField(int document_id, int index, string text);
    }
}
