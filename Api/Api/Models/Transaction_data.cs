using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class Transaction_data
    {
        [Key]
        public int autoID { get; set; }

        [Required]
        public string EmployeeID { get; set; }
        public string Name { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        [DataType(DataType.Date)]
        public DateTime DateTime { get; set; }
        public int CameraNo { get; set; }
        public string Image { get; set; }
    }

}