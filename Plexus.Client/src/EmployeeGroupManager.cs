using Microsoft.EntityFrameworkCore;
using Plexus.Client.ViewModel;
using Plexus.Client.ViewModel.DropDown;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Database;
using Plexus.Database.Model;
using Plexus.Database.Model.Localization;
using Plexus.Entity.Exception;
using Plexus.Utility.Extensions;
using Plexus.Utility.ViewModel;

namespace Plexus.Client.src
{
    public class EmployeeGroupManager : IEmployeeGroupManager
    {
        private readonly DatabaseContext _dbContext;

        public EmployeeGroupManager(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public EmployeeGroupViewModel Create(CreateEmployeeGroupViewModel request, Guid userId)
        {
            var model = new EmployeeGroup
            {
                Name = request.Name,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "requester", // TODO : Add requester
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = "requester" // TODO : Add requester
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

            var response = MapModelToViewModel(model, localizations);

            return response;
        }

        public PagedViewModel<EmployeeGroupViewModel> Search(SearchEmployeeGroupCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var query = GenerateSearchQuery(parameters);

            var pagedEmployeeGroup = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<EmployeeGroupViewModel>
            {
                Page = pagedEmployeeGroup.Page,
                TotalPage = pagedEmployeeGroup.TotalPage,
                TotalItem = pagedEmployeeGroup.TotalItem,
                Items = (from employeeGroup in pagedEmployeeGroup.Items
                         select MapModelToViewModel(employeeGroup, employeeGroup.Localizations))
                        .ToList()
            };

            return response;
        }

        public IEnumerable<EmployeeGroupViewModel> Search(SearchEmployeeGroupCriteriaViewModel parameters)
        {
            var query = GenerateSearchQuery(parameters);

            var employeeGroups = query.ToList();

            var response = (from employeeGroup in employeeGroups
                            select MapModelToViewModel(employeeGroup, employeeGroup.Localizations))
                           .ToList();

            return response;
        }

        public IEnumerable<BaseDropDownViewModel> GetDropDownList(SearchEmployeeGroupCriteriaViewModel parameters)
        {
            var employeeGroups = Search(parameters);

            var response = (from employeeGroup in employeeGroups
                            select MapViewModelToDropDown(employeeGroup))
                           .ToList();

            return response;
        }

        public EmployeeGroupViewModel GetById(Guid id)
        {
            var employeeGroup = _dbContext.EmployeeGroups.Include(x => x.Localizations)
                                                         .AsNoTracking()
                                                         .SingleOrDefault(x => x.Id == id);

            if (employeeGroup is null)
            {
                throw new EmployeeGroupException.NotFound(id);
            }

            var response = MapModelToViewModel(employeeGroup, employeeGroup.Localizations);

            return response;
        }

        public EmployeeGroupViewModel Update(Guid id, CreateEmployeeGroupViewModel request, Guid userId)
        {
            var employeeGroup = _dbContext.EmployeeGroups.Include(x => x.Localizations)
                                                           .SingleOrDefault(x => x.Id == id);

            if (employeeGroup is null)
            {
                throw new EmployeeGroupException.NotFound(id);
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
                employeeGroup.UpdatedBy = "requester"; // TODO : Add requester

                _dbContext.EmployeeGroupLocalizations.RemoveRange(employeeGroup.Localizations);

                if (localizations.Any())
                {
                    _dbContext.EmployeeGroupLocalizations.AddRange(localizations);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToViewModel(employeeGroup, localizations);

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

        private static EmployeeGroupViewModel MapModelToViewModel(EmployeeGroup model, IEnumerable<EmployeeGroupLocalization> localizations)
        {
            var response = new EmployeeGroupViewModel
            {
                Id = model.Id,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                Localizations = localizations is null ? Enumerable.Empty<EmployeeGroupLocalizationViewModel>()
                                                      : (from localize in localizations
                                                         orderby localize.Language
                                                         select new EmployeeGroupLocalizationViewModel
                                                         {
                                                             Language = localize.Language,
                                                             Name = localize.Name
                                                         })
                                                        .ToList()
            };

            return response;
        }

        private BaseDropDownViewModel MapViewModelToDropDown(EmployeeGroupViewModel dto)
        {
            var response = new BaseDropDownViewModel
            {
                Id = dto.Id.ToString(),
                Name = dto.Name
            };

            return response;
        }

        private IQueryable<EmployeeGroup> GenerateSearchQuery(SearchEmployeeGroupCriteriaViewModel? parameters = null)
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