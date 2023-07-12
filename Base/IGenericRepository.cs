namespace EasySearchApi.Base
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();

        Task<T> GetById(int Id);

        Task<T> Add(T entity);

        Task<bool> Update(T entity);

        Task<bool> Delete(int Id);
    }
}
