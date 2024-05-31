using Microsoft.EntityFrameworkCore;
using Plexus.Database;
using Plexus.Database.Enum;
using Plexus.Database.Model.Announcement;
using Plexus.Service.Exception;
using Plexus.Service.ViewModel;
using Plexus.Utility.Extensions;
using Plexus.Utility.ViewModel;

namespace Plexus.Service.src
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly DatabaseContext _dbContext;

        public AnnouncementService(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<NewsCategoryViewModel> GetNewsCategories(LanguageCode language)
        {
            var categories = _dbContext.NewsCategories.AsNoTracking()
                                                      .Include(x => x.Localizations)
                                                      .Where(x => x.IsActive)
                                                      .ToList();

            if (!categories.Any())
            {
                return Enumerable.Empty<NewsCategoryViewModel>();
            }

            var response = (from data in categories
                            orderby data.Ordering, data.Name
                            let localeData = data.Localizations.SingleOrDefault(x => x.Language == language)
                            select new NewsCategoryViewModel
                            {
                                Id = data.Id,
                                Name = localeData?.Name ?? data.Name
                            })
                           .ToList();

            return response;
        }

        public PagedViewModel<NewsListViewModel> SearchNews(
            NewsFilterViewModel filter, int page, int pageSize,
            Guid? instructorId, Guid? studentId, LanguageCode language)
        {
            // GENERATE QUERY
            var query = _dbContext.News.Include(x => x.Localizations)
                                       .Include(x => x.Publisher)
                                            .ThenInclude(x => x.Localizations)
                                       .Where(x => x.PublishAt < DateTime.UtcNow)
                                       .AsNoTracking();

            // APPLIED FILTER JUST ACCORDINGLY
            // CATEGORY
            if (filter.CategoryIds.Any())
            {
                query = query.Where(x => filter.CategoryIds.Contains(x.CategoryId));
            }

            // PINNED
            if (filter.IsPinned.HasValue)
            {
                query = query.Where(x => x.IsPinned == filter.IsPinned.Value);
            }

            // BUILD HANDLE BOOKMARK FLAG
            var bookmarkNewsIds = Enumerable.Empty<Guid>();

            if (instructorId.HasValue)
            {
                bookmarkNewsIds = _dbContext.BookmarkNews.AsNoTracking()
                                                         .Where(x => x.InstructorId.HasValue
                                                                     && x.InstructorId.Value == instructorId.Value)
                                                         .Select(x => x.NewsId)
                                                         .ToList();
            }
            else if (studentId.HasValue)
            {
                bookmarkNewsIds = _dbContext.BookmarkNews.AsNoTracking()
                                                         .Where(x => x.StudentId.HasValue
                                                                     && x.StudentId.Value == studentId.Value)
                                                         .Select(x => x.NewsId)
                                                         .ToList();
            }

            // BOOKMARK FLAG
            if (filter.IsBookmarked.HasValue)
            {
                if (filter.IsBookmarked.Value)
                {
                    query = query.Where(x => bookmarkNewsIds.Contains(x.Id));
                }
                else
                {
                    query = query.Where(x => !bookmarkNewsIds.Contains(x.Id));
                }
            }

            // ORDERING BEFORE PERFORM PAGING
            query = query.OrderByDescending(x => x.PublishAt);
            var pagedNews = query.GetPagedViewModel(page, pageSize);

            // MAP RESPONSE
            var response = new PagedViewModel<NewsListViewModel>
            {
                Page = page,
                PageSize = pageSize,
                TotalPage = pagedNews.TotalPage,
                TotalItem = pagedNews.TotalItem
            };

            // NO DATA, DO NOTHING
            if (!pagedNews.Items.Any())
            {
                return response;
            }

            // MAP BOOKMARKED
            var newsIds = pagedNews.Items.Select(x => x.Id)
                                         .ToList();

            response.Items = (from data in pagedNews.Items
                              let localeData = data.Localizations.FirstOrDefault(x => x.Language == language)
                              let publisher = data.Publisher
                              let publisherLocale = publisher.Localizations.FirstOrDefault(x => x.Language == language)
                              select new NewsListViewModel
                              {
                                  Id = data.Id,
                                  Title = localeData?.Title ?? data.Title,
                                  Publisher = publisherLocale?.Name ?? publisher.Name,
                                  PublishAt = data.PublishAt,
                                  ThumbnailUrl = data.ThumbnailUrl,
                                  IsPinned = data.IsPinned,
                                  IsBookmarked = bookmarkNewsIds.Contains(data.Id),
                              })
                             .ToList();

            return response;
        }

        public NewsDetailViewModel GetNewsDetailById(Guid newsId, Guid? instructorId, Guid? studentId, LanguageCode language)
        {
            var news = _dbContext.News.AsNoTracking()
                                      .Include(x => x.Localizations)
                                      .Include(x => x.Publisher)
                                            .ThenInclude(x => x.Localizations)
                                      .SingleOrDefault(x => x.Id == newsId);

            if (news is null || news.PublishAt > DateTime.UtcNow)
            {
                throw new NewsException.NotFound();
            }

            var isBookmark = _dbContext.BookmarkNews.AsNoTracking()
                                                    .Any(x => x.InstructorId == instructorId
                                                              && x.StudentId == studentId
                                                              && x.NewsId == news.Id);

            var localeData = news.Localizations.SingleOrDefault(x => x.Language == language);
            var publisher = news.Publisher;
            var publisherLocale = publisher.Localizations.SingleOrDefault(x => x.Language == language);

            var response = new NewsDetailViewModel
            {
                Id = news.Id,
                Title = localeData?.Title ?? news.Title,
                Content = localeData?.Content ?? news.Content,
                Publisher = publisherLocale?.Name ?? publisher.Name,
                PublishAt = news.PublishAt,
                IsBookmarked = isBookmark,
                IsPinned = news.IsPinned
            };

            if (!string.IsNullOrWhiteSpace(news.ThumbnailUrl))
            {
                response.ContentImageUrls = new List<string>
                {
                    news.ThumbnailUrl
                };
            }

            return response;
        }

        public void AddBookmarkNews(Guid newsId, Guid? instructorId, Guid? studentId)
        {
            // ADMIN or OTHER roles do nothing
            if(!studentId.HasValue && !instructorId.HasValue)
            {
                return;
            }

            // CHECK NEWS EXISTS
            var isNewsExists = _dbContext.News.AsNoTracking()
                                              .Any(x => x.Id == newsId);
            if (!isNewsExists)
            {
                throw new NewsException.NotFound();
            }

            // IF ALREADY BOOKMARK DO NOTHING
            var isBookmark = _dbContext.BookmarkNews.AsNoTracking()
                                                    .Any(x => x.NewsId == newsId
                                                              && x.InstructorId == instructorId
                                                              && x.StudentId == studentId);
            if (isBookmark)
            {
                return;
            }

            var bookMarkModel = new BookmarkNews
            {
                NewsId = newsId,
                InstructorId = instructorId,
                StudentId = studentId,
                BookmarkAt = DateTime.UtcNow
            };

            _dbContext.BookmarkNews.Add(bookMarkModel);
            _dbContext.SaveChanges();
        }

        public void RemoveBookmarkNews(Guid newsId, Guid? instructorId, Guid? studentId)
        {
            // CHECK BOOKMARK EXISTS
            var bookMark = _dbContext.BookmarkNews.Single(x => x.NewsId == newsId
                                                               && x.InstructorId == instructorId
                                                               && x.StudentId == studentId);
            if (bookMark is null)
            {
                return;
            }

            _dbContext.BookmarkNews.Remove(bookMark);
            _dbContext.SaveChanges();
        }
    }
}

