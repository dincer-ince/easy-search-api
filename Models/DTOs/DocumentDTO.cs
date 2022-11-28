using System.Security.Policy;

namespace EasySearchApi.Models.DTOs
{
    public class DocumentDTO
    {
        public string text { get; set; }
        public string[] tokens { get; set; }
        

    }
}
