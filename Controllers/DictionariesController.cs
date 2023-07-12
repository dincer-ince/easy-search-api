using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EasySearchApi.Data;
using EasySearchApi.Models;
using Microsoft.AspNetCore.Identity;
using EasySearchApi.Repository.IRepositories;
using Microsoft.AspNetCore.Authorization;
using EasySearchApi.Models.DTOs;

namespace EasySearchApi.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class DictionariesController : ControllerBase
    {
        private readonly IDictionaryRepository _repository;

        public DictionariesController(IDictionaryRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Dictionaries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dictionary>>> GetDictionaries()
        {
            return await _repository.GetDictionaries();
        }

        // GET: api/Dictionaries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Dictionary>> GetDictionary(int id)
        {
            return await _repository.GetById(id);
        }

        // PUT: api/Dictionaries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<bool> PutDictionary(Dictionary dictionary)
        {
            return await _repository.Update(dictionary);
        }

        // POST: api/Dictionaries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<int> PostDictionary([FromBody] CreateDictionaryDTO createDictionaryDTO)
        {
            return await _repository.PostDictionary(createDictionaryDTO);
        }

        // DELETE: api/Dictionaries/5
        [HttpDelete("{id}")]
        public async Task<bool> DeleteDictionary(int id)
        {
            return await _repository.Delete(id);
        }

        [HttpPut("changeDictionaryTitle")]
        public async Task<bool> ChangeDictionaryTitle(int id, string name)
        {
            return await _repository.ChangeDictionaryTitle(id, name);
        }

        [HttpPut("changeDictionaryFieldDescription")]
        public async Task<bool> ChangeDictionaryFieldDescription(int id, string[] description)
        {
            return await _repository.ChangeDictionaryFieldDescription(id, description);
        }

        [HttpPut("changeDictionarySearch")]
        public async Task<bool> ChangeDictionarySearch(int dictionaryId, int technique)
        {
            return await _repository.ChangeDictionarySearch(dictionaryId, technique);
        }
    }
}
