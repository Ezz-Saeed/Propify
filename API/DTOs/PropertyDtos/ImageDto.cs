using System.ComponentModel.DataAnnotations.Schema;

namespace API.DTOs.PropertyDtos
{
    public class ImageDto
    {
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public string? PublicId { get; set; }
        public int PropertyId { get; set; }
    }
}
