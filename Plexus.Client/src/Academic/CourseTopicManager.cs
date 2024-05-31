using Microsoft.EntityFrameworkCore;
using Plexus.Client.ViewModel.Academic;
using Plexus.Database;
using Plexus.Database.Model;
using Plexus.Database.Model.Academic;
using Plexus.Database.Model.Localization.Academic;
using Plexus.Entity.Exception;
using Plexus.Utility.Extensions;
using Plexus.Utility.ViewModel;
using ServiceStack;

namespace Plexus.Client.src.Payment
{
    public class CourseTopicManager : ICourseTopicManager
    {
        private readonly DatabaseContext _dbContext;
        private readonly IUnitOfWork _uow;
        private readonly IAsyncRepository<CourseTopic> _courseTopicRepository;
        private readonly IAsyncRepository<CourseTopicLocalization> _courseTopicLocalizationRepository;
        private readonly IAsyncRepository<CourseTopicInstructor> _courseTopicInstructorRepository;
        private readonly IAsyncRepository<Employee> _employeeRepository;
        private readonly IAsyncRepository<Course> _courseRepository;

        public CourseTopicManager(IUnitOfWork uow,
                                  IAsyncRepository<CourseTopic> courseTopicRepository,
                                  IAsyncRepository<CourseTopicLocalization> courseTopicLocalizationRepository,
                                  IAsyncRepository<CourseTopicInstructor> courseTopicInstructorRepository,
                                  IAsyncRepository<Employee> employeeRepository,
                                  IAsyncRepository<Course> courseRepository,
                                  DatabaseContext dbContext)
        {
            _dbContext = dbContext;
            _uow = uow;
            _courseTopicRepository = courseTopicRepository;
            _courseTopicLocalizationRepository = courseTopicLocalizationRepository;
            _courseTopicInstructorRepository = courseTopicInstructorRepository;
            _employeeRepository = employeeRepository;
            _courseRepository = courseRepository;
        }

        public CourseTopicViewModel Create(Guid courseId, CreateCourseTopicViewModel request, Guid userId)
        {
            var course = _courseRepository.Query()
                                          .Include(x => x.Localizations)
                                          .FirstOrDefault(x => x.Id == courseId);

            if (course is null)
            {
                throw new CourseException.NotFound(courseId);
            }

            var model = new CourseTopic
            {
                CourseId = courseId,
                Code = request.Code,
                Name = request.Name,
                Description = request.Description,
                LectureHour = request.LectureHour,
                LabHour = request.LabHour,
                OtherHour = request.OtherHour,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "", //  TODO : Add requester
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = "" // TODO : Add requester
            };

            var localizes = MapLocalizationViewModelToModel(request.Localizations, model).ToList();

            var topicInstructors = MapInstructorToCourseTopic(request.InstructorIds, model).ToList();

            _uow.BeginTran();
            _courseTopicRepository.Add(model);

            if (localizes.Any())
            {
                _courseTopicLocalizationRepository.AddRange(localizes);
            }

            if (topicInstructors.Any())
            {
                _courseTopicInstructorRepository.AddRange(topicInstructors);
            }

            _uow.Complete();
            _uow.CommitTran();

            var instructorIds = topicInstructors.Select(x => x.InstructorId)
                                                .ToList();

            var instructors = _employeeRepository.Query()
                                                 .Include(x => x.Localizations)
                                                 .Where(x => instructorIds.Contains(x.Id))
                                                 .ToList();

            var response = MapCourseTopicViewModel(model, course, instructors, localizes);

            return response;
        }

        public PagedViewModel<CourseTopicViewModel> Search(Guid courseId, int page = 1, int pageSize = 25)
        {
            var query = _courseTopicRepository.Query()
                                              .Include(x => x.Localizations)
                                              .Where(x => x.CourseId == courseId);

            var pagedCourseTopics = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<CourseTopicViewModel>
            {
                Page = pagedCourseTopics.Page,
                PageSize = pagedCourseTopics.PageSize,
                TotalPage = pagedCourseTopics.TotalPage,
                TotalItem = pagedCourseTopics.TotalItem,
                Items = (from courseTopic in pagedCourseTopics.Items

                         let course = _courseRepository.Query()
                                                       .Include(x => x.Localizations)
                                                       .FirstOrDefault(x => x.Id == courseId)
                                                                       ?? throw new CourseException.NotFound(courseId)

                         let instructorIds = _courseTopicInstructorRepository.Query()
                                                                             .Where(x => x.CourseTopicId == courseTopic.Id)
                                                                             .Select(x => x.InstructorId)
                                                                             .ToList()

                         let instructors = _employeeRepository.Query()
                                                              .Where(x => instructorIds.Contains(x.Id))
                                                              .ToList()

                         select MapCourseTopicViewModel(courseTopic, course, instructors, courseTopic.Localizations))
            };

            return response;
        }

        public IEnumerable<CourseTopicViewModel> GetByCourseId(Guid courseId)
        {
            var courseTopics = _courseTopicRepository.Query()
                                                     .Include(x => x.Localizations)
                                                     .Where(x => x.CourseId == courseId)
                                                     .ToList();

            var response = (from courseTopic in courseTopics
                            orderby courseTopic.Code, courseTopic.Name

                            let course = _courseRepository.Query()
                                                          .Include(x => x.Localizations)
                                                          .FirstOrDefault(x => x.Id == courseId)
                                                                          ?? throw new CourseException.NotFound(courseId)


                            let instructorIds = _courseTopicInstructorRepository.Query()
                                                                                .Where(x => x.CourseTopicId == courseTopic.Id)
                                                                                .Select(x => x.InstructorId)
                                                                                .ToList()

                            let instructors = _employeeRepository.Query()
                                                                 .Where(x => instructorIds.Contains(x.Id))
                                                                 .ToList()

                            select MapCourseTopicViewModel(courseTopic, course, instructors, courseTopic.Localizations))
                           .ToList();

            return response;
        }

        public CourseTopicViewModel GetById(Guid courseTopicId)
        {
            var courseTopic = _courseTopicRepository.Query()
                                                    .Include(x => x.Localizations)
                                                    .FirstOrDefault(x => x.Id == courseTopicId);

            if (courseTopic is null)
            {
                throw new CourseTopicException.NotFound(courseTopicId);
            }

            var course = _courseRepository.Query()
                                          .Include(x => x.Localizations)
                                          .FirstOrDefault(x => x.Id == courseTopic.CourseId);

            if (course is null)
            {
                throw new CourseException.NotFound(courseTopic.CourseId);
            }

            var instructorIds = _courseTopicInstructorRepository.Query()
                                                                .Where(x => x.CourseTopicId == courseTopicId)
                                                                .Select(x => x.InstructorId)
                                                                .ToList();

            var instructors = _employeeRepository.Query()
                                                 .Include(x => x.Localizations)
                                                 .Where(x => instructorIds.Contains(x.Id))
                                                 .ToList();

            var response = MapCourseTopicViewModel(courseTopic, course, instructors, courseTopic.Localizations);

            return response;
        }

        public CourseTopicViewModel Update(Guid courseId, Guid courseTopicId, CreateCourseTopicViewModel request, Guid userId)
        {
            var course = _courseRepository.Query()
                                          .Include(x => x.Localizations)
                                          .FirstOrDefault(x => x.Id == courseId);

            if (course is null)
            {
                throw new CourseException.NotFound(courseId);
            }

            var courseTopic = _courseTopicRepository.Query()
                                                    .Include(x => x.Localizations)
                                                    .SingleOrDefault(x => x.Id == courseTopicId);

            if (courseTopic is null)
            {
                throw new CourseTopicException.NotFound(courseTopicId);
            }

            var localizes = MapLocalizationViewModelToModel(request.Localizations, courseTopic).ToList();

            var topicInstructors = MapInstructorToCourseTopic(request.InstructorIds, courseTopic).ToList();

            courseTopic.Name = request.Name;
            courseTopic.Code = request.Code;
            courseTopic.Description = request.Description;
            courseTopic.LectureHour = request.LectureHour;
            courseTopic.LabHour = request.LabHour;
            courseTopic.OtherHour = request.OtherHour;
            courseTopic.UpdatedAt = DateTime.UtcNow;
            courseTopic.UpdatedBy = ""; // TODO : Add requester
            courseTopic.IsActive = request.IsActive;

            _uow.BeginTran();
            _courseTopicRepository.Update(courseTopic);


            if (courseTopic.Instructors != null)
            {
                _courseTopicInstructorRepository.DeleteRange(courseTopic.Instructors.ToList());
            }

            if (topicInstructors.Any())
            {
                _courseTopicInstructorRepository.AddRange(topicInstructors);
            }

            _courseTopicLocalizationRepository.DeleteRange(courseTopic.Localizations.ToList());

            if (localizes.Any())
            {
                _courseTopicLocalizationRepository.AddRange(localizes);
            }

            _uow.Complete();
            _uow.CommitTran();

            var instructorIds = topicInstructors.Select(x => x.InstructorId)
                                                .ToList();

            var instructors = _employeeRepository.Query()
                                                 .Include(x => x.Localizations)
                                                 .Where(x => instructorIds.Contains(x.Id))
                                                 .ToList();

            var response = MapCourseTopicViewModel(courseTopic, course, instructors, localizes);

            return response;
        }

