using Microsoft.EntityFrameworkCore;
using Plexus.Client.ViewModel.Academic;
using Plexus.Client.ViewModel.DropDown;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Database;
using Plexus.Database.Model.Academic;
using Plexus.Database.Model.Localization.Academic;
using Plexus.Entity.Exception;
using Plexus.Utility.Extensions;
using Plexus.Utility.ViewModel;

namespace Plexus.Client.src.Academic
{
    public class CareerPositionManager : ICareerPositionManager
    {
        private readonly DatabaseContext _dbContext;

        public CareerPositionManager(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public CareerPositionViewModel Create(CreateCareerPositionViewModel request)
        {
            var careerPosition = new CareerPosition
            {
                Abbreviation = request.Abbreviation,
                Name = request.Name,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "", // TODO : Add requester
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = "" // TODO : Add requester
            };

            var localizes = MapLocalizationViewModelToModel(request.Localizations, careerPosition).ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.CareerPositions.Add(careerPosition);

                if (localizes.Any())
                {
                    _dbContext.CareerPositionLocalizations.AddRange(localizes);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToViewModel(careerPosition, careerPosition.Localizations);

            return response;
        }

        public PagedViewModel<CareerPositionViewModel> Search(SearchCareerPositionCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var query = GenerateSearchQuery(parameters);

            var pagedCareerPosition = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<CareerPositionViewModel>
            {
                Page = pagedCareerPosition.Page,
                PageSize = pagedCareerPosition.PageSize,
                TotalPage = pagedCareerPosition.TotalPage,
                TotalItem = pagedCareerPosition.TotalItem,
                Items = (from careerPosition in pagedCareerPosition.Items
                         select MapModelToViewModel(careerPosition, careerPosition.Localizations))
                        .ToList()
            };

            return response;
        }

        public IEnumerable<CareerPositionViewModel> Search(SearchCareerPositionCriteriaViewModel parameters)
        {
            var query = GenerateSearchQuery(parameters);

            var careerPositions = query.ToList();

            var response = (from position in careerPositions
                            select MapModelToViewModel(position, position.Localizations))
                           .ToList();

            return response;
        }

        public IEnumerable<BaseDropDownViewModel> GetDropDownList(SearchCareerPositionCriteriaViewModel parameters)
        {
            var careerPositions = Search(parameters).ToList();

            var response = (from position in careerPositions
                            select MapViewModelToDropDown(position))
                           .ToList();

            return response;
        }

        public CareerPositionViewModel Update(Guid id, CreateCareerPositionViewModel request)
        {
            var careerPosition = _dbContext.CareerPositions.Include(x => x.Localizations)
                                                           .SingleOrDefault(x => x.Id == id);

            if (careerPosition is null)
            {
                throw new CareerPositionException.NotFound(id);
            }

            var localizes = request.Localizations is null ? Enumerable.Empty<CareerPositionLocalization>()
                                                          : (from locale in request.Localizations
                                                             orderby locale.Language
                                                             select new CareerPositionLocalization
                                                             {
                                                                 CareerPositionId = id,
                                                                 Language = locale.Language,
                                                                 Abbreviation = locale.Abbreviation,
                                                                 Name = locale.Name
                                                             })
                                                             .ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                careerPosition.Abbreviation = request.Abbreviation;
                careerPosition.Name = request.Name;
                careerPosition.IsActive = request.IsActive;
                careerPosition.UpdatedAt = DateTime.UtcNow;
                careerPosition.UpdatedBy = ""; // TODO : Add requester

                _dbContext.CareerPositionLocalizations.RemoveRange(careerPosition.Localizations);

                if (localizes.Any())
                {
                    _dbContext.CareerPositionLocalizations.AddRange(localizes);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToViewModel(careerPosition, careerPosition.Localizations);

            return response;
        }

        public void Delete(Guid id)
        {
            var careerPosition = _dbContext.CareerPositions.SingleOrDefault(x => x.Id == id);

            if (careerPosition is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.CareerPositions.Remove(careerPosition);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        private static CareerPositionViewModel MapModelToViewModel(
                       CareerPosition model,
                       IEnumerable<CareerPositionLocalization> localizations)
        {
            return new CareerPositionViewModel
            {
                Id = model.Id,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                IsActive = model.IsActive,
                Localizations = localizations is null ? Enumerable.Empty<CareerPositionLocalizationViewModel>()
                                                      : (from locale in localizations
                                                         orderby locale.Language
                                                         select new CareerPositionLocalizationViewModel
                                                         {
                                                             Language = locale.Language,
                                                             Abbreviation = locale.Abbreviation,
                                                             Name = locale.Name
                                                         })
                                                         .ToList()
            };
        }

        private static IEnumerable<CareerPositionLocalization> MapLocalizationViewModelToModel(
                       IEnumerable<CareerPositionLocalizationViewModel>? localizations,
                       CareerPosition model
        )
        {
            if (localizations is null)
            {
                return Enumerable.Empty<CareerPositionLocalization>();
            }

            var response = (from locale in localizations
                            orderby locale.Language
                            select new CareerPositionLocalization
                            {
                                CareerPosition = model,
                                Language = locale.Language,
                                Abbreviation = locale.Abbreviation,
                                Name = locale.Name,
                            })
                            .ToList();

            return response;
        }

        private IQueryable<CareerPosition> GenerateSearchQuery(SearchCareerPositionCriteriaViewModel? parameters = null)
        {
            var query = _dbContext.CareerPositions.Include(x => x.Localizations)
                                                  .AsNoTracking();

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

        private static BaseDropDownViewModel MapViewModelToDropDown(CareerPositionViewModel viewModel)
        {
            var response = new BaseDropDownViewModel
            {
                Id = viewModel.Id.ToString(),
                Name = viewModel.Name
            };

            return response;
        }
    }
}