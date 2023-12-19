using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    [Keyless]
    public class HeadCountbyDiv
    {
        [Required]
        public string Division { get; set; }
        public string Department { get; set; }
        public int HC { get; set; }
    }
}
