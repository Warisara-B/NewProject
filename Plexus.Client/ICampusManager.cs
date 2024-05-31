using Plexus.Client.ViewModel.DropDown;
using Plexus.Client.ViewModel.Facility;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface ICampusManager
    {
        /// <summary>
        /// Create new campus record
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        CampusViewModel Create(CreateCampusViewModel request, Guid userId);

        /// <summary>
        /// Get campus by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        CampusViewModel GetById(Guid id);

        /// <summary>
        /// Search campus as no paging
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<CampusViewModel> Search(SearchCampusCriteriaViewModel? parameters);

        /// <summary>
        /// Search campus according to given criteria
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<CampusViewModel> Search(SearchCampusCriteriaViewModel parameters, int page, int pageSize);

        /// <summary>
        /// Update campus information
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        CampusViewModel Update(Guid id, CreateCampusViewModel request, Guid userId);

        /// <summary>
        /// Delete campus information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);

        /// <summary>
        /// Get Campus Dropdown list
        /// </summary>
        /// <returns></returns>
        IEnumerable<BaseDropDownViewModel> GetDropdownList(SearchCampusCriteriaViewModel parameters);
    }
}