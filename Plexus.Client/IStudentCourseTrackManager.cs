using Plexus.Client.ViewModel.Academic;

namespace Plexus.Client
{
    public interface IStudentCourseTrackManager
    {
        /// <summary>
        /// Get course tracks by student id.
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        IEnumerable<CourseTrackViewModel> GetByStudentId(Guid studentId);

        /// <summary>
        /// Update student course track.
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="courseTrackIds"></param>
        /// <returns></returns>
        IEnumerable<CourseTrackViewModel> Update(Guid studentId, IEnumerable<Guid> courseTrackIds);
    }
}