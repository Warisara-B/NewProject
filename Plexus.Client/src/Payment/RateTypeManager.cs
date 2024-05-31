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
    public class RateTypeManager : IRateTypeManager
    {
        private readonly IUnitOfWork _uow;
        private readonly IAsyncRepository<RateType> _rateTypeRepo;
        private readonly IAsyncRepository<RateTypeLocalization> _rateTypeLocalizationRepo;

        public RateTypeManager(IUnitOfWork uow,
                               IAsyncRepository<RateType> rateTypeRepo,
                               IAsyncRepository<RateTypeLocalization> rateTypeLocalizationRepo)
        {
            _uow = uow;
            _rateTypeRepo = rateTypeRepo;
            _rateTypeLocalizationRepo = rateTypeLocalizationRepo;
        }

        public RateTypeViewModel Create(CreateRateTypeViewModel request)
        {
            var model = new RateType
            {
                Name = request.Name,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "", // TODO : Add requester
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = "" // TODO : Add requester
            };

            var localizations = MapLocalizationViewModelToModel(request.Localizations, model);

            _uow.BeginTran();
            _rateTypeRepo.Add(model);

            if (localizations.Any())
            {
                _rateTypeLocalizationRepo.AddRange(localizations);
            }

            _uow.Complete();
            _uow.CommitTran();

            var response = MapModelToViewModel(model);

            return response;
        }

        public PagedViewModel<RateTypeViewModel> Search(SearchRateTypeCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var query = GenerateSearchQuery(parameters);

            var pagedRateType = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<RateTypeViewModel>
            {
                Page = pagedRateType.Page,
                PageSize = pagedRateType.PageSize,
                TotalPage = pagedRateType.TotalPage,
                TotalItem = pagedRateType.TotalItem,
                Items = (from rateType in pagedRateType.Items
                         select MapModelToViewModel(rateType))
                        .ToList()
            };

            return response;
        }

        public IEnumerable<RateTypeViewModel> Search(SearchRateTypeCriteriaViewModel parameters)
        {
            var query = GenerateSearchQuery(parameters);

            var rateTypes = query.ToList();

            var response = (from rateType in rateTypes
                            select MapModelToViewModel(rateType))
                           .ToList();

            return response;
        }

        public IEnumerable<RateTypeDropDownViewModel> GetDropDownList(SearchRateTypeCriteriaViewModel parameters)
        {
            var rateTypes = Search(parameters);

            var response = (from rateType in rateTypes
                            select MapViewModelToDropDown(rateType))
                           .ToList();

            return response;
        }

        public RateTypeViewModel GetById(Guid id)
        {
            var rateType = _rateTypeRepo.Query()
                                        .Include(x => x.Localizations)
                                        .FirstOrDefault(x => x.Id == id);

            if (rateType is null)
            {
                throw new RateTypeException.NotFound(id);
            }

            var response = MapModelToViewModel(rateType);

            return response;
        }

        public RateTypeViewModel Update(Guid id, CreateRateTypeViewModel request)
        {
            var rateType = _rateTypeRepo.Query()
                                        .Include(x => x.Localizations)
                                        .FirstOrDefault(x => x.Id == id);

            if (rateType is null)
            {
                throw new RateTypeException.NotFound(id);
            }

            rateType.Name = request.Name;
            rateType.IsActive = request.IsActive;

            var localizations = MapLocalizationViewModelToModel(request.Localizations, rateType);

            _uow.BeginTran();
            _rateTypeRepo.Update(rateType);
            _rateTypeLocalizationRepo.DeleteRange(rateType.Localizations.ToList());

            if (localizations.Any())
            {
                _rateTypeLocalizationRepo.AddRange(localizations);
            }

            _uow.Complete();
            _uow.CommitTran();

            var response = MapModelToViewModel(rateType);

            return response;
        }

        public void Delete(Guid id)
        {
            var rateType = _rateTypeRepo.Query()
                                        .FirstOrDefault(x => x.Id == id);

            if (rateType is null)
            {
                return;
            }

            _uow.BeginTran();
            _rateTypeRepo.Delete(rateType);
            _uow.Complete();
            _uow.CommitTran();
        }

        private static RateTypeViewModel MapModelToViewModel(RateType model)
        {
            var response = new RateTypeViewModel
            {
                Id = model.Id,
                IsActive = model.IsActive,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                Localizations = (from localize in model.Localizations
                                 orderby localize.Language
                                 select new RateTypeLocalizationViewModel
                                 {
                                     Language = localize.Language,
                                     Name = localize.Name
                                 })
                                .ToList()
            };

            return response;
        }

        private static IEnumerable<RateTypeLocalization> MapLocalizationViewModelToModel(
                       IEnumerable<RateTypeLocalizationViewModel>? localizations,
                       RateType model
)
        {
            if (localizations is null)
            {
                return Enumerable.Empty<RateTypeLocalization>();
            }

            var response = (from locale in localizations
                            select new RateTypeLocalization
                            {
                                RateType = model,
                                Language = locale.Language,
                                Name = locale.Name
                            })
                            .ToList();

            return response;
        }

        private RateTypeDropDownViewModel MapViewModelToDropDown(RateTypeViewModel viewModel)
        {
            var response = new RateTypeDropDownViewModel
            {
                Id = viewModel.Id.ToString(),
                Name = viewModel.Name
            };

            return response;
        }

        private IQueryable<RateType> GenerateSearchQuery(SearchRateTypeCriteriaViewModel? parameters = null)
        {
            var query = _rateTypeRepo.Query()
                                     .Include(x => x.Localizations);

            if (parameters != null)
            {
                if (!string.IsNullOrEmpty(parameters.Name))
                {
                    query.Where(x => x.Name.Contains(parameters.Name));
                }

                if (!string.IsNullOrEmpty(parameters.SortBy))
                {
                    try
                    {
                        query.OrderBy(parameters.SortBy, parameters.OrderBy);
                    }
                    catch (System.Exception)
                    {
                        // invalid property name
                    }
                }
            }

            query.OrderBy(x => x.Name);

            return query;
        }
    }
}