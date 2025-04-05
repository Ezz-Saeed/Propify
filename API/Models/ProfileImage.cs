using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class ProfileImage : Image
    {
        public int Id { get; set; }
        [ForeignKey(nameof(AppUser))]
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
    }
}
