using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Owned]
    public class Image
    {
        public string Url { get; set; }
        public bool IsMain  { get; set; }
        public string? PublicId { get; set; }
        [ForeignKey(nameof(Property))]
        public int PropertyId { get; set; }
        public Property Property { get; set; }
    }
}
