using Microsoft.EntityFrameworkCore;
using Plexus.Client.ViewModel.DropDown;
using Plexus.Client.ViewModel.Payment;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Database;
using Plexus.Database.Model.Localization.Payment;
using Plexus.Database.Model.Payment;
using Plexus.Entity.Exception;
using Plexus.Utility.Extensions;
using Plexus.Utility.ViewModel;

namespace Plexus.Client.src.Payment
{
    public class FeeItemManager : IFeeItemManager
    {
        private readonly DatabaseContext _dbContext;

        public FeeItemManager(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public FeeItemViewModel Create(CreateFeeItemViewModel request, Guid userId)
        {
            var model = new FeeItem
            {
                FeeGroupId = request.FeeGroupId,
                Code = request.Code,
                Name = request.Name,
                CreatedAt = DateTime.UtcNow,
                // CreatedBy = requester, TODO : Add requester
                UpdatedAt = DateTime.UtcNow,
                // UpdatedBy = requester, TODO : Add requester
                IsActive = request.IsActive
            };

            var localizes = MapLocalizationViewModelToModel(request.Localizations!, model).ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.FeeItems.Add(model);

                if (localizes.Any())
                {
                    _dbContext.FeeItemLocalizations.AddRange(localizes);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToViewModel(model, localizes);

            return response;
        }

        public IEnumerable<FeeItemViewModel> Search(SearchFeeItemCriteriaViewModel? parameters = null)
        {
            var query = GenerateSearchQuery(parameters);

            var feeItems = query.ToList();

            var response = (from feeItem in feeItems
                            select MapModelToViewModel(feeItem, feeItem.Localizations))
                           .ToList();

            return response;
        }

        public FeeItemViewModel GetById(Guid id)
        {
            var feeItem = _dbContext.FeeItems.Include(x => x.Localizations)
                                             .AsNoTracking()
                                             .SingleOrDefault(x => x.Id == id);

            if (feeItem is null)
            {
                throw new FeeItemException.NotFound(id);
            }

            var response = MapModelToViewModel(feeItem, feeItem.Localizations);

            return response;
        }

        public IEnumerable<BaseDropDownViewModel> GetDropDownList(SearchFeeItemCriteriaViewModel parameters)
        {
            var feeItems = Search(parameters).ToList();

            var response = (from feeItem in feeItems
                            select MapViewModelToDropDown(feeItem))
                           .ToList();

            return response;
        }

        public PagedViewModel<FeeItemViewModel> Search(SearchFeeItemCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var query = GenerateSearchQuery(parameters);

            var pagedFeeItem = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<FeeItemViewModel>
            {
                Page = pagedFeeItem.Page,
                PageSize = pagedFeeItem.PageSize,
                TotalPage = pagedFeeItem.TotalPage,
                TotalItem = pagedFeeItem.TotalItem,
                Items = (from feeItem in pagedFeeItem.Items
                         select MapModelToViewModel(feeItem, feeItem.Localizations))
                        .ToList()
            };

            return response;
        }

        public FeeItemViewModel Update(Guid id, CreateFeeItemViewModel request, Guid userId)
        {
            var feeItem = _dbContext.FeeItems.Include(x => x.Localizations)
                                             .SingleOrDefault(x => x.Id == id);

            if (feeItem is null)
            {
                throw new FeeItemException.NotFound(id);
            }

            var localizes = request.Localizations is null ? Enumerable.Empty<FeeItemLocalization>()
                                                          : (from data in request.Localizations
                                                             select new FeeItemLocalization
                                                             {
                                                                 FeeItemId = feeItem.Id,
                                                                 Language = data.Language,
                                                                 Name = data.Name
                                                             })
                                                            .ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                feeItem.FeeGroupId = request.FeeGroupId;
                feeItem.Code = request.Code;
                feeItem.Name = request.Name;
                feeItem.IsActive = request.IsActive;
                feeItem.UpdatedAt = DateTime.UtcNow;
                // feeItem.UpdatedBy = requester; TODO : Add requester

                _dbContext.FeeItemLocalizations.RemoveRange(feeItem.Localizations);

                if (localizes.Any())
                {
                    _dbContext.FeeItemLocalizations.AddRange(localizes);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToViewModel(feeItem, localizes);

            return response;
        }

        public void Delete(Guid id)
        {
            var feeItem = _dbContext.FeeItems.SingleOrDefault(x => x.Id == id);

            if (feeItem is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.FeeItems.Remove(feeItem);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        private static FeeItemViewModel MapModelToViewModel(FeeItem model, IEnumerable<FeeItemLocalization> localizations)
        {
            return new FeeItemViewModel
            {
                Id = model.Id,
                FeeGroupId = model.FeeGroupId,
                Code = model.Code,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                IsActive = model.IsActive,
                Localizations = localizations is null ? Enumerable.Empty<FeeItemLocalizationViewModel>()
                                                      : (from localize in localizations
                                                         orderby localize.Language
                                                         select new FeeItemLocalizationViewModel
                                                         {
                                                             Language = localize.Language,
                                                             Name = localize.Name
                                                         })
                                                        .ToList()
            };
        }

        private static IEnumerable<FeeItemLocalization> MapLocalizationViewModelToModel(
        IEnumerable<FeeItemLocalizationViewModel> localizations,
        FeeItem model)
        {
            if (localizations is null)
            {
                return Enumerable.Empty<FeeItemLocalization>();
            }

            var response = (from locale in localizations
                            select new FeeItemLocalization
                            {
                                FeeItem = model,
                                Language = locale.Language,
                                Name = locale.Name
                            })
                           .ToList();

            return response;
        }

        private static BaseDropDownViewModel MapViewModelToDropDown(FeeItemViewModel viewModel)
        {
            var response = new BaseDropDownViewModel
            {
                Id = viewModel.Id.ToString(),
                Name = viewModel.Name
            };

            return response;
        }

        private IQueryable<FeeItem> GenerateSearchQuery(SearchFeeItemCriteriaViewModel? parameters = null)
        {
            var query = _dbContext.FeeItems.Include(x => x.Localizations)
                                           .AsNoTracking();

            if (parameters is not null)
            {
                if (!string.IsNullOrEmpty(parameters.Code))
                {
                    query = query.Where(x => x.Code.Contains(parameters.Code));
                }

                if (!string.IsNullOrEmpty(parameters.Name))
                {
                    query = query.Where(x => x.Name.Contains(parameters.Name));
                }

                if (parameters.FeeGroupId.HasValue)
                {
                    query = query.Where(x => x.FeeGroupId == parameters.FeeGroupId.Value);
                }

                if (parameters.IsActive.HasValue)
                {
                    query = query.Where(x => x.IsActive == parameters.IsActive.Value);
                }
            }

            query = query.OrderBy(x => x.Code);

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