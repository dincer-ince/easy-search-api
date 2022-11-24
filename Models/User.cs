using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace EasySearchApi.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string userName { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty; 
        public List<Dictionary> dictionaries { get; set; }

    }
}
