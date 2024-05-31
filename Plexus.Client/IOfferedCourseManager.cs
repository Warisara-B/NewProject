using Plexus.Client.ViewModel;
using Plexus.Client.ViewModel.Academic.Section;
using Plexus.Entity.DTO;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface IOfferedCourseManager
    {
        /// <summary>
        /// Create new section with all related.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        OfferedCourseViewModel Create(CreateOfferedCourseViewModel request, Guid userId);

        /// <summary>
        /// Search section by given parameters as paged.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<OfferedCourseViewModel> Search(SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25);

        /// <summary>
        /// Search section student by given parameters as paged.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<StudentViewModel> SearchStudents(Guid id, SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25);

        /// <summary>
        /// Get section by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        OfferedCourseViewModel GetById(Guid id);

        /// <summary>
        /// Update section with all related except joint.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        OfferedCourseViewModel Update(OfferedCourseViewModel request, Guid userId);

        /// <summary>
        /// Update section seats.
        /// </summary>
        /// <param name="sectionId"></param>
        /// <param name="requests"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<SectionSeatViewModel> UpdateSeats(Guid sectionId, IEnumerable<UpsertSectionSeatViewModel>? requests, Guid userId);

        /// <summary>
        /// Delete section by id.
        /// </summary>
        /// <param name="id"></param>
        void Delete(Guid id);
    }
}