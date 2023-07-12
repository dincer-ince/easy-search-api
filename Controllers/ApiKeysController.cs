using EasySearchApi.Repository.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasySearchApi.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class ApiKeysController : ControllerBase
    {
        private readonly IApiKeyRepository _repository;

        public ApiKeysController(IApiKeyRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("user")]
        public async Task<List<ApiKey>> GetUserKeys()
        {
            string user_id = User.Claims.First(c => c.Type == "UserID").Value;
            return await _repository.GetUserKeys(user_id);
        }

        [HttpGet]
        public async Task<List<ApiKey>> GetKeys()
        {
            return await _repository.GetKeys();
        }

        [HttpPost]
        public async Task<ApiKey> CreateApiKey(string user_id,string name)
        {
            return await _repository.CreateApiKey(user_id,name);
        }

        [HttpDelete("{keyId}")]
        public async Task<bool> delete(int keyId)
        {
            return await _repository.Delete(keyId);
        }
    }
}
