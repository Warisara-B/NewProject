using Microsoft.EntityFrameworkCore;
using Plexus.Database;
using Plexus.Database.Model.Localization.Research;
using Plexus.Database.Model.Research;
using Plexus.Entity.DTO.Research;
using Plexus.Entity.DTO.SearchFilter;
using Plexus.Entity.Exception;
using Plexus.Utility.Extensions;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider.src.Research
{
    public class ResearchCategoryProvider : IResearchCategoryProvider
    {
        private readonly DatabaseContext _dbContext;

        public ResearchCategoryProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ResearchCategoryDTO Create(CreateResearchCategoryDTO request, string requester)
        {
            var model = new ResearchCategory
            {
                Name = request.Name,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = requester
            };

            var localizes = MapLocalizationDTOToModel(request.Localizations, model).ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.ResearchCategories.Add(model);
                if (localizes.Any())
                {
                    _dbContext.ResearchCategoryLocalizations.AddRange(localizes);
                }
                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(model, localizes);

            return response;
        }

        public PagedViewModel<ResearchCategoryDTO> Search(SearchResearchCategoryCriteriaDTO parameters, int page = 1, int pageSize = 25)
        {
            var query = GenerateSearchQuery(parameters);

            var pagedResearchCategory = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<ResearchCategoryDTO>
            {
                Page = pagedResearchCategory.Page,
                TotalPage = pagedResearchCategory.TotalPage,
                TotalItem = pagedResearchCategory.TotalItem,
                Items = (from researchCategory in pagedResearchCategory.Items
                         select MapModelToDTO(researchCategory, researchCategory.Localizations))
                         .ToList()
            };

            return response;
        }

        public ResearchCategoryDTO GetById(Guid id)
        {
            var researchCategory = _dbContext.ResearchCategories.Include(x => x.Localizations)
                                                           .AsNoTracking()
                                                           .SingleOrDefault(x => x.Id == id);
            if (researchCategory is null)
            {
                throw new ResearchCategoryException.NotFound(id);
            }
            var response = MapModelToDTO(researchCategory, researchCategory.Localizations);
            return response;
        }

        public ResearchCategoryDTO Update(ResearchCategoryDTO request, string requester)
        {
            var researchCategory = _dbContext.ResearchCategories.Include(x => x.Localizations)
                                                     .SingleOrDefault(x => x.Id == request.Id);
            if (researchCategory is null)
            {
                throw new ResearchCategoryException.NotFound(request.Id);
            }
            var localizes = request.Localizations is null ? Enumerable.Empty<ResearchCategoryLocalization>()
                                                          : (from data in request.Localizations
                                                             select new ResearchCategoryLocalization
                                                             {
                                                                 Language = data.Language,
                                                                 ResearchCategoryId = researchCategory.Id,
                                                                 Name = data.Name
                                                             })
                                                             .ToList();
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                researchCategory.Name = request.Name;
                researchCategory.IsActive = request.IsActive;
                researchCategory.UpdatedAt = DateTime.UtcNow;
                researchCategory.UpdatedBy = requester;
                _dbContext.ResearchCategoryLocalizations.RemoveRange(researchCategory.Localizations);
                if (localizes.Any())
                {
                    _dbContext.ResearchCategoryLocalizations.AddRange(localizes);
                }
                transaction.Commit();
            }
            _dbContext.SaveChanges();
            var response = MapModelToDTO(researchCategory, localizes);
            return response;
        }

        public void Delete(Guid id)
        {
            var researchCategory = _dbContext.ResearchCategories.SingleOrDefault(x => x.Id == id);
            if (researchCategory is null)
            {
                return;
            }
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.ResearchCategories.Remove(researchCategory);
                transaction.Commit();
            }
            _dbContext.SaveChanges();
        }


        private IQueryable<ResearchCategory> GenerateSearchQuery(SearchResearchCategoryCriteriaDTO? parameters = null)
        {
            var query = _dbContext.ResearchCategories.Include(x => x.Localizations)
                                               .AsNoTracking();
            if (parameters is not null)
            {
                if (!string.IsNullOrEmpty(parameters.Keyword))
                {
                    query = query.Where(x => x.Name.Contains(parameters.Keyword));
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

        private static ResearchCategoryDTO MapModelToDTO(ResearchCategory model, IEnumerable<ResearchCategoryLocalization> localizations)
        {
            return new ResearchCategoryDTO
            {
                Id = model.Id,
                Name = model.Name,
                IsActive = model.IsActive,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                Localizations = localizations is null ? Enumerable.Empty<ResearchCategoryLocalizationDTO>()
                                          : (from localize in localizations
                                             orderby localize.Language
                                             select new ResearchCategoryLocalizationDTO
                                             {
                                                 Language = localize.Language,
                                                 Name = localize.Name
                                             })
                                             .ToList()
            };
        }

        private static IEnumerable<ResearchCategoryLocalization> MapLocalizationDTOToModel(
        IEnumerable<ResearchCategoryLocalizationDTO> localizations,
        ResearchCategory model)
        {
            if (localizations is null)
            {
                return Enumerable.Empty<ResearchCategoryLocalization>();
            }
            var response = (from locale in localizations
                            select new ResearchCategoryLocalization
                            {
                                ResearchCategory = model,
                                Language = locale.Language,
                                Name = locale.Name
                            })
                            .ToList();
            return response;
        }
    }
}