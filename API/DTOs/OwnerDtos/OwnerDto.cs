using System.ComponentModel.DataAnnotations;

namespace API.DTOs.OwnerDtos
{
    public class OwnerDto
    {
        [MaxLength(100)]
        public string FirstName { get; set; }
        [MaxLength(100)]
        public string LastName { get; set; }
        public string? DisplayName { get; set; }
        public ProfileImageDto? ProfileImage { get; set; }
    }
}
