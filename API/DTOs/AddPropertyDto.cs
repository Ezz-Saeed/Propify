namespace API.DTOs
{
    public class AddPropertyDto : PropertyDto
    {
        public int TypeId { get; set; }
        public ICollection<IFormFile>? Images { get; set; }
    }
}
