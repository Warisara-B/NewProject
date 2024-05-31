using Microsoft.EntityFrameworkCore;
using Plexus.Database;
using Plexus.Database.Model.Academic;
using Plexus.Database.Model.Localization.Research;
using Plexus.Database.Model.Research;
using Plexus.Entity.DTO.Research;
using Plexus.Entity.DTO.SearchFilter;
using Plexus.Entity.Exception;
using Plexus.Utility.Extensions;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider.src.Research
{
    public class ArticleTypeProvider : IArticleTypeProvider
    {
        private readonly DatabaseContext _dbContext;

        public ArticleTypeProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ArticleTypeDTO Create(CreateArticleTypeDTO request, string requester)
        {
            var model = new ArticleType
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
                _dbContext.ArticleTypes.Add(model);

                if (localizes.Any())
                {
                    _dbContext.ArticleTypeLocalizations.AddRange(localizes);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(model, localizes);

            return response;
        }


        public PagedViewModel<ArticleTypeDTO> Search(SearchArticleTypeCriteriaDTO parameters, int page = 1, int pageSize = 25)
        {
            var query = GenerateSearchQuery(parameters);

            var pagedArticleType = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<ArticleTypeDTO>
            {
                Page = pagedArticleType.Page,
                TotalPage = pagedArticleType.TotalPage,
                TotalItem = pagedArticleType.TotalItem,
                Items = (from articleType in pagedArticleType.Items
                         select MapModelToDTO(articleType, articleType.Localizations))
                         .ToList()
            };

            return response;
        }

        public IEnumerable<ArticleTypeDTO> Search(SearchArticleTypeCriteriaDTO parameters)
        {
            var query = GenerateSearchQuery(parameters);

            var articleTypes = query.ToList();

            var response = (from articleType in articleTypes
                            select MapModelToDTO(articleType, articleType.Localizations))
                            .ToList();

            return response;
        }

        public ArticleTypeDTO GetById(Guid id)
        {
            var articleType = _dbContext.ArticleTypes.Include(x => x.Localizations)
                                                      .AsNoTracking()
                                                      .SingleOrDefault(x => x.Id == id);

            if (articleType is null)
            {
                throw new ArticleTypeException.NotFound(id);
            }

            var response = MapModelToDTO(articleType, articleType.Localizations);

            return response;
        }

        public ArticleTypeDTO Update(ArticleTypeDTO request, string requester)
        {
            var articleType = _dbContext.ArticleTypes.Include(x => x.Localizations)
                                                     .SingleOrDefault(x => x.Id == request.Id);

            if (articleType is null)
            {
                throw new ArticleTypeException.NotFound(request.Id);
            }

            var localizes = request.Localizations is null ? Enumerable.Empty<ArticleTypeLocalization>()
                                                          : (from data in request.Localizations
                                                             select new ArticleTypeLocalization
                                                             {
                                                                 Language = data.Language,
                                                                 ArticleTypeId = articleType.Id,
                                                                 Name = data.Name
                                                             })
                                                             .ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                articleType.Name = request.Name;
                articleType.IsActive = request.IsActive;
                articleType.UpdatedAt = DateTime.UtcNow;
                articleType.UpdatedBy = requester;

                _dbContext.ArticleTypeLocalizations.RemoveRange(articleType.Localizations);

                if (localizes.Any())
                {
                    _dbContext.ArticleTypeLocalizations.AddRange(localizes);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(articleType, localizes);

            return response;
        }

        public void Delete(Guid id)
        {
            var articleType = _dbContext.ArticleTypes.SingleOrDefault(x => x.Id == id);

            if (articleType is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.ArticleTypes.Remove(articleType);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        private IQueryable<ArticleType> GenerateSearchQuery(SearchArticleTypeCriteriaDTO? parameters = null)
        {
            var query = _dbContext.ArticleTypes.Include(x => x.Localizations)
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

        private static ArticleTypeDTO MapModelToDTO(ArticleType model, IEnumerable<ArticleTypeLocalization> localizations)
        {
            return new ArticleTypeDTO
            {
                Id = model.Id,
                Name = model.Name,
                IsActive = model.IsActive,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                Localizations = localizations is null ? Enumerable.Empty<ArticleTypeLocalizationDTO>()
                                                      : (from localize in localizations
                                                         orderby localize.Language
                                                         select new ArticleTypeLocalizationDTO
                                                         {
                                                             Language = localize.Language,
                                                             Name = localize.Name
                                                         })
                                                         .ToList()
            };
        }

        private static IEnumerable<ArticleTypeLocalization> MapLocalizationDTOToModel(
                IEnumerable<ArticleTypeLocalizationDTO> localizations,
                ArticleType model)
        {
            if (localizations is null)
            {
                return Enumerable.Empty<ArticleTypeLocalization>();
            }

            var response = (from locale in localizations
                            select new ArticleTypeLocalization
                            {
                                ArticleType = model,
                                Language = locale.Language,
                                Name = locale.Name
                            })
                            .ToList();

            return response;
        }
    }
}