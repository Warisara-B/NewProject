using Microsoft.EntityFrameworkCore;
using Plexus.Database;
using Plexus.Database.Model;
using Plexus.Database.Model.Localization;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.SearchFilter;
using Plexus.Entity.Exception;
using Plexus.Utility.Extensions;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider.src
{
    public class InstructorRoleProvider : IInstructorRoleProvider
    {
        private readonly DatabaseContext _dbContext;

        public InstructorRoleProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public InstructorRoleDTO Create(CreateInstructorRoleDTO request, string requester)
        {
            var model = new InstructorRole
            {
                Name = request.Name,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = requester,
            };

            var localizations = request.Localizations is null ? Enumerable.Empty<InstructorRoleLocalization>()
                                                              : (from localize in request.Localizations
                                                                 select new InstructorRoleLocalization
                                                                 {
                                                                     InstructorRole = model,
                                                                     Language = localize.Language,
                                                                     Name = localize.Name
                                                                 })
                                                                .ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.InstructorRoles.Add(model);

                if (localizations.Any())
                {
                    _dbContext.instructorRoleLocalizations.AddRange(localizations);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(model, localizations);

            return response;
        }

        public IEnumerable<InstructorRoleDTO> Search(SearchInstructorRoleCriteriaDTO parameters)
        {
            var query = GenerateSearchQuery(parameters);

            var instructorRoles = query.ToList();

            var response = (from role in instructorRoles
                            select MapModelToDTO(role, role.Localizations))
                           .ToList();

            return response;
        }

        public PagedViewModel<InstructorRoleDTO> Search(SearchInstructorRoleCriteriaDTO parameters, int page = 1, int pageSize = 5)
        {
            var query = GenerateSearchQuery(parameters);

            var pagedInstructorRole = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<InstructorRoleDTO>
            {
                Page = pagedInstructorRole.Page,
                TotalPage = pagedInstructorRole.TotalPage,
                TotalItem = pagedInstructorRole.TotalItem,
                Items = (from instructorRole in pagedInstructorRole.Items
                         select MapModelToDTO(instructorRole, instructorRole.Localizations))
                         .ToList()
            };

            return response;
        }

        public InstructorRoleDTO GetById(Guid id)
        {
            var instructorRole = _dbContext.InstructorRoles.Include(x => x.Localizations)
                                                           .AsNoTracking()
                                                           .SingleOrDefault(x => x.Id == id);

            if (instructorRole is null)
            {
                throw new InstructorRoleException.NotFound(id);
            }

            var response = MapModelToDTO(instructorRole, instructorRole.Localizations);

            return response;
        }

        public InstructorRoleDTO Update(InstructorRoleDTO request, string requester)
        {
            var instructorRole = _dbContext.InstructorRoles.Include(x => x.Localizations)
                                                           .SingleOrDefault(x => x.Id == request.Id);

            if (instructorRole is null)
            {
                throw new InstructorRoleException.NotFound(request.Id);
            }

            var localizations = request.Localizations is null ? Enumerable.Empty<InstructorRoleLocalization>()
                                                              : (from localize in request.Localizations
                                                                 select new InstructorRoleLocalization
                                                                 {
                                                                     InstructorRoleId = instructorRole.Id,
                                                                     Language = localize.Language,
                                                                     Name = localize.Name
                                                                 })
                                                                .ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                instructorRole.Name = request.Name;
                instructorRole.IsActive = request.IsActive;
                instructorRole.UpdatedAt = DateTime.UtcNow;
                instructorRole.UpdatedBy = requester;

                _dbContext.instructorRoleLocalizations.RemoveRange(instructorRole.Localizations);

                if (localizations.Any())
                {
                    _dbContext.instructorRoleLocalizations.AddRange(localizations);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(instructorRole, localizations);

            return response;
        }

        public void Delete(Guid id)
        {
            var instructorRole = _dbContext.InstructorRoles.SingleOrDefault(x => x.Id == id);

            if (instructorRole is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.InstructorRoles.Remove(instructorRole);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        private IQueryable<InstructorRole> GenerateSearchQuery(SearchInstructorRoleCriteriaDTO? parameters = null)
        {
            var query = _dbContext.InstructorRoles.Include(x => x.Localizations)
                                                  .AsNoTracking();

            if (parameters is not null)
            {
                if (!string.IsNullOrEmpty(parameters.Keyword))
                {
                    query = query.Where(x => x.Name.Contains(parameters.Keyword));
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

        private static InstructorRoleDTO MapModelToDTO(InstructorRole model, IEnumerable<InstructorRoleLocalization> localizations)
        {
            var response = new InstructorRoleDTO
            {
                Id = model.Id,
                Name = model.Name,
                IsActive = model.IsActive,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                Localizations = localizations is null ? Enumerable.Empty<InstructorRoleLocalizationDTO>()
                                                      : (from localize in localizations
                                                         orderby localize.Language
                                                         select new InstructorRoleLocalizationDTO
                                                         {
                                                             Language = localize.Language,
                                                             Name = localize.Name
                                                         })
                                                        .ToList()
            };

            return response;
        }
    }
}