using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EasySearchApi.Data;
using EasySearchApi.Models;

namespace EasySearchApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DictionariesController : ControllerBase
    {
        private readonly DataContext _context;

        public DictionariesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Dictionaries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dictionary>>> GetDictionaries()
        {
          if (_context.Dictionaries == null)
          {
              return NotFound();
          }
            return await _context.Dictionaries.Include(c=> c.documents).ToListAsync();
        }

        // GET: api/Dictionaries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Dictionary>> GetDictionary(int id)
        {
          if (_context.Dictionaries == null)
          {
              return NotFound();
          }
            var dictionary = await _context.Dictionaries.FindAsync(id);

            if (dictionary == null)
            {
                return NotFound();
            }

            return dictionary;
        }

        // PUT: api/Dictionaries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDictionary(int id, Dictionary dictionary)
        {
            if (id != dictionary.Id)
            {
                return BadRequest();
            }

            _context.Entry(dictionary).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DictionaryExists(id))
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

        // POST: api/Dictionaries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Dictionary>> PostDictionary(int UserID, string Name)
        {
            if (_context.Dictionaries == null)
            {
                return Problem("Entity set 'DataContext.Dictionaries'  is null.");
            }

            var user= await _context.Users.FindAsync(UserID);
            
            if (user == null)
            {
                return Problem("User not found.");
            }

            var dictionary = new Dictionary();
            dictionary.Name = Name;
            dictionary.user = user;
            dictionary.documents = new List<Document>();

            _context.Dictionaries.Add(dictionary);
            
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDictionary", new { id = dictionary.Id }, dictionary);
        }

        // DELETE: api/Dictionaries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDictionary(int id)
        {
            if (_context.Dictionaries == null)
            {
                return NotFound();
            }
            var dictionary = await _context.Dictionaries.FindAsync(id);
            if (dictionary == null)
            {
                return NotFound();
            }

            _context.Dictionaries.Remove(dictionary);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DictionaryExists(int id)
        {
            return (_context.Dictionaries?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
