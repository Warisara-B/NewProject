using Plexus.Client.ViewModel.Research;
using Plexus.Entity.DTO.SearchFilter;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface IPublicationManager
    {
        /// <summary>
        /// Create publication.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        PublicationViewModel Create(CreatePublicationViewModel request, Guid userId);

        /// <summary>
        /// Get publications by criteria.
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<PublicationViewModel> Search(SearchPublicationCriteriaViewModel criteria, int page = 1, int pageSize = 25);

        /// <summary>
        /// Get publication by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        PublicationViewModel GetById(Guid id);

        /// <summary>
        /// Update publication by id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        PublicationViewModel Update(Guid id, CreatePublicationViewModel request, Guid userId);

        /// <summary>
        /// Delete publication by id.
        /// </summary>
        /// <param name="id"></param>
        void Delete(Guid id);
    }
}