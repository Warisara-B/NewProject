using Plexus.Client.ViewModel.DropDown;
using Plexus.Client.ViewModel.Payment;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Entity.DTO;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface IRateTypeManager
    {
        /// <summary>
        /// Create new rate type.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        RateTypeViewModel Create(CreateRateTypeViewModel request);

        /// <summary>
        /// Search rate type by given parameters as paged.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<RateTypeViewModel> Search(SearchRateTypeCriteriaViewModel parameters, int page = 1, int pageSize = 25);

        /// <summary>
        /// Search rate type by given parameters. 
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<RateTypeViewModel> Search(SearchRateTypeCriteriaViewModel parameters);

        /// <summary>
        /// Search rate type by given parameters as drop down list.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<RateTypeDropDownViewModel> GetDropDownList(SearchRateTypeCriteriaViewModel parameters);

        /// <summary>
        /// Get rate type by given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        RateTypeViewModel GetById(Guid id);

        /// <summary>
        /// Update rate type.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        RateTypeViewModel Update(Guid id, CreateRateTypeViewModel request);

        /// <summary>
        /// Delete rate type by given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);
    }
}