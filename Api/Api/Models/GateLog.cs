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
        public DateTime Datetime { get; set; }

        [Required]
        public string Room { get; set; }
        public string GateNo { get; set; }
        public int EmpID { get; set; }
        public string Status { get; set; }

    }
}
