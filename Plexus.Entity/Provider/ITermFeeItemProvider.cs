using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Payment;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider
{
    public interface ITermFeeItemProvider
    {
        /// <summary>
        /// Create new term fee item.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        TermFeeItemDTO Create(CreateTermFeeItemDTO request, string requester);

        /// <summary>
        /// Search term fee items as paging.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<TermFeeItemDTO> Search(SearchCriteriaViewModel parameters, int page, int pageSize);

        /// <summary>
        /// Get term fee by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TermFeeItemDTO GetById(Guid id);

        /// <summary>
        /// Update term fee item.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        TermFeeItemDTO Update(TermFeeItemDTO request, string requester);

        /// <summary>
        /// Delete term fee item by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);
    }
}