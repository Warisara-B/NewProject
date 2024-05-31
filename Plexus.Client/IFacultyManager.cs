using Plexus.Client.ViewModel.Academic;
using Plexus.Client.ViewModel.DropDown;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface IFacultyManager
    {
        /// <summary>
        /// Create new faculty.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<FacultyViewModel> CreateAsync(CreateFacultyViewModel request, Guid userId);

        /// <summary>
        /// Get faculty by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        FacultyViewModel GetById(Guid id);

        /// <summary>
        /// Search faculty as drop down list by given parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<BaseDropDownViewModel> GetDropdownList(SearchFacultyCriteriaViewModel parameters);

        /// <summary>
        /// Get faculties by given parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<FacultyViewModel> Search(SearchFacultyCriteriaViewModel? parameters = null);

        /// <summary>
        /// Search faculty.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<FacultyViewModel> Search(SearchFacultyCriteriaViewModel parameters, int page, int pageSize);

        /// <summary>
        /// Update faculty.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<FacultyViewModel> UpdateAsync(Guid id, UpdateFacultyViewModel request, Guid userId);

        /// <summary>
        /// Delete faculty.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);
    }
}