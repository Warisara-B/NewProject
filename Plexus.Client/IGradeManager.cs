using Plexus.Client.ViewModel.Academic;
using Plexus.Client.ViewModel.DropDown;
using Plexus.Entity.DTO;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface IGradeManager
    {
        /// <summary>
        /// Create new grade
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        GradeViewModel Create(CreateGradeViewModel request, Guid userId);

        /// <summary>
        /// Get grade by id
        /// </summary>
        /// <param name="gradeId"></param>
        /// <returns></returns>
        GradeViewModel GetById(Guid gradeId);

        /// <summary>
        /// Get grade by given parameters as paged response
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<GradeViewModel> Search(SearchCriteriaViewModel parameters, int page, int pageSize);

        /// <summary>
        /// Update grade information
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        GradeViewModel Update(GradeViewModel request, Guid userId);

        /// <summary>
        /// Delete grade
        /// </summary>
        /// <param name="id"></param>
        void Delete(Guid id);

        /// <summary>
        /// Get grade drop down list
        /// </summary>
        /// <returns></returns>
        IEnumerable<BaseDropDownViewModel> GetDropdownList();
    }
}