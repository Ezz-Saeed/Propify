namespace API.DTOs
{
    public class GetPropertiesDto : PropertyDto
    {
        public string TypeName { get; set; }
        public string CategoryName { get; set; }
        public string OwnerName { get; set; }

    }
}
