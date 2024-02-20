using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    [Keyless]
    public class ManpowerRequire
    {
        [Required]
        public string biz { get; set; }
        public string process { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        public DateTime date { get; set; }

        [Required]
        public int require { get; set; }
        public string skillgroup { get; set; }


    }
}