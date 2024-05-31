using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plexus.Service;
using Plexus.Service.ViewModel;
using Plexus.Utility;

namespace Plexus.API.Controllers.ClientControllers
{
    [Authorize]
    [ApiController]
    [Route(_clientPathPrefix + "[controller]")]
    public class AnnouncementController : BaseController
	{
        private readonly IAnnouncementService _announcementService;

        public AnnouncementController(IAnnouncementService announcementService)
        {
            _announcementService = announcementService;
        }

        [HttpGet("categories")]
        public IActionResult GetNewsCategories()
        {
            var language = GetRequestLanguage();
            var categories = _announcementService.GetNewsCategories(language);
            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, categories));
        }

        [HttpPost("list")]
        public IActionResult GetNewList([FromBody] NewsFilterViewModel filter, [FromQuery] int page = 1, [FromQuery] int pageSize = 25)
        {
            var language = GetRequestLanguage();
            var user = GetRequesterInformation();
            var newList = _announcementService.SearchNews(filter,page,pageSize,
                                                          user.InstructorId, user.StudentId, language);
            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, newList));
        }

        [HttpGet("{newsId}")]
        public IActionResult GetNewsDetail([FromRoute] Guid newsId)
        {
            var language = GetRequestLanguage();
            var user = GetRequesterInformation();
            var newList = _announcementService.GetNewsDetailById(newsId,user.InstructorId, user.StudentId, language);
            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, newList));
        }

        [HttpPost("{newsId}/bookmark")]
        public IActionResult MarkBookmarkNews([FromRoute] Guid newsId)
        {
            var user = GetRequesterInformation();
            _announcementService.AddBookmarkNews(newsId, user.InstructorId, user.StudentId);
            return Ok(ResponseWrapper.Success(HttpStatusCode.OK));
        }

        [HttpDelete("{newsId}/bookmark")]
        public IActionResult DeleteBookmarkNews([FromRoute] Guid newsId)
        {
            var user = GetRequesterInformation();
            _announcementService.RemoveBookmarkNews(newsId, user.InstructorId, user.StudentId);
            return Ok(ResponseWrapper.Success(HttpStatusCode.OK));
        }
    }
}

