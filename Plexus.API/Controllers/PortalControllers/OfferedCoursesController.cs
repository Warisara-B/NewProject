// using System.Net;
// using Microsoft.AspNetCore.Mvc;
// using Plexus.Client;
// using Plexus.Client.ViewModel.Academic.Section;
// using Plexus.Entity.DTO;
// using Plexus.Utility;

// namespace Plexus.API.Controllers
// {
//     [ApiController]
// 	[Route(_pathPrefix + "[controller]")]
//     public class OfferedCoursesController : BaseController
//     {
//         private readonly IOfferedCourseManager _offeredCourseManager;

//         public OfferedCoursesController(IOfferedCourseManager offeredCourseManager)
//         {
//             _offeredCourseManager = offeredCourseManager;
//         }

//         // TODO: Get user id
//         [HttpPost]
//         public IActionResult Create([FromBody] CreateOfferedCourseViewModel request)
//         {
//             var offeredCourse = _offeredCourseManager.Create(request, Guid.Empty);

//             return StatusCode(201,
//                 ResponseWrapper.Success(HttpStatusCode.Created, offeredCourse));
//         }

//         [HttpGet("{id}")]
//         public IActionResult GetById(Guid id)
//         {
//             var offeredCourse = _offeredCourseManager.GetById(id);

//             return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, offeredCourse));
//         }

//         [HttpGet("search")]
//         public IActionResult Search([FromQuery] SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25)
//         {
//             var pagedOfferedCourse = _offeredCourseManager.Search(parameters, page, pageSize);

//             if (pagedOfferedCourse is null || !pagedOfferedCourse.Items.Any())
//             {
//                 return StatusCode(204);
//             }

//             return StatusCode(200,
//                 ResponseWrapper.Success(HttpStatusCode.OK, pagedOfferedCourse));
//         }

//         [HttpGet("{id}/students")]
//         public IActionResult SearchStudents(Guid id, [FromQuery] SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25)
//         {
//             var pagedStudents = _offeredCourseManager.SearchStudents(id, parameters, page, pageSize);

//             if (pagedStudents is null || !pagedStudents.Items.Any())
//             {
//                 return StatusCode(204);
//             }

//             return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, pagedStudents));
//         }

//         [HttpPut]
//         public IActionResult Update([FromBody] OfferedCourseViewModel request)
//         {
//             var offeredCourse = _offeredCourseManager.Update(request, Guid.Empty);

//             return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, offeredCourse));
//         }

//         [HttpPut("{id}/seats")]
//         public IActionResult UpdateSeats(Guid id, [FromBody] IEnumerable<UpsertSectionSeatViewModel> requests)
//         {
//             var sectionSeats = _offeredCourseManager.UpdateSeats(id, requests, Guid.Empty);

//             return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, sectionSeats));
//         }

//         [HttpDelete("{id}")]
//         public IActionResult Delete(Guid id)
//         {
//             _offeredCourseManager.Delete(id);

//             return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK));
//         }
//     }
// }