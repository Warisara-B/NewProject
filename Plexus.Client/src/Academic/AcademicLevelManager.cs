using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Client.ViewModel.Academic;
using Plexus.Client.ViewModel.DropDown;
using Plexus.Entity.DTO.Academic;
using Plexus.Entity.Exception;
using Plexus.Utility.ViewModel;
using Plexus.Database;
using Plexus.Database.Model.Academic;
using Microsoft.EntityFrameworkCore;
using Plexus.Database.Model.Localization.Academic;
using Plexus.Utility.Extensions;

namespace Plexus.Client.src.Academic
{
    public class AcademicLevelManager : IAcademicLevelManager
    {
        private readonly DatabaseContext _dbContext;

        public AcademicLevelManager(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public AcademicLevelViewModel Create(CreateAcademicLevelViewModel request, Guid userId)
        {
            var model = new AcademicLevel
            {
                Name = request.Name,
                FormalName = request.FormalName,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "",
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = "",
                IsActive = request.IsActive
            };

            var localizes = MapLocalizationViewModelToModel(request.Localizations!, model).ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.AcademicLevels.Add(model);

                if (localizes.Any())
                {
                    _dbContext.AcademicLevelLocalizations.AddRange(localizes);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToViewModel(model, localizes);

            return response;
        }

        public AcademicLevelViewModel GetById(Guid id)
        {
            var academicLevel = _dbContext.AcademicLevels.AsNoTracking()
                                                         .Include(x => x.Localizations)
                                                         .SingleOrDefault(x => x.Id == id);

            if (academicLevel is null)
            {
                throw new AcademicLevelException.NotFound(id);
            }

            var response = MapModelToViewModel(academicLevel, academicLevel.Localizations);

            return response;
        }

        public IEnumerable<BaseDropDownViewModel> GetDropDownList(SearchAcademicLevelCriteriaViewModel parameters)
        {
            var academicLevels = Search(parameters).ToList();

            var response = (from academicLevel in academicLevels
                            select MapViewModelToDropDown(academicLevel))
                           .ToList();

            return response;
        }

        public IEnumerable<AcademicLevelViewModel> Search(SearchAcademicLevelCriteriaViewModel? parameters = null)
        {
            var query = GenerateSearchQuery(parameters);

            var academicLevels = query.ToList();

            var response = (from academicLevel in academicLevels
                            select MapModelToViewModel(academicLevel, academicLevel.Localizations))
                           .ToList();

            return response;
        }

        public PagedViewModel<AcademicLevelViewModel> Search(SearchAcademicLevelCriteriaViewModel? parameters, int page, int pageSize)
        {
            var query = GenerateSearchQuery(parameters);

            var pagedAcademicLevel = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<AcademicLevelViewModel>
            {
                Page = pagedAcademicLevel.Page,
                TotalPage = pagedAcademicLevel.TotalPage,
                TotalItem = pagedAcademicLevel.TotalItem,
                Items = (from academicLevel in pagedAcademicLevel.Items
                         select MapModelToViewModel(academicLevel, academicLevel.Localizations))
                        .ToList()
            };

            return response;
        }

        public AcademicLevelViewModel Update(Guid id, CreateAcademicLevelViewModel request, Guid userId)
        {
            var academicLevel = _dbContext.AcademicLevels.Include(x => x.Localizations)
                                             .SingleOrDefault(x => x.Id == id);

            if (academicLevel is null)
            {
                throw new AcademicLevelException.NotFound(id);
            }

            var localizes = request.Localizations is null ? Enumerable.Empty<AcademicLevelLocalization>()
                                                          : (from data in request.Localizations
                                                             select new AcademicLevelLocalization
                                                             {
                                                                 Language = data.Language,
                                                                 AcademicLevelId = academicLevel.Id,
                                                                 Name = data.Name,
                                                                 FormalName = data.FormalName
                                                             })
                                                            .ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                academicLevel.Name = request.Name;
                academicLevel.FormalName = request.FormalName;
                academicLevel.UpdatedAt = DateTime.UtcNow;
                academicLevel.UpdatedBy = "";
                academicLevel.IsActive = request.IsActive;

                _dbContext.AcademicLevelLocalizations.RemoveRange(academicLevel.Localizations);

                if (localizes.Any())
                {
                    _dbContext.AcademicLevelLocalizations.AddRange(localizes);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToViewModel(academicLevel, localizes);

            return response;
        }

        public void Delete(Guid id)
        {
            var academicLevel = _dbContext.AcademicLevels.SingleOrDefault(x => x.Id == id);

            if (academicLevel is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.AcademicLevels.Remove(academicLevel);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        private static IEnumerable<AcademicLevelLocalization> MapLocalizationViewModelToModel(
            IEnumerable<AcademicLevelLocalizationViewModel> localizations,
            AcademicLevel model
        )
        {
            if (localizations is null)
            {
                return Enumerable.Empty<AcademicLevelLocalization>();
            }

            var response = (from locale in localizations
                            select new AcademicLevelLocalization
                            {
                                AcademicLevel = model,
                                Language = locale.Language,
                                Name = locale.Name,
                                FormalName = locale.FormalName
                            })
                            .ToList();

            return response;
        }

        public static AcademicLevelViewModel MapDTOToViewModel(AcademicLevelDTO dto)
        {
            var response = new AcademicLevelViewModel
            {
                Id = dto.Id,
                // Name = dto.Name,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt,
                IsActive = dto.IsActive,
                Localizations = (from data in dto.Localizations
                                 orderby data.Language
                                 select new AcademicLevelLocalizationViewModel
                                 {
                                     Language = data.Language,
                                     Name = data.Name,
                                     FormalName = data.FormalName
                                 })
                                .ToList()
            };

            return response;
        }

        private static BaseDropDownViewModel MapViewModelToDropDown(AcademicLevelViewModel viewModel)
        {
            var response = new BaseDropDownViewModel
            {
                Id = viewModel.Id.ToString(),
                Name = viewModel.Name
            };

            return response;
        }

        private static IEnumerable<AcademicLevelLocalizationDTO> MapLocalizationViewModelToDTO(
            IEnumerable<AcademicLevelLocalizationViewModel>? localizations)
        {
            if (localizations is null)
            {
                return Enumerable.Empty<AcademicLevelLocalizationDTO>();
            }

            var response = (from locale in localizations
                            select new AcademicLevelLocalizationDTO
                            {
                                Language = locale.Language,
                                Name = locale.Name,
                                FormalName = locale.FormalName
                            })
                           .ToList();

            return response;
        }

        private static AcademicLevelViewModel MapModelToViewModel(AcademicLevel model, IEnumerable<AcademicLevelLocalization> localizations)
        {
            return new AcademicLevelViewModel
            {
                Id = model.Id,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                IsActive = model.IsActive,
                Localizations = localizations is null ? Enumerable.Empty<AcademicLevelLocalizationViewModel>()
                                                      : (from localize in localizations
                                                         orderby localize.Language
                                                         select new AcademicLevelLocalizationViewModel
                                                         {
                                                             Language = localize.Language,
                                                             Name = localize.Name,
                                                             FormalName = localize.FormalName
                                                         })
                                                         .ToList()
            };
        }

        private IQueryable<AcademicLevel> GenerateSearchQuery(SearchAcademicLevelCriteriaViewModel? parameters = null)
        {
            var query = _dbContext.AcademicLevels.Include(x => x.Localizations)
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

            query = query.OrderBy(x => x.Name);

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