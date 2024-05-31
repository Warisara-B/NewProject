using System;
using System.Net;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Plexus.Client;
using Plexus.Client.src.Academic;
using Plexus.Client.ViewModel.Academic;
using Plexus.Database.Model.Academic;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Academic;
using Plexus.Utility;
using ServiceStack.Text;

namespace Plexus.API.Controllers.PortalControllers
{
    [ApiController]
    [Route(_pathPrefix + "[controller]")]
    public class GradingController : BaseController
    {
        private readonly IGradingManager _gradingManager;

        public GradingController(IGradingManager gradingManager)
        {
            _gradingManager = gradingManager;
        }

        //[HttpPost]
        //public IActionResult ImportScore(IFormFile request)
        //{
        //    var grading = _gradingManager.ImportScore(request, Guid.Empty);

        //    return StatusCode(201, ResponseWrapper.Success(HttpStatusCode.OK, grading));
        //}

        //[HttpPost]
        //public IActionResult Grading([FromBody] List<CreateGradingDTO> request, string activity, string adjustmentValue, string? grade)
        //{
        //    List<CreateGradingViewModel> NewdtoList = new List<CreateGradingViewModel>();
        //    foreach (var data in request)
        //    {
        //        var dto = new CreateGradingViewModel
        //        {
        //            studentCode = data.studentCode,
        //            studentName = data.studentName,
        //            coursesCode = data.coursesCode,
        //            section = data.section,
        //            mitdtermExam = data.mitdtermExam,
        //            mitdtermReport = data.mitdtermReport,
        //            finalExam = data.finalExam,
        //            gradingThresholds = data.gradingThresholds
        //        };
        //        NewdtoList.Add(dto);
        //    }
        //    var grading = _gradingManager.Grading(NewdtoList, activity, adjustmentValue, grade, Guid.Empty);
        //    return StatusCode(201, ResponseWrapper.Success(HttpStatusCode.OK, grading));
        //}

        [HttpPost]
        public IActionResult NewGrading([FromBody] List<CreateGradingDTO> request, int format, string interval ,string grades, string maxScore,string minScore)
        {
            List<CreateGradingViewModel> NewdtoList = new List<CreateGradingViewModel>();
            List<GradingViewModel> grading = new List<GradingViewModel>();
            foreach (var data in request)
            {
                var dto = new CreateGradingViewModel
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
            if (format == 5 || format == 3 || format == 1)
            {
                 grading = _gradingManager.NewGrading(NewdtoList, format, interval , grades, maxScore,minScore, Guid.Empty);
            }
            return StatusCode(201, ResponseWrapper.Success(HttpStatusCode.OK, grading));
        }
    }
}
