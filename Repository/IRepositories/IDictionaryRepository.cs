using EasySearchApi.Base;
using EasySearchApi.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EasySearchApi.Repository.IRepositories
{
    public interface IDictionaryRepository : IGenericRepository<Dictionary>
    {
        Task<ActionResult<IEnumerable<Dictionary>>> GetDictionaries();
        Task<int> PostDictionary(CreateDictionaryDTO createDictionaryDTO);
        Task<bool> ChangeDictionaryTitle(int id, string name);
        Task<bool> ChangeDictionaryFieldDescription(int id, string[] description);
        Task<bool> ChangeDictionarySearch(int dictionaryId, int technique);
    }
}
