using Plexus.Client.ViewModel.Academic;
using Plexus.Client.ViewModel.DropDown;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface ICareerPositionManager
    {
        /// <summary>
        /// Create career position.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        CareerPositionViewModel Create(CreateCareerPositionViewModel request);

        /// <summary>
        /// Search career position by given criteria as paged.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<CareerPositionViewModel> Search(SearchCareerPositionCriteriaViewModel parameters, int page = 1, int pageSize = 25);

        /// <summary>
        /// Search career position by given criteria.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<CareerPositionViewModel> Search(SearchCareerPositionCriteriaViewModel parameters);

        /// <summary>
        /// Get career position as dropdown.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<BaseDropDownViewModel> GetDropDownList(SearchCareerPositionCriteriaViewModel parameters);

        /// <summary>
        /// Update career position by given id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        CareerPositionViewModel Update(Guid id, CreateCareerPositionViewModel request);

        /// <summary>
        /// Delete career position by given id.
        /// </summary>
        /// <param name="id"></param>
        void Delete(Guid id);
    }
}