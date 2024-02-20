using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    [Keyless]
    public class HeadCountbyDiv
    {
        [Required]
        public string division { get; set; }
        public string department { get; set; }
        public int hc { get; set; }
    }
}
