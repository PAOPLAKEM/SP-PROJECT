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
            public DateTime date  { get; set; }

            [Required]
            public string empid { get; set; }
            public string attendance { get; set; }
            public string shiftcode { get; set; }
            public string shift { get; set; }
        }
    
}
