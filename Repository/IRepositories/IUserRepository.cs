using EasySearchApi.Base;
using EasySearchApi.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EasySearchApi.Repository.IRepositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<ActionResult<IEnumerable<UserDTO>>> GetUsers();
        IQueryable<UserDTO> GetUser(string id);
        Task<int> UpdateUser(string id, User user);
        Task<IdentityResult> RegisterUser(string UserName, string Password);
        Task<IdentityResult> DeleteUser(string id);
        Task<bool> ChangePassword(PasswordUpdateDTO passwordUpdateDTO, string user_id);
    }
}
