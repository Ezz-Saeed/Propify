using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class AppUser : IdentityUser
    {
        [MaxLength(100)]
        public string FirstName { get; set; }
        [MaxLength(100)]
        public string LastName { get; set; }
        public List<RefreshToken>? RefreshTokens { get; set; }
        public List<Property>? Properties { get; set; } = new List<Property>();
    }
}
