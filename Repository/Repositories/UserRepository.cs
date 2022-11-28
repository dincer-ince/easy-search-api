using EasySearchApi.Repository.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EasySearchApi.Repository.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(DataContext context) : base(context) { 
        } 

    }
}
