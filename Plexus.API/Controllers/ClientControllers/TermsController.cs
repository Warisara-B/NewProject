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
    public class TermsController : BaseController
    {
        private readonly ITermService _termService;

        public TermsController(ITermService termService)
        {
            _termService = termService;
        }

        [HttpGet]
        [Authorize(Roles = "STUDENT")]
        public IActionResult GetAllTerms()
        {
            var language = GetRequestLanguage();
            var user = GetRequesterInformation();
            var terms = _termService.GetAllTerms(user.StudentId.Value, language);
            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, terms));
        }
    }
}
