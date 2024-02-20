using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class EmployeeInfo
    {
        [Key]
        public string empid { get; set; }

        [Required]
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string division { get; set; }
        public string department { get; set; }
        public string biz { get; set; }
        public string process { get; set; }
    }

}