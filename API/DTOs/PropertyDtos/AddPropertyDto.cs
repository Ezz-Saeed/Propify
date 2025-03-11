namespace API.DTOs.PropertyDtos
{
    public class AddPropertyDto : PropertyDto
    {
        public int TypeId { get; set; }
        public ICollection<IFormFile>? Images { get; set; }
    }
}
