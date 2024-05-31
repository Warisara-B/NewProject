using Plexus.Client.ViewModel.Payment;
using Plexus.Entity.DTO;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface ITermFeeItemManager
    {
        /// <summary>
        /// Create new term fee item.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        TermFeeItemViewModel Create(CreateTermFeeItemViewModel request, Guid userId);

        /// <summary>
        /// Search term fee item according to given parameters as paged.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<TermFeeItemViewModel> Search(SearchCriteriaViewModel parameters, int page, int pageSize);

        /// <summary>
        /// Get term fee item by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TermFeeItemViewModel GetById(Guid id);

        /// <summary>
        /// Update term fee item information.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        TermFeeItemViewModel Update(TermFeeItemViewModel request, Guid userId);

        /// <summary>
        /// Delete term fee item by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);
    }
}