using System.ComponentModel.DataAnnotations;

namespace API.DTOs.OwnerDtos
{
    public class EditProfileDto
    {
        [MaxLength(100)]
        public string? FirstName { get; set; }
        [MaxLength(100)]
        public string? LastName { get; set; }
        public string? DisplayName { get; set; }
        public IFormFile? ProfileImage { get; set; }
    }
}
