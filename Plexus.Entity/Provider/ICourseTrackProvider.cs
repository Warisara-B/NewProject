using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Academic;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider
{
    public interface ICourseTrackProvider
    {
        /// <summary>
        /// Create new course track.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        CourseTrackDTO Create(CreateCourseTrackDTO request, string requester);

        /// <summary>
        /// Search course track by given parameters as paged.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<CourseTrackDTO> Search(SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25);

        /// <summary>
        /// Search course track by given parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<CourseTrackDTO> Search(SearchCriteriaViewModel? parameters = null);

        /// <summary>
        /// Get course track by given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        CourseTrackDTO GetById(Guid id);

        /// <summary>
        /// Get course tracks by given ids.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        IEnumerable<CourseTrackDTO> GetById(IEnumerable<Guid> ids);

        /// <summary>
        /// Get course tracks by student id.
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        IEnumerable<CourseTrackDTO> GetByStudentId(Guid studentId);

        /// <summary>
        /// Update course track.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        CourseTrackDTO Update(CourseTrackDTO request, string requester);

        /// <summary>
        /// Delete course track by given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);

        /// <summary>
        /// Get course track details by course track id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IEnumerable<UpdateCourseTrackDetailDTO> GetDetailByCourseTrackId(Guid id);

        /// <summary>
        /// Get course track details by course track ids.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        IEnumerable<UpdateCourseTrackDetailDTO> GetDetailByCourseTrackId(IEnumerable<Guid> ids);

        /// <summary>
        /// Update course track details.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="details"></param>
        /// <returns></returns>
        void UpdateDetails(Guid id, IEnumerable<UpdateCourseTrackDetailDTO> details);
    }
}