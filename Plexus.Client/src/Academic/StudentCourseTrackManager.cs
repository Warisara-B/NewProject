using Plexus.Client.ViewModel.Academic;
using Plexus.Entity.Exception;
using Plexus.Entity.Provider;

namespace Plexus.Client.src.Academic
{
    public class StudentCourseTrackManager : IStudentCourseTrackManager
    {
        private IStudentProvider _studentProvider;
        private IStudentCourseTrackProvider _studentCourseTrackProvider;
        private ICourseTrackProvider _courseTrackProvider;

        public StudentCourseTrackManager(IStudentProvider studentProvider,
                                         IStudentCourseTrackProvider studentCourseTrackProvider,
                                         ICourseTrackProvider courseTrackProvider)
        {
            _studentProvider = studentProvider;
            _studentCourseTrackProvider = studentCourseTrackProvider;
            _courseTrackProvider = courseTrackProvider;
        }

        public IEnumerable<CourseTrackViewModel> GetByStudentId(Guid studentId)
        {
            var student = _studentProvider.GetById(studentId);

            var courseTrackIds = _studentCourseTrackProvider.GetByStudentId(studentId);

            var courseTracks = _courseTrackProvider.GetById(courseTrackIds);

            var response = (from courseTrackId in courseTrackIds
                            let courseTrack = courseTracks.SingleOrDefault(x => x.Id == courseTrackId)
                            select CourseTrackManager.MapDTOToViewModel(courseTrack))
                           .ToList();
            
            return response;
        }

        public IEnumerable<CourseTrackViewModel> Update(Guid studentId, IEnumerable<Guid> courseTrackIds)
        {
            var student = _studentProvider.GetById(studentId);

            var updatedCourseTrackIds = courseTrackIds is null ? Enumerable.Empty<Guid>()
                                                               : courseTrackIds;
            
            var courseTracks = _courseTrackProvider.GetById(updatedCourseTrackIds);
                
            foreach (var courseTrackId in updatedCourseTrackIds)
            {
                var courseTrack = courseTracks.SingleOrDefault(x => x.Id == courseTrackId);

                if (courseTrack is null)
                {
                    throw new CourseTrackException.NotFound(courseTrackId);
                }
            }

            _studentCourseTrackProvider.Update(studentId, updatedCourseTrackIds);

            var response = (from courseTrackId in updatedCourseTrackIds
                            let courseTrack = courseTracks.SingleOrDefault(x => x.Id == courseTrackId)
                            select CourseTrackManager.MapDTOToViewModel(courseTrack))
                           .ToList();
            
            return response;
        }
    }
}