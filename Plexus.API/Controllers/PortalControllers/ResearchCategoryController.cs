using System.Net;
using Microsoft.AspNetCore.Mvc;
using Plexus.Client;
using Plexus.Client.ViewModel.Research;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Utility;

namespace Plexus.API.Controllers
{
    [ApiController]
    [Route(_pathPrefix + "[controller]")]
    public class ResearchCategoryController : BaseController
    {
        private readonly IResearchCategoryManager _researchCategoryManager;

        public ResearchCategoryController(IResearchCategoryManager researchCategoryManager)
        {
            _researchCategoryManager = researchCategoryManager;
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateResearchCategoryViewModel request)
        {
            var researchCategory = _researchCategoryManager.Create(request, Guid.Empty);

            return StatusCode(201, ResponseWrapper.Success(HttpStatusCode.Created, researchCategory));
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] SearchResearchCategoryCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var pagedResearchCategory = _researchCategoryManager.Search(parameters, page, pageSize);

            if (pagedResearchCategory is null || !pagedResearchCategory.Items.Any())
            {
                return NoContent();
            }
            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, pagedResearchCategory));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var researchCategory = _researchCategoryManager.GetById(id);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, researchCategory));
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] CreateResearchCategoryViewModel request)
        {
            var researchCategory = _researchCategoryManager.Update(id, request, Guid.Empty);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, researchCategory));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _researchCategoryManager.Delete(id);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK));
        }
    }
}