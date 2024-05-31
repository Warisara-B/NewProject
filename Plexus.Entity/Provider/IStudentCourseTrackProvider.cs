using Plexus.Entity.DTO.Academic;

namespace Plexus.Entity.Provider
{
    public interface IStudentCourseTrackProvider
    {
        /// <summary>
        /// Get course tracks by student id.
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        IEnumerable<Guid> GetByStudentId(Guid studentId);

        /// <summary>
        /// Update student course track.
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="courseTrackIds"></param>
        /// <returns></returns>
        void Update(Guid studentId, IEnumerable<Guid> courseTrackIds);
    }
}