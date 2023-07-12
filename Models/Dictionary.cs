using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasySearchApi.Models
{
    public class Dictionary
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Document> Documents { get; set; }
        public int preferredSearch { get; set; }
        public int NumberOfDocuments { get; set; } = 0;
        public int TotalNumberOfWords { get; set; } = 0;
        public string[] ExtraFieldDescription { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
