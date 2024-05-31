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
    public class CourseTopicProvider : ICourseTopicProvider
    {
        private readonly DatabaseContext _dbContext;

        public CourseTopicProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public CourseTopicDTO Create(CreateCourseTopicDTO request, string requester)
        {
            var model = new CourseTopic
            {
                CourseId = request.CourseId,
                Code = request.Code,
                Name = request.Name,
                Description = request.Description,
                LectureHour = request.LectureHour,
                LabHour = request.LabHour,
                OtherHour = request.OtherHour,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = requester
            };

            var localizes = MapLocalizationDTOToModel(request.Localizations, model).ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.CourseTopics.Add(model);

                if (localizes.Any())
                {
                    _dbContext.CourseTopicLocalizations.AddRange(localizes);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapCourseTopicDTO(model, localizes);

            return response;
        }

        public CourseTopicDTO GetById(Guid courseTopicId)
        {
            var courseTopic = _dbContext.CourseTopics.AsNoTracking()
                                                     .Include(x => x.Localizations)
                                                     .SingleOrDefault(x => x.Id == courseTopicId);

            if (courseTopic is null)
            {
                throw new CourseTopicException.NotFound(courseTopicId);
            }

            var response = MapCourseTopicDTO(courseTopic, courseTopic.Localizations);

            return response;
        }

        public IEnumerable<CourseTopicDTO> GetById(IEnumerable<Guid> courseTopicIds)
        {
            var courseTopics = _dbContext.CourseTopics.AsNoTracking()
                                                      .Include(x => x.Localizations)
                                                      .Where(x => courseTopicIds.Contains(x.Id))
                                                      .ToList();

            var response = (from courseTopic in courseTopics
                            orderby courseTopic.Code, courseTopic.Name
                            select MapCourseTopicDTO(courseTopic, courseTopic.Localizations))
                           .ToList();

            return response;
        }

        public IEnumerable<CourseTopicDTO> GetByCourseId(Guid courseId)
        {
            var courseTopics = _dbContext.CourseTopics.AsNoTracking()
                                                      .Include(x => x.Localizations)
                                                      .Where(x => x.CourseId == courseId)
                                                      .ToList();

            var response = (from courseTopic in courseTopics
                            orderby courseTopic.Code, courseTopic.Name
                            select MapCourseTopicDTO(courseTopic, courseTopic.Localizations))
                           .ToList();

            return response;
        }

        public CourseTopicDTO Update(CourseTopicDTO request, string requester)
        {
            var courseTopic = _dbContext.CourseTopics.Include(x => x.Localizations)
                                                     .SingleOrDefault(x => x.Id == request.Id);

            if (courseTopic is null)
            {
                throw new CourseTopicException.NotFound(request.Id);
            }

            var localizes = MapLocalizationDTOToModel(request.Localizations, courseTopic).ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                courseTopic.Name = request.Name;
                courseTopic.Code = request.Code;
                courseTopic.Description = request.Description;
                courseTopic.LectureHour = request.LectureHour;
                courseTopic.LabHour = request.LabHour;
                courseTopic.OtherHour = request.OtherHour;
                courseTopic.UpdatedAt = DateTime.UtcNow;
                courseTopic.UpdatedBy = requester;
                courseTopic.IsActive = request.IsActive;

                _dbContext.CourseTopicLocalizations.RemoveRange(courseTopic.Localizations);

                if (localizes.Any())
                {
                    _dbContext.CourseTopicLocalizations.AddRange(localizes);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapCourseTopicDTO(courseTopic, localizes);

            return response;
        }

        public IEnumerable<CourseTopicDTO> Search(SearchCourseTopicCriteriaDTO parameters)
        {
            var query = GenerateSearchQuery(parameters);

            var courses = query.ToList();

            var response = (from course in courses
                            select MapCourseTopicDTO(course, course.Localizations))
                           .ToList();

            return response;
        }

        public PagedViewModel<CourseTopicDTO> Search(SearchCourseTopicCriteriaDTO parameters, int page, int pageSize)
        {
            var query = GenerateSearchQuery(parameters);

            var pagedCourse = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<CourseTopicDTO>
            {
                Page = pagedCourse.Page,
                TotalPage = pagedCourse.TotalPage,
                TotalItem = pagedCourse.TotalItem,
                Items = pagedCourse.Items is null ? Enumerable.Empty<CourseTopicDTO>()
                                                  : (from courseTopic in pagedCourse.Items
                                                     orderby courseTopic.Code, courseTopic.Name
                                                     select MapCourseTopicDTO(courseTopic, courseTopic.Localizations))
                                                    .ToList()
            };

            return response;
        }

        public void DeleteCourseTopic(Guid courseTopicId)
        {
            var courseTopic = _dbContext.CourseTopics.SingleOrDefault(x => x.Id == courseTopicId);

            if (courseTopic is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.CourseTopics.Remove(courseTopic);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        private IQueryable<CourseTopic> GenerateSearchQuery(SearchCourseTopicCriteriaDTO? parameters)
        {
            var query = _dbContext.CourseTopics.Include(x => x.Localizations)
                                               .AsNoTracking();

            if (parameters is not null)
            {
                if (!string.IsNullOrEmpty(parameters.Keyword))
                {
                    query = query.Where(x => !string.IsNullOrEmpty(x.Code)
                                                 && x.Code.Contains(parameters.Keyword)
                                             || (!string.IsNullOrEmpty(x.Name)
                                                 && x.Name.Contains(parameters.Keyword)));
                }

                if (parameters.CourseId.HasValue)
                {
                    query = query.Where(x => x.CourseId == parameters.CourseId.Value);
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

        private static IEnumerable<CourseTopicLocalization> MapLocalizationDTOToModel(
            IEnumerable<CourseTopicLocalizationDTO>? localizations,
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

        private static CourseTopicDTO MapCourseTopicDTO(CourseTopic courseTopic, IEnumerable<CourseTopicLocalization> localizations)
        {
            return new CourseTopicDTO
            {
                Id = courseTopic.Id,
                CourseId = courseTopic.CourseId,
                Code = courseTopic.Code,
                Name = courseTopic.Name,
                Description = courseTopic.Description,
                LabHour = courseTopic.LabHour,
                LectureHour = courseTopic.LectureHour,
                OtherHour = courseTopic.OtherHour,
                CreatedAt = courseTopic.CreatedAt,
                UpdatedAt = courseTopic.UpdatedAt,
                IsActive = courseTopic.IsActive,
                Localizations = localizations is null ? Enumerable.Empty<CourseTopicLocalizationDTO>()
                                                      : (from localize in localizations
                                                         orderby localize.Language
                                                         select new CourseTopicLocalizationDTO
                                                         {
                                                             Language = localize.Language,
                                                             Name = localize.Name,
                                                             Description = localize.Description
                                                         })
                                                        .ToList()
            };
        }
    }
}

