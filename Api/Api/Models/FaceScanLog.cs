using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    [Keyless]
    public class FaceScanLog
    {
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        [DataType(DataType.Date)]
        public DateTime Datetime { get; set; }

        [Required]
        public string EMPLOYEE_ID { get; set; }
        public string Status { get; set; }
    }
}
