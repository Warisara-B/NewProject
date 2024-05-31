using System.Net;
using Microsoft.AspNetCore.Mvc;
using Plexus.Client;
using Plexus.Client.ViewModel.Research;
using Plexus.Entity.DTO.SearchFilter;
using Plexus.Utility;

namespace Plexus.API.Controllers
{
    [ApiController]
    [Route(_pathPrefix + "[controller]")]
    public class ArticleTypesController : BaseController
    {
        private readonly IArticleTypeManager _articleTypeManager;

        public ArticleTypesController(IArticleTypeManager articleTypeManager)
        {
            _articleTypeManager = articleTypeManager;
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateArticleTypeViewModel request)
        {
            var articleType = _articleTypeManager.Create(request, Guid.Empty);

            return StatusCode(201, ResponseWrapper.Success(HttpStatusCode.Created, articleType));
        }

        [HttpGet("search")]
        public IActionResult SearchArticleType([FromQuery] SearchArticleTypeCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var pagedArticleType = _articleTypeManager.Search(parameters, page, pageSize);

            if (pagedArticleType is null || !pagedArticleType.Items.Any())
            {
                return NoContent();
            }

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, pagedArticleType));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var articleType = _articleTypeManager.GetById(id);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, articleType));
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] CreateArticleTypeViewModel request)
        {
            var articleType = _articleTypeManager.Update(id, request, Guid.Empty);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, articleType));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _articleTypeManager.Delete(id);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK));
        }
    }
}