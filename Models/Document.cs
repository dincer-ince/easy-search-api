using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasySearchApi.Models
{
    public class Document
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string RawDocument { get; set; } = string.Empty;
        public List<DocumentWord> Words { get; set; }
        public int NumberOfWords { get; set; } = 0;
        public string[] ExtraFields { get; set; }
        public decimal? Similarity { get; set; }
        public int DictionaryId { get; set; }
        [ForeignKey("DictionaryId")]
        public Dictionary Dictionary { get; set; }
    }
}
