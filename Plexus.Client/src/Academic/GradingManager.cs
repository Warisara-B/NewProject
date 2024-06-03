using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OfficeOpenXml;
using Plexus.Client.ViewModel.Academic;
using Plexus.Client.ViewModel.DropDown;
using Plexus.Database.Model;
using Plexus.Database.Model.Academic;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Academic;
using Plexus.Entity.Exception;
using Plexus.Entity.Provider;
using Plexus.Entity.Provider.src.Academic;
using Plexus.Utility.ViewModel;
using ServiceStack;
using ServiceStack.Script;
using ServiceStack.Text;

namespace Plexus.Client.src.Academic
{
    public class GradingManager : IGradingManager
    {
        private readonly IGradingProvider _gradingProvider;

        public GradingManager(IGradingProvider gradingProvider)
        {
            _gradingProvider = gradingProvider;
        }

        public List<GradingViewModel> ImportScore(IFormFile file, Guid userId)
        {

            //var response = new GradingViewModel();
            //var file = request.File;
            var fileName = string.Empty;
            var response = new GradingViewModel
            {
                id = Guid.NewGuid(),
                createdAt = DateTime.UtcNow,
                updatedAt = DateTime.UtcNow
            };
            var responseList = new List<GradingViewModel>();
            if (file != null && file.Length > 0)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        if (worksheet != null)
                        {
                            var dataTable = new DataTable();
                            dataTable.Columns.Add("StudentCode", typeof(string));
                            dataTable.Columns.Add("StudentName", typeof(string));
                            dataTable.Columns.Add("CoursesCode", typeof(string));
                            dataTable.Columns.Add("Section", typeof(string));
                            dataTable.Columns.Add("MitdtermExam", typeof(string));
                            dataTable.Columns.Add("MitdtermReport", typeof(string));
                            dataTable.Columns.Add("FinalExam", typeof(string));

                            for (int row = 3; row <= worksheet.Dimension.End.Row; row++)
                            {
                                var dataRow = dataTable.NewRow();
                                dataRow["StudentCode"] = worksheet.Cells[row, 2].Text;
                                dataRow["StudentName"] = worksheet.Cells[row, 3].Text;
                                dataRow["CoursesCode"] = worksheet.Cells[row, 4].Text;
                                dataRow["Section"] = worksheet.Cells[row, 5].Text;
                                dataRow["MitdtermExam"] = worksheet.Cells[row, 6].Text;
                                dataRow["MitdtermReport"] = worksheet.Cells[row, 7].Text;
                                dataRow["FinalExam"] = worksheet.Cells[row, 8].Text;
                                dataTable.Rows.Add(dataRow);
                            }

                            var dtoList = new List<CreateGradingDTO>();

                            foreach (DataRow row in dataTable.Rows)
                            {
                                var dto = new CreateGradingDTO
                                {
                                    studentCode = row["StudentCode"].ToString(),
                                    studentName = row["StudentName"].ToString(),
                                    coursesCode = row["CoursesCode"].ToString(),
                                    section = row["Section"].ToString(),
                                    mitdtermExam = row["MitdtermExam"].ToString(),
                                    mitdtermReport = row["MitdtermReport"].ToString(),                                 
                                    finalExam = row["FinalExam"].ToString()
                                };
                                dtoList.Add(dto);
                            }
                            var gradingList = _gradingProvider.ImportScore(dtoList, userId.ToString());
                           /*
                            var thresholds = new GradingThresholds();
                            // Adjust thresholds by +0.5
                            //thresholds.AdjustThresholds("+0.5");
                            var gradingResults = _gradingProvider.Grading(dtoList, thresholds);
                            */
                            responseList = MapDTOToViewModel(gradingList);
                        }
                    }
                }
            }
            return responseList;
        }

        public List<GradingViewModel> Grading(List<CreateGradingViewModel> dtoList, string activity, string adjustmentValue, string grade,Guid userId)
        {            
            var responseList = new List<GradingViewModel>();
            var NewdtoList = new List<CreateGradingDTO>();

            foreach (var data in dtoList)
            {
                var dto = new CreateGradingDTO
                {
                    studentCode = data.studentCode,
                    studentName = data.studentName,
                    coursesCode = data.coursesCode,
                    section = data.section,
                    mitdtermExam = data.mitdtermExam,
                    mitdtermReport = data.mitdtermReport,
                    finalExam = data.finalExam,
                    gradingThresholds = data.gradingThresholds
                };
                NewdtoList.Add(dto);
            }
            var gradingResults = _gradingProvider.Grading(NewdtoList, activity, adjustmentValue, grade);
            responseList = MapDTOToViewModel(gradingResults);
            return responseList;
            /*
            var NewdtoList = new List<CreateGradingDTO>();


            foreach (var data in dtoList)
            {
                var dto = new CreateGradingDTO
                {
                    StudentCode = data.StudentCode,
                    StudentName = data.StudentName,
                    CoursesCode = data.CoursesCode,
                    Section = data.Section,
                    MitdtermExam = data.MitdtermExam,
                    MitdtermReport = data.MitdtermReport,
                    FinalExam = data.FinalExam
                };
                NewdtoList.Add(dto);
            }
            var thresholds = new GradingThresholds();
            // Adjust thresholds by +0.5
            //thresholds.AdjustThresholds("+0.5");
            var gradingResults = _gradingProvider.Grading(NewdtoList, thresholds);
            responseList = MapDTOToViewModel(gradingResults);
          

            return responseList;  
            */
        }


        public List<GradingViewModel> NewGrading(List<CreateGradingViewModel> dtoList, int format, string interval, string grades, string maxScore, string minScore, string rangeGrade, string median, string llf, Guid userId)
        {
            var responseList = new List<GradingViewModel>();
            var NewdtoList = new List<CreateGradingDTO>();         
            foreach (var data in dtoList)
            {
                var dto = new CreateGradingDTO
                {
                    studentCode = data.studentCode,
                    studentName = data.studentName,
                    coursesCode = data.coursesCode,
                    section = data.section,
                    mitdtermExam = data.mitdtermExam,
                    mitdtermReport = data.mitdtermReport,
                    finalExam = data.finalExam,
                    gradingThresholds = data.gradingThresholds
                };
                NewdtoList.Add(dto);
            }

            var gradingResults = _gradingProvider.NewGrading(NewdtoList, format, interval, grades, maxScore,minScore, rangeGrade, median, llf);
            responseList = MapDTOToViewModel(gradingResults);
            return responseList;
        }
        public static List<GradingViewModel> MapDTOToViewModel(List<GradingDTO> dtoList)
        {
            return dtoList.Select(dto => new GradingViewModel
            {
                id = dto.id,
                studentCode = dto.studentCode,
                studentName = dto.studentName,
                coursesCode = dto.coursesCode,
                section = dto.section,               
                mitdtermExam = dto.mitdtermExam,
                mitdtermReport = dto.mitdtermReport,
                finalExam = dto.finalExam,
                final = dto.final,
                mitdterm = dto.mitdterm,
                totalScore = dto.totalScore,
                grade = dto.grade,
                createdAt = dto.createdAt,
                updatedAt = dto.updatedAt,
                gradingThresholds = dto.gradingThresholds
            }).ToList();
        }
        
    }

}
