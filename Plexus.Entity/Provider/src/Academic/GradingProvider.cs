using Plexus.Database.Model.Academic;
using Plexus.Database;
using Plexus.Entity.DTO.Academic;
using Plexus.Entity.DTO;
using Plexus.Entity.Exception;
using Plexus.Utility.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Plexus.Database.Model.Registration;
using ServiceStack.Web;

namespace Plexus.Entity.Provider.src.Academic
{

    public class GradingProvider : IGradingProvider
    {
        private readonly DatabaseContext _dbContext;

        public GradingProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<GradingDTO> ImportScore(List<CreateGradingDTO> dtoList, string requester)
        {
            return dtoList.Select(dto => new GradingDTO
            {
                id = Guid.NewGuid(),
                studentCode = dto.studentCode,
                studentName = dto.studentName,
                coursesCode = dto.coursesCode,
                section = dto.section,
                mitdtermExam = dto.mitdtermExam,
                mitdtermReport = dto.mitdtermReport,
                finalExam = dto.finalExam,
                createdAt = DateTime.UtcNow,
                updatedAt = DateTime.UtcNow
            }).ToList();


        }

        public List<GradingDTO> Grading(List<CreateGradingDTO> request, string activity, string adjustmentValue, string _grade)
        {
            List<GradingDTO> newlist = new List<GradingDTO>();
            var gradingThresholds = new GradingThresholds();
            if (activity == "manually")
            {
                gradingThresholds.AdjustThreshold(_grade, adjustmentValue);
            }
            else if (activity == "range")
            {
                gradingThresholds.AdjustThresholds(adjustmentValue);
            }
            newlist = request.Select(dto =>
            {
                var mitdtermScore = Convert.ToDecimal((Convert.ToDouble(dto.mitdtermExam) + Convert.ToDouble(dto.mitdtermReport)) / 110 * 40);
                var finalScore = Convert.ToDecimal((Convert.ToDouble(dto.finalExam)) / 120 * 60);
                var totalScore = mitdtermScore + finalScore;
                var thresholds = gradingThresholds;
                var (grade, color) = CalculateGrade(totalScore, thresholds, _grade);
                return new GradingDTO
                {
                    id = Guid.NewGuid(),
                    studentCode = dto.studentCode,
                    studentName = dto.studentName,
                    coursesCode = dto.coursesCode,
                    section = dto.section,
                    mitdtermExam = dto.mitdtermExam,
                    mitdtermReport = dto.mitdtermReport,
                    finalExam = dto.finalExam,
                    final = finalScore.ToString("00.00"),
                    mitdterm = mitdtermScore.ToString("00.00"),
                    totalScore = totalScore.ToString("00.00"),
                    grade = grade,
                    color = color,
                    gradingThresholds = thresholds,
                    createdAt = DateTime.UtcNow,
                    updatedAt = DateTime.UtcNow
                };
            }).ToList();
            return newlist;
        }

        public List<GradingDTO> NewGrading(List<CreateGradingDTO> request, int format, string interval, string grades, string maxScore, string minScore, string rangeGrade, string median, string llf)
        {
            List<GradingDTO> newlist = new List<GradingDTO>();
            var gradingThresholds = new GradingThresholds();

            newlist = request.Select(dto =>
            {
                var mitdtermScore = Convert.ToDecimal((Convert.ToDouble(dto.mitdtermExam) + Convert.ToDouble(dto.mitdtermReport)) / 110 * 40);
                var finalScore = Convert.ToDecimal((Convert.ToDouble(dto.finalExam)) / 120 * 60);
                var totalScore = mitdtermScore + finalScore;
                var thresholds = gradingThresholds;
                if (format == 1)
                {
                    thresholds = CalculateThresholds(Convert.ToDecimal(interval), grades, Convert.ToDecimal(maxScore));
                }
                else if (format == 2)
                {
                    thresholds = CalculateThresholds(Convert.ToDecimal(interval), grades, Convert.ToDecimal(maxScore), Convert.ToDecimal(rangeGrade));
                }
                else if (format == 3)
                {
                    thresholds = CalculateThresholds(50, grades);
                }
                else if (format == 4)
                {
                    thresholds = CalculateThresholds(Convert.ToDecimal(interval), grades, Convert.ToDecimal(maxScore), Convert.ToDecimal(rangeGrade), Convert.ToDecimal(median), Convert.ToDecimal(llf));
                }
                else if (format == 5)
                {
                    thresholds = CalculateThresholds(Convert.ToDecimal(minScore));
                }
                var (grade, color) = CalculateGrade(totalScore, thresholds, grades);
                return new GradingDTO
                {
                    id = Guid.NewGuid(),
                    studentCode = dto.studentCode,
                    studentName = dto.studentName,
                    coursesCode = dto.coursesCode,
                    section = dto.section,
                    mitdtermExam = dto.mitdtermExam,
                    mitdtermReport = dto.mitdtermReport,
                    finalExam = dto.finalExam,
                    final = finalScore.ToString("00.00"),
                    mitdterm = mitdtermScore.ToString("00.00"),
                    totalScore = totalScore.ToString("00.00"),
                    grade = grade,
                    color = color,
                    gradingThresholds = thresholds,
                    createdAt = DateTime.UtcNow,
                    updatedAt = DateTime.UtcNow
                };
            }).ToList();
            return newlist;
        }

        public static (string Grade, string Color) CalculateGrade(decimal totalScore, GradingThresholds thresholds, string grades)
        {
            string[] gradesArr = grades.Split(',');
            if (totalScore >= thresholds.A && thresholds.A != 0) return ("A", "#27AE60");      // Green
            if (totalScore >= thresholds.BPlus && thresholds.BPlus != 0) return ("B+", "#2ECC71");  // LightGreen
            if (totalScore >= thresholds.B && thresholds.B != 0) return ("B", "#F1C40F");       // YellowGreen
            if (totalScore >= thresholds.CPlus && thresholds.CPlus != 0) return ("C+", "#F39C12");  // Yellow
            if (totalScore >= thresholds.C && thresholds.C != 0) return ("C", "#E67E22");       // Orange
            if (totalScore >= thresholds.DPlus && thresholds.DPlus != 0) return ("D+", "#E74C3C");  // OrangeRed
            if (totalScore >= thresholds.D && thresholds.D != 0) return ("D", "#C0392B");       // Red           

            // หาเกรดที่ต่ำที่สุดใน gradesArr
            string lowestGrade = gradesArr.OrderByDescending(g => g).First();
            string color;

            switch (lowestGrade)
            {
                case "A":
                    color = "#27AE60"; // Green
                    break;
                case "B+":
                    color = "#2ECC71"; // LightGreen
                    break;
                case "B":
                    color = "#F1C40F"; // YellowGreen
                    break;
                case "C+":
                    color = "#F39C12"; // Yellow
                    break;
                case "C":
                    color = "#E67E22"; // Orange
                    break;
                case "D+":
                    color = "#E74C3C"; // OrangeRed
                    break;
                case "D":
                    color = "#C0392B"; // Red
                    break;
                case "F":
                default:
                    color = "#8B0000"; // DarkRed
                    break;
            }

            return (lowestGrade, color);
        }

        //public static (string Grade, string Color) CalculateGrade(decimal totalScore, GradingThresholds thresholds ,string grades)
        //{
        //    string[] gradesArr = grades.Split(',');
        //    if (totalScore >= thresholds.A && thresholds.A != 0) return ("A", "#27AE60");      // Green
        //    if (totalScore >= thresholds.BPlus && thresholds.BPlus != 0) return ("B+", "#2ECC71");  // LightGreen
        //    if (totalScore >= thresholds.B && thresholds.B != 0) return ("B", "#F1C40F");       // YellowGreen
        //    if (totalScore >= thresholds.CPlus && thresholds.CPlus != 0) return ("C+", "#F39C12");  // Yellow
        //    if (totalScore >= thresholds.C && thresholds.C != 0) return ("C", "#E67E22");       // Orange
        //    if (totalScore >= thresholds.DPlus && thresholds.DPlus != 0) return ("D+", "#E74C3C");  // OrangeRed
        //    if (totalScore >= thresholds.D && thresholds.D != 0) return ("D", "#C0392B");       // Red           
        //    return ("F", "#8B0000");                                       // DarkRed
        //}

        public GradingThresholds CalculateThresholds(decimal minScore)
        {
            var gradingThresholds = new GradingThresholds();
            // กำหนดค่าเริ่มต้นและค่าจบ
            decimal start = minScore;
            decimal end = 100;
            // คำนวณช่วงระหว่างค่าทั้งสอง
            decimal delta = end - start;
            // แบ่งช่วงออกเป็น 7 ส่วน
            decimal interval = delta / 7;
            // คำนวณและแสดงค่าของแต่ละจุด
            decimal a = Math.Round(end, 2);
            decimal bPlus = Math.Round(end - interval, 2);
            decimal b = Math.Round(end - 2 * interval, 2);
            decimal cPlus = Math.Round(end - 3 * interval, 2);
            decimal c = Math.Round(end - 4 * interval, 2);
            decimal dPlus = Math.Round(end - 5 * interval, 2);
            decimal d = Math.Round(start, 2);

            gradingThresholds.A = a;
            gradingThresholds.BPlus = bPlus;
            gradingThresholds.B = b;
            gradingThresholds.CPlus = cPlus;
            gradingThresholds.C = c;
            gradingThresholds.DPlus = dPlus;
            gradingThresholds.D = d;
            return gradingThresholds;
        }

        public GradingThresholds CalculateThresholds(decimal minScore, string grades)
        {
            var gradingThresholds = new GradingThresholds();
            // กำหนดค่าเริ่มต้นและค่าจบ
            decimal start = minScore;
            decimal end = 100;
            // คำนวณช่วงระหว่างค่าทั้งสอง
            decimal delta = end - start;
            // แบ่งช่วงตามจำนวนเกรดที่เลือก
            string[] gradesArr = grades.Split(',');
            int gradeCount = gradesArr.Length;
            decimal interval = delta / (gradeCount - 1);

            // คำนวณและแสดงค่าของแต่ละจุด
            var gradeValues = new Dictionary<string, decimal>();
            for (int i = 0; i < gradeCount; i++)
            {
                gradeValues[gradesArr[i].Trim()] = Math.Round(end - i * interval, 2);
            }

            // ตั้งค่าเกรดใน gradingThresholds
            foreach (var grade in gradesArr)
            {
                switch (grade.Trim())
                {
                    case "A":
                        gradingThresholds.A = gradeValues["A"];
                        break;
                    case "BPlus":
                        gradingThresholds.BPlus = gradeValues["BPlus"];
                        break;
                    case "B":
                        gradingThresholds.B = gradeValues["B"];
                        break;
                    case "CPlus":
                        gradingThresholds.CPlus = gradeValues["CPlus"];
                        break;
                    case "C":
                        gradingThresholds.C = gradeValues["C"];
                        break;
                    case "DPlus":
                        gradingThresholds.DPlus = gradeValues["DPlus"];
                        break;
                    case "D":
                        gradingThresholds.D = gradeValues["D"];
                        break;
                    default:
                        // เกรดที่ไม่ถูกต้องจะไม่ถูกกำหนดค่าใด ๆ
                        break;
                }
            }

            // ตัดเกรดที่ไม่ได้ถูกกำหนดค่าออกไป
            if (!gradesArr.Contains("A"))
            {
                gradingThresholds.BPlus = 0;
            }
            if (!gradesArr.Contains("BPlus"))
            {
                gradingThresholds.BPlus = 0;
            }
            if (!gradesArr.Contains("C"))
            {
                gradingThresholds.CPlus = 0;
            }
            if (!gradesArr.Contains("CPlus"))
            {
                gradingThresholds.CPlus = 0;
            }
            if (!gradesArr.Contains("D"))
            {
                gradingThresholds.DPlus = 0;
            }
            if (!gradesArr.Contains("DPlus"))
            {
                gradingThresholds.DPlus = 0;
            }

            return gradingThresholds;
        }

