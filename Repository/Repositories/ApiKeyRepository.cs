using EasySearchApi.Base;
using EasySearchApi.Config;
using EasySearchApi.Models;
using EasySearchApi.Models.DTOs;
using EasySearchApi.Repository.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EasySearchApi.Repository.Repositories
{
    public class ApiKeyRepository : GenericRepository<ApiKey>, IApiKeyRepository
    {
        private readonly ApplicationSettings _appSettings;

        public ApiKeyRepository(DataContext context, UserManager<User> userManager, IOptions<ApplicationSettings> appSettings) : base(context, userManager) {
            _appSettings = appSettings.Value;
        }

        public async Task<ApiKey> CreateApiKey(string user_id,string name)
        {
            ApiKey apiKey = new ApiKey();
            apiKey.Name = name;
            apiKey.Value = GenerateKey();
            apiKey.UserId = user_id;
            apiKey.CreateDate = DateTime.UtcNow;

            return await Add(apiKey);
        }

        public async Task<List<ApiKey>> GetUserKeys(string user_id)
        {
            return await _context.ApiKeys.AsNoTracking().Where(x => x.UserId == user_id).ToListAsync();
        }

        public async Task<List<ApiKey>> GetKeys()
        {
            return await _context.ApiKeys.AsNoTracking().ToListAsync();
        }

        private string GenerateKey()
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddYears(1),
                SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JWT_Secret)),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);
            return token;
        }
    }
}
