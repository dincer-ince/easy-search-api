using System.ComponentModel.DataAnnotations;

namespace EasySearchApi.Models
{
    public class Word
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Term { get; set; } = string.Empty;
        public List<DocumentWord> Documents { get; set; }
    }
}
