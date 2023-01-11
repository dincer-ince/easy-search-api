using System.ComponentModel.DataAnnotations;

namespace EasySearchApi.Models
{
    public class Word
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string term { get; set; } = string.Empty;
        public List<DocumentWord> documents { get; set; }
    }
}
