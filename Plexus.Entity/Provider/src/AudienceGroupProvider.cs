using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Plexus.Database;
using Plexus.Database.Model;
using Plexus.Entity.DTO;
using Plexus.Utility.ViewModel;
using Plexus.Utility.Extensions;
using Plexus.Entity.Exception;

namespace Plexus.Entity.Provider.src
{
    public class AudienceGroupProvider : IAudienceGroupProvider
    {
        private readonly DatabaseContext _dbContext;

        public AudienceGroupProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public AudienceGroupDTO Create(CreateAudienceGroupDTO request, string requester)
        {
            var model = new AudienceGroup
            {
                Name = request.Name,
                Description = request.Description,
                Conditions = JsonConvert.SerializeObject(request.Conditions),
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = requester
            };

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.AudienceGroups.Add(model);

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(model);

            return response;
        }

        public PagedViewModel<AudienceGroupDTO> Search(SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var query = GenerateSearchQuery(parameters);

            var pagedAudienceGroup = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<AudienceGroupDTO>
            {
                Page = pagedAudienceGroup.Page,
                TotalPage = pagedAudienceGroup.TotalPage,
                TotalItem = pagedAudienceGroup.TotalItem,
                Items = (from audienceGroup in pagedAudienceGroup.Items
                         select MapModelToDTO(audienceGroup))
                        .ToList()
            };

            return response;
        }

        public IEnumerable<AudienceGroupDTO> Search(SearchCriteriaViewModel parameters)
        {
            var query = GenerateSearchQuery(parameters);

            var audienceGroups = query.ToList();

            var response = (from audienceGroup in audienceGroups
                            select MapModelToDTO(audienceGroup))
                           .ToList();

            return response;
        }

        public AudienceGroupDTO GetById(Guid id)
        {
            var audienceGroup = _dbContext.AudienceGroups.AsNoTracking()
                                                         .SingleOrDefault(x => x.Id == id);

            if (audienceGroup is null)
            {
                throw new AudienceGroupException.NotFound(id);
            }

            var response = MapModelToDTO(audienceGroup);

            return response;
        }

        public AudienceGroupDTO Update(AudienceGroupDTO request, string requester)
        {
            var audienceGroup = _dbContext.AudienceGroups.SingleOrDefault(x => x.Id == request.Id);

            if (audienceGroup is null)
            {
                throw new AudienceGroupException.NotFound(request.Id);
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                audienceGroup.Name = request.Name;
                audienceGroup.Description = request.Description;
                audienceGroup.IsActive = request.IsActive;
                audienceGroup.Conditions = JsonConvert.SerializeObject(request.Conditions);
                audienceGroup.UpdatedAt = DateTime.UtcNow;
                audienceGroup.UpdatedBy = requester;

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(audienceGroup);

            return response;
        }

        public void Delete(Guid id)
        {
            var audienceGroup = _dbContext.AudienceGroups.SingleOrDefault(x => x.Id == id);

            if (audienceGroup is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.AudienceGroups.Remove(audienceGroup);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        private static AudienceGroupDTO MapModelToDTO(AudienceGroup model)
        {
            return new AudienceGroupDTO
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                IsActive = model.IsActive,
                Conditions = JsonConvert.DeserializeObject<IEnumerable<AudienceGroupConditionDTO>>(model.Conditions)!,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt
            };
        }

        private IQueryable<AudienceGroup> GenerateSearchQuery(SearchCriteriaViewModel? parameters = null)
        {
            var query = _dbContext.AudienceGroups.AsNoTracking();

            if (parameters is not null)
            {
                if (!string.IsNullOrEmpty(parameters.Keyword))
                {
                    query = query.Where(x => x.Name.Contains(parameters.Keyword)
                                             || x.Description.Contains(parameters.Keyword));
                }

                if (parameters.IsActive.HasValue)
                {
                    query = query.Where(x => x.IsActive == parameters.IsActive.Value);
                }
            }

            query = query.OrderBy(x => x.CreatedAt);

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