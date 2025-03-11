using API.Models;

namespace API.DTOs.PropertyDtos
{
    public class GetPropertiesDto : PropertyDto
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
        public string CategoryName { get; set; }
        public string OwnerName { get; set; }
        public ICollection<Image>? Images { get; set; }
    }
}
