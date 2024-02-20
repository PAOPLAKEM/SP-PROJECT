using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    [Keyless]
    public class ProductionPlan
    {
        [Required]
        public string biz { get; set; }
        public string process { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        public DateTime date { get; set; }

        [Required]
        public int plan { get; set; }
    }
}
