using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Client.ViewModel.Academic;
using Plexus.Client.ViewModel.DropDown;
using Plexus.Utility.ViewModel;
using Plexus.Database;
using Microsoft.EntityFrameworkCore;
using Plexus.Entity.Exception;
using Plexus.Database.Model.Academic;
using Plexus.Database.Model.Localization.Academic;
using Plexus.Database.Model.Academic.Faculty;
using Plexus.Utility.Extensions;
using ServiceStack;

namespace Plexus.Client.src.Academic
{
    public class CourseManager : ICourseManager
    {
        private readonly DatabaseContext _dbContext;

        public CourseManager(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public CourseViewModel Create(CreateCourseViewModel request, Guid userId)
        {
            var academicLevel = _dbContext.AcademicLevels.Include(x => x.Localizations)
                                                         .FirstOrDefault(x => x.Id == request.AcademicLevelId);

            if (academicLevel is null)
            {
                throw new AcademicLevelException.NotFound(request.AcademicLevelId);
            }

            var faculty = request.FacultyId.HasValue ? _dbContext.Faculties.Include(x => x.Localizations)
                                                                           .FirstOrDefault(x => x.Id == request.FacultyId.Value)
                                                     : null;

            var department = request.DepartmentId.HasValue ? _dbContext.Departments.Include(x => x.Localizations)
                                                                                   .FirstOrDefault(x => x.Id == request.DepartmentId.Value)
                                                           : null;

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
                CreatedBy = "", // TODO : Add requester
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = "" // TODO : Add requester
            };

            var localizes = MapLocalizationViewModelToModel(request.Localizations, model).ToList();

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

            var response = MapCourseToViewModel(model, academicLevel, faculty, department);

            return response;
        }

        public IEnumerable<CourseViewModel> Search(SearchCourseCriteriaViewModel parameters)
        {
            var query = GenerateSearchQuery(parameters);

            var courses = query.ToList();

            var response = (from course in courses
                            select MapCourseToViewModel(course))
                           .ToList();

            return response;
        }


        public IEnumerable<CourseDropDownViewModel> GetDropDownList(SearchCourseCriteriaViewModel parameters)
        {
            var courses = Search(parameters);

            var response = (from course in courses
                            select new CourseDropDownViewModel
                            {
                                Id = course.Id.ToString(),
                                Name = course.Name,
                                Code = course.Code,
                                AcademicLevelId = course.AcademicLevelId,
                                FacultyId = course.FacultyId,
                                DepartmentId = course.DepartmentId
                            })
                           .ToList();

            return response;
        }

        public PagedViewModel<CourseViewModel> Search(SearchCourseCriteriaViewModel parameters, int page, int pageSize)
        {
            var query = GenerateSearchQuery(parameters);

            var pagedCourse = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<CourseViewModel>
            {
                Page = pagedCourse.Page,
                PageSize = pagedCourse.PageSize,
                TotalPage = pagedCourse.TotalPage,
                TotalItem = pagedCourse.TotalItem,
                Items = pagedCourse.Items is null ? Enumerable.Empty<CourseViewModel>()
                                                  : (from course in pagedCourse.Items
                                                     orderby course.Code, course.Name
                                                     select MapCourseToViewModel(course, course.AcademicLevel, course.Faculty, course.Department))
                                                    .ToList()
            };

            return response;
        }

        public CourseViewModel GetById(Guid courseId)
        {
            var course = _dbContext.Courses.AsNoTracking()
                                           .Include(x => x.Localizations)
                                           .FirstOrDefault(x => x.Id == courseId);

            if (course is null)
            {
                throw new AcademicLevelException.NotFound(courseId);
            }

            var academicLevel = _dbContext.AcademicLevels.AsNoTracking()
                                                         .Include(x => x.Localizations)
                                                         .FirstOrDefault(x => x.Id == course.AcademicLevelId);

            var faculty = course.FacultyId.HasValue ? _dbContext.Faculties.AsNoTracking()
                                                                          .Include(x => x.Localizations)
                                                                          .FirstOrDefault(x => x.Id == course.FacultyId.Value)
                                                    : null;

            var department = course.DepartmentId.HasValue ? _dbContext.Departments.AsNoTracking()
                                                                      .Include(x => x.Localizations)
                                                                      .FirstOrDefault(x => x.Id == course.DepartmentId.Value)
                                                          : null;

            var response = MapCourseToViewModel(course, academicLevel, faculty, department);

            return response;
        }

        public CourseViewModel Update(Guid id, CreateCourseViewModel request, Guid userId)
        {
            var course = _dbContext.Courses.Include(x => x.Localizations)
                                           .SingleOrDefault(x => x.Id == id);

            if (course is null)
            {
                throw new CourseException.NotFound(id);
            }

            var localizes = MapLocalizationViewModelToModel(request.Localizations, course).ToList();

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
                course.UpdatedBy = ""; // TODO : Add requester
                course.IsActive = request.IsActive;

                _dbContext.CourseLocalizations.RemoveRange(course.Localizations);

                if (localizes.Any())
                {
                    _dbContext.CourseLocalizations.AddRange(localizes);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapCourseToViewModel(course, course.AcademicLevel, course.Faculty, course.Department);

            return response;
        }

        public void Delete(Guid courseId)
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

        private static CourseViewModel MapCourseToViewModel(Course course, AcademicLevel? academicLevel = null, Faculty? faculty = null, Department? department = null)
        {
            return new CourseViewModel
            {
                Id = course.Id,
                Code = course.Code,
                AcademicLevelId = course.AcademicLevelId,
                FacultyId = course.FacultyId,
                DepartmentId = course.DepartmentId,
                TeachingTypeId = course.TeachingTypeId,
                GradeTemplateId = course.GradeTemplateId,
                Credit = course.Credit,
                RegistrationCredit = course.RegistrationCredit,
                PaymentCredit = course.PaymentCredit,
                Hour = course.Hour,
                LabCredit = course.LabCredit,
                LectureCredit = course.LectureCredit,
                OtherCredit = course.OtherCredit,
                CreatedAt = course.CreatedAt,
                UpdatedAt = course.UpdatedAt,
                IsActive = course.IsActive,
                AcademicLevelName = academicLevel?.Name,
                FacultyName = faculty?.Name,
                DepartmentName = department?.Name,
                Localizations = (from localize in course.Localizations
                                 orderby localize.Language
                                 select new CourseLocalizationViewModel
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

        private static IEnumerable<CourseLocalization> MapLocalizationViewModelToModel(IEnumerable<CourseLocalizationViewModel>? localizations, Course model)
        {
            if (localizations is null)
            {
                return Enumerable.Empty<CourseLocalization>();
            }

            var response = (from locale in localizations
                            orderby locale.Language
                            select new CourseLocalization
                            {
                                Language = locale.Language,
                                Course = model,
                                Name = locale.Name,
                                Description = locale.Description,
                                TranscriptName1 = locale.TranscriptName1,
                                TranscriptName2 = locale.TranscriptName2,
                                TranscriptName3 = locale.TranscriptName3
                            })
                            .ToList();

            return response;
        }

        private IQueryable<Course> GenerateSearchQuery(SearchCourseCriteriaViewModel? parameters)
        {
            var query = _dbContext.Courses.Include(x => x.Localizations)
                                          .Include(x => x.AcademicLevel)
                                            .ThenInclude(x => x.Localizations)
                                          .Include(x => x.Faculty)
                                            .ThenInclude(x => x.Localizations)
                                          .Include(x => x.Department)
                                            .ThenInclude(x => x.Localizations)
                                          .AsNoTracking();

            if (parameters is not null)
            {
                if (!string.IsNullOrEmpty(parameters.Code))
                {
                    query = query.Where(x => !string.IsNullOrEmpty(x.Code)
                                                && x.Code.Contains(parameters.Code));
                }

                if (!string.IsNullOrEmpty(parameters.Name))
                {
                    query = query.Where(x => (!string.IsNullOrEmpty(x.Name)
                                                 && x.Name.Contains(parameters.Name))
                                             || (!string.IsNullOrEmpty(x.TranscriptName1)
                                                 && x.TranscriptName1.Contains(parameters.Name))
                                             || (!string.IsNullOrEmpty(x.TranscriptName2)
                                                 && x.TranscriptName2.Contains(parameters.Name))
                                             || (!string.IsNullOrEmpty(x.TranscriptName3)
                                                 && x.TranscriptName3.Contains(parameters.Name))
                                             || (!string.IsNullOrEmpty(x.Description)
                                                 && x.Description.Contains(parameters.Name)));
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
    }
}

