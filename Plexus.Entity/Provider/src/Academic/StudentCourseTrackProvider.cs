using Plexus.Database;
using Plexus.Database.Model.Academic;
using Plexus.Entity.DTO.Academic;

namespace Plexus.Entity.Provider.src.Academic
{
    public class StudentCourseTrackProvider : IStudentCourseTrackProvider
    {
        private readonly DatabaseContext _dbContext;

        public StudentCourseTrackProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Guid> GetByStudentId(Guid studentId)
        {
            var studentCourseTracks = _dbContext.StudentCourseTracks.Where(x => x.StudentId == studentId)
                                                                    .ToList();
            
            var response = (from studentCourseTrack in studentCourseTracks
                            select studentCourseTrack.CourseTrackId)
                           .ToList();
            
            return response;
        }

        public void Update(Guid studentId, IEnumerable<Guid> courseTrackIds)
        {
            var existingStudentCourseTracks = _dbContext.StudentCourseTracks.Where(x => x.StudentId == studentId)
                                                                            .ToList();
            
            var newStudentCourseTracks = courseTrackIds is null ? Enumerable.Empty<StudentCourseTrack>()
                                                                : (from courseTrackId in courseTrackIds
                                                                   select new StudentCourseTrack
                                                                   {
                                                                       StudentId = studentId,
                                                                       CourseTrackId = courseTrackId
                                                                   })
                                                                  .ToList();
            
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                if (existingStudentCourseTracks.Any())
                {
                    _dbContext.StudentCourseTracks.RemoveRange(existingStudentCourseTracks);
                }

                if (newStudentCourseTracks.Any())
                {
                    _dbContext.StudentCourseTracks.AddRange(newStudentCourseTracks);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }
    }
}