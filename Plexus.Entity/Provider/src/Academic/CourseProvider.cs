using Microsoft.EntityFrameworkCore;
using Plexus.Database;
using Plexus.Database.Model.Academic;
using Plexus.Database.Model.Localization.Academic;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Academic;
using Plexus.Entity.Exception;
using Plexus.Utility.Extensions;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider.src.Academic
{
    public class CourseProvider : ICourseProvider
    {
        private readonly DatabaseContext _dbContext;

        public CourseProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public CourseDTO Create(CreateCourseDTO request, string requester)
        {
            var model = new Course
            {
                Code = request.Code,
                Name = request.Name,
                Description = request.Description,
                TranscriptName1 = request.TranscriptName1,
                TranscriptName2 = request.TranscriptName2,
                TranscriptName3 = request.TranscriptName3,
                AcademicLevelId = request.AcademicLevelId,
                FacultyId = request.FacultyId,
                DepartmentId = request.DepartmentId,
                TeachingTypeId = request.TeachingTypeId,
                GradeTemplateId = request.GradeTemplateId,
                Credit = request.Credit,
                RegistrationCredit = request.RegistrationCredit,
                PaymentCredit = request.PaymentCredit,
                Hour = request.Hour,
                LectureCredit = request.LectureCredit,
                LabCredit = request.LabCredit,
                OtherCredit = request.OtherCredit,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = requester
            };

            var localizes = MapLocalizationDTOToModel(request.Localizations, model).ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Courses.Add(model);

                if (localizes.Any())
                {
                    _dbContext.CourseLocalizations.AddRange(localizes);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapCourseDTO(model, localizes);

            return response;
        }

        public CourseDTO GetById(Guid courseId)
        {
            var course = _dbContext.Courses.AsNoTracking()
                                           .Include(x => x.Localizations)
                                           .SingleOrDefault(x => x.Id == courseId);

            if (course is null)
            {
                throw new CourseException.NotFound(courseId);
            }

            var response = MapCourseDTO(course, course.Localizations);

            return response;
        }

        public IEnumerable<CourseDTO> GetById(IEnumerable<Guid> courseIds)
        {
            var courses = _dbContext.Courses.AsNoTracking()
                                            .Include(x => x.Localizations)
                                            .Where(x => courseIds.Contains(x.Id))
                                            .ToList();

            var response = (from course in courses
                            orderby course.Code, course.Name
                            select MapCourseDTO(course, course.Localizations))
                           .ToList();

            return response;
        }

        public CourseDTO Update(CourseDTO request, string requester)
        {
            var course = _dbContext.Courses.Include(x => x.Localizations)
                                           .SingleOrDefault(x => x.Id == request.Id);

            if (course is null)
            {
                throw new CourseException.NotFound(request.Id);
            }

            var localizes = MapLocalizationDTOToModel(request.Localizations, course).ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                course.Name = request.Name;
                course.Code = request.Code;
                course.Description = request.Description;
                course.TranscriptName1 = request.TranscriptName1;
                course.TranscriptName2 = request.TranscriptName2;
                course.TranscriptName3 = request.TranscriptName3;
                course.AcademicLevelId = request.AcademicLevelId;
                course.FacultyId = request.FacultyId;
                course.DepartmentId = request.DepartmentId;
                course.TeachingTypeId = request.TeachingTypeId;
                course.GradeTemplateId = request.GradeTemplateId;
                course.Credit = request.Credit;
                course.RegistrationCredit = request.RegistrationCredit;
                course.PaymentCredit = request.PaymentCredit;
                course.Hour = request.Hour;
                course.LectureCredit = request.LectureCredit;
                course.LabCredit = request.LabCredit;
                course.OtherCredit = request.OtherCredit;
                course.UpdatedAt = DateTime.UtcNow;
                course.UpdatedBy = requester;
                course.IsActive = request.IsActive;

                _dbContext.CourseLocalizations.RemoveRange(course.Localizations);

                if (localizes.Any())
                {
                    _dbContext.CourseLocalizations.AddRange(localizes);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapCourseDTO(course, localizes);

            return response;
        }

        public IEnumerable<CourseDTO> Search(SearchCourseCriteriaDTO parameters)
        {
            var query = GenerateSearchQuery(parameters);

            var courses = query.ToList();

            var response = (from course in courses
                            select MapCourseDTO(course, course.Localizations))
                           .ToList();

            return response;
        }

        public PagedViewModel<CourseDTO> Search(SearchCourseCriteriaDTO parameters, int page, int pageSize)
        {
            var query = GenerateSearchQuery(parameters);

            var pagedCourse = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<CourseDTO>
            {
                Page = pagedCourse.Page,
                TotalPage = pagedCourse.TotalPage,
                TotalItem = pagedCourse.TotalItem,
                Items = pagedCourse.Items is null ? Enumerable.Empty<CourseDTO>()
                                                  : (from course in pagedCourse.Items
                                                     orderby course.Code, course.Name
                                                     select MapCourseDTO(course, course.Localizations))
                                                    .ToList()
            };

            return response;
        }

        public void DeleteCourse(Guid courseId)
        {
            var course = _dbContext.Courses.SingleOrDefault(x => x.Id == courseId);

            if (course is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Courses.Remove(course);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        public IEnumerable<CourseDTO> GetTermAvailableCourse(Guid termId)
        {
            return Enumerable.Empty<CourseDTO>();

            //var courseIds = _dbContext.Sections.AsNoTracking()
            //                                   .Where(x => x.TermId == termId)
            //                                   .Select(x => x.CourseId)
            //                                   .Distinct()
            //                                   .ToList();

            //if (!courseIds.Any())
            //{
            //    return Enumerable.Empty<CourseDTO>();
            //}

            //var courses = _dbContext.Courses.AsNoTracking()
            //                                .Where(x => courseIds.Contains(x.Id))
            //                                .ToList();

            //var response = (from course in courses
            //                orderby course.Code, course.Name
            //                select MapCourseDTO(course))
            //               .ToList();

            //return response;
        }

        private IQueryable<Course> GenerateSearchQuery(SearchCourseCriteriaDTO? parameters)
        {
            var query = _dbContext.Courses.Include(x => x.Localizations)
                                          .AsNoTracking();

            if (parameters is not null)
            {
                if (!string.IsNullOrEmpty(parameters.Keyword))
                {
                    query = query.Where(x => !string.IsNullOrEmpty(x.Code)
                                                 && x.Code.Contains(parameters.Keyword)
                                             || (!string.IsNullOrEmpty(x.Name)
                                                 && x.Name.Contains(parameters.Keyword))
                                             || (!string.IsNullOrEmpty(x.TranscriptName1)
                                                 && x.TranscriptName1.Contains(parameters.Keyword))
                                             || (!string.IsNullOrEmpty(x.TranscriptName2)
                                                 && x.TranscriptName2.Contains(parameters.Keyword))
                                             || (!string.IsNullOrEmpty(x.TranscriptName3)
                                                 && x.TranscriptName3.Contains(parameters.Keyword))
                                             || (!string.IsNullOrEmpty(x.Description)
                                                 && x.Description.Contains(parameters.Keyword)));
                }

                if (parameters.AcademicLevelId.HasValue)
                {
                    query = query.Where(x => x.AcademicLevelId == parameters.AcademicLevelId.Value);
                }

                if (parameters.FacultyId.HasValue)
                {
                    query = query.Where(x => x.FacultyId == parameters.FacultyId.Value);
                }

                if (parameters.DepartmentId.HasValue)
                {
                    query = query.Where(x => x.DepartmentId == parameters.DepartmentId.Value);
                }

                if (parameters.IsActive.HasValue)
                {
                    query = query.Where(x => x.IsActive == parameters.IsActive.Value);
                }
            }

            query = query.OrderBy(x => x.Code)
                         .ThenBy(x => x.Name);

            if (parameters is not null)
            {
                if (!string.IsNullOrEmpty(parameters.SortBy))
                {
                    try
                    {
                        query = query.OrderBy(parameters.SortBy, parameters.OrderBy);
                    }
                    catch (System.Exception)
                    {
                        // invalid property name
                    }
                }
            }

            return query;
        }

        private static IEnumerable<CourseLocalization> MapLocalizationDTOToModel(
            IEnumerable<CourseLocalizationDTO>? localizations,
            Course model)
        {
            if (localizations is null)
            {
                return Enumerable.Empty<CourseLocalization>();
            }

            var response = (from locale in localizations
                            select new CourseLocalization
                            {
                                Course = model,
                                Language = locale.Language,
                                Name = locale.Name,
                                Description = locale.Description,
                                TranscriptName1 = locale.TranscriptName1,
                                TranscriptName2 = locale.TranscriptName2,
                                TranscriptName3 = locale.TranscriptName3
                            })
                           .ToList();

            return response;
        }

        private static CourseDTO MapCourseDTO(Course course, IEnumerable<CourseLocalization> localizations)
        {
            return new CourseDTO
            {
                Id = course.Id,
                Code = course.Code,
                Name = course.Name,
                Description = course.Description,
                TranscriptName1 = course.TranscriptName1,
                TranscriptName2 = course.TranscriptName2,
                TranscriptName3 = course.TranscriptName3,
                AcademicLevelId = course.AcademicLevelId,
                FacultyId = course.FacultyId,
                DepartmentId = course.DepartmentId,
                TeachingTypeId = course.TeachingTypeId,
                GradeTemplateId = course.GradeTemplateId,
                Credit = course.Credit,
                RegistrationCredit = course.RegistrationCredit,
                PaymentCredit = course.PaymentCredit,
                Hour = course.Hour,
                LectureCredit = course.LectureCredit,
                LabCredit = course.LabCredit,
                OtherCredit = course.OtherCredit,
                CreatedAt = course.CreatedAt,
                UpdatedAt = course.UpdatedAt,
                IsActive = course.IsActive,
                Localizations = localizations is null ? Enumerable.Empty<CourseLocalizationDTO>()
                                                      : (from localize in localizations
                                                         orderby localize.Language
                                                         select new CourseLocalizationDTO
                                                         {
                                                             Language = localize.Language,
                                                             Name = localize.Name,
                                                             TranscriptName1 = localize.TranscriptName1,
                                                             TranscriptName2 = localize.TranscriptName2,
                                                             TranscriptName3 = localize.TranscriptName3,
                                                             Description = localize.Description
                                                         })
                                                        .ToList()
            };
        }
    }
}

