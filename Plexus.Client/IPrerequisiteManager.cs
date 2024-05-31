using Plexus.Client.ViewModel.Registration;

namespace Plexus.Client
{
    public interface IPrerequisiteManager
    {
        /// <summary>
        /// Update course prerequisite.
        /// </summary>
        /// <param name="request"></param>
        void VerifyPrerequisite(RegistrationViewModel request);

        /// <summary>
        /// Update course prerequisite.
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        CoursePrerequisiteViewModel UpdateCoursePrerequisite(Guid courseId, CreatePrerequisiteViewModel request, Guid userId);

        /// <summary>
        /// Get list of course that has set prerequisite in curriculum version
        /// </summary>
        /// <param name="curriculumVersionId"></param>
        /// <returns></returns>
        IEnumerable<CoursePrerequisiteViewModel> GetCurriculumVersionPrerequisites(Guid curriculumVersionId);
    }
}