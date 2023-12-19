using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    [Keyless]
    public class RBAControl
    {
        [Required]
        public string Biz { get; set; }
        public string Process { get; set; }
        public string Status { get; set; }
        public int StatusVal { get; set; }
    }
}
