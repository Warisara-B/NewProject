using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Academic;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider
{
    public interface ICourseProvider
    {
        /// <summary>
        /// Create new course record
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        CourseDTO Create(CreateCourseDTO request, string requester);

        /// <summary>
        /// Get course by id
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        CourseDTO GetById(Guid courseId);

        /// <summary>
        /// Get course by ids
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        IEnumerable<CourseDTO> GetById(IEnumerable<Guid> courseIds);

        /// <summary>
        /// Search course by given parameters
        /// </summary>
        /// <param name="parameters">search parameter, used code, academic level id, faculty id and department id</param>
        /// <returns></returns>
        IEnumerable<CourseDTO> Search(SearchCourseCriteriaDTO parameters);

        /// <summary>
        /// Search course by code and faculty id
        /// </summary>
        /// <param name="parameters">search parameter, used code and faculty id</param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<CourseDTO> Search(SearchCourseCriteriaDTO parameters, int page, int pageSize);

        /// <summary>
        /// Update course record
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        CourseDTO Update(CourseDTO request, string requester);

        /// <summary>
        /// Delete course record
        /// </summary>
        /// <param name="courseId"></param>
        void DeleteCourse(Guid courseId);
    }
}