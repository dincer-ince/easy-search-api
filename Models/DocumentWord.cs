
using System.ComponentModel.DataAnnotations;

namespace EasySearchApi.Models
{
    public class DocumentWord
    {
        [Key]
        public int Id { get; set; }

        public Word word { get; set; }
        public int wordId { get; set; }
        public Document document { get; set; }
        public int documentId { get; set; }

        public int count { get; set; } = 0;
    }
}
