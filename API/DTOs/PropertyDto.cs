using API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.DTOs
{
    public class PropertyDto
    {
        [Required]
        public string Description { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public decimal Price { get; set; }
        public int? BedRooms { get; set; }
        public int? BathRooms { get; set; }
        public double Area { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsRental { get; set; }
              //public string AppUserId { get; set; }
    }
}
