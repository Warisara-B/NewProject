using Plexus.Client.ViewModel.Academic;
using Plexus.Client.ViewModel.DropDown;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface ISectionManager
    {
        /// <summary>
        /// Create section.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        SectionViewModel Create(CreateSectionViewModel request);

        /// <summary>
        /// Search section by given parameters as paged.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<SectionViewModel> Search(SearchSectionCriteriaViewModel parameters, int page, int pageSize);

        IEnumerable<SectionViewModel> GetByIds(IEnumerable<Guid> ids);

        /// <summary>
        /// Get section by given parameters as drop down.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<SectionDropDownViewModel> GetDropDownList(SearchSectionCriteriaViewModel parameters);

        /// <summary>
        /// Update section status by id.
        /// </summary>
        /// <param name="id"></param>
        void UpdateStatus(Guid id);

        /// <summary>
        /// Delete by id.
        /// </summary>
        /// <param name="id"></param>
        void Delete(Guid id);
    }
}