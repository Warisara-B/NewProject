using Plexus.Entity.DTO;
using Plexus.Entity.DTO.SearchFilter;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider
{
    public interface IPublicationProvider
    {
        /// <summary>
        /// Create publication.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        PublicationDTO Create(CreatePublicationDTO request, string requester);

        /// <summary>
        /// Get all publications by given parameters
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<PublicationDTO> Search(SearchPublicationCriteriaDTO parameters);

        /// <summary>
        /// Search publication according to criteria
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<PublicationDTO> Search(SearchPublicationCriteriaDTO parameters, int page = 1, int pageSize = 25);

        /// <summary>
        /// Get publication by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        PublicationDTO GetById(Guid id);

        /// <summary>
        /// Get publications by id
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>

        IEnumerable<PublicationDTO> GetById(IEnumerable<Guid> ids);

        /// <summary>
        /// Get publications by article type id.
        /// </summary>
        /// <param name="articleTypeId"></param>
        /// <returns></returns>
        IEnumerable<PublicationDTO> GetByArticleTypeId(Guid articleTypeId);

        /// <summary>
        /// Update publication by id.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        PublicationDTO Update(PublicationDTO request, string requester);

        /// <summary>
        /// Delete publication by id.
        /// </summary>
        /// <param name="id"></param>
        void Delete(Guid id);
    }
}