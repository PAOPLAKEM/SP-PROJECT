    using Api.Data;
    using Api.Models;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;




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
            public IActionResult EmployeeInfo_with_SKILL()
            {
            var query = from ei in _db.EmployeeInfo
                        join ojt in _db.OJT_InspectionSkill on ei.EmpId equals ojt.EmpID
                        select new
                        {
                            EmployeeInfo = ei,
                            OJT_InspectionSkill = ojt
                        };

            return Ok(query);
            }

           //[HttpGet]
           // [Route("[action]")]
           // public IActionResult OJT_InspectionSkill()
           // {
           //     IEnumerable <OJT_InspectionSkill> OJT_InspectionSkill = _db.OJT_InspectionSkill;
           //    return Ok(OJT_InspectionSkill);
           // }

           // [HttpGet]
           // [Route("[action]")]
           // public IActionResult ManpowerRequire()
           // {
           //    IEnumerable <ManpowerRequire> ManpowerRequire = _db.ManpowerRequire;
           //    return Ok(ManpowerRequire);
           // }

            [HttpGet]
            [Route("[action]")]
            public IActionResult ProductionPlan()
            {
                IEnumerable <ProductionPlan> ProductionPlan = _db.ProductionPlan;
                return Ok(ProductionPlan);
            }
            [HttpGet]
            [Route("[action]")]
            public IActionResult ManpowerPlanWithFaceScanLogandGateLog()
            {
            DateTime targetDate = new DateTime(2023, 9, 1);

            var query = from m in _db.Manpower_Plan
                        where m.Date.Date == targetDate
                        select new
                        {
                            ManpowerPlan = m,
                            FaceScanLog = _db.FaceScanLog.FirstOrDefault(s => s.Datetime.Date == targetDate && s.EMPLOYEE_ID == m.EMPID),
                            GateLog = _db.GateLog.FirstOrDefault(g => g.EmpID == m.EMPID)
                        };

            return Ok(query.ToList());
            }


           // [HttpGet]
           // [Route("[action]")]
           // public IActionResult GateLog()
           // {
           //     IEnumerable <GateLog> GateLog = _db.GateLog;
           //     return Ok(GateLog);
           //}

            [HttpGet]
            [Route("[action]")]
            public IActionResult RBAControl()
            {
                IEnumerable<RBAControl> RBAControl = _db.RBAControl;
                return Ok(RBAControl);
            }

           // [HttpGet]
           // [Route("[action]")]
           // public IActionResult HeadCountTransition()
           // {
           //    IEnumerable <HeadCountTransition> HeadCountTransition = _db.HeadCountTransition;
           //    return Ok(HeadCountTransition);
           // }

        [HttpGet]
        [Route("[action]")]
        public IActionResult HeadCountTransitionCOUNT()
        {
            var currentMonth = 9;    //DateTime.Now.Month;

            var query = from m in _db.ManpowerRequire
                        join ei in _db.EmployeeInfo on m.Process equals ei.Process
                        where m.Date.Month == currentMonth
                        group new { m, ei } by new { m.Biz, m.Process, m.Date, m.Require, m.SkillGroup } into grouped
                        select new
                        {
                            Biz = grouped.Key.Biz,
                            Process = grouped.Key.Process,
                            Date = grouped.Key.Date,
                            Require = grouped.Key.Require,
                            SkillGroup = grouped.Key.SkillGroup,
                            TotalEmployees = grouped.Count()
                        };

            return Ok(query.ToList());



        }

        [HttpGet]
            [Route("[action]")]
            public IActionResult HeadCountTransitionMonth()
            {
            List<object> monthDataList = new List<object>();

            for (int i = 1; i <= 12; i++)
            {
                var monthData = (from hct in _db.HeadCountTransition
                                 where hct.Datetime.Month == i
                                 select new
                                 {
                                     EmpID = hct.EmpID,
                                     Datetime = hct.Datetime,
                                     TransType = hct.TransType,
                                     Biz = hct.Biz,
                                     Process = hct.Process
                                 }).ToList();

                // Create a dictionary to store the data for the month
                Dictionary<string, object> monthDict = new Dictionary<string, object>();
                string monthName = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(i);

                // Add data to the dictionary
                monthDict.Add(monthName, monthData);

                // Add the dictionary to the list
                monthDataList.Add(monthDict);
            }

            // Return the data
            return Ok(monthDataList);
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

            //[HttpGet]
            //[Route("[action]")]
            //public IActionResult Manpower_Plan()
            //{
              //  IEnumerable<Manpower_Plan> Manpower_Plan = _db.Manpower_Plan;
                //return Ok(Manpower_Plan);
            //}

        }

    }