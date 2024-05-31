using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Plexus.API.DTO;
using Plexus.Database.Enum;

namespace Plexus.API.Controllers
{
    [ApiVersion("1")]
	public abstract class BaseController : Controller
	{
		protected const string _pathPrefix = "api/v{version:apiVersion}/";
		protected const string _clientPathPrefix = "client/v{version:apiVersion}/";

        protected LanguageCode GetRequestLanguage()
        {
            StringValues language;

            if (Request.Headers.TryGetValue("Accept-Language", out language))
            {
                if (Enum.TryParse(language, out LanguageCode parseLang))
                {
                    return parseLang;
                }
                else
                {
                    return LanguageCode.TH;
                }
            }
            else
            {
                return LanguageCode.TH;
            }
        }

        protected TokenInformationDTO GetRequesterInformation()
        {
            var response = new TokenInformationDTO();

            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            var roleClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);
            var nameClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
            var entityClaim = User.Claims.FirstOrDefault(x => x.Type == "entity_id");
            
            if (userIdClaim is not null &&
                !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                response.UserId = userId;
            }

            if (nameClaim is not null)
            {
                response.Name = nameClaim.Value;
            }

            if (roleClaim is not null
                && entityClaim is not null
                && Guid.TryParse(entityClaim.Value, out Guid entityId))
            {
                switch (roleClaim.Value)
                {
                    case "STUDENT":
                        response.StudentId = entityId;
                        break;
                    case "INSTRUCTOR":
                        response.InstructorId = entityId;
                        break;
                    default:
                        break;
                }
            }

            return response;
        }
    }
}

