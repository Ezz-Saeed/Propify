using API.DTOs.OwnerDtos;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.AuthDtos
{
    public class UserDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? DisplayName { get; set; }
        public List<string> Roles { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresOn { get; set; }
        public bool IsAuthenticated { get; set; }
        public ProfileImageDto? ProfileImage { get; set; } = new ProfileImageDto();
    }
}
