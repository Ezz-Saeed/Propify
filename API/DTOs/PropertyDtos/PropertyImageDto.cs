using System.ComponentModel.DataAnnotations.Schema;

namespace API.DTOs.PropertyDtos
{
    public class PropertyImageDto : ImageDto
    {
        public bool IsMain { get; set; }       
        public int PropertyId { get; set; }
    }
}
