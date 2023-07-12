using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasySearchApi.Models
{
    public class ApiKey
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public bool Expired { get; set; }
        [NotMapped]
        public string ExpireTimeRemained { get {
                return (13 - (((DateTime.Now.Year - CreateDate.Year) * 12) + (DateTime.Now.Month - CreateDate.Month))) + " months left to expire"; 
            } 
        }
    }
}