        public void Delete(Guid courseTopicId)
        {
            var courseTopic = _courseTopicRepository.Query()
                                                    .FirstOrDefault(x => x.Id == courseTopicId);

            if (courseTopic is null)
            {
                return;
            }

            _uow.BeginTran();
            _courseTopicRepository.Delete(courseTopic);
            _uow.Complete();
            _uow.CommitTran();
        }

        private static CourseTopicViewModel MapCourseTopicViewModel(
                       CourseTopic courseTopic,
                       Course course,
                       IEnumerable<Employee>? instructors,
                       IEnumerable<CourseTopicLocalization>? localizations)
        {
            return new CourseTopicViewModel
            {
                Id = courseTopic.Id,
                Code = courseTopic.Code,
                LabHour = courseTopic.LabHour,
                LectureHour = courseTopic.LectureHour,
                OtherHour = courseTopic.OtherHour,
                CreatedAt = courseTopic.CreatedAt,
                UpdatedAt = courseTopic.UpdatedAt,
                IsActive = courseTopic.IsActive,
                CourseId = course.Id,
                CourseName = course.Name,
                Instructors = instructors is null ? Enumerable.Empty<CourseTopicInstructorViewModel>()
                                                  : (from instructor in instructors
                                                     orderby instructor.Code
                                                     select new CourseTopicInstructorViewModel
                                                     {
                                                         InstructorId = instructor.Id,
                                                         InstructorFirstName = instructor.FirstName,
                                                         InstructorMiddleName = instructor.MiddleName is null ? null
                                                                                                              : instructor.MiddleName,
                                                         InstructorLastName = instructor.LastName
                                                     })
                                                     .ToList(),
                Localizations = localizations is null ? Enumerable.Empty<CourseTopicLocalizationViewModel>()
                                                      : (from localize in localizations
                                                         orderby localize.Language
                                                         select new CourseTopicLocalizationViewModel
                                                         {
                                                             Language = localize.Language,
                                                             Name = localize.Name,
                                                             Description = localize.Description
                                                         })
                                                        .ToList()
            };
        }

        private static IEnumerable<CourseTopicLocalization> MapLocalizationViewModelToModel(
                       IEnumerable<CourseTopicLocalizationViewModel>? localizations,
                       CourseTopic model)
        {
            if (localizations is null)
            {
                return Enumerable.Empty<CourseTopicLocalization>();
            }

            var response = (from locale in localizations
                            select new CourseTopicLocalization
                            {
                                CourseTopic = model,
                                Language = locale.Language,
                                Name = locale.Name,
                                Description = locale.Description,
                            })
                           .ToList();

            return response;
        }

        private IEnumerable<CourseTopicInstructor> MapInstructorToCourseTopic(
                       IEnumerable<Guid>? instructorIds,
                       CourseTopic courseTopic
        )
        {
            if (instructorIds is null)
            {
                return Enumerable.Empty<CourseTopicInstructor>();
            }

            var instructors = _dbContext.Employees.AsNoTracking()
                                                    .Include(x => x.Localizations)
                                                    .Where(x => instructorIds.Contains(x.Id))
                                                    .ToList();

            var response = (from instructor in instructors
                            select new CourseTopicInstructor
                            {
                                CourseTopic = courseTopic,
                                InstructorId = instructor.Id
                            })
                            .ToList();

            return response;
        }
    }
}