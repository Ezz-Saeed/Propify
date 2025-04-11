using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Image
    {
        public string Url { get; set; }
        public string PublicId { get; set; }
    }
}
