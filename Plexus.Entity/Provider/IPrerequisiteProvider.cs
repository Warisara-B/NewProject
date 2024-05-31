using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Registration;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider
{
    public interface IPrerequisiteProvider
    {
        /// <summary>
        /// Get course prerequisite by course ids.
        /// </summary>
        /// <param name="courseIds"></param>
        /// <returns></returns>
        IEnumerable<CoursePrerequisiteDTO> GetCoursePrerequisiteByCourseId(IEnumerable<Guid> courseIds);

        /// <summary>
        /// Search course prerequisite by given parameters as paged.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<CoursePrerequisiteDTO> SearchCoursePrerequisite(SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25);

        /// <summary>
        /// Update new course prerequisite.
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        CoursePrerequisiteDTO UpdateCoursePrerequisite(Guid courseId, IEnumerable<CreatePrerequisiteConditionDTO> requests, string requester);

        /// <summary>
        /// Delete course prerequisite by course id.
        /// </summary>
        /// <param name="courseId"></param>
        void DeleteCoursePrerequisite(Guid courseId);
    }
}