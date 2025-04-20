using System.Text.Json.Serialization;

namespace API.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public virtual ICollection<PropertyType> Types { get; set; }
    }
}
