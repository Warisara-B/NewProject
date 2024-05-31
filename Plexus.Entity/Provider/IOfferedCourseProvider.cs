using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Academic;
using Plexus.Entity.DTO.Academic.Section;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider
{
    public interface IOfferedCourseProvider
    {
        /// <summary>
        /// Create new section.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        OfferedCourseDTO Create(CreateOfferedCourseDTO request, string requester);

        /// <summary>
        /// Search section by given parameters as paged.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<OfferedCourseDTO> Search(SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25);

        /// <summary>
        /// Search section student by given parameters as paged.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<StudentDTO> SearchStudents(Guid id, SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25);

        /// <summary>
        /// Get section by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        OfferedCourseDTO GetById(Guid id);

        /// <summary>
        /// Update section info.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="seats"></param>
        /// <param name="details"></param>
        /// <param name="examinations"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        OfferedCourseDTO Update(SectionDTO request, IEnumerable<UpsertSectionSeatDTO> seats, IEnumerable<UpdateSectionDetailDTO> details, IEnumerable<UpdateSectionExaminationDTO> examinations, string requester);

        /// <summary>
        /// Update section info.
        /// </summary>
        /// <param name="sectionId"></param>
        /// <param name="requests"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        IEnumerable<SectionSeatDTO> UpdateSeats(Guid sectionId, IEnumerable<UpsertSectionSeatDTO> requests, string requester);

        /// <summary>
        /// Delete section by id.
        /// </summary>
        /// <param name="id"></param>
        void Delete(Guid id);
    }
}