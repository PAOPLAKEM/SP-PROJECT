using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    [Keyless]
    public class HeadCountbyWorkGroup
    {
        [Required]
        public string Group { get; set; }
        public int HC { get; set; }
    }
}