        public GradingThresholds CalculateThresholds(decimal interval, string grades, decimal maxScore)
        {
            var gradingThresholds = new GradingThresholds();
            // กำหนดค่าเริ่มต้นและค่าจบ
            decimal start = 0;
            decimal end = maxScore;
            // คำนวณช่วงระหว่างค่าทั้งสอง
            decimal delta = end - start;
            // แบ่งช่วงตามจำนวนเกรดที่เลือก
            string[] gradesArr = grades.Split(',');
            int gradeCount = gradesArr.Length;
            // คำนวณและแสดงค่าของแต่ละจุด
            var gradeValues = new Dictionary<string, decimal>();
            for (int i = 0; i < gradeCount; i++)
            {
                gradeValues[gradesArr[i].Trim()] = Math.Round(end - i * interval, 2);
            }

            // ตั้งค่าเริ่มต้นทั้งหมดเป็น 0 โดยใช้ Reflection
            foreach (var prop in typeof(GradingThresholds).GetProperties())
            {
                prop.SetValue(gradingThresholds, 0m);
            }
            // ตั้งค่าเกรดใน gradingThresholds
            foreach (var grade in gradesArr)
            {
                switch (grade.Trim())
                {
                    case "A":
                        gradingThresholds.A = gradeValues["A"];
                        break;
                    case "BPlus":
                        gradingThresholds.BPlus = gradeValues["BPlus"];
                        break;
                    case "B":
                        gradingThresholds.B = gradeValues["B"];
                        break;
                    case "CPlus":
                        gradingThresholds.CPlus = gradeValues["CPlus"];
                        break;
                    case "C":
                        gradingThresholds.C = gradeValues["C"];
                        break;
                    case "DPlus":
                        gradingThresholds.DPlus = gradeValues["DPlus"];
                        break;
                    case "D":
                        gradingThresholds.D = gradeValues["D"];
                        break;

                    default:
                        // เกรดที่ไม่ถูกต้องจะไม่ถูกกำหนดค่าใด ๆ
                        break;
                }
            }

            // ตัดเกรดที่ไม่ได้ถูกกำหนดค่าออกไป
            if (!gradesArr.Contains("A")) gradingThresholds.A = 0;
            if (!gradesArr.Contains("BPlus")) gradingThresholds.BPlus = 0;
            if (!gradesArr.Contains("B")) gradingThresholds.B = 0;
            if (!gradesArr.Contains("CPlus")) gradingThresholds.CPlus = 0;
            if (!gradesArr.Contains("C")) gradingThresholds.C = 0;
            if (!gradesArr.Contains("DPlus")) gradingThresholds.DPlus = 0;
            if (!gradesArr.Contains("D")) gradingThresholds.D = 0;


            return gradingThresholds;
        }

        public GradingThresholds CalculateThresholds(decimal interval, string grades, decimal maxScore, decimal rangeGrade)
        {
            var gradingThresholds = new GradingThresholds();
            // กำหนดค่าเริ่มต้นและค่าจบ
            decimal start = 0;
            decimal end = maxScore;
            // คำนวณช่วงระหว่างค่าทั้งสอง
            decimal delta = end - start;
            // แบ่งช่วงตามจำนวนเกรดที่เลือก
            string[] gradesArr = grades.Split(',');
            int gradeCount = gradesArr.Length;
            // คำนวณและแสดงค่าของแต่ละจุด
            var gradeValues = new Dictionary<string, decimal>();
            //คำนวณปรับความก้างของช่วงเกรด
            interval = interval * rangeGrade;

            for (int i = 0; i < gradeCount; i++)
            {
                gradeValues[gradesArr[i].Trim()] = Math.Round(end - i * interval, 2);
            }

            // ตั้งค่าเริ่มต้นทั้งหมดเป็น 0 โดยใช้ Reflection
            foreach (var prop in typeof(GradingThresholds).GetProperties())
            {
                prop.SetValue(gradingThresholds, 0m);
            }
            // ตั้งค่าเกรดใน gradingThresholds
            foreach (var grade in gradesArr)
            {
                switch (grade.Trim())
                {
                    case "A":
                        gradingThresholds.A = gradeValues["A"];
                        break;
                    case "BPlus":
                        gradingThresholds.BPlus = gradeValues["BPlus"];
                        break;
                    case "B":
                        gradingThresholds.B = gradeValues["B"];
                        break;
                    case "CPlus":
                        gradingThresholds.CPlus = gradeValues["CPlus"];
                        break;
                    case "C":
                        gradingThresholds.C = gradeValues["C"];
                        break;
                    case "DPlus":
                        gradingThresholds.DPlus = gradeValues["DPlus"];
                        break;
                    case "D":
                        gradingThresholds.D = gradeValues["D"];
                        break;

                    default:
                        // เกรดที่ไม่ถูกต้องจะไม่ถูกกำหนดค่าใด ๆ
                        break;
                }
            }

            // ตัดเกรดที่ไม่ได้ถูกกำหนดค่าออกไป
            if (!gradesArr.Contains("A")) gradingThresholds.A = 0;
            if (!gradesArr.Contains("BPlus")) gradingThresholds.BPlus = 0;
            if (!gradesArr.Contains("B")) gradingThresholds.B = 0;
            if (!gradesArr.Contains("CPlus")) gradingThresholds.CPlus = 0;
            if (!gradesArr.Contains("C")) gradingThresholds.C = 0;
            if (!gradesArr.Contains("DPlus")) gradingThresholds.DPlus = 0;
            if (!gradesArr.Contains("D")) gradingThresholds.D = 0;


            return gradingThresholds;
        }

        public GradingThresholds CalculateThresholds(decimal interval, string grades, decimal maxScore, decimal rangeGrade, decimal median, decimal llf)
        {
            var gradingThresholds = new GradingThresholds();
            // กำหนดค่าเริ่มต้นและค่าจบ
            decimal start = 0;
            decimal end = maxScore;
            // คำนวณช่วงระหว่างค่าทั้งสอง
            decimal delta = end - start;
            // แบ่งช่วงตามจำนวนเกรดที่เลือก
            string[] gradesArr = grades.Split(',');
            int gradeCount = gradesArr.Length;
            // คำนวณและแสดงค่าของแต่ละจุด
            var gradeValues = new Dictionary<string, decimal>();
            //คำนวณปรับความก้างของช่วงเกรด
            interval = interval * rangeGrade;
            decimal cruScore = 0;

            // ตั้งค่าเริ่มต้นทั้งหมดเป็น 0 โดยใช้ Reflection
            foreach (var prop in typeof(GradingThresholds).GetProperties())
            {
                prop.SetValue(gradingThresholds, 0m);
            }
            // ตั้งค่าเกรดใน gradingThresholds
            if (gradeCount == 5)
            {
                for (int i = 0; i < gradeCount; i++)
                {
                    if (i == 0)
                    {
                        gradeValues[gradesArr[i].Trim()] = Math.Round(median + (interval * llf), 2);
                        cruScore = Math.Round(median + (interval * llf), 2);
                    }
                    else
                    {
                        gradeValues[gradesArr[i].Trim()] = Math.Round(cruScore - interval, 2);
                        cruScore = Math.Round(cruScore - interval, 2);
                    }
                }
            }
            else if (gradeCount == 8)
            {
                for (int i = 0; i < gradeCount; i++)
                {
                    if (i == 0)
                    {
                        gradeValues[gradesArr[i].Trim()] = Math.Round(median + (interval * llf), 2);
                        cruScore = Math.Round(median + (interval * llf), 2);
                    }
                    else
                    {
                        gradeValues[gradesArr[i].Trim()] = Math.Round(cruScore - (interval / 2), 2);
                        cruScore = Math.Round(cruScore - (interval / 2), 2);
                    }
                }
            }
            else
            {
                throw new GradeException.IncorrectGrade();
            }

            foreach (var grade in gradesArr)
            {
                switch (grade.Trim())
                {
                    case "A":
                        gradingThresholds.A = gradeValues["A"];
                        break;
                    case "BPlus":
                        gradingThresholds.BPlus = gradeValues["BPlus"];
                        break;
                    case "B":
                        gradingThresholds.B = gradeValues["B"];
                        break;
                    case "CPlus":
                        gradingThresholds.CPlus = gradeValues["CPlus"];
                        break;
                    case "C":
                        gradingThresholds.C = gradeValues["C"];
                        break;
                    case "DPlus":
                        gradingThresholds.DPlus = gradeValues["DPlus"];
                        break;
                    case "D":
                        gradingThresholds.D = gradeValues["D"];
                        break;

                    default:
                        // เกรดที่ไม่ถูกต้องจะไม่ถูกกำหนดค่าใด ๆ
                        break;
                }
            }
            // ตัดเกรดที่ไม่ได้ถูกกำหนดค่าออกไป
            if (!gradesArr.Contains("A")) gradingThresholds.A = 0;
            if (!gradesArr.Contains("BPlus")) gradingThresholds.BPlus = 0;
            if (!gradesArr.Contains("B")) gradingThresholds.B = 0;
            if (!gradesArr.Contains("CPlus")) gradingThresholds.CPlus = 0;
            if (!gradesArr.Contains("C")) gradingThresholds.C = 0;
            if (!gradesArr.Contains("DPlus")) gradingThresholds.DPlus = 0;
            if (!gradesArr.Contains("D")) gradingThresholds.D = 0;

            return gradingThresholds;
        }
        //public GradingThresholds CalculateThresholds(decimal interval, string grades,decimal maxScore)
        //{
        //    var gradingThresholds = new GradingThresholds();
        //    // กำหนดค่าเริ่มต้นและค่าจบ
        //    decimal start = 0;
        //    decimal end = maxScore;
        //    // คำนวณช่วงระหว่างค่าทั้งสอง
        //    decimal delta = end - start;
        //    // แบ่งช่วงตามจำนวนเกรดที่เลือก
        //    string[] gradesArr = grades.Split(',');
        //    int gradeCount = gradesArr.Length;
        //    //decimal interval = delta / (gradeCount - 1);

        //    // คำนวณและแสดงค่าของแต่ละจุด
        //    var gradeValues = new Dictionary<string, decimal>();
        //    for (int i = 0; i < gradeCount; i++)
        //    {
        //        gradeValues[gradesArr[i].Trim()] = Math.Round(end - i * interval, 2);
        //    }

        //    // ตั้งค่าเกรดใน gradingThresholds
        //    foreach (var grade in gradesArr)
        //    {
        //        switch (grade.Trim())
        //        {
        //            case "A":
        //                gradingThresholds.A = gradeValues["A"];
        //                break;
        //            case "BPlus":
        //                gradingThresholds.BPlus = gradeValues["BPlus"];
        //                break;
        //            case "B":
        //                gradingThresholds.B = gradeValues["B"];
        //                break;
        //            case "CPlus":
        //                gradingThresholds.CPlus = gradeValues["CPlus"];
        //                break;
        //            case "C":
        //                gradingThresholds.C = gradeValues["C"];
        //                break;
        //            case "DPlus":
        //                gradingThresholds.DPlus = gradeValues["DPlus"];
        //                break;
        //            case "D":
        //                gradingThresholds.D = gradeValues["D"];
        //                break;
        //            default:
        //                // เกรดที่ไม่ถูกต้องจะไม่ถูกกำหนดค่าใด ๆ
        //                break;
        //        }
        //    }

        //    // ตัดเกรดที่ไม่ได้ถูกกำหนดค่าออกไป
        //    if (!gradesArr.Contains("A"))
        //    {
        //        gradingThresholds.BPlus = 0;
        //    }
        //    if (!gradesArr.Contains("BPlus"))
        //    {
        //        gradingThresholds.BPlus = 0;
        //    }
        //    if (!gradesArr.Contains("C"))
        //    {
        //        gradingThresholds.CPlus = 0;
        //    }
        //    if (!gradesArr.Contains("CPlus"))
        //    {
        //        gradingThresholds.CPlus = 0;
        //    }
        //    if (!gradesArr.Contains("D"))
        //    {
        //        gradingThresholds.DPlus = 0;
        //    }
        //    if (!gradesArr.Contains("DPlus"))
        //    {
        //        gradingThresholds.DPlus = 0;
        //    }

        //    return gradingThresholds;
        //}

        public IEnumerable<GradingDTO> GetByStudentCode(string studentCode)
        {
            //var gradesTemplate = _dbContext.GradeTemplates.AsNoTracking()
            //                              .Where(x => x.Name == studentCode)
            //                              .ToList();

            //var response = (from grade in gradesTemplate
            //                orderby grade.Name descending, grade.Name
            //                select MapModelToDTO(grade))
            //               .ToList();
            var response = new List<GradingDTO>();
            return response;
        }
        
    }
}
