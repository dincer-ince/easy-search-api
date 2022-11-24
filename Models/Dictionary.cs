using System.ComponentModel.DataAnnotations;

namespace EasySearchApi.Models
{
    public class Dictionary
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Document> documents { get; set; }
        public User user { get; set; }
        public int userId { get; set; }
        public int NumberOfDocuments { get; set; } = 0;
        public int totalNumberOfWords { get; set; } = 0;
        
    }
}
