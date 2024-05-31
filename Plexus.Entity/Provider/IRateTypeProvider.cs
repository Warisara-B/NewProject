using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Payment;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider
{
    public interface IRateTypeProvider
    {
        /// <summary>
        /// Create new rate type.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        RateTypeDTO Create(CreateRateTypeDTO request, string requester);

        /// <summary>
        /// Search rate type by given parameters as paged.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<RateTypeDTO> Search(SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25);

        /// <summary>
        /// Search rate type by given parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<RateTypeDTO> Search(SearchCriteriaViewModel parameters);

        /// <summary>
        /// Get rate type by given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        RateTypeDTO GetById(Guid id);

        /// <summary>
        /// Get rate types by given ids.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        IEnumerable<RateTypeDTO> GetById(IEnumerable<Guid> ids);

        /// <summary>
        /// Update rate type.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        RateTypeDTO Update(RateTypeDTO request, string requester);

        /// <summary>
        /// Delete rate type by given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);
    }
}