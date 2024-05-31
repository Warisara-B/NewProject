using Plexus.Client.ViewModel.Academic;
using Plexus.Client.ViewModel.Registration;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface IRegistrationManager
    {
        /// <summary>
        /// Get study courses by student.
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="termId"></param>
        /// <param name="isIncludedTransfer"></param>
        /// <returns></returns>
        IEnumerable<StudyCourseViewModel> GetByStudent(Guid studentId, Guid? termId, bool isIncludedTransfer);

        /// <summary>
        /// Get registration logs by student.
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="termId"></param>
        /// <returns></returns>
        PagedViewModel<RegistrationLogViewModel> GetLogs(Guid studentId, Guid termId, int page = 1, int pageSize = 25);

        /// <summary>
        /// Update study courses. (Do Registration)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        void Update(RegistrationViewModel request, Guid userId);

        /// <summary>
        /// Verify conflict section examination.
        /// </summary>
        /// <param name="sections"></param>
        /// <returns></returns>
        IEnumerable<SectionViewModel> VerifySectionExaminations(IEnumerable<RegistrationCourseViewModel>? sections);

        /// <summary>
        /// Verify conflict class time.
        /// </summary>
        /// <param name="sections"></param>
        /// <returns></returns>
        void VerifyClassTimes(IEnumerable<RegistrationCourseViewModel>? sections);
    }
}