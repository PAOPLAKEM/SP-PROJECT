using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class EmployeeInfo
    {
        [Key]
        public string EmpId { get; set; }

        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Division { get; set; }
        public string Department { get; set; }
        public string Biz { get; set; }
        public string Process { get; set; }
    }

}