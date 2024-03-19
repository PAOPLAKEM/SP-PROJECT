using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) :base(options) {

        }

        public DbSet<EmployeeInfo> EmployeeInfo {  get; set; }
        public DbSet<OJT_InspectionSkill> OJT_InspectionSkill { get; set; }
        public DbSet<Manpower_Plan> Manpower_Plan { get; set; }
        public DbSet<ManpowerRequire> ManpowerRequire { get; set; }
        public DbSet<ProductionPlan> ProductionPlan { get; set; }
        public DbSet<FaceScanLog> FaceScanLog { get; set; }
        public DbSet<GateLog> GateLog { get; set; }
        public DbSet<RBAControl> RBAControl { get; set; }
        public DbSet<HeadCountTransition> HeadCountTransition { get; set; }
        public DbSet<HeadCountbyDiv> HeadCountbyDiv { get; set; }
        public DbSet<HeadCountbyWorkGroup> HeadCountbyWorkGroup { get; set; }
        public DbSet<Transaction_data> face_recog_transaction { get; set; }
        public DbSet<Replacement> Replacement { get; set; }
        public DbSet<LOGCAM> face_recog_camera { get; set; }

    }
}