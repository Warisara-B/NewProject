using Plexus.Entity.DTO.Academic.Curriculum;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider
{
    public interface ICurriculumCourseGroupProvider
    {
        /// <summary>
        /// Create curriculum course group
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        CurriculumCourseGroupDTO Create(CreateCurriculumCourseGroupDTO request, string requester);

        /// <summary>
        /// Get curriculum course group by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        CurriculumCourseGroupDTO GetById(Guid id);

        /// <summary>
        /// Get curriculum course group by version id
        /// </summary>
        /// <param name="curriculumVersionId"></param>
        /// <returns></returns>
        IEnumerable<CurriculumCourseGroupDTO> GetByCurriculumVersionId(Guid curriculumVersionId);

        /// <summary>
        /// Update curriculum course group
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        CurriculumCourseGroupDTO Update(CurriculumCourseGroupDTO request, string requester);

        /// <summary>
        /// Delete curriculum course group
        /// </summary>
        /// <param name="id"></param>
        void Delete(Guid id);

        /// <summary>
        /// Get course by course group id.
        /// </summary>
        /// <param name="courseGroupid"></param>
        /// <returns></returns>
        IEnumerable<CurriculumCourseDTO> GetCourses(Guid courseGroupid);

        /// <summary>
        /// Update course under course group
        /// </summary>
        /// <param name="courseGroupId"></param>
        /// <param name="request"></param>
        void UpdateCourses(Guid courseGroupId, IEnumerable<CurriculumCourseDTO> request);

        /// <summary>
        /// Get ignore courses by course group id
        /// </summary>
        /// <param name="courseGroupId"></param>
        /// <returns></returns>
        IEnumerable<CurriculumCourseGroupIgnoreCourseDTO> GetIgnoreCourses(Guid courseGroupId);


        /// <summary>
        /// Update ignore course under course group
        /// </summary>
        /// <param name="courseGroupId"></param>
        /// <param name="courseIds"></param>
        /// <returns></returns>
        void UpdateIgnoreCourses(Guid courseGroupId, IEnumerable<Guid> courseIds);
    }
}