using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Client.ViewModel.Academic;
using Plexus.Client.ViewModel.DropDown;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface IAcademicLevelManager
    {
        /// <summary>
        /// Create new academic level record
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        AcademicLevelViewModel Create(CreateAcademicLevelViewModel request, Guid userId);

        /// <summary>
        /// Get by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        AcademicLevelViewModel GetById(Guid id);

        /// <summary>
        /// Get all academic levels as drop down by givien parameters
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<BaseDropDownViewModel> GetDropDownList(SearchAcademicLevelCriteriaViewModel parameters);

        /// <summary>
        /// Get academic level by given parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<AcademicLevelViewModel> Search(SearchAcademicLevelCriteriaViewModel? parameters = null);

        /// <summary>
        /// Search academic level according to given parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<AcademicLevelViewModel> Search(SearchAcademicLevelCriteriaViewModel? parameters, int page, int pageSize);

        /// <summary>
        /// Update academic level information
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        AcademicLevelViewModel Update(Guid id, CreateAcademicLevelViewModel request, Guid userId);

        /// <summary>
        /// Delete academic level information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);
    }
}