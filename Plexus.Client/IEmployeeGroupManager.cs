using Plexus.Client.ViewModel;
using Plexus.Client.ViewModel.DropDown;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Entity.DTO;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface IEmployeeGroupManager
    {
        /// <summary>
        /// Create new instructor rank.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        EmployeeGroupViewModel Create(CreateEmployeeGroupViewModel request, Guid userId);

        /// <summary>
        /// Search instructor rank by given parameters as paged.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<EmployeeGroupViewModel> Search(SearchEmployeeGroupCriteriaViewModel parameters, int page = 1, int pageSize = 25);

        /// <summary>
        /// Search employee group by given parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<EmployeeGroupViewModel> Search(SearchEmployeeGroupCriteriaViewModel parameters);

        /// <summary>
        /// Search instructor rank by given parameters as drop down list.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<BaseDropDownViewModel> GetDropDownList(SearchEmployeeGroupCriteriaViewModel parameters);

        /// <summary>
        /// Get instructor rank by given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        EmployeeGroupViewModel GetById(Guid id);

        /// <summary>
        /// Update instructor rank.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        EmployeeGroupViewModel Update(Guid id, CreateEmployeeGroupViewModel request, Guid userId);

        /// <summary>
        /// Delete instructor rank by given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);
    }
}