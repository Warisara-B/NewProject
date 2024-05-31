using Plexus.Database.Enum;
using Plexus.Service.ViewModel;
using Plexus.Utility.ViewModel;

namespace Plexus.Service
{
    public interface IAnnouncementService
    {
        /// <summary>
        /// Get List of news categories
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        IEnumerable<NewsCategoryViewModel> GetNewsCategories(LanguageCode language);

        /// <summary>
        /// Search new List according to given parameters
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="instructorId"></param>
        /// <param name="studentId"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        PagedViewModel<NewsListViewModel> SearchNews(
            NewsFilterViewModel filter, int page, int pageSize,
            Guid? instructorId, Guid? studentId, LanguageCode language);

        /// <summary>
        /// Get news detail by ID
        /// </summary>
        /// <param name="newsId"></param>
        /// <param name="instructorId"></param>
        /// <param name="studentId"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        NewsDetailViewModel GetNewsDetailById(Guid newsId, Guid? instructorId, Guid? studentId, LanguageCode language);

        /// <summary>
        /// Add news to bookmark list
        /// </summary>
        /// <param name="newsId"></param>
        /// <param name="instructorId"></param>
        /// <param name="studentId"></param>
        void AddBookmarkNews(Guid newsId, Guid? instructorId, Guid? studentId);

        /// <summary>
        /// Remove news from bookmark list
        /// </summary>
        /// <param name="newsId"></param>
        /// <param name="instructorId"></param>
        /// <param name="studentId"></param>
        void RemoveBookmarkNews(Guid newsId, Guid? instructorId, Guid? studentId);
    }
}