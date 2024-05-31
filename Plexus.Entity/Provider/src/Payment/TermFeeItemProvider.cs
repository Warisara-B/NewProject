using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Plexus.Database;
using Plexus.Database.Model.Payment;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Payment;
using Plexus.Entity.Exception;
using Plexus.Utility.Extensions;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider.src.Payment
{
    public class TermFeeItemProvider : ITermFeeItemProvider
    {
        private readonly DatabaseContext _dbContext;

        public TermFeeItemProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public TermFeeItemDTO Create(CreateTermFeeItemDTO request, string requester)
        {
            var model = new TermFeeItem
            {
                TermFeePackageId = request.TermFeePackageId,
                FeeItemId = request.FeeItemId,
                TermType = request.TermType?.ToFlags(),
                RecurringType = request.RecurringType,
                Conditions = JsonConvert.SerializeObject(request.Condition),
                Amount = request.Amount,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = requester
            };

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.TermFeeItems.Add(model);

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(model);

            return response;
        }

        public PagedViewModel<TermFeeItemDTO> Search(SearchCriteriaViewModel parameters, int page, int pageSize)
        {
            var query = GenerateSearchQuery(parameters);

            var pagedTermFeeItem = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<TermFeeItemDTO>
            {
                Page = pagedTermFeeItem.Page,
                TotalPage = pagedTermFeeItem.TotalPage,
                TotalItem = pagedTermFeeItem.TotalItem,
                Items = (from item in pagedTermFeeItem.Items
                         select MapModelToDTO(item))
                        .ToList()
            };

            return response;
        }

        public TermFeeItemDTO GetById(Guid id)
        {
            var termFeeItem = _dbContext.TermFeeItems.AsNoTracking()
                                                     .SingleOrDefault(x => x.Id == id);

            if (termFeeItem is null)
            {
                throw new TermFeeException.ItemNotFound(id);
            }

            var response = MapModelToDTO(termFeeItem);

            return response;
        }

        public TermFeeItemDTO Update(TermFeeItemDTO request, string requester)
        {
            var termFeeItem = _dbContext.TermFeeItems.SingleOrDefault(x => x.Id == request.Id);

            if (termFeeItem is null)
            {
                throw new TermFeeException.ItemNotFound(request.Id);
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                termFeeItem.TermFeePackageId = request.TermFeePackageId;
                termFeeItem.FeeItemId = request.FeeItemId;
                termFeeItem.TermType = request.TermType?.ToFlags();
                termFeeItem.RecurringType = request.RecurringType;
                termFeeItem.Conditions = JsonConvert.SerializeObject(request.Condition);
                termFeeItem.Amount = request.Amount;
                termFeeItem.IsActive = request.IsActive;
                termFeeItem.UpdatedAt = DateTime.UtcNow;
                termFeeItem.UpdatedBy = requester;

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(termFeeItem);

            return response;
        }

        public void Delete(Guid id)
        {
            var termFeeItem = _dbContext.TermFeeItems.SingleOrDefault(x => x.Id == id);

            if (termFeeItem is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.TermFeeItems.Remove(termFeeItem);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        private TermFeeItemDTO MapModelToDTO(TermFeeItem model)
        {
            var condition = string.IsNullOrEmpty(model.Conditions) ? null
                                                                   : JsonConvert.DeserializeObject<TermFeeItemConditionDTO>(model.Conditions);

            var response = new TermFeeItemDTO
            {
                Id = model.Id,
                TermFeePackageId = model.TermFeePackageId,
                FeeItemId = model.FeeItemId,
                TermType = model.TermType?.ToIEnumerable(),
                RecurringType = model.RecurringType,
                Condition = condition,
                Amount = model.Amount,
                IsActive = model.IsActive,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt
            };

            return response;
        }

        private IQueryable<TermFeeItem> GenerateSearchQuery(SearchCriteriaViewModel? parameters = null)
        {
            var query = _dbContext.TermFeeItems.AsNoTracking();

            if (parameters is not null)
            {
                if (parameters.FeeItemId.HasValue)
                {
                    query = query.Where(x => x.FeeItemId == parameters.FeeItemId.Value);
                }

                if (parameters.TermType.HasValue)
                {
                    query = query.Where(x => x.TermType.HasValue && x.TermType.Value.HasFlag(parameters.TermType.Value));
                }

                if (parameters.RecurringType.HasValue)
                {
                    query = query.Where(x => x.RecurringType == parameters.RecurringType.Value);
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

            var a = query.ToString();

            return query;
        }
    }
}