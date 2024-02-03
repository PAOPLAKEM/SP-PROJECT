using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
        [Keyless]
        public class Manpower_Plan
        {
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
            [DataType(DataType.Date)]
            public DateTime Date  { get; set; }

            [Required]
            public string EMPID { get; set; }
            public string Attendance { get; set; }
            public string ShiftCode { get; set; }
            public string Shift { get; set; }
        }
    
}
