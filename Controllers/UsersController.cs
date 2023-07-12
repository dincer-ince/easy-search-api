using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EasySearchApi.Data;
using EasySearchApi.Models;
using EasySearchApi.Repository.Repositories;
using EasySearchApi.Repository.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using EasySearchApi.Models.DTOs;

namespace EasySearchApi.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repository;

        public UsersController(IUserRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            return await _repository.GetUsers();
        }

        [HttpGet("{id}")]
        public IQueryable<UserDTO> GetUser(string id)
        {
            return _repository.GetUser(id);
        }

        [HttpPut("{id}")]
        public async Task<int> UpdateUser(string id, User user)
        {
            return await _repository.UpdateUser(id, user);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IdentityResult> RegisterUser(LoginDTO request)
        {
            return await _repository.RegisterUser(request.UserName, request.Password);
        }

        [HttpDelete("{id}")]
        public async Task<IdentityResult> DeleteUser(string id)
        {
            return await _repository.DeleteUser(id);
        }

        [HttpPut("changePassword/{user_id}")]
        public async Task<bool> ChangePassword(string user_id, PasswordUpdateDTO passwordUpdateDTO)
        {
            return await _repository.ChangePassword(passwordUpdateDTO, user_id);
        }
    }
}
