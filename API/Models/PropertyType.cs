using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class PropertyType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
