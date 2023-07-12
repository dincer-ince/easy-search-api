namespace EasySearchApi.Models.DTOs
{
    public class CreateDictionaryDTO
    {
        public string UserID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string[] ExtraFieldDescription { get; set; }
    }
}
