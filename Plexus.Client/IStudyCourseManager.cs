using Plexus.Client.ViewModel.Academic;

namespace Plexus.Client
{
    public interface IStudyCourseManager
    {
        /// <summary>
        /// Create study course for students
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="termId"></param>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<StudyCourseViewModel> Create(Guid studentId, Guid termId, IEnumerable<CreateStudyCourseViewModel> request, Guid userId);

        /// <summary>
        /// Get study course by section id
        /// </summary>
        /// <param name="sectionId"></param>
        /// <returns></returns>
        IEnumerable<StudyCourseViewModel> GetBySectionId(Guid sectionId);

        /// <summary>
        /// Get study course by student
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="termId">Specify term id</param>
        /// <returns></returns>
        IEnumerable<StudyCourseViewModel> GetByStudent(Guid studentId, Guid? termId = null);

        /// <summary>
        /// Update study courses
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<StudyCourseViewModel> Update(IEnumerable<UpdateStudyCourseViewModel> request, Guid userId, Guid? studentId = null, Guid? sectionId = null);
    }
}