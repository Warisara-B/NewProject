using Microsoft.EntityFrameworkCore;
using Plexus.Client.ViewModel.DropDown;
using Plexus.Client.ViewModel.Payment;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Database;
using Plexus.Database.Model.Payment;
using Plexus.Entity.Exception;
using Plexus.Utility.Extensions;
using Plexus.Utility.ViewModel;

namespace Plexus.Client.src.Payment
{
    public class FeeGroupManager : IFeeGroupManager
    {
        private readonly DatabaseContext _dbContext;

        public FeeGroupManager(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public FeeGroupViewModel Create(CreateFeeGroupViewModel request, Guid userId)
        {
            var model = new FeeGroup
            {
                Name = request.Name,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                // CreatedBy = requester,  TODO : Add requester
                UpdatedAt = DateTime.UtcNow,
                // UpdatedBy = requester TODO : Add requester
            };

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.FeeGroups.Add(model);

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToViewModel(model);

            return response;
        }

        public IEnumerable<FeeGroupViewModel> Search(SearchFeeGroupCriteriaViewModel? parameters = null)
        {
            var query = GenerateSearchQuery(parameters);

            var feeGroups = query.ToList();

            var response = (from feeGroup in feeGroups
                            select MapModelToViewModel(feeGroup))
                           .ToList();

            return response;
        }

        public PagedViewModel<FeeGroupViewModel> Search(SearchFeeGroupCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var query = GenerateSearchQuery(parameters);

            var pagedFeeGroup = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<FeeGroupViewModel>
            {
                Page = pagedFeeGroup.Page,
                PageSize = pagedFeeGroup.PageSize,
                TotalPage = pagedFeeGroup.TotalPage,
                TotalItem = pagedFeeGroup.TotalItem,
                Items = (from feeGroup in pagedFeeGroup.Items
                         select MapModelToViewModel(feeGroup))
                        .ToList()
            };

            return response;
        }

        public IEnumerable<BaseDropDownViewModel> GetDropDownList(SearchFeeGroupCriteriaViewModel parameters)
        {
            var feeGroups = Search(parameters);

            var response = (from feeGroup in feeGroups
                            select new BaseDropDownViewModel
                            {
                                Id = feeGroup.Id.ToString(),
                                Name = feeGroup.Name
                            })
                           .ToList();

            return response;
        }

        public FeeGroupViewModel GetById(Guid id)
        {
            var feeGroup = _dbContext.FeeGroups.AsNoTracking()
                                               .SingleOrDefault(x => x.Id == id);

            if (feeGroup is null)
            {
                throw new FeeGroupException.NotFound(id);
            }

            var response = MapModelToViewModel(feeGroup);

            return response;
        }

        public FeeGroupViewModel Update(Guid id, CreateFeeGroupViewModel request, Guid userId)
        {
            var feeGroup = _dbContext.FeeGroups.SingleOrDefault(x => x.Id == id);

            if (feeGroup is null)
            {
                throw new FeeGroupException.NotFound(id);
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                feeGroup.Name = request.Name;
                feeGroup.IsActive = request.IsActive;
                feeGroup.UpdatedAt = DateTime.UtcNow;
                // feeGroup.UpdatedBy = requester; TODO : Add requester.

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToViewModel(feeGroup);

            return response;
        }

        public void Delete(Guid id)
        {
            var feeGroup = _dbContext.FeeGroups.SingleOrDefault(x => x.Id == id);

            if (feeGroup is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.FeeGroups.Remove(feeGroup);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        private static FeeGroupViewModel MapModelToViewModel(FeeGroup model)
        {
            var response = new FeeGroupViewModel
            {
                Id = model.Id,
                Name = model.Name,
                IsActive = model.IsActive,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt
            };

            return response;
        }

        private IQueryable<FeeGroup> GenerateSearchQuery(SearchFeeGroupCriteriaViewModel? parameters = null)
        {
            var query = _dbContext.FeeGroups.AsNoTracking();

            if (parameters is not null)
            {
                if (!string.IsNullOrEmpty(parameters.Name))
                {
                    query = query.Where(x => x.Name.Contains(parameters.Name));
                }

                if (parameters.IsActive.HasValue)
                {
                    query = query.Where(x => x.IsActive == parameters.IsActive.Value);
                }
            }

            query = query.OrderBy(x => x.UpdatedAt);

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