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
    public class PublicationController : BaseController
    {
        private readonly IPublicationManager _publicationManager;

        public PublicationController(IPublicationManager publicationManager)
        {
            _publicationManager = publicationManager;
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreatePublicationViewModel request)
        {
            var publication = _publicationManager.Create(request, Guid.Empty);

            return StatusCode(201, ResponseWrapper.Success(HttpStatusCode.Created, publication));
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] SearchPublicationCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var pagedPublication = _publicationManager.Search(parameters);

            if (pagedPublication is null || !pagedPublication.Items.Any())
            {
                return NoContent();
            }

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, pagedPublication));
        }


        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var publication = _publicationManager.GetById(id);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, publication));
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, CreatePublicationViewModel request)
        {
            var publication = _publicationManager.Update(id, request, Guid.Empty);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, publication));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _publicationManager.Delete(id);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK));
        }
    }
}