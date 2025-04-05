using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class AppUser : IdentityUser
    {
        [MaxLength(100)]
        public string FirstName { get; set; }
        [MaxLength(100)]
        public string LastName { get; set; }
        public string? DisplayName { get; set; }
        public List<RefreshToken>? RefreshTokens { get; set; }
        public List<Property>? Properties { get; set; } = new List<Property>();
        [ForeignKey(nameof(ProfileImage))]
        public int? ProfileImageId { get; set; }
        public ProfileImage? ProfileImage { get; set; }
    }
}
