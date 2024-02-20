using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) :base(options) {

        }

        public DbSet<EmployeeInfo> employeeinfo {  get; set; }
        public DbSet<OJT_InspectionSkill> ojt_inspectionskill { get; set; }
        public DbSet<Manpower_Plan> manpower_plan { get; set; }
        public DbSet<ManpowerRequire> manpowerrequire { get; set; }
        public DbSet<ProductionPlan> productionplan { get; set; }
        public DbSet<FaceScanLog> facescanlog { get; set; }
        public DbSet<GateLog> gatelog { get; set; }
        public DbSet<RBAControl> rbacontrol { get; set; }
        public DbSet<HeadCountTransition> headcounttransition { get; set; }
        public DbSet<HeadCountbyDiv> headcountbydiv { get; set; }
        public DbSet<HeadCountbyWorkGroup> headcountbyworkgroup { get; set; }

    }
}