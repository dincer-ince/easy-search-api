using EasySearchApi.Base;
using EasySearchApi.Models.DTOs;
using EasySearchApi.Repository.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace EasySearchApi.Repository.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(DataContext context, UserManager<User> userManager)  : base(context, userManager)
        {
        }

        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            return await _userManager.Users.Select(u => new UserDTO()
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                Dictionaries = u.Dictionaries,
                ApiKeys = u.ApiKeys
            }).ToListAsync();
        }

        public IQueryable<UserDTO> GetUser(string id)
        {
            return _userManager.Users.Include(x=>x.Dictionaries).Select(u => new UserDTO()
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                Dictionaries = u.Dictionaries,
                ApiKeys = u.ApiKeys
            }).Where(x => x.Id == id);
        }

        public async Task<int> UpdateUser(string id, User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _userManager.UpdateNormalizedEmailAsync(user);
            return await _context.SaveChangesAsync();
        }

        public async Task<IdentityResult> RegisterUser(string UserName, string Password)
        {
            var user = new User();
            user.UserName = UserName;

            try
            {
                return await _userManager.CreateAsync(user, Password);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IdentityResult> DeleteUser(string id)
        {

            var deleteUser = await _userManager.FindByIdAsync(id);
            return await _userManager.DeleteAsync(deleteUser);
        }

        public async Task<bool> ChangePassword(PasswordUpdateDTO passwordUpdateDTO, string user_id)
        {
            var user = _userManager.FindByIdAsync(user_id).Result;
            var result = await _userManager.ChangePasswordAsync(user, passwordUpdateDTO.CurrentPassword, passwordUpdateDTO.NewPassword);
            if (result.Succeeded)
            {
                _context.SaveChanges();
                return true;
            }
            else
                return false;
        }
    }
}
