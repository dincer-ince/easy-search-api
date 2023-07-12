namespace EasySearchApi.Models.DTOs
{
    public class Similarity
    {
    }

    public class WithSimilarityModel
    {
        public int id { get; set; }
        public string title { get; set; }
        public string rawDocument { get; set; }
        public int numberOfWords { get; set; }
        public double similarity { get; set; }
    }
}
