using Microsoft.EntityFrameworkCore;
using Plexus.Database;
using Plexus.Database.Model.Research;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.SearchFilter;
using Plexus.Entity.Exception;
using Plexus.Utility.Extensions;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider.src.Research
{
    public class PublicationProvider : IPublicationProvider
    {
        private readonly DatabaseContext _dbContext;

        public PublicationProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public PublicationDTO Create(CreatePublicationDTO request, string requester)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var publication = new Publication
            {
                ArticleTypeId = request.ArticleTypeId,
                Authors = request.Authors,
                Pages = request.Pages,
                Year = request.Year,
                CitationPages = request.CitationPages,
                CitationDOI = request.CitationDOI
            };

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Publications.Add(publication);

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapPublicationDTO(publication);

            return response;
        }

        public IEnumerable<PublicationDTO> Search(SearchPublicationCriteriaDTO parameters)
        {
            var query = GenerateSearchQuery(parameters);

            var publications = query.ToList();

            var response = (from publication in publications
                            orderby publication.Year descending
                            select MapPublicationDTO(publication))
                            .ToList();

            return response;
        }

        public PagedViewModel<PublicationDTO> Search(SearchPublicationCriteriaDTO parameters, int page = 1, int pageSize = 25)
        {
            var query = GenerateSearchQuery(parameters);

            var pagedPublication = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<PublicationDTO>
            {
                Page = pagedPublication.Page,
                TotalPage = pagedPublication.TotalPage,
                TotalItem = pagedPublication.TotalItem,
                Items = (from publication in pagedPublication.Items
                         select MapPublicationDTO(publication))
                         .ToList()
            };

            return response;
        }

        public PublicationDTO GetById(Guid id)
        {
            var publication = _dbContext.Publications.AsNoTracking()
                                                     .SingleOrDefault(x => x.Id == id);

            if (publication is null)
            {
                throw new PublicationException.NotFound(id);
            }

            var response = MapPublicationDTO(publication);

            return response;
        }

        public IEnumerable<PublicationDTO> GetById(IEnumerable<Guid> ids)
        {
            var publications = _dbContext.Publications.AsNoTracking()
                                                      .Where(x => ids.Contains(x.Id))
                                                      .ToList();

            var response = (from publication in publications
                            orderby publication.Year descending
                            select MapPublicationDTO(publication))
                            .ToList();

            return response;
        }

        public IEnumerable<PublicationDTO> GetByArticleTypeId(Guid articleTypeId)
        {
            var publications = _dbContext.Publications.AsNoTracking()
                                                      .Where(x => x.ArticleTypeId == articleTypeId)
                                                      .ToList();

            var response = (from publication in publications
                            orderby publication.Year descending
                            select MapPublicationDTO(publication))
                            .ToList();

            return response;

        }

        public PublicationDTO Update(PublicationDTO request, string requester)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var publication = _dbContext.Publications.SingleOrDefault(x => x.Id == request.Id);

            if (publication is null)
            {
                throw new PublicationException.NotFound(request.Id);
            }

            publication.ArticleTypeId = request.ArticleTypeId;
            publication.Authors = request.Authors;
            publication.Pages = request.Pages;
            publication.Year = request.Year;
            publication.CitationPages = request.CitationPages;
            publication.CitationDOI = request.CitationDOI;

            _dbContext.SaveChanges();

            var response = MapPublicationDTO(publication);

            return response;
        }

        public void Delete(Guid id)
        {
            var publication = _dbContext.Publications.AsNoTracking()
                                                     .SingleOrDefault(x => x.Id == id);

            if (publication is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Publications.Remove(publication);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        public IQueryable<Publication> GenerateSearchQuery(SearchPublicationCriteriaDTO? parameters)
        {
            var query = _dbContext.Publications.AsNoTracking();

            if (parameters is not null)
            {
                if (parameters.ArticleTypeId.HasValue)
                {
                    query = query.Where(x => x.ArticleTypeId == parameters.ArticleTypeId.Value);
                }

                if (!string.IsNullOrEmpty(parameters.Keyword))
                {
                    query = query.Where(x => parameters.Keyword.Contains(x.Authors)
                                       || (!string.IsNullOrEmpty(x.CitationPages)
                                           && parameters.Keyword.Contains(x.CitationPages))
                                       || (!string.IsNullOrEmpty(x.CitationDOI)
                                           && parameters.Keyword.Contains(x.CitationDOI)));
                }

                if (parameters.Year.HasValue)
                {
                    query = query.Where(x => x.Year == parameters.Year.Value);
                }
            }

            query = query.OrderBy(x => x.Year);

            if (parameters is not null)
            {
                if (!string.IsNullOrEmpty(parameters.SortBy))
                {
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
                }
            }

            return query;
        }

        private static PublicationDTO MapPublicationDTO(Publication publication)
        {
            if (publication is null)
            {
                return null;
            }

            return new PublicationDTO
            {
                Id = publication.Id,
                ArticleTypeId = publication.ArticleTypeId,
                Authors = publication.Authors,
                Pages = publication.Pages,
                Year = publication.Year,
                CitationPages = publication.CitationPages,
                CitationDOI = publication.CitationDOI
            };
        }
    }
}