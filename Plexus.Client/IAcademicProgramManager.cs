using Plexus.Client.ViewModel.Academic;
using Plexus.Client.ViewModel.DropDown;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Entity.DTO;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface IAcademicProgramManager
    {
        /// <summary>
        /// Create new academic program.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        AcademicProgramViewModel Create(CreateAcademicProgramViewModel request, Guid userId);

        IEnumerable<AcademicProgramViewModel> Search(SearchAcademicProgramCriteriaViewModel? parameters = null);

        /// <summary>
        /// Search academic program by given parameters as paged.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<AcademicProgramViewModel> Search(SearchAcademicProgramCriteriaViewModel parameters, int page = 1, int pageSize = 25);

        /// <summary>
        /// Search academic program by given parameters as drop down list.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<AcademicProgramDropDownViewModel> GetDropDownList(SearchAcademicProgramCriteriaViewModel parameters);

        /// <summary>
        /// Get academic program by given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        AcademicProgramViewModel GetById(Guid id);

        /// <summary>
        /// Update academic program.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        AcademicProgramViewModel Update(Guid id, CreateAcademicProgramViewModel request, Guid userId);

        /// <summary>
        /// Delete academic program by given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);
    }
}