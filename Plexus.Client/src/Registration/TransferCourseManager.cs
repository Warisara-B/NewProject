using Plexus.Client.ViewModel.Academic;
using Plexus.Client.ViewModel.Registration;
using Plexus.Database.Enum.Academic;
using Plexus.Entity.DTO.Academic;
using Plexus.Entity.Exception;
using Plexus.Entity.Provider;
using Plexus.Entity.Provider.src.Academic;
using Plexus.Client.src.Academic;

namespace Plexus.Client.src.Registration
{
    public class TransferCourseManager : ITransferCourseManager
    {
        private readonly IStudentProvider _studentProvider;
        private readonly ITermProvider _termProvider;
        private readonly IStudyCourseProvider _studyCourseProvider;
        private readonly ICourseProvider _courseProvider;
        private readonly IGradeProvider _gradeProvider;
        
        public TransferCourseManager(IStudentProvider studentProvider, 
                                     ITermProvider termProvider,
                                     IStudyCourseProvider studyCourseProvider,
                                     ICourseProvider courseProvider,
                                     IGradeProvider gradeProvider)
        {
            _studentProvider = studentProvider;
            _termProvider = termProvider;
            _studyCourseProvider = studyCourseProvider;
            _courseProvider = courseProvider;
            _gradeProvider = gradeProvider;
        }

        public IEnumerable<TransferViewModel> Create(Guid id, CreateTransferViewModel request, Guid userId)
        {
            var student = _studentProvider.GetById(id);

            var termIds = request.Courses.Select(x => x.TermId)
                                         .Distinct()
                                         .ToList();

            var terms = _termProvider.GetById(termIds)
                                     .ToList();

            foreach (var termId in termIds)
            {
                var matchedTerm = terms.SingleOrDefault(x => x.Id == termId);

                if (matchedTerm is null)
                {
                    throw new TermException.NotFound(termId);
                }
            }

            if (request.Courses is null || !request.Courses.Any())
            {
                return Enumerable.Empty<TransferViewModel>();
            }

            var courseIds = request.Courses.Select(x => x.CourseId)
                                           .ToList();

            var courses = _courseProvider.GetById(courseIds)
                                         .ToList();
            
            foreach (var courseId in courseIds)
            {
                var matchedCourse = courses.SingleOrDefault(x => x.Id == courseId);

                if (matchedCourse is null)
                {
                    throw new CourseException.NotFound(courseId);
                }
            }

            var gradeIds = request.Courses.Where(x => x.GradeId.HasValue)
                                          .Select(x => x.GradeId!.Value)
                                          .Distinct()
                                          .ToList();
            
            var grades = _gradeProvider.GetById(gradeIds)
                                       .ToList();
            
            foreach (var gradeId in gradeIds)
            {
                var matchedGrade = grades.SingleOrDefault(x => x.Id == gradeId);

                if (matchedGrade is null)
                {
                    throw new GradeException.NotFound(gradeId);
                }
            }

            var dto = MapViewModelToDTO(id, request, courses);

            var transferCourses = _studyCourseProvider.CreateTransferCourses(dto, userId.ToString());

            var response = MapStudyCourseDTOToViewModel(transferCourses, terms, courses, grades);

            return response;
        }

        public IEnumerable<TransferViewModel> GetByStudent(Guid studentId)
        {
            var transferCourses = _studyCourseProvider.GetByStudent(studentId, null, new List<StudyCourseStatus> { StudyCourseStatus.TRANSFERED });

            var termIds = transferCourses.Select(x => x.TermId)
                                         .Distinct()
                                         .ToList();

            var terms = _termProvider.GetById(termIds)
                                     .ToList();

            var courseIds = transferCourses.Select(x => x.CourseId)
                                           .ToList();

            var courses = _courseProvider.GetById(courseIds)
                                         .ToList();
            
            var gradeIds = transferCourses.Where(x => x.GradeId.HasValue)
                                          .Select(x => x.GradeId!.Value)
                                          .Distinct()
                                          .ToList();
            
            var grades = _gradeProvider.GetById(gradeIds)
                                       .ToList();
            
            var response = MapStudyCourseDTOToViewModel(transferCourses, terms, courses, grades);

            return response;
        }

        private static IEnumerable<CreateStudyCourseDTO> MapViewModelToDTO(Guid id, CreateTransferViewModel request, IEnumerable<CourseDTO> courses)
        {
            var response = (from course in request.Courses
                            let matchedCourse = courses.SingleOrDefault(x => x.Id == course.CourseId)
                            select new CreateStudyCourseDTO
                            {
                                StudentId = id,
                                TermId = course.TermId,
                                CourseId = course.CourseId,
                                GradeId = course.GradeId,
                                RegistrationChannel = request.RegistrationChannel,
                                Remark = course.Remark,
                                Status = StudyCourseStatus.TRANSFERED,
                                Credit = matchedCourse.Credit,
                                RegistrationCredit = matchedCourse.RegistrationCredit
                            })
                           .ToList();

            return response;
        }

        private static IEnumerable<TransferViewModel> MapStudyCourseDTOToViewModel(IEnumerable<StudyCourseDTO> transferCourses, 
            IEnumerable<TermDTO> terms, IEnumerable<CourseDTO> courses, IEnumerable<GradeDTO> grades)
        {
            var response = (from studyCourses in transferCourses.GroupBy(x => x.TermId)
                            let term = terms.SingleOrDefault(x => x.Id == studyCourses.Key)
                            orderby term.Year, term.Number
                            select new TransferViewModel
                            {
                                TermId = studyCourses.Key,
                                Courses = (from studyCourse in studyCourses
                                           let course = courses.SingleOrDefault(x => x.Id == studyCourse.CourseId)
                                           let grade = studyCourse.GradeId.HasValue ? grades.SingleOrDefault(x => x.Id == studyCourse.GradeId.Value)
                                                                                    : null
                                           orderby course.Code
                                           select new TransferCourseViewModel
                                           {
                                               CourseId = studyCourse.CourseId,
                                               TermId = studyCourses.Key,
                                               CourseCode = course.Code,
                                               CourseName = course.Name,
                                               Credit = course.Credit,
                                               RegistrationCredit = course.RegistrationCredit,
                                               LectureCredit = course.LectureCredit,
                                               LabCredit = course.LabCredit,
                                               OtherCredit = course.OtherCredit,
                                               GradeId = studyCourse.GradeId,
                                               GradeLetter = grade.Letter,
                                               Remark = studyCourse.Remark
                                           })
                                          .ToList()
                            })
                           .ToList();
            
            return response;
        }
    }
}