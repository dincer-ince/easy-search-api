using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasySearchApi.Models
{
    public class User : IdentityUser
    {
        public List<Dictionary> Dictionaries { get; set; }
        public List<ApiKey> ApiKeys { get; set; }
    }
}
