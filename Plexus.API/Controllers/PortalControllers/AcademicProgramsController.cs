using System.Net;
using Microsoft.AspNetCore.Mvc;
using Plexus.Client;
using Plexus.Client.ViewModel.Academic;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Utility;

namespace Plexus.API.Controllers
{
    [ApiController]
    [Route(_pathPrefix + "[controller]")]
    public class AcademicProgramsController : BaseController
    {
        private readonly IAcademicProgramManager _academicProgramManager;

        public AcademicProgramsController(IAcademicProgramManager academicProgramManager)
        {
            _academicProgramManager = academicProgramManager;
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateAcademicProgramViewModel request)
        {
            var academicProgram = _academicProgramManager.Create(request, Guid.Empty);

            return StatusCode(201, ResponseWrapper.Success(HttpStatusCode.Created, academicProgram));
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] SearchAcademicProgramCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var pagedAcademicProgram = _academicProgramManager.Search(parameters, page, pageSize);

            if (pagedAcademicProgram is null || !pagedAcademicProgram.Items.Any())
            {
                return NoContent();
            }

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, pagedAcademicProgram));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var academicProgram = _academicProgramManager.GetById(id);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, academicProgram));
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] CreateAcademicProgramViewModel request)
        {
            var academicProgram = _academicProgramManager.Update(id, request, Guid.Empty);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, academicProgram));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _academicProgramManager.Delete(id);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK));
        }
    }
}