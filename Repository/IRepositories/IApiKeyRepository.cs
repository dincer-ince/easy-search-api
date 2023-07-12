using EasySearchApi.Base;

namespace EasySearchApi.Repository.IRepositories
{
    public interface IApiKeyRepository : IGenericRepository<ApiKey>
    {
        Task<ApiKey> CreateApiKey(string user_id,string name);
        Task<List<ApiKey>> GetUserKeys(string user_id);
        Task<List<ApiKey>> GetKeys();
    }
}
