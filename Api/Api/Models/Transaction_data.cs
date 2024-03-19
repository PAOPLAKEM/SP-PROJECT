using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class Transaction_data
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string EmpID { get; set; }
        public string Name { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        [DataType(DataType.Date)]
        public DateTime DateTime { get; set; }
        public string Image { get; set; }
        public string CameraNO_id { get; set; }
    }

}