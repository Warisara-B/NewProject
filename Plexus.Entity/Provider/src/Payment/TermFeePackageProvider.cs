using Microsoft.EntityFrameworkCore;
using Plexus.Database;
using Plexus.Database.Model.Localization.Payment;
using Plexus.Database.Model.Payment;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Payment;
using Plexus.Entity.Exception;
using Plexus.Utility.Extensions;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider.src.Payment
{
    public class TermFeePackageProvider : ITermFeePackageProvider
    {
        private readonly DatabaseContext _dbContext;

        public TermFeePackageProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public TermFeePackageDTO Create(CreateTermFeePackageDTO request, string requester)
        {
            var model = new TermFeePackage
            {
                Name = request.Name,
                Type = request.Type,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = requester
            };

            var localizes = MapLocalizationDTOToModel(request.Localizations, model).ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.TermFeePackages.Add(model);

                if (localizes.Any())
                {
                    _dbContext.TermFeePackageLocalizations.AddRange(localizes);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(model);

            return response;
        }

        public IEnumerable<TermFeePackageDTO> Search(SearchCriteriaViewModel? parameters = null)
        {
            var query = GenerateSearchQuery(parameters);

            var packages = query.ToList();

            var response = (from package in packages
                            select MapModelToDTO(package))
                           .ToList();

            return response;
        }

        public PagedViewModel<TermFeePackageDTO> Search(SearchCriteriaViewModel? parameters, int page, int pageSize)
        {
            var query = GenerateSearchQuery(parameters);

            var pagedPackage = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<TermFeePackageDTO>
            {
                Page = pagedPackage.Page,
                TotalPage = pagedPackage.TotalPage,
                TotalItem = pagedPackage.TotalItem,
                Items = (from package in pagedPackage.Items
                         select MapModelToDTO(package))
                        .ToList()
            };

            return response;
        }

        public TermFeePackageDTO GetById(Guid id)
        {
            var package = _dbContext.TermFeePackages.AsNoTracking()
                                                    .SingleOrDefault(x => x.Id == id);

            if (package is null)
            {
                throw new TermFeeException.PackageNotFound(id);
            }

            var response = MapModelToDTO(package);

            return response;
        }

        public IEnumerable<TermFeePackageDTO> GetById(IEnumerable<Guid> ids)
        {
            var packages = _dbContext.TermFeePackages.AsNoTracking()
                                                     .Where(x => ids.Contains(x.Id))
                                                     .ToList();

            var response = (from package in packages
                            select MapModelToDTO(package))
                           .ToList();

            return response;
        }

        public TermFeePackageDTO Update(TermFeePackageDTO request, string requester)
        {
            var package = _dbContext.TermFeePackages.SingleOrDefault(x => x.Id == request.Id);

            if (package is null)
            {
                throw new TermFeeException.PackageNotFound(request.Id);
            }

            var localizes = request.Localizations is null ? Enumerable.Empty<TermFeePackageLocalization>()
                                                          : (from data in request.Localizations
                                                             select new TermFeePackageLocalization
                                                             {
                                                                 TermFeePackageId = package.Id,
                                                                 Language = data.Language,
                                                                 Name = data.Name
                                                             })
                                                            .ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                package.Name = request.Name;
                package.Type = request.Type;
                package.IsActive = request.IsActive;
                package.UpdatedAt = DateTime.UtcNow;
                package.UpdatedBy = requester;

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(package);

            return response;
        }

        public void Delete(Guid id)
        {
            var package = _dbContext.TermFeePackages.SingleOrDefault(x => x.Id == id);

            if (package is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.TermFeePackages.Remove(package);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        private static TermFeePackageDTO MapModelToDTO(TermFeePackage model)
        {
            return new TermFeePackageDTO
            {
                Id = model.Id,
                Name = model.Name,
                Type = model.Type,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                IsActive = model.IsActive
            };
        }

        private static IEnumerable<TermFeePackageLocalization> MapLocalizationDTOToModel(
                IEnumerable<TermFeePackageLocalizationDTO> localizations,
                TermFeePackage model)
        {
            if (localizations is null)
            {
                return Enumerable.Empty<TermFeePackageLocalization>();
            }

            var response = (from locale in localizations
                            select new TermFeePackageLocalization
                            {
                                TermFeePackage = model,
                                Language = locale.Language,
                                Name = locale.Name
                            })
                           .ToList();

            return response;
        }

        private IQueryable<TermFeePackage> GenerateSearchQuery(SearchCriteriaViewModel? parameters = null)
        {
            var query = _dbContext.TermFeePackages.AsNoTracking();

            if (parameters is not null)
            {
                if (!string.IsNullOrEmpty(parameters.Name))
                {
                    query = query.Where(x => x.Name.Contains(parameters.Name));
                }

                if (parameters.TermFeePackageType.HasValue)
                {
                    query = query.Where(x => x.Type == parameters.TermFeePackageType.Value);
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