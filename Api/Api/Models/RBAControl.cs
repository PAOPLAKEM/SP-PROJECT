using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    [Keyless]
    public class RBAControl
    {
        [Required]
        public string biz { get; set; }
        public string process { get; set; }
        public string status { get; set; }
        public int statusval { get; set; }
    }
}
