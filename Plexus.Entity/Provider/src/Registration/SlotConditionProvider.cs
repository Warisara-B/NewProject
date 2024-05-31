using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Plexus.Database;
using Plexus.Database.Model.Registration;
using Plexus.Entity.DTO.Registration;
using Plexus.Utility.ViewModel;
using Plexus.Utility.Extensions;
using Plexus.Entity.Exception;
using Plexus.Entity.DTO.SearchFilter;

namespace Plexus.Entity.Provider.src.Registration
{
    public class SlotConditionProvider : ISlotConditionProvider
    {
        private readonly DatabaseContext _dbContext;

        public SlotConditionProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public SlotConditionDTO Create(Guid slotId, CreateSlotConditionDTO request, string requester)
        {
            var model = new SlotCondition
            {
                SlotId = slotId,
                Conditions = JsonConvert.SerializeObject(request.Conditions),
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = requester
            };

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.SlotConditions.Add(model);

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(model);

            return response;
        }

        public PagedViewModel<SlotConditionDTO> Search(SearchSlotConditionCriteriaDTO parameters, int page = 1, int pageSize = 25)
        {
            var query = GenerateSearchQuery(parameters);

            var pagedSlotCondition = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<SlotConditionDTO>
            {
                Page = pagedSlotCondition.Page,
                TotalPage = pagedSlotCondition.TotalPage,
                TotalItem = pagedSlotCondition.TotalItem,
                Items = (from slotCondition in pagedSlotCondition.Items
                         select MapModelToDTO(slotCondition))
                        .ToList()
            };

            return response;
        }

        public IEnumerable<SlotConditionDTO> GetBySlotId(Guid slotId)
        {
            var slotConditions = _dbContext.SlotConditions.AsNoTracking()
                                                          .Where(x => x.SlotId == slotId)
                                                          .ToList();

            var response = (from slotCondition in slotConditions
                            orderby slotCondition.CreatedAt
                            select MapModelToDTO(slotCondition))
                           .ToList();

            return response;
        }

        public SlotConditionDTO GetById(Guid id)
        {
            var slotCondition = _dbContext.SlotConditions.AsNoTracking()
                                                         .SingleOrDefault(x => x.Id == id);

            if (slotCondition is null)
            {
                throw new SlotConditionException.NotFound(id);
            }

            var response = MapModelToDTO(slotCondition);

            return response;
        }

        public SlotConditionDTO Update(SlotConditionDTO request, string requester)
        {
            var slotCondition = _dbContext.SlotConditions.SingleOrDefault(x => x.Id == request.Id);

            if (slotCondition is null)
            {
                throw new SlotConditionException.NotFound(request.Id);
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                slotCondition.Conditions = JsonConvert.SerializeObject(request.Conditions);
                slotCondition.IsActive = request.IsActive;
                slotCondition.UpdatedAt = DateTime.UtcNow;
                slotCondition.UpdatedBy = requester;

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(slotCondition);

            return response;
        }

        public void Delete(Guid id)
        {
            var slotCondition = _dbContext.SlotConditions.SingleOrDefault(x => x.Id == id);

            if (slotCondition is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.SlotConditions.Remove(slotCondition);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        private static SlotConditionDTO MapModelToDTO(SlotCondition model)
        {
            var response = new SlotConditionDTO
            {
                Id = model.Id,
                SlotId = model.SlotId,
                Conditions = JsonConvert.DeserializeObject<IEnumerable<ConditionDTO>>(model.Conditions)!,
                IsActive = model.IsActive,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt
            };

            return response;
        }

        private IQueryable<SlotCondition> GenerateSearchQuery(SearchSlotConditionCriteriaDTO? parameters = null)
        {
            var query = _dbContext.SlotConditions.AsNoTracking();

            if (parameters is not null)
            {
                if (parameters.SlotId.HasValue)
                {
                    query = query.Where(x => x.SlotId == parameters.SlotId.Value);
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