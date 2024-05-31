using Plexus.Client.ViewModel.DropDown;
using Plexus.Client.ViewModel.Payment;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Entity.DTO;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface IFeeItemManager
    {
        /// <summary>
        /// Create new fee item.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        FeeItemViewModel Create(CreateFeeItemViewModel request, Guid userId);

        /// <summary>
        /// Get fee item by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        FeeItemViewModel GetById(Guid id);

        /// <summary>
        /// Get all fee item as drop down by givien parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<BaseDropDownViewModel> GetDropDownList(SearchFeeItemCriteriaViewModel parameters);

        /// <summary>
        /// Search fee item according to given parameters. 
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<FeeItemViewModel> Search(SearchFeeItemCriteriaViewModel? parameters = null);

        /// <summary>
        /// Search fee item according to given parameters as paged.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<FeeItemViewModel> Search(SearchFeeItemCriteriaViewModel parameters, int page = 1, int pageSize = 25);

        /// <summary>
        /// Update fee item information.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        FeeItemViewModel Update(Guid id, CreateFeeItemViewModel request, Guid userId);

        /// <summary>
        /// Delete fee item by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);
    }
}