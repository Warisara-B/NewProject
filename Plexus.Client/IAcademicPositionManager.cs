using Plexus.Client.ViewModel.Academic;
using Plexus.Client.ViewModel.DropDown;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface IAcademicPositionManager
    {
        /// <summary>
        /// Create academic position.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        AcademicPositionViewModel Create(CreateAcademicPositionViewModel request);

        /// <summary>
        /// Search academic position by given criteria.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<AcademicPositionViewModel> Search(SearchAcademicPositionCriteriaViewModel parameters);

        /// <summary>
        /// Search academic position by given criteria as paged.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<AcademicPositionViewModel> Search(SearchAcademicPositionCriteriaViewModel parameters, int page = 1, int pageSize = 25);

        /// <summary>
        /// Get academic position as dropdown.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<BaseDropDownViewModel> GetDropDownList(SearchAcademicPositionCriteriaViewModel parameters);

        /// <summary>
        /// Update academic position by given id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        AcademicPositionViewModel Update(Guid id, CreateAcademicPositionViewModel request);

        /// <summary>
        /// Delete academic position by given id.
        /// </summary>
        /// <param name="id"></param>
        void Delete(Guid id);
    }
}