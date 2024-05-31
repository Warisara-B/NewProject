using Plexus.Client.ViewModel;
using Plexus.Entity.DTO;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface IAudienceGroupManager
    {
        /// <summary>
        /// Create audience group.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        AudienceGroupViewModel Create(CreateAudienceGroupViewModel request, Guid userId);

        /// <summary>
        /// Search audience group with given parameters as paged.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<AudienceGroupViewModel> Search(SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25);

        /// <summary>
        /// Search audience group with given parameters as list.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<AudienceGroupViewModel> Search(SearchCriteriaViewModel parameters);

        /// <summary>
        /// Get audience group by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        AudienceGroupViewModel GetById(Guid id);

        /// <summary>
        /// Update audience group info.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        AudienceGroupViewModel Update(AudienceGroupViewModel request, Guid userId);

        /// <summary>
        /// Delete audience group by id.
        /// </summary>
        /// <param name="id"></param>
        void Delete(Guid id);
    }
}