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

            /*[HttpGet]
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
            }*/

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
        public IActionResult CombinedData()
        {
            DateTime targetDate = new DateTime(2023, 9, 1);

            var query = from m in _db.Manpower_Plan
                        join ei in _db.EmployeeInfo on m.EMPID equals ei.EmpId
                        where m.Date.Date == targetDate
                        select new
                        {
                            ManpowerPlan = m,
                            EmployeeInfo = ei
                        };

            var combinedDataList = new List<object>();

            foreach (var item in query.ToList()) // ใช้ .ToList() ที่นี่
            {
                var ojtSkills = _db.OJT_InspectionSkill.Where(o => o.EmpID == item.EmployeeInfo.EmpId).ToList();
                var faceScanLog = _db.FaceScanLog.Where(s => s.Datetime.Date == targetDate && s.EMPLOYEE_ID == item.ManpowerPlan.EMPID)
                                                  .OrderByDescending(s => s.Datetime)
                                                  .FirstOrDefault();
                var gateLog = _db.GateLog.Where(g => g.EmpID == item.ManpowerPlan.EMPID)
                                          .OrderByDescending(g => g.Datetime)
                                          .FirstOrDefault();

                var combinedData = new
                {
                    ManpowerPlan = item.ManpowerPlan,
                    EmployeeInfo = item.EmployeeInfo,
                    OJT_InspectionSkill = ojtSkills,
                    FaceScanLog = faceScanLog,
                    GateLog = gateLog
                };

                combinedDataList.Add(combinedData);
            }

            return Ok(combinedDataList);

        }




        /*[HttpGet]
        [Route("[action]")]
        public IActionResult ManpowerPlanWithFaceScanLogandGateLog()
        {
            // สร้างตัวแปร DateTime สำหรับระบุวันที่เป้าหมาย
            DateTime targetDate = new DateTime(2023, 9, 1);

            var query = from m in _db.Manpower_Plan
                        join ei in _db.EmployeeInfo on m.EMPID equals ei.EmpId
                        join ojt in _db.OJT_InspectionSkill on m.EMPID equals ojt.EmpID into ojtJoin
                        from ojt in ojtJoin.DefaultIfEmpty()
                        where m.Date.Date == targetDate
                        let faceScanLog = _db.FaceScanLog
                                             .Where(s => s.Datetime.Date == targetDate && s.EMPLOYEE_ID == m.EMPID)
                                             .OrderByDescending(s => s.Datetime)
                                             .FirstOrDefault()
                        let gateLog = _db.GateLog
                                            .Where(g => g.EmpID == m.EMPID)
                                            .OrderByDescending(g => g.Datetime)
                                            .FirstOrDefault()
                        select new
                        {
                            EmployeeInfo = new
                            {
                                EmpID = ei.EmpId,
                                FirstName = ei.FirstName,
                                LastName = ei.LastName,
                                Biz = ei.Biz,
                                Process = ei.Process
                            },
                            OJT_InspectionSkill = ojt,
                            Status = GetStatus(faceScanLog, gateLog)
                        };

            // ฟังก์ชันสำหรับกำหนดสถานะตามเงื่อนไขที่กำหนด
            private string GetStatus(FaceScanLog faceScanLog, GateLog gateLog)
            {
                if (faceScanLog == null && gateLog == null)
                {
                    return "Absent";
                }
                else if (faceScanLog != null && faceScanLog.Status == "IN" && gateLog != null && gateLog.Status == "IN")
                {
                    return "IN Cleanroom";
                }
                else if (faceScanLog != null && faceScanLog.Status == "IN" && gateLog != null && gateLog.Status == "OUT")
                {
                    return "Outside Cleanroom";
                }
                else if (faceScanLog != null && faceScanLog.Status == "OUT" && gateLog != null && gateLog.Status == "OUT")
                {
                    return "OUT OF COMPANY";
                }
                else
                {
                    return "Unknown";
                }
            }

            return Ok(query.ToList());

        }*/


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

        /*[HttpGet]
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
        }*/
        [HttpGet]
        [Route("[action]")]
        public IActionResult HeadCountTransitionYearMonth()
        {
            // Dictionary สำหรับเก็บข้อมูลตามปีและเดือน
            Dictionary<int, Dictionary<string, List<object>>> yearMonthData = new Dictionary<int, Dictionary<string, List<object>>>();

            // ดึงข้อมูลจากฐานข้อมูล
            var headCountTransitionData = _db.HeadCountTransition.ToList();

            // วนลูปเพื่อจัดเก็บข้อมูลตามปีและเดือน
            foreach (var item in headCountTransitionData)
            {
                int year = item.Datetime.Year;
                string monthName = item.Datetime.ToString("MMM");

                // ตรวจสอบว่ามีปีนี้อยู่ใน Dictionary หรือไม่
                if (!yearMonthData.ContainsKey(year))
                {
                    yearMonthData[year] = new Dictionary<string, List<object>>();
                }

                // ตรวจสอบว่ามีเดือนนี้อยู่ในปีนี้หรือไม่
                if (!yearMonthData[year].ContainsKey(monthName))
                {
                    yearMonthData[year][monthName] = new List<object>();
                }

                // เพิ่มข้อมูลลงในรายการเดือนนี้
                yearMonthData[year][monthName].Add(new
                {
                    EmpID = item.EmpID,
                    Datetime = item.Datetime.ToString("yyyy-MM-ddTHH:mm:ss"),
                    TransType = item.TransType,
                    Biz = item.Biz,
                    Process = item.Process
                });
            }

            // เพิ่มเดือนที่ไม่มีข้อมูลเป็น array ว่าง
            foreach (var year in yearMonthData.Keys)
            {
                for (int month = 1; month <= 12; month++)
                {
                    string monthName = new DateTime(year, month, 1).ToString("MMM");
                    if (!yearMonthData[year].ContainsKey(monthName))
                    {
                        yearMonthData[year][monthName] = new List<object>();
                    }
                }
            }

            // เรียงลำดับเดือนตามเลขที่แทนเดือนในระบบภาษาอังกฤษ
            foreach (var yearData in yearMonthData.Values)
            {
                var sortedMonthData = yearData.OrderBy(x => DateTime.ParseExact(x.Key, "MMM", CultureInfo.InvariantCulture).Month).ToDictionary(x => x.Key, x => x.Value);
                yearData.Clear();
                foreach (var monthData in sortedMonthData)
                {
                    yearData.Add(monthData.Key, monthData.Value);
                }
            }

            return Ok(yearMonthData);
        }


        /*[HttpGet]
        [Route("[action]")]
        public IActionResult HeadCountTransitionYearMonth()
        {
            // Dictionary สำหรับเก็บข้อมูลตามปีและเดือน
            Dictionary<int, Dictionary<string, List<object>>> yearMonthData = new Dictionary<int, Dictionary<string, List<object>>>();

            // ดึงข้อมูลจากฐานข้อมูล
            var headCountTransitionData = _db.HeadCountTransition.ToList();

            // วนลูปเพื่อจัดเก็บข้อมูลตามปีและเดือน
            foreach (var item in headCountTransitionData)
            {
                int year = item.Datetime.Year;
                string monthName = item.Datetime.ToString("MMM");

                // ตรวจสอบว่ามีปีนี้อยู่ใน Dictionary หรือไม่
                if (!yearMonthData.ContainsKey(year))
                {
                    yearMonthData[year] = new Dictionary<string, List<object>>();
                }

                // ตรวจสอบว่ามีเดือนนี้อยู่ในปีนี้หรือไม่
                if (!yearMonthData[year].ContainsKey(monthName))
                {
                    yearMonthData[year][monthName] = new List<object>();
                }

                // เพิ่มข้อมูลลงในรายการเดือนนี้
                yearMonthData[year][monthName].Add(new
                {
                    EmpID = item.EmpID,
                    Datetime = item.Datetime.ToString("yyyy-MM-ddTHH:mm:ss"),
                    TransType = item.TransType,
                    Biz = item.Biz,
                    Process = item.Process
                });
            }

            return Ok(yearMonthData);
        }*/


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