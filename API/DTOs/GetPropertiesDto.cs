namespace API.DTOs
{
    public class GetPropertiesDto : PropertyDto
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
        public string CategoryName { get; set; }
        public string OwnerName { get; set; }

    }
}
