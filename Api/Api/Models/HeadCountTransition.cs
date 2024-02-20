using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.EntityFrameworkCore;

namespace Api.Models
{
    [Keyless]
    public class HeadCountTransition
    {
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        [DataType(DataType.Date)]
        public DateTime datetime { get; set; }

        [Required]
        public string empid { get; set; }
        public string transtype { get; set; }
        public string biz { get; set; }
        public string process { get; set; }
    }
}
