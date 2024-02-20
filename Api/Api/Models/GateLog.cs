using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    [Keyless]
    public class GateLog
    {
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        [DataType(DataType.Date)]
        public DateTime datetime { get; set; }

        [Required]
        public string room { get; set; }
        public string gateno { get; set; }
        public string empid { get; set; }
        public string status { get; set; }

    }
}
