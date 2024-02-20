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
            IEnumerable<ProductionPlan> ProductionPlan = _db.productionplan;
            return Ok(ProductionPlan);
        }
        [HttpGet]
        [Route("[action]")]
        public IActionResult CombinedData()
        {
            DateTime targetDate = new DateTime(2024, 2, 1);

            var query = from m in _db.manpower_plan
                        join ei in _db.employeeinfo on m.empid equals ei.empid
                        where m.date.Date == targetDate && m.attendance == "O"
                        select new
                        {
                            ManpowerPlan = m,
                            EmployeeInfo = ei
                        };

            var combinedDataList = new List<object>();

            foreach (var item in query.ToList())
            {
                var ojtSkills = _db.ojt_inspectionskill.Where(o => o.empid == item.EmployeeInfo.empid).ToList();
                var faceScanLog = _db.facescanlog.Where(s => s.employee_id == item.ManpowerPlan.empid)
                                                  .OrderByDescending(s => s.datetime)
                                                  .FirstOrDefault();
                var gateLog = _db.gatelog.Where(g => g.empid == item.ManpowerPlan.empid)
                                          .OrderByDescending(g => g.datetime)
                                          .FirstOrDefault();

                // ตรวจสอบเงื่อนไขเวลาเพื่อเลือก shift จาก ManpowerPlan
                var shift = item.ManpowerPlan.shift;
                if (shift == "Day" && targetDate.Hour >= 7 && targetDate.Hour < 19)
                {
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
                else if (shift == "Night" && (targetDate.Hour >= 19 || targetDate.Hour < 7))
                {
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
            }

            return Ok(combinedDataList);
        }



        [HttpGet]
        [Route("[action]")]
        public IActionResult NOT_INCLEAN()
        {
            DateTime targetDate = new DateTime(2024, 2, 1);

            var query = from m in _db.manpower_plan
                        join ei in _db.employeeinfo on m.empid equals ei.empid
                        where m.date.Date == targetDate
                        select new
                        {
                            ManpowerPlan = m,
                            EmployeeInfo = ei
                        };

            var combinedDataList = new List<object>();

            foreach (var item in query.ToList()) // ใช้ .ToList() ที่นี่
            {
                var faceScanLog = _db.facescanlog.Where(s => s.datetime.Date == targetDate && s.employee_id == item.ManpowerPlan.empid)
                                                  .OrderByDescending(s => s.datetime)
                                                  .FirstOrDefault();

                var gateLog = _db.gatelog.Where(g => g.empid == item.ManpowerPlan.empid)
                                          .OrderByDescending(g => g.datetime)
                                          .FirstOrDefault();

                // เพิ่มเงื่อนไขเชื่อมโยงเพื่อตรวจสอบสถานะของ faceScanLog และ gateLog
                if ((faceScanLog != null && faceScanLog.status == "IN") && (gateLog == null || gateLog.status == "OUT"))
                {
                    var combinedData = new
                    {
                        EmployeeInfo = item.EmployeeInfo
                    };

                    combinedDataList.Add(combinedData);
                }
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
                IEnumerable<RBAControl> RBAControl = _db.rbacontrol;
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
            DateTime targetDate = new DateTime(2024, 2, 1);    //DateTime.Now.Month;

            var query = from m in _db.manpowerrequire
                        join ei in _db.employeeinfo on m.process equals ei.process
                        where m.date.Date == targetDate
                        group new { m, ei } by new { m.biz, m.process, m.date, m.require, m.skillgroup } into grouped
                        select new
                        {
                            Biz = grouped.Key.biz,
                            Process = grouped.Key.process,
                            Date = grouped.Key.date,
                            Require = grouped.Key.require,
                            SkillGroup = grouped.Key.skillgroup,
                            TotalEmployees = grouped.Count()
                        };

            return Ok(query.ToList());


        }
        /*[HttpGet]
        [Route("[action]")]
        public IActionResult HeadCountTransitionCOUNT()
        {
            DateTime targetDate = new DateTime(2024, 2, 1);    //DateTime.Now.Month;

            var query = from m in _db.ManpowerRequire
                        join ei in _db.EmployeeInfo on m.Process equals ei.Process
                        where m.Date.Date == targetDate
                        group new { m, ei } by new { m.Biz, m.Process, m.Date, m.Require, m.SkillGroup, m.Shift } into grouped
                        select new
                        {
                            Biz = grouped.Key.Biz,
                            Process = grouped.Key.Process,
                            Date = grouped.Key.Date,
                            Require = grouped.Key.Require,
                            SkillGroup = grouped.Key.SkillGroup,
                            Shift = grouped.Key.Shift,
                            TotalEmployees = grouped.Count()
                        };

            // เพิ่มเงื่อนไขเวลาเพื่อเลือก shift จาก ManpowerRequire
            var targetHour = targetDate.Hour;
            var filteredQuery = query.ToList().Where(item =>
                (item.Shift == "Day" && targetHour >= 7 && targetHour < 19) ||
                (item.Shift == "Night" && (targetHour >= 19 || targetHour < 7))
            );

            return Ok(filteredQuery);
        }*/

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
            var headCountTransitionData = _db.headcounttransition.ToList();

            // วนลูปเพื่อจัดเก็บข้อมูลตามปีและเดือน
            foreach (var item in headCountTransitionData)
            {
                int year = item.datetime.Year;
                string monthName = item.datetime.ToString("MMM");

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
                    EmpID = item.empid,
                    Datetime = item.datetime.ToString("yyyy-MM-ddTHH:mm:ss"),
                    TransType = item.transtype,
                    Biz = item.biz,
                    Process = item.process
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
                IEnumerable <HeadCountbyDiv> HeadCountbyDiv = _db.headcountbydiv;
                return Ok(HeadCountbyDiv);
            }

            [HttpGet]
            [Route("[action]")]
            public IActionResult HeadCountbyWorkGroup()
            {
                IEnumerable <HeadCountbyWorkGroup> HeadCountbyWorkGroup = _db.headcountbyworkgroup;
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