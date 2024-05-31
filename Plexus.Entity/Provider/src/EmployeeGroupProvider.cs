using Microsoft.EntityFrameworkCore;
using Plexus.Database;
using Plexus.Entity.DTO;
using Plexus.Utility.ViewModel;
using Plexus.Utility.Extensions;
using Plexus.Database.Model;
using Plexus.Database.Model.Localization;
using Plexus.Entity.Exception;

namespace Plexus.Entity.Provider.src
{
    public class EmployeeGroupProvider : IEmployeeGroupProvider
    {
        private readonly DatabaseContext _dbContext;

        public EmployeeGroupProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public EmployeeGroupDTO Create(CreateEmployeeGroupDTO request, string requester)
        {
            var model = new EmployeeGroup
            {
                Name = request.Name,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = requester
            };

            var localizations = request.Localizations is null ? Enumerable.Empty<EmployeeGroupLocalization>()
                                                              : (from localize in request.Localizations
                                                                 select new EmployeeGroupLocalization
                                                                 {
                                                                     EmployeeGroup = model,
                                                                     Language = localize.Language,
                                                                     Name = localize.Name
                                                                 })
                                                                .ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.EmployeeGroups.Add(model);

                if (localizations.Any())
                {
                    _dbContext.EmployeeGroupLocalizations.AddRange(localizations);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(model, localizations);

            return response;
        }

        public PagedViewModel<EmployeeGroupDTO> Search(SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var query = GenerateSearchQuery(parameters);

            var pagedEmployeeGroup = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<EmployeeGroupDTO>
            {
                Page = pagedEmployeeGroup.Page,
                TotalPage = pagedEmployeeGroup.TotalPage,
                TotalItem = pagedEmployeeGroup.TotalItem,
                Items = (from employeeGroup in pagedEmployeeGroup.Items
                         select MapModelToDTO(employeeGroup, employeeGroup.Localizations))
                        .ToList()
            };

            return response;
        }

        public IEnumerable<EmployeeGroupDTO> Search(SearchCriteriaViewModel parameters)
        {
            var query = GenerateSearchQuery(parameters);

            var employeeGroups = query.ToList();

            var response = (from employeeGroup in employeeGroups
                            select MapModelToDTO(employeeGroup, employeeGroup.Localizations))
                           .ToList();

            return response;
        }

        public EmployeeGroupDTO GetById(Guid id)
        {
            var employeeGroup = _dbContext.EmployeeGroups.Include(x => x.Localizations)
                                                           .AsNoTracking()
                                                           .SingleOrDefault(x => x.Id == id);

            if (employeeGroup is null)
            {
                throw new EmployeeGroupException.NotFound(id);
            }

            var response = MapModelToDTO(employeeGroup, employeeGroup.Localizations);

            return response;
        }

        public IEnumerable<EmployeeGroupDTO> GetById(IEnumerable<Guid> ids)
        {
            var employeeGroups = _dbContext.EmployeeGroups.Include(x => x.Localizations)
                                                            .AsNoTracking()
                                                            .Where(x => ids.Contains(x.Id))
                                                            .ToList();

            var response = (from employeeGroup in employeeGroups
                            select MapModelToDTO(employeeGroup, employeeGroup.Localizations))
                           .ToList();

            return response;
        }

        public EmployeeGroupDTO Update(EmployeeGroupDTO request, string requester)
        {
            var employeeGroup = _dbContext.EmployeeGroups.Include(x => x.Localizations)
                                                           .SingleOrDefault(x => x.Id == request.Id);

            if (employeeGroup is null)
            {
                throw new EmployeeGroupException.NotFound(request.Id);
            }

            var localizations = request.Localizations is null ? Enumerable.Empty<EmployeeGroupLocalization>()
                                                              : (from localize in request.Localizations
                                                                 select new EmployeeGroupLocalization
                                                                 {
                                                                     EmployeeGroupId = employeeGroup.Id,
                                                                     Language = localize.Language,
                                                                     Name = localize.Name
                                                                 })
                                                                .ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                employeeGroup.Name = request.Name;
                employeeGroup.UpdatedAt = DateTime.UtcNow;
                employeeGroup.UpdatedBy = requester;

                _dbContext.EmployeeGroupLocalizations.RemoveRange(employeeGroup.Localizations);

                if (localizations.Any())
                {
                    _dbContext.EmployeeGroupLocalizations.AddRange(localizations);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(employeeGroup, localizations);

            return response;
        }

        public void Delete(Guid id)
        {
            var employeeGroup = _dbContext.EmployeeGroups.SingleOrDefault(x => x.Id == id);

            if (employeeGroup is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.EmployeeGroups.Remove(employeeGroup);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        private static EmployeeGroupDTO MapModelToDTO(EmployeeGroup model, IEnumerable<EmployeeGroupLocalization> localizations)
        {
            var response = new EmployeeGroupDTO
            {
                Id = model.Id,
                Name = model.Name,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                Localizations = localizations is null ? Enumerable.Empty<EmployeeGroupLocalizationDTO>()
                                                      : (from localize in localizations
                                                         orderby localize.Language
                                                         select new EmployeeGroupLocalizationDTO
                                                         {
                                                             Language = localize.Language,
                                                             Name = localize.Name
                                                         })
                                                        .ToList()
            };

            return response;
        }

        private IQueryable<EmployeeGroup> GenerateSearchQuery(SearchCriteriaViewModel? parameters = null)
        {
            var query = _dbContext.EmployeeGroups.Include(x => x.Localizations)
                                                  .AsNoTracking();

            if (parameters != null)
            {
                if (!string.IsNullOrEmpty(parameters.Name))
                {
                    query = query.Where(x => x.Name.Contains(parameters.Name));
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