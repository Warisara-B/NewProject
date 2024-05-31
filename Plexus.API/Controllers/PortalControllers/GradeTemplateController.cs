using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Plexus.Client;
using Plexus.Client.ViewModel.Academic;
using Plexus.Entity.DTO;
using Plexus.Utility;

namespace Plexus.API.Controllers
{
    [ApiController]
    [Route(_pathPrefix + "[controller]")]
    public class GradeTemplateController : BaseController
    {
        private readonly IGradeTemplateManager _gradeTemplateManager;

        public GradeTemplateController(IGradeTemplateManager gradeTemplateManager)
        {
            _gradeTemplateManager = gradeTemplateManager;
        }

        [HttpPost]
        public IActionResult Create(CreateGradeTemplateViewModel request)
        {
            var grade = _gradeTemplateManager.Create(request, Guid.Empty);

            return StatusCode(201, ResponseWrapper.Success(HttpStatusCode.Created, grade));
        }


        [HttpGet("search")]
        public IActionResult Search([FromQuery] SearchGradeTemplateViewModel parameters, int page = 1, int pageSize = 25)
        {
            var pagedGrades = _gradeTemplateManager.Search(parameters, page, pageSize);

            if (pagedGrades is null || !pagedGrades.Items.Any())
            {
                return StatusCode(204);
            }

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, pagedGrades));
        }


        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var grade = _gradeTemplateManager.GetById(id);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, grade));
        }

        [HttpPut]
        public IActionResult Update(GradeTemplateViewModel request)
        {
            var grade = _gradeTemplateManager.Update(request, Guid.Empty);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, grade));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _gradeTemplateManager.Delete(id);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK));
        }

    }
}
