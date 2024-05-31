using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Plexus.Database;
using Plexus.Database.Model.Registration;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Registration;
using Plexus.Utility.ViewModel;
using Plexus.Utility.Extensions;

namespace Plexus.Entity.Provider.src.Registration
{
    public class PrerequisiteProvider : IPrerequisiteProvider
    {
        private readonly DatabaseContext _dbContext;

        public PrerequisiteProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<CoursePrerequisiteDTO> GetCoursePrerequisiteByCourseId(IEnumerable<Guid> courseIds)
        {
            var prerequisites = _dbContext.CoursePrerequisites.AsNoTracking()
                                                              .Where(x => courseIds.Contains(x.CourseId)
                                                                          && !x.DeactivatedAt.HasValue)
                                                              .ToList();

            var response = (from prerequisite in prerequisites
                            select MapCourseModelToDTO(prerequisite))
                           .ToList();

            return response;
        }

        public PagedViewModel<CoursePrerequisiteDTO> SearchCoursePrerequisite(SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var query = _dbContext.CoursePrerequisites.AsNoTracking()
                                                      .Where(x => !x.DeactivatedAt.HasValue);

            if (parameters is not null)
            {
                if (parameters.CourseId.HasValue)
                {
                    query = query.Where(x => x.CourseId == parameters.CourseId);
                }
            }

            query = query.OrderBy(x => x.CreatedAt);

            var pagedPrerequisite = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<CoursePrerequisiteDTO>
            {
                Page = pagedPrerequisite.Page,
                TotalPage = pagedPrerequisite.TotalPage,
                TotalItem = pagedPrerequisite.TotalItem,
                Items = (from prerequisite in pagedPrerequisite.Items
                         select MapCourseModelToDTO(prerequisite))
                        .ToList()
            };

            return response;
        }

        public CoursePrerequisiteDTO UpdateCoursePrerequisite(Guid courseId, IEnumerable<CreatePrerequisiteConditionDTO> requests, string requester)
        {
            var existingPrerequisite = _dbContext.CoursePrerequisites.SingleOrDefault(x => x.CourseId == courseId
                                                                                           && !x.DeactivatedAt.HasValue);

            var model = new CoursePrerequisite
            {
                CourseId = courseId,
                Conditions = (from request in requests
                              select JsonConvert.SerializeObject(request)),
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester
            };

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                if (existingPrerequisite is not null)
                {
                    existingPrerequisite.DeactivatedAt = DateTime.UtcNow;
                }

                _dbContext.CoursePrerequisites.Add(model);

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapCourseModelToDTO(model);

            return response;
        }

        public void DeleteCoursePrerequisite(Guid courseId)
        {
            var prerequisite = _dbContext.CoursePrerequisites.SingleOrDefault(x => x.CourseId == courseId
                                                                                   && !x.DeactivatedAt.HasValue);

            if (prerequisite is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                prerequisite.DeactivatedAt = DateTime.UtcNow;

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        private static CoursePrerequisiteDTO MapCourseModelToDTO(CoursePrerequisite model)
        {
            var response = new CoursePrerequisiteDTO
            {
                Id = model.Id,
                CourseId = model.CourseId,
                Condition = (from condition in model.Conditions
                             select JsonConvert.DeserializeObject<CreatePrerequisiteConditionDTO>(condition)),
                CreatedAt = model.CreatedAt,
                DeactivatedAt = model.DeactivatedAt
            };

            return response;
        }
    }
}