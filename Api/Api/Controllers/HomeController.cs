using Api.Data;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Api.Controllers
{
    [ApiController]
    public class HomeController : Controller
    {
        private readonly ApplicationDBContext _db;

        public HomeController(ApplicationDBContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult EmployeeInfo()
        {
            IEnumerable <EmployeeInfo> EmployeeInfo = _db.EmployeeInfo;
            return Ok(EmployeeInfo);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult OJT_InspectionSkill()
        {
            IEnumerable <OJT_InspectionSkill> OJT_InspectionSkill = _db.OJT_InspectionSkill;
            return Ok(OJT_InspectionSkill);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult ManpowerRequire()
        {
            IEnumerable <ManpowerRequire> ManpowerRequire = _db.ManpowerRequire;
            return Ok(ManpowerRequire);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult ProductionPlan()
        {
            IEnumerable <ProductionPlan> ProductionPlan = _db.ProductionPlan;
            return Ok(ProductionPlan);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult FaceScanLog()
        {
            IEnumerable <FaceScanLog> FaceScanLog = _db.FaceScanLog;
            return Ok(FaceScanLog);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult GateLog()
        {
            IEnumerable <GateLog> GateLog = _db.GateLog;
            return Ok(GateLog);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult RBAControl()
        {
            IEnumerable<RBAControl> RBAControl = _db.RBAControl;
            return Ok(RBAControl);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult HeadCountTransition()
        {
            IEnumerable <HeadCountTransition> HeadCountTransition = _db.HeadCountTransition;
            return Ok(HeadCountTransition);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult HeadCountbyDiv()
        {
            IEnumerable <HeadCountbyDiv> HeadCountbyDiv = _db.HeadCountbyDiv;
            return Ok(HeadCountbyDiv);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult HeadCountbyWorkGroup()
        {
            IEnumerable <HeadCountbyWorkGroup> HeadCountbyWorkGroup = _db.HeadCountbyWorkGroup;
            return Ok(HeadCountbyWorkGroup);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Manpower_Plan()
        {
            IEnumerable<Manpower_Plan> Manpower_Plan = _db.Manpower_Plan;
            return Ok(Manpower_Plan);
        }

    }

}