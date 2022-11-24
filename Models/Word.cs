using System.ComponentModel.DataAnnotations;

namespace EasySearchApi.Models
{
    public class Word
    {
        [Key]
        public int Id { get; set; }
        public string term { get; set; } = string.Empty;
        public List<DocumentWord> documents { get; set; }
    }
}
