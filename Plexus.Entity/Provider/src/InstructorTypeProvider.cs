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
    public class InstructorTypeProvider : IInstructorTypeProvider
    {
        private readonly DatabaseContext _dbContext;

        public InstructorTypeProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public InstructorTypeDTO Create(CreateInstructorTypeDTO request, string requester)
        {
            var model = new InstructorType
            {
                Name = request.Name,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = requester
            };

            var localizations = request.Localizations is null ? Enumerable.Empty<InstructorTypeLocalization>()
                                                              : (from localize in request.Localizations
                                                                 select new InstructorTypeLocalization
                                                                 {
                                                                    InstructorType = model,
                                                                    Language = localize.Language,
                                                                    Name = localize.Name
                                                                 })
                                                                .ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.InstructorTypes.Add(model);

                if (localizations.Any())
                {
                    _dbContext.InstructorTypeLocalizations.AddRange(localizations);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(model, localizations);

            return response;
        }

        public PagedViewModel<InstructorTypeDTO> Search(SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var query = GenerateSearchQuery(parameters);

            var pagedInstructorType = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<InstructorTypeDTO>
            {
                Page = pagedInstructorType.Page,
                TotalPage = pagedInstructorType.TotalPage,
                TotalItem = pagedInstructorType.TotalItem,
                Items = (from instructorType in pagedInstructorType.Items
                         select MapModelToDTO(instructorType, instructorType.Localizations))
                        .ToList()
            };

            return response;
        }

        public IEnumerable<InstructorTypeDTO> Search(SearchCriteriaViewModel parameters)
        {
            var query = GenerateSearchQuery(parameters);

            var instructorTypes = query.ToList();

            var response = (from instructorType in instructorTypes
                            select MapModelToDTO(instructorType, instructorType.Localizations))
                           .ToList();
            
            return response;
        }

        public InstructorTypeDTO GetById(Guid id)
        {
            var instructorType = _dbContext.InstructorTypes.Include(x => x.Localizations)
                                                           .AsNoTracking()
                                                           .SingleOrDefault(x => x.Id == id);

            if (instructorType is null)
            {
                throw new InstructorTypeException.NotFound(id);
            }

            var response = MapModelToDTO(instructorType, instructorType.Localizations);

            return response;
        }

        public IEnumerable<InstructorTypeDTO> GetById(IEnumerable<Guid> ids)
        {
            var instructorTypes = _dbContext.InstructorTypes.Include(x => x.Localizations)
                                                            .AsNoTracking()
                                                            .Where(x => ids.Contains(x.Id))
                                                            .ToList();
            
            var response = (from instructorType in instructorTypes
                            select MapModelToDTO(instructorType, instructorType.Localizations))
                           .ToList();
            
            return response;
        }

        public InstructorTypeDTO Update(InstructorTypeDTO request, string requester)
        {
            var instructorType = _dbContext.InstructorTypes.Include(x => x.Localizations)
                                                           .SingleOrDefault(x => x.Id == request.Id);

            if (instructorType is null)
            {
                throw new InstructorTypeException.NotFound(request.Id);
            }

            var localizations = request.Localizations is null ? Enumerable.Empty<InstructorTypeLocalization>()
                                                              : (from localize in request.Localizations
                                                                 select new InstructorTypeLocalization
                                                                 {
                                                                     InstructorTypeId = instructorType.Id,
                                                                     Language = localize.Language,
                                                                     Name = localize.Name
                                                                 })
                                                                .ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                instructorType.Name = request.Name;
                instructorType.IsActive = request.IsActive;
                instructorType.UpdatedAt = DateTime.UtcNow;
                instructorType.UpdatedBy = requester;

                _dbContext.InstructorTypeLocalizations.RemoveRange(instructorType.Localizations);

                if (localizations.Any())
                {
                    _dbContext.InstructorTypeLocalizations.AddRange(localizations);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(instructorType, localizations);

            return response;
        }

        public void Delete(Guid id)
        {
            var instructorType = _dbContext.InstructorTypes.SingleOrDefault(x => x.Id == id);

            if (instructorType is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.InstructorTypes.Remove(instructorType);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        private static InstructorTypeDTO MapModelToDTO(InstructorType model, IEnumerable<InstructorTypeLocalization> localizations)
        {
            var response = new InstructorTypeDTO
            {
                Id = model.Id,
                Name = model.Name,
                IsActive = model.IsActive,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                Localizations = localizations is null ? Enumerable.Empty<InstructorTypeLocalizationDTO>()
                                                      : (from localize in localizations
                                                         orderby localize.Language
                                                         select new InstructorTypeLocalizationDTO
                                                         {
                                                             Language = localize.Language,
                                                             Name = localize.Name
                                                         })
                                                        .ToList()
            };

            return response;
        }

        private IQueryable<InstructorType> GenerateSearchQuery(SearchCriteriaViewModel? parameters = null)
        {
            var query = _dbContext.InstructorTypes.Include(x => x.Localizations)
                                                  .AsNoTracking();

            if (parameters != null)
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