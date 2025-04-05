using API.Models;

namespace API.DTOs.PropertyDtos
{
    public class GetPropertiesDto : PropertyDto
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
        public int TypeId { get; set; }
        public string CategoryName { get; set; }
        public string OwnerName { get; set; }
        public ICollection<PropertyImage>? Images { get; set; }
    }
}
