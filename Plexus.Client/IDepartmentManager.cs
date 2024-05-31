using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Client.ViewModel.Academic;
using Plexus.Client.ViewModel.DropDown;
using Plexus.Entity.DTO.Academic;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface IDepartmentManager
    {
        /// <summary>
        /// Create new department.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<DepartmentViewModel> CreateAsync(CreateDepartmentViewModel request, Guid userId);

        /// <summary>
        /// Get department by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DepartmentViewModel GetById(Guid id);

        /// <summary>
        /// Search department by given parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<DepartmentViewModel> Search(SearchDepartmentCriteriaViewModel? parameters = null);

        /// <summary>
        /// Search department as drop down list by given parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<DepartmentDropDownViewModel> GetDropdownList(SearchDepartmentCriteriaViewModel parameters);

        /// <summary>
        /// Search department.
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<DepartmentViewModel> Search(SearchDepartmentCriteriaViewModel criteria, int page, int pageSize);

        /// <summary>
        /// Update department information.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<DepartmentViewModel> UpdateAsync(Guid id, UpdateDepartmentViewModel request, Guid userId);

        /// <summary>
        /// Delete department.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);
    }
}