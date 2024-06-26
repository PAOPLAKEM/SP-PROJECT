﻿    using Api.Data;
    using Api.Models;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;




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

         /*[HttpGet]
         [Route("[action]")]
         public IActionResult get_Repalce()
         {
            IEnumerable <Replacement> Replace = _db.Replacement;
            return Ok(Replace);
         }*/

        [HttpGet]
        [Route("[action]")]
        public IActionResult REALTIME_ATTEN()
        {
            DateTime targetDate = DateTime.Now;

            var query = from m in _db.Manpower_Plan
                        join ei in _db.EmployeeInfo on m.EMPID equals ei.EmpId
                        where m.Date.Date == targetDate.Date && m.Attendance == "O"
                        select new
                        {
                            ManpowerPlan = m,
                            EmployeeInfo = ei
                        };

            var combinedDataList = new List<object>();

            foreach (var item in query.ToList())
            {
                var faceScanLog = _db.FaceScanLog.Where(s => s.EMPLOYEE_ID == item.ManpowerPlan.EMPID)
                                                  .OrderByDescending(s => s.Datetime)
                                                  .FirstOrDefault();
                var gateLog = _db.GateLog.Where(g => g.EmpID == item.ManpowerPlan.EMPID)
                                          .OrderByDescending(g => g.Datetime)
                                          .FirstOrDefault();

                // ตรวจสอบเงื่อนไขเวลาเพื่อเลือก shift จาก ManpowerPlan
                var shift = item.ManpowerPlan.Shift;
                if (shift == "Day" && targetDate.Hour >= 7 && targetDate.Hour < 19)
                {
                    var combinedData = new
                    {
                        ManpowerPlan = item.ManpowerPlan,
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
        public IActionResult CombinedData()
        {
            DateTime targetDate = DateTime.Now;

            var query = from m in _db.Manpower_Plan
                        join ei in _db.EmployeeInfo on m.EMPID equals ei.EmpId
                        where m.Date.Date == targetDate.Date && m.Attendance == "O"
                        select new
                        {
                            ManpowerPlan = m,
                            EmployeeInfo = ei
                        };

            var combinedDataList = new List<object>();

            foreach (var item in query.ToList())
            {
                var ojtSkills = _db.OJT_InspectionSkill.Where(o => o.EmpID == item.EmployeeInfo.EmpId).ToList();
                var faceScanLog = _db.FaceScanLog.Where(s => s.EMPLOYEE_ID == item.ManpowerPlan.EMPID)
                                                  .OrderByDescending(s => s.Datetime)
                                                  .FirstOrDefault();
                var gateLog = _db.GateLog.Where(g => g.EmpID == item.ManpowerPlan.EMPID)
                                          .OrderByDescending(g => g.Datetime)
                                          .FirstOrDefault();

                // ตรวจสอบเงื่อนไขเวลาเพื่อเลือก shift จาก ManpowerPlan
                var shift = item.ManpowerPlan.Shift;
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

        /*        [HttpGet]
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

                }*/



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


        [HttpGet]
        [Route("[action]")]
        public IActionResult Replacement()
        {
            DateTime targetDate = DateTime.Now;

            IEnumerable<Replacement> replacements;

            // เช็คเงื่อนไข Shift เป็น "Day" และตรงกับเวลาที่กำหนด
            if (targetDate.Hour >= 7 && targetDate.Hour < 19)
            {
                replacements = _db.Replacement
                    .Where(r => r.Date.Date == targetDate.Date && r.Shift == "Day")
                    .ToList();
            }
            // เช็คเงื่อนไข Shift เป็น "Night" และตรงกับเวลาที่กำหนด
            else if (targetDate.Hour >= 19 || targetDate.Hour < 7)
            {
                replacements = _db.Replacement
                    .Where(r => r.Date.Date == targetDate.Date && r.Shift == "Night")
                    .ToList();
            }
            else
            {
                replacements = Enumerable.Empty<Replacement>(); // ไม่มีข้อมูลใน shift ที่กำหนด
            }

            return Ok(replacements);
        }


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
            DateTime targetDate = DateTime.Now;

            var query = from m in _db.ManpowerRequire
                        where m.Date.Date == targetDate.Date // เปรียบเทียบแค่วันที่
                        join ojt in _db.OJT_InspectionSkill on new { m.Biz, m.Process, m.SkillGroup } equals new { ojt.Biz, ojt.Process, ojt.SkillGroup }
                        join mp in _db.Manpower_Plan on ojt.EmpID equals mp.EMPID
                        where mp.Date.Date == targetDate.Date // เปรียบเทียบแค่วันที่
                        group new { mp, ojt } by new { ojt.Biz, ojt.Process, ojt.SkillGroup, m.Require } into grouped
                        select new
                        {
                            Biz = grouped.Key.Biz,
                            Process = grouped.Key.Process,
                            SkillGroup = grouped.Key.SkillGroup,
                            Day = grouped.Count(x => x.mp.Shift == "Day"),
                            Night = grouped.Count(x => x.mp.Shift == "Night"),
                            Require = grouped.Key.Require
                        };

            return Ok(query.ToList());

        }



        [HttpGet]
        [Route("[action]")]
        public IActionResult NOT_INCLEAN()
        {
            DateTime targetDate = DateTime.Now;

            var query = from m in _db.Manpower_Plan
                        join ei in _db.EmployeeInfo on m.EMPID equals ei.EmpId
                        where m.Date.Date == targetDate.Date &&
                              ((targetDate.Hour >= 7 && targetDate.Hour < 19 && m.Shift == "Day") ||
                               (targetDate.Hour >= 19 || targetDate.Hour < 7 && m.Shift == "Night"))
                        select new
                        {
                            ManpowerPlan = m,
                            EmployeeInfo = ei
                        };

            var combinedDataList = new List<object>();

            foreach (var item in query.ToList()) // ใช้ .ToList() ที่นี่
            {
                var faceScanLog = _db.FaceScanLog.Where(s => s.Datetime.Date == targetDate.Date && s.EMPLOYEE_ID == item.ManpowerPlan.EMPID)
                                                  .OrderByDescending(s => s.Datetime)
                                                  .FirstOrDefault();

                var gateLog = _db.GateLog.Where(g => g.Datetime.Date == targetDate.Date && g.EmpID == item.ManpowerPlan.EMPID)
                                          .OrderByDescending(g => g.Datetime)
                                          .FirstOrDefault();
                var transactionDataList = _db.face_recog_transaction
                    .Where(td => td.EmpID == item.EmployeeInfo.EmpId)
                    .Join(_db.face_recog_camera,
                          transaction => transaction.CameraNO_id,
                          logcam => logcam.CameraNo,
                          (transaction, logcam) => new
                          {
                              transaction.EmpID,
                              transaction.Name,
                              transaction.DateTime,
                              logcam.Location, 
                              item.EmployeeInfo.Biz,
                              item.EmployeeInfo.Process
                          })
                    .ToList();

                // เพิ่มเงื่อนไขเชื่อมโยงเพื่อตรวจสอบสถานะของ faceScanLog และ gateLog
                if ((faceScanLog != null && faceScanLog.Status == "IN") && (gateLog == null || gateLog.Status == "OUT"))
                {
                    // Concatenate the transaction data directly to the combinedDataList
                    combinedDataList.AddRange(transactionDataList);
                }
            }

            return Ok(combinedDataList);
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
        [HttpPost]
        [Route("api/replacement")] 
        public async Task<IActionResult> PostReplacement([FromBody] Replacement replacement)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                
                replacement.Date = DateTime.Now;

                
                _db.Replacement.Add(replacement); 

               
                await _db.SaveChangesAsync(); 

                return CreatedAtAction("GetReplacement", new { id = replacement.EmpID }, replacement);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }


}