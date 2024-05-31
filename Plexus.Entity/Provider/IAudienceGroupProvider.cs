using Plexus.Entity.DTO;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider
{
    public interface IAudienceGroupProvider
    {
        /// <summary>
        /// Create audience group.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        AudienceGroupDTO Create(CreateAudienceGroupDTO request, string requester);

        /// <summary>
        /// Search audience group with given parameters as paged.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<AudienceGroupDTO> Search(SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25);

        /// <summary>
        /// Search audience group with given parameters as list.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<AudienceGroupDTO> Search(SearchCriteriaViewModel parameters);

        /// <summary>
        /// Get audience group by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        AudienceGroupDTO GetById(Guid id);

        /// <summary>
        /// Update audience group info.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        AudienceGroupDTO Update(AudienceGroupDTO request, string requester);

        /// <summary>
        /// Delete audience group by id.
        /// </summary>
        /// <param name="id"></param>
        void Delete(Guid id);
    }
}