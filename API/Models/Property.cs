using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API.Models
{
    //[Owned]
    public class Property
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public decimal Price { get; set; }
        public int? BedRooms { get; set; }
        public int? BathRooms { get; set; }
        public double Area { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsRental { get; set; }
        [ForeignKey(nameof(Type))]
        public int TypeId { get; set; }
        [JsonIgnore]
        public PropertyType Type { get; set; }
        [ForeignKey(nameof(AppUser))]
        public string AppUserId { get; set; }
        [JsonIgnore]
        public AppUser AppUser { get; set; }
        public string ImageUrl => Images?.FirstOrDefault(i => i.IsMain)?.Url ?? string.Empty;
        public bool IsDeleted { get; set; }
        [JsonIgnore]
        public ICollection<Image>? Images { get; set; }
    }
}
