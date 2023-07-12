namespace EasySearchApi.Models.DTOs
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<Dictionary> Dictionaries { get; set; }
        public List<ApiKey> ApiKeys { get; set; }
    }
}
