using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class OJT_InspectionSkill
    {
        [Key]
        public string CourseNo { get; set; }

        [Required]
        public string CourseGroup { get; set; }
        public string Biz { get; set; }
        public string Process { get; set; }
        public string CerNo { get; set; }
        public string EmpID { get; set; }
        public int Active { get; set; }

        public string SkillGroup { get; set; }
    }
}
