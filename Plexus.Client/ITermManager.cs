using Plexus.Client.ViewModel;
using Plexus.Client.ViewModel.Academic;
using Plexus.Client.ViewModel.DropDown;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface ITermManager
    {
        /// <summary>
        /// Create new term record
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        TermViewModel Create(CreateTermViewModel request, Guid userId);

        /// <summary>
        /// Get by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TermViewModel GetById(Guid id);

        /// <summary>
        /// Get all terms as drop down by givien parameters
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<BaseDropDownViewModel> GetDropDownList(SearchTermCriteriaViewModel parameters);

        /// <summary>
        /// Get all terms by given parameters
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<TermViewModel> Search(SearchTermCriteriaViewModel parameters);

        /// <summary>
        /// Search term according to given criteria
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<TermViewModel> Search(SearchTermCriteriaViewModel criteria, int page, int pageSize);

        /// <summary>
        /// Update term information
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        TermViewModel Update(Guid id, CreateTermViewModel request, Guid userId);

        /// <summary>
        /// Delete term information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);

        /// <summary>
        /// Check term status conflict.
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        TermViewModel CheckStatus(TermStatusCheckViewModel criteria);
    }
}