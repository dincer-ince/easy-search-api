using EasySearchApi.Repository.IRepositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace EasySearchApi.Repository.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected DataContext _context;
        protected DbSet<T> dbSet;
        //protected readonly ILogger _logger;

        public GenericRepository(DataContext context)
        {
            _context = context;
            //_logger = loggerFactory.CreateLogger("Logger");
            dbSet = context.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            try
            {
                var test = await dbSet.ToListAsync();
                return test;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "{Result} Repository GetAll method error", typeof(T));
                return new List<T>();
            }
        }

        public virtual async Task<T> GetById(int Id)
        {
            try
            {
                return await dbSet.FindAsync(Id);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "{Result} Repository GetById method error", typeof(T));
                return null;
            }
        }

        public virtual async Task<T> Add(T entity)
        {
            try
            {
                dbSet.Add(entity);
                await _context.SaveChangesAsync();
                return entity; 
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "{Result} Repository Add method error", typeof(T));
                return null;
            }
        }

        public virtual async Task<bool> Update(T entity)
        {
            try
            {
                dbSet.Update(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "{Result} Repository Update method error", typeof(T));
                return false;
            }
        }

        public  async Task<bool> Delete(int Id)
        {
            try
            {
                var user = await dbSet.FindAsync(Id);
                if(user !=null)
                {
                    dbSet.Remove(user);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                //_logger.LogError(ex, "{Result} Repository Delete method error", typeof(T));
                return false;
            }
        }
    }
}
