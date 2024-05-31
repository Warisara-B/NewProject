using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Plexus.Service;
using Plexus.Service.src;
using Plexus.Utility;
using System.Net;

namespace Plexus.API.Controllers.ClientControllers
{
    [Authorize]
    [ApiController]
    [Route(_clientPathPrefix + "[controller]")]
    public class NotificationsController : BaseController
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("count")]
        [Authorize(Roles = "STUDENT")]
        public IActionResult GetCountUnread()
        {
            var language = GetRequestLanguage();
            var user = GetRequesterInformation();
            var unreadCount = _notificationService.GetCountUnreadNotification(user.StudentId.Value);
            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, unreadCount));
        }

        [HttpGet("list")]
        [Authorize(Roles = "STUDENT")]
        public IActionResult GetNotificationList([FromQuery] int page = 1, [FromQuery] int pageSize = 25)
        {
            var language = GetRequestLanguage();
            var user = GetRequesterInformation();
            var data = _notificationService
                .GetNotificationByStudentId(user.StudentId.Value, language, page, pageSize);
            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, data));
        }

        [HttpGet("{id}/detail")]
        [Authorize(Roles = "STUDENT")]
        public IActionResult GetNotificationDetail(Guid id)
        {
            var language = GetRequestLanguage();
            var user = GetRequesterInformation();
            var data = _notificationService
                .GetNotificationDetailById(user.StudentId.Value, language, id);
            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, data));
        }

        [HttpPut("readall")]
        [Authorize(Roles = "STUDENT")]
        public IActionResult UpdateNotificationReadAll()
        {
            var language = GetRequestLanguage();
            var user = GetRequesterInformation();
            _notificationService.UpdateReadNotificationAll(user.StudentId.Value);
            return Ok(ResponseWrapper.Success(HttpStatusCode.OK));
        }

        [HttpPut("{id}/read")]
        [Authorize(Roles = "STUDENT")]
        public IActionResult UpdateNotificationReadById(Guid id)
        {
            var language = GetRequestLanguage();
            var user = GetRequesterInformation();
            _notificationService.UpdateReadNotificationById(user.StudentId.Value, id);
            return Ok(ResponseWrapper.Success(HttpStatusCode.OK));
        }
    }
}
