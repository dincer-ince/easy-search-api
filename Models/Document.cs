using System.ComponentModel.DataAnnotations;

namespace EasySearchApi.Models
{
    public class Document
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string rawDocument { get; set; } = string.Empty;

        public Dictionary dictionary { get; set; }
        public int dictionaryId { get; set; }   
        public List<DocumentWord> words { get; set; }
        public int numberOfWords { get; set; } = 0;
    }
}
