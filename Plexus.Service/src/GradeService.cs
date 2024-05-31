using Microsoft.EntityFrameworkCore;
using Plexus.Database;
using Plexus.Database.Enum;
using Plexus.Database.Enum.Student;
using Plexus.Database.Model.Academic;
using Plexus.Database.Model.Academic.Curriculum;
using Plexus.Entity.Exception;
using Plexus.Service.ViewModel;
using ServiceStack;

namespace Plexus.Service.src
{
    public class GradeService : IGradeService
    {
        private readonly DatabaseContext _dbContext;

        public GradeService(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public StudentGradeViewModel GetGradeByTerm(Guid studentId, LanguageCode language)
        {
            var student = _dbContext.Students.AsNoTracking()
                                             .FirstOrDefault(x => x.Id == studentId);

            if (student is null)
            {
                throw new StudentException.NotFound(studentId);
            }

            var termIds = _dbContext.Terms.AsNoTracking()
                                          .Select(x => x.Id)
                                          .ToList();

            var terms = _dbContext.StudentTerms.AsNoTracking()
                                               .Where(x => x.StudentId == studentId)
                                               .Include(x => x.Term)
                                               .ToList();

            var studyCourses = _dbContext.StudyCourses.AsNoTracking()
                                                      .Where(x => x.StudentId == studentId)
                                                      .Include(x => x.Course)
                                                        .ThenInclude(x => x.Localizations)
                                                      .Include(x => x.Grade)
                                                      .ToList();

            var courseIds = _dbContext.StudyCourses.AsNoTracking()
                                                   .Where(x => x.StudentId == studentId
                                                          && termIds.Contains(x.TermId))
                                                   .Select(x => x.CourseId)
                                                   .ToList();

            var curriculumVersion = _dbContext.CurriculumVersions.AsNoTracking()
                                                                 .FirstOrDefault(x => x.Id == student.CurriculumVersionId);

            if (curriculumVersion is null)
            {
                throw new CurriculumException.VersionNotFound(student.CurriculumVersionId);
            }

            var curriculumCourses = _dbContext.CurriculumCourses.AsNoTracking()
                                                                .Where(x => x.CourseGroup.CurriculumVersionId == curriculumVersion.Id)
                                                                .Include(x => x.RequiredGrade)
                                                                .ToList();

            return MapStudentGradeViewModel(terms, studyCourses, curriculumCourses, language);
        }

        public IEnumerable<StudentCurriculumViewModel> GetGradeByCurriculum(Guid studentId, LanguageCode language)
        {
            var student = _dbContext.Students.AsNoTracking()
                                             .SingleOrDefault(x => x.Id == studentId);

            if (student is null)
            {
                throw new StudentException.NotFound(studentId);
            }

            var curriculumVersion = _dbContext.CurriculumVersions.AsNoTracking()
                                                                 .SingleOrDefault(x => x.Id == student.CurriculumVersionId);

            if (curriculumVersion is null)
            {
                throw new CurriculumException.VersionNotFound(student.CurriculumVersionId);
            }

            var curriculumCourses = _dbContext.CurriculumCourses.AsNoTracking()
                                                                .Where(x => x.CourseGroup.CurriculumVersionId == curriculumVersion.Id)
                                                                .Include(x => x.RequiredGrade)
                                                                .ToList();

            var curriculumCourseGroups = _dbContext.CurriculumCourseGroups.AsNoTracking()
                                                                          .Include(x => x.CurriculumCourses)
                                                                            .ThenInclude(x => x.Course)
                                                                          .Include(x => x.CurriculumCourses)
                                                                            .ThenInclude(x => x.RequiredGrade)
                                                                          .Where(x => x.CurriculumVersionId == curriculumVersion.Id)
                                                                          .ToList();


            var studyCourses = _dbContext.StudyCourses.AsNoTracking()
                                                      .Where(x => x.StudentId == studentId)
                                                      .Include(x => x.Course)
                                                        .ThenInclude(x => x.Localizations)
                                                      .Include(x => x.Grade)
                                                      .ToList();

            var response = MapCurriculumToViewModel(curriculumCourseGroups, studyCourses, curriculumCourses, language);

            return response;
        }

        private static StudentGradeViewModel MapStudentGradeViewModel(
            IEnumerable<StudentTerm> terms,
            IEnumerable<StudyCourse> studyCourses,
            IEnumerable<CurriculumCourse>? courses,
            LanguageCode language)
        {
            return new StudentGradeViewModel
            {
                GPAX = null, // TODO: Calculate GPAX
                CompletedCredit = null, // TODO: Calculate CompletedCredit
                Terms = (from term in terms
                         orderby term.Term.Year descending,
                                 term.Term.Number ascending
                         select MapTermToViewModel(term, studyCourses, courses, language))
            };
        }

        private static StudentStudyTermViewModel MapTermToViewModel(
            StudentTerm term,
            IEnumerable<StudyCourse> studyCourses,
            IEnumerable<CurriculumCourse>? courses,
            LanguageCode language)
        {
            return new StudentStudyTermViewModel
            {
                Year = term.Term.Year,
                Term = $"{term.Term.Year}/{term.Term.Number}",
                Credit = term.TotalCredit,
                GPA = term.GPAX,
                Courses = (from studyCourse in studyCourses
                           orderby studyCourse.Course.Code ascending,
                                   studyCourse.Course.Name ascending
                           select MapStudyCourseToViewModel(studyCourse, courses, language))
            };
        }

        private static StudentCourseViewModel MapStudyCourseToViewModel(
            StudyCourse studyCourse,
            IEnumerable<CurriculumCourse>? courses,
            LanguageCode language)
        {
            var matchingCourse = courses?.FirstOrDefault(x => x.CourseId == studyCourse.CourseId);

            var locale = studyCourse?.Course.Localizations.SingleOrDefault(x => x.Language == language);

            return new StudentCourseViewModel
            {
                Code = studyCourse?.Course.Code,
                Name = locale?.Name ?? studyCourse?.Course.Name,
                Credit = studyCourse?.Credit,
                Status = (studyCourse?.GradeWeight >= matchingCourse?.RequiredGrade?.Weight)
                          ? GradeStatus.NORMAL
                          : GradeStatus.FAILED,
                Grade = studyCourse?.Grade?.Letter,
                PassGrade = matchingCourse?.RequiredGrade?.Letter
            };
        }

        private static IEnumerable<StudentCurriculumViewModel> MapCurriculumToViewModel(
                       IEnumerable<CurriculumCourseGroup> topLevelGroups,
                       IEnumerable<StudyCourse> studyCourses,
                       IEnumerable<CurriculumCourse> courses,
                       LanguageCode language)
        {
            var response = new List<StudentCurriculumViewModel>();

            var seenSubGroups = new HashSet<CurriculumCourseGroup>();

            foreach (var group in topLevelGroups)
            {
                if (seenSubGroups.Contains(group))
                {

                    continue;
                }

                var relevantStudyCourses = studyCourses.Where(x => group.CurriculumCourses.Any(y => x.CourseId == y.CourseId)).ToList();

                var curriculumGroup = new StudentCurriculumViewModel
                {
                    Name = group.Name,
                    RequiredCredit = group.RequiredCredit,
                    SubGroups = MapCurriculumToViewModel(topLevelGroups
                                .Where(subgroup => subgroup.ParentCourseGroupId == group.Id),
                                studyCourses,
                                courses,
                                language),
                    Courses = (from studyCourse in relevantStudyCourses
                               orderby studyCourse.Course.Code ascending,
                                       studyCourse.Course.Name ascending
                               select MapStudyCourseToViewModel(studyCourse, courses, language))
                };

                response.Add(curriculumGroup);

                foreach (var subgroup in topLevelGroups.Where(subgroup => subgroup.ParentCourseGroupId == group.Id))
                {
                    seenSubGroups.Add(subgroup);
                }
            }

            return response;
        }
    }
}