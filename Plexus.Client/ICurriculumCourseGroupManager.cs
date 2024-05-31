using Plexus.Client.ViewModel.Academic.Curriculum;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface ICurriculumCourseGroupManager
    {
        /// <summary>
        /// Create course group.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        CurriculumCourseGroupViewModel Create(CreateCurriculumCourseGroupViewModel request, Guid userId);

        /// <summary>
        /// Get by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        CurriculumCourseGroupViewModel GetById(Guid id);

        /// <summary>
        /// Get course group by curriculum version id.
        /// </summary>
        /// <param name="curriculumVersionId"></param>
        /// <returns></returns>
        IEnumerable<CurriculumCourseGroupViewModel> GetByCurriculumVersionId(Guid curriculumVersionId);

        /// <summary>
        /// Update course group.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        CurriculumCourseGroupViewModel Update(Guid id, CreateCurriculumCourseGroupViewModel request, Guid userId);

        /// <summary>
        /// Delete course group.
        /// </summary>
        /// <param name="id"></param>
        void Delete(Guid id);

        /// <summary>
        /// Get courses from course group.
        /// </summary>
        /// <param name="courseGroupId"></param>
        /// <returns></returns>
        IEnumerable<CurriculumCourseViewModel> GetCourses(Guid courseGroupId);

        /// <summary>
        /// Add course to course group.
        /// </summary>
        /// <param name="courseGroupId"></param>
        /// <param name="requests"></param>
        /// <returns></returns>
        IEnumerable<CurriculumCourseViewModel> UpdateCourses(Guid courseGroupId, IEnumerable<CreateCurriculumCourseViewModel> requests);

        /// <summary>
        /// Get ignored courses from course group.
        /// </summary>
        /// <param name="courseGroupId"></param>
        /// <returns></returns>
        IEnumerable<CurriculumCourseGroupIgnoreCourseViewModel> GetIgnoreCourses(Guid courseGroupId);

        /// <summary>
        /// Add ignore course.
        /// </summary>
        /// <param name="courseGroupId"></param>
        /// <param name="courseIds"></param>
        /// <returns></returns>
        IEnumerable<CurriculumCourseGroupIgnoreCourseViewModel> UpdateIgnoreCourses(Guid courseGroupId, IEnumerable<Guid> courseIds);
    }
}