using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class OJT_InspectionSkill
    {
        [Key]
        public string courseno { get; set; }

        [Required]
        public string coursegroup { get; set; }
        public string biz { get; set; }
        public string process { get; set; }
        public string cerno { get; set; }
        public string empid { get; set; }
        public int active { get; set; }

        public string skillgroup { get; set; }
    }
}
