using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Academic;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider
{
    public interface ICourseTopicProvider
    {
        /// <summary>
        /// Create new course topic record
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        CourseTopicDTO Create(CreateCourseTopicDTO request, string requester);

        /// <summary>
        /// Get course topic by id
        /// </summary>
        /// <param name="courseTopicId"></param>
        /// <returns></returns>
        CourseTopicDTO GetById(Guid courseTopicId);

        /// <summary>
        /// Get course topic by ids
        /// </summary>
        /// <param name="courseTopicIds"></param>
        /// <returns></returns>
        IEnumerable<CourseTopicDTO> GetById(IEnumerable<Guid> courseTopicIds);

        /// <summary>
        /// Get course topic by ids
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        IEnumerable<CourseTopicDTO> GetByCourseId(Guid courseId);

        /// <summary>
        /// Update course topic record
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        CourseTopicDTO Update(CourseTopicDTO request, string requester);

        /// <summary>
        /// Search course topic by given parameters
        /// </summary>
        /// <param name="parameters">search parameter, used code, academic level id, faculty id and department id</param>
        /// <returns></returns>
        IEnumerable<CourseTopicDTO> Search(SearchCourseTopicCriteriaDTO parameters);

        /// <summary>
        /// Search course topic by code and course id
        /// </summary>
        /// <param name="parameters">search parameter, used code and faculty id</param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<CourseTopicDTO> Search(SearchCourseTopicCriteriaDTO parameters, int page, int pageSize);

        /// <summary>
        /// Delete course topic record
        /// </summary>
        /// <param name="courseId"></param>
        void DeleteCourseTopic(Guid courseTopicId);
    }
}