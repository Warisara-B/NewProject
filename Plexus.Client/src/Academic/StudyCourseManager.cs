using System;
using Azure;
using Plexus.Client.ViewModel.Academic;
using Plexus.Database.Enum.Academic;
using Plexus.Database.Model;
using Plexus.Database.Model.Academic;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Academic;
using Plexus.Entity.Exception;
using Plexus.Entity.Provider;
using Plexus.Entity.Provider.src.Academic;

namespace Plexus.Client.src.Academic
{
    public class StudyCourseManager : IStudyCourseManager
    {
        private readonly IStudentProvider _studentProvider;
        private readonly IStudyCourseProvider _studyCourseProvider;
        private readonly IGradeProvider _gradeProvider;
        private readonly ICourseProvider _courseProvider;
        // private readonly ISectionProvider _sectionProvider;
        private readonly ITermProvider _termProvider;

        private static IEnumerable<StudyCourseStatus> activeStudyCourseStatus =
            new[] { StudyCourseStatus.REGISTERED,
                    StudyCourseStatus.ACTIVE, StudyCourseStatus.WITHDRAWN,
                    StudyCourseStatus.TRANSFERED };

        public StudyCourseManager(IStudentProvider studentProvider,
                                  IStudyCourseProvider studyCourseProvider,
                                  IGradeProvider gradeProvider,
                                  ICourseProvider courseProvider,
                                  //   ISectionProvider sectionProvider,
                                  ITermProvider termProvider)
        {
            _studentProvider = studentProvider;
            _studyCourseProvider = studyCourseProvider;
            _gradeProvider = gradeProvider;
            _courseProvider = courseProvider;
            // _sectionProvider = sectionProvider;
            _termProvider = termProvider;
        }

        public IEnumerable<StudyCourseViewModel> Create(
            Guid studentId, Guid termId, IEnumerable<CreateStudyCourseViewModel> request, Guid userId)
        {
            if (request is null || !request.Any())
            {
                return Enumerable.Empty<StudyCourseViewModel>();
            }

            var student = _studentProvider.GetById(studentId);

            var term = _termProvider.GetById(termId);

            var courseIds = request.Select(x => x.CourseId)
                                   .Distinct()
                                   .ToList();

            var sectionIds = request.Where(x => x.SectionId.HasValue)
                                    .Select(x => x.SectionId.Value)
                                    .ToList();

            var courses = _courseProvider.GetById(courseIds)
                                         .ToList();

            // var sections = _sectionProvider.GetById(sectionIds)
            //                                .ToList();

            var grades = _gradeProvider.GetAll()
                                       .ToList();

            var dtos = new List<CreateStudyCourseDTO>();

            foreach (var data in request)
            {
                var matchingCourse = courses.SingleOrDefault(x => x.Id == data.CourseId);

                if (matchingCourse is null)
                {
                    throw new CourseException.NotFound(data.CourseId);
                }

                var dto = new CreateStudyCourseDTO
                {
                    StudentId = studentId,
                    TermId = termId,
                    CourseId = matchingCourse.Id,
                    Credit = matchingCourse.Credit,
                    RegistrationCredit = matchingCourse.RegistrationCredit,
                    RegistrationChannel = data.RegistrationChannel
                };

                // if (data.SectionId.HasValue)
                // {
                //     var matchingSection = sections.SingleOrDefault(x => x.Id == data.SectionId.Value);

                //     if (matchingSection is null || matchingSection.CourseId != data.CourseId)
                //     {
                //         throw new SectionException.NotFound(data.SectionId.Value);
                //     }

                //     dto.SectionId = matchingSection.Id;
                // }

                if (data.GradeId.HasValue)
                {
                    var matchingGrade = grades.SingleOrDefault(x => x.Id == data.GradeId.Value);

                    if (matchingGrade is null)
                    {
                        throw new GradeException.NotFound(data.GradeId.Value);
                    }

                    dto.GradeId = matchingGrade.Id;
                    dto.GradeWeight = matchingGrade.Weight;
                    dto.GradePublishedAt = data.GradePublishedAt;
                }

                dtos.Add(dto);
            }

            var studyCourses = _studyCourseProvider.Create(dtos, userId.ToString());

            var response = MapStudyCourseDTOListToViewModel(studyCourses, students: new[] { student }
                                                            , terms: new[] { term }
                                                            , courses: courses).ToList(); // ,sections: section

            return response;
        }

        public IEnumerable<StudyCourseViewModel> GetByStudent(Guid studentId, Guid? termId = null)
        {
            var student = _studentProvider.GetById(studentId);

            var studyCourses = _studyCourseProvider.GetByStudent(studentId, termId, statuses: activeStudyCourseStatus)
                                                   .ToList();

            var response = MapStudyCourseDTOListToViewModel(studyCourses, students: new[] { student }).ToList();

            return response;
        }

        public IEnumerable<StudyCourseViewModel> GetBySectionId(Guid sectionId)
        {
            var studyCourses = _studyCourseProvider.GetBySectionId(sectionId, statuses: activeStudyCourseStatus)
                                                   .ToList();

            var response = MapStudyCourseDTOListToViewModel(studyCourses).ToList();

            return response;
        }

        public IEnumerable<StudyCourseViewModel> Update(IEnumerable<UpdateStudyCourseViewModel> request, Guid userId, Guid? studentId = null, Guid? sectionId = null)
        {
            if (request is null || !request.Any())
            {
                return Enumerable.Empty<StudyCourseViewModel>();
            }

            var studyCourseIds = request.Select(x => x.Id)
                                        .Distinct()
                                        .ToList();

            var studyCourses = _studyCourseProvider.GetById(studyCourseIds)
                                                   .ToList();

            var grades = _gradeProvider.GetAll()
                                       .ToList();

            var updateDTOs = new List<UpdateStudyCourseDTO>();

            foreach (var data in request)
            {
                var matchingStudyCourse = studyCourses.SingleOrDefault(x => x.Id == data.Id);

                if (matchingStudyCourse is null)
                {
                    throw new StudyCourseException.NotFound(data.Id);
                }

                if (studentId.HasValue && matchingStudyCourse.StudentId != studentId.Value)
                {
                    throw new StudyCourseException.NotFound(data.Id);
                }

                if (sectionId.HasValue && matchingStudyCourse.SectionId != sectionId.Value)
                {
                    throw new StudyCourseException.NotFound(data.Id);
                }

                var dto = new UpdateStudyCourseDTO
                {
                    Id = matchingStudyCourse.Id,
                    Status = data.Status,
                    GradeId = data.GradeId,
                    GradeWeight = data.GradeId.HasValue ? null
                                                        : matchingStudyCourse.GradeWeight,
                    GradePublishedAt = data.GradePublishedAt
                };

                if (matchingStudyCourse.Status != data.Status)
                {
                    if (matchingStudyCourse.Status > data.Status)
                    {
                        throw new StudyCourseException.NotAllowUpdateStatusBackward(matchingStudyCourse.Status, data.Status);
                    }
                }

                if (data.GradeId.HasValue
                    && matchingStudyCourse.GradeId != data.GradeId.Value)
                {
                    var matchingGrade = grades.SingleOrDefault(x => x.Id == data.GradeId.Value);

                    if (matchingGrade is null)
                    {
                        throw new GradeException.NotFound(data.GradeId.Value);
                    }

                    dto.GradeWeight = matchingGrade.Weight;
                }

                updateDTOs.Add(dto);
            }

            var updatedStudyCourses = _studyCourseProvider.Update(updateDTOs, userId.ToString())
                                                          .ToList();

            var response = MapStudyCourseDTOListToViewModel(updatedStudyCourses, grades: grades).ToList();

            return response;
        }

        private IEnumerable<StudyCourseViewModel> MapStudyCourseDTOListToViewModel(IEnumerable<StudyCourseDTO> studyCourses,
                                                                                   IEnumerable<StudentDTO> students = null,
                                                                                   IEnumerable<TermDTO> terms = null,
                                                                                   IEnumerable<CourseDTO> courses = null,
                                                                                   IEnumerable<SectionDTO> sections = null,
                                                                                   IEnumerable<GradeDTO> grades = null)
        {
            if (studyCourses is null || !studyCourses.Any())
            {
                return Enumerable.Empty<StudyCourseViewModel>();
            }

            if (students is null || !students.Any())
            {
                var studentIds = studyCourses.Select(x => x.StudentId)
                                             .Distinct()
                                             .ToList();

                students = _studentProvider.GetById(studentIds)
                                               .ToList();
            }

            if (terms is null || terms.Any())
            {
                var termIds = studyCourses.Select(x => x.TermId)
                                          .Distinct()
                                          .ToList();

                terms = _termProvider.GetById(termIds)
                                         .ToList();
            }

            if (courses is null || !courses.Any())
            {
                var courseIds = studyCourses.Select(x => x.CourseId)
                                            .Distinct()
                                            .ToList();

                courses = _courseProvider.GetById(courseIds)
                                             .ToList();
            }

            if (sections is null || !sections.Any())
            {
                var sectionIds = studyCourses.Where(x => x.SectionId.HasValue)
                                             .Select(x => x.SectionId.Value)
                                             .Distinct()
                                             .ToList();

                // sections = _sectionProvider.GetById(sectionIds)
                //                               .ToList();
            }

            if (grades is null || !grades.Any())
            {
                grades = _gradeProvider.GetAll()
                                       .ToList();
            }

            var response = (from data in studyCourses
                            let student = students.Single(x => x.Id == data.StudentId)
                            let term = terms.Single(x => x.Id == data.TermId)
                            let course = courses.SingleOrDefault(x => x.Id == data.CourseId)
                            let section = !data.SectionId.HasValue ? null
                                                                   : sections!.SingleOrDefault(x => x.Id == data.SectionId.Value)
                            let grade = !data.GradeId.HasValue ? null
                                                               : grades.SingleOrDefault(x => x.Id == data.GradeId.Value)
                            orderby term.Year, term.Number, course.Code
                            select MapDTOtoViewModel(data, student, term, course, section, grade))
                        .ToList();

            return response;
        }

        private static StudyCourseViewModel MapDTOtoViewModel(StudyCourseDTO dto, StudentDTO student, TermDTO term, CourseDTO course, SectionDTO? section, GradeDTO? grade)
        {
            var response = new StudyCourseViewModel
            {
                Id = dto.Id,
                TermId = dto.TermId,
                StudentId = dto.StudentId,
                StudentCode = student.Code,
                FirstName = student.FirstName,
                MiddleName = student.MiddleName,
                LastName = student.LastName,
                CourseId = dto.CourseId,
                CourseCode = course.Code,
                CourseName = course.Name,
                SectionId = dto.SectionId,
                SectionNumber = section?.Number,
                Status = dto.Status,
                RegistrationChannel = dto.RegistrationChannel,
                GradeId = dto.GradeId,
                GradeLetter = grade?.Letter,
                GradePublishedAt = dto.GradePublishedAt,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt
            };

            return response;
        }
    }
}

