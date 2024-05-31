using Plexus.Client.ViewModel.DropDown;
using Plexus.Client.ViewModel.Payment;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface IFeeGroupManager
    {
        /// <summary>
        /// Create new fee group.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        FeeGroupViewModel Create(CreateFeeGroupViewModel request, Guid userId);

        /// <summary>
        /// Get fee group by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        FeeGroupViewModel GetById(Guid id);

        /// <summary>
        /// Get all fee group as drop down by givien parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<BaseDropDownViewModel> GetDropDownList(SearchFeeGroupCriteriaViewModel parameters);

        /// <summary>
        /// Search fee group according to given parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<FeeGroupViewModel> Search(SearchFeeGroupCriteriaViewModel? parameters = null);

        /// <summary>
        /// Search fee group according to given parameters as paged.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<FeeGroupViewModel> Search(SearchFeeGroupCriteriaViewModel parameters, int page = 1, int pageSize = 25);

        /// <summary>
        /// Update fee group information.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        FeeGroupViewModel Update(Guid id, CreateFeeGroupViewModel request, Guid userId);

        /// <summary>
        /// Delete fee group by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);
    }
}