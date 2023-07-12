
using System.ComponentModel.DataAnnotations;

namespace EasySearchApi.Models
{
    public class DocumentWord
    {
        [Key]
        public int Id { get; set; }

        public Word Word { get; set; }
        public Guid WordId { get; set; }
        public Document Document { get; set; }
        public int DocumentId { get; set; }

        public int Count { get; set; } = 0;
    }
}
