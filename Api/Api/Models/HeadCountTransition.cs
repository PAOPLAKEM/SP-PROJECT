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
        public DateTime Datetime { get; set; }

        [Required]
        public string EmpID { get; set; }
        public string TransType { get; set; }
        public string Biz { get; set; }
        public string Process { get; set; }
    }
}
