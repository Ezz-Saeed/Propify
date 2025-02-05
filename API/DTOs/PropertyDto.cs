using API.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.DTOs
{
    public class PropertyDto
    {
        public string Description { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public decimal Price { get; set; }
        public int? BedRooms { get; set; }
        public int? BathRooms { get; set; }
        public double Area { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsRental { get; set; }
        public int TypeId { get; set; }
        //public string AppUserId { get; set; }
    }
}
