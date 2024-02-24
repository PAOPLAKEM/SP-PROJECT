using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    [Keyless]
    public class Replacement
    {

        [Required]
        public string EmpID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Biz { get; set; }
        public string Process { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public string Shift { get; set; }

        public string R_EmpID { get; set; }
        public string R_FirstName { get; set; }
        public string R_LastName { get; set; }
        public string R_Biz { get; set; }
        public string R_Process { get; set; }
    }

}