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
    public class AcademicPositionManager : IAcademicPositionManager
    {
        private readonly DatabaseContext _dbContext;

        public AcademicPositionManager(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public AcademicPositionViewModel Create(CreateAcademicPositionViewModel request)
        {
            var academicPosition = new AcademicPosition
            {
                Abbreviation = request.Abbreviation,
                Name = request.Name,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "", // TODO : Add requester
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = "" // TODO : Add requester
            };

            var localizes = MapLocalizationViewModelToModel(request.Localizations, academicPosition).ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.AcademicPositions.Add(academicPosition);

                if (localizes.Any())
                {
                    _dbContext.AcademicPositionLocalizations.AddRange(localizes);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToViewModel(academicPosition, academicPosition.Localizations);

            return response;
        }

        public IEnumerable<AcademicPositionViewModel> Search(SearchAcademicPositionCriteriaViewModel parameters)
        {
            var query = GenerateSearchQuery(parameters);

            var positions = query.ToList();

            var response = (from position in positions
                            select MapModelToViewModel(position, position.Localizations))
                           .ToList();

            return response;
        }

        public PagedViewModel<AcademicPositionViewModel> Search(SearchAcademicPositionCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var query = GenerateSearchQuery(parameters);

            var pagedAcademicPosition = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<AcademicPositionViewModel>
            {
                Page = pagedAcademicPosition.Page,
                PageSize = pagedAcademicPosition.PageSize,
                TotalPage = pagedAcademicPosition.TotalPage,
                TotalItem = pagedAcademicPosition.TotalItem,
                Items = (from academicPosition in pagedAcademicPosition.Items
                         select MapModelToViewModel(academicPosition, academicPosition.Localizations))
                        .ToList()
            };

            return response;
        }

        public IEnumerable<BaseDropDownViewModel> GetDropDownList(SearchAcademicPositionCriteriaViewModel parameters)
        {
            var academicPositions = Search(parameters).ToList();

            var response = (from position in academicPositions
                            select MapViewModelToDropDown(position))
                           .ToList();

            return response;
        }

        public AcademicPositionViewModel Update(Guid id, CreateAcademicPositionViewModel request)
        {
            var academicPosition = _dbContext.AcademicPositions.Include(x => x.Localizations)
                                                                .SingleOrDefault(x => x.Id == id);

            if (academicPosition is null)
            {
                throw new AcademicPositionException.NotFound(id);
            }

            var localizes = request.Localizations is null ? Enumerable.Empty<AcademicPositionLocalization>()
                                                          : (from locale in request.Localizations
                                                             orderby locale.Language
                                                             select new AcademicPositionLocalization
                                                             {
                                                                 AcademicPositionId = id,
                                                                 Language = locale.Language,
                                                                 Abbreviation = locale.Abbreviation,
                                                                 Name = locale.Name
                                                             })
                                                             .ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                academicPosition.Abbreviation = request.Abbreviation;
                academicPosition.Name = request.Name;
                academicPosition.IsActive = request.IsActive;
                academicPosition.UpdatedAt = DateTime.UtcNow;
                academicPosition.UpdatedBy = ""; // TODO : Add requester

                _dbContext.AcademicPositionLocalizations.RemoveRange(academicPosition.Localizations);

                if (localizes.Any())
                {
                    _dbContext.AcademicPositionLocalizations.AddRange(localizes);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToViewModel(academicPosition, academicPosition.Localizations);

            return response;
        }

        public void Delete(Guid id)
        {
            var academicPosition = _dbContext.AcademicPositions.SingleOrDefault(x => x.Id == id);

            if (academicPosition is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.AcademicPositions.Remove(academicPosition);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        private static AcademicPositionViewModel MapModelToViewModel(
                       AcademicPosition model,
                       IEnumerable<AcademicPositionLocalization> localizations)
        {
            return new AcademicPositionViewModel
            {
                Id = model.Id,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                IsActive = model.IsActive,
                Localizations = localizations is null ? Enumerable.Empty<AcademicPositionLocalizationViewModel>()
                                                      : (from locale in localizations
                                                         orderby locale.Language
                                                         select new AcademicPositionLocalizationViewModel
                                                         {
                                                             Language = locale.Language,
                                                             Abbreviation = locale.Abbreviation,
                                                             Name = locale.Name
                                                         })
                                                         .ToList()
            };
        }

        private static BaseDropDownViewModel MapViewModelToDropDown(AcademicPositionViewModel viewModel)
        {
            var response = new BaseDropDownViewModel
            {
                Id = viewModel.Id.ToString(),
                Name = viewModel.Name
            };

            return response;
        }

        private static IEnumerable<AcademicPositionLocalization> MapLocalizationViewModelToModel(
                       IEnumerable<AcademicPositionLocalizationViewModel>? localizations,
                       AcademicPosition model
        )
        {
            if (localizations is null)
            {
                return Enumerable.Empty<AcademicPositionLocalization>();
            }

            var response = (from locale in localizations
                            orderby locale.Language
                            select new AcademicPositionLocalization
                            {
                                AcademicPosition = model,
                                Language = locale.Language,
                                Abbreviation = locale.Abbreviation,
                                Name = locale.Name,
                            })
                            .ToList();

            return response;
        }

        private IQueryable<AcademicPosition> GenerateSearchQuery(SearchAcademicPositionCriteriaViewModel? parameters = null)
        {
            var query = _dbContext.AcademicPositions.Include(x => x.Localizations)
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
    }
}