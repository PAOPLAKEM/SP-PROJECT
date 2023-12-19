using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    [Keyless]
    public class ProductionPlan
    {
        [Required]
        public string Biz { get; set; }
        public string Process { get; set; }
        public int Plan { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

    }
}
