using EasySearchApi.Base;
using EasySearchApi.Models.DTOs;
using EasySearchApi.Repository.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EasySearchApi.Repository.Repositories
{
    public class DictionaryRepository : GenericRepository<Dictionary>, IDictionaryRepository
    {
        public DictionaryRepository(DataContext context, UserManager<User> userManager) : base(context, userManager)
        {

        }

        public async Task<ActionResult<IEnumerable<Dictionary>>> GetDictionaries()
        {
            return await _context.Dictionaries.Include(c => c.Documents).ToListAsync();
        }

        public async Task<int> PostDictionary(CreateDictionaryDTO createDictionaryDTO)
        {
            var user = await _userManager.FindByIdAsync(createDictionaryDTO.UserID);

            var dictionary = new Dictionary();
            dictionary.preferredSearch = (int) SearchTechniqueEnum.VectorizedSearch;
            dictionary.Name = createDictionaryDTO.Name;
            dictionary.UserId = createDictionaryDTO.UserID;
            dictionary.User = user;
            dictionary.Documents = new List<Document>();
            dictionary.ExtraFieldDescription = createDictionaryDTO.ExtraFieldDescription;

            _context.Dictionaries.Add(dictionary);

            return await _context.SaveChangesAsync();
        }

        public async Task<bool> ChangeDictionaryTitle(int id, string name)
        {
            var dictionary = await GetById(id);
            dictionary.Name = name;
            return await Update(dictionary);
        }

        public async Task<bool> ChangeDictionaryFieldDescription(int id, string[] description)
        {
            var dictionary = await GetById(id);
            dictionary.ExtraFieldDescription = description;
            return await Update(dictionary);
        }

        public async Task<bool> ChangeDictionarySearch(int dictionaryId, int technique )
        {
            var dictionary = await GetById(dictionaryId);
            dictionary.preferredSearch = technique;
            return await Update(dictionary);
        }
    }
}
