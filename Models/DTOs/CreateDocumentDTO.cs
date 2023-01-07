namespace EasySearchApi.Models.DTOs
{
    public class CreateDocumentDTO
    {
        public int DictionaryID { get; set; }
        public string Title { get; set; } =string.Empty;
        public string RawDocument { get; set; } =string.Empty;
    }
}
