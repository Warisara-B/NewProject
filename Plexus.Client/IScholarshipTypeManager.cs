using Plexus.Client.ViewModel.DropDown;
using Plexus.Client.ViewModel.Payment.Scholarship;
using Plexus.Entity.DTO;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface IScholarshipTypeManager
    {
        /// <summary>
        /// Create new scholarship type.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        ScholarshipTypeViewModel Create(CreateScholarshipTypeViewModel request, Guid userId);

        /// <summary>
        /// Search scholarship type by given parameters as paged.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<ScholarshipTypeViewModel> Search(SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25);

        /// <summary>
        /// Search scholarship type by given parameters as drop down list.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<BaseDropDownViewModel> GetDropDownList(SearchCriteriaViewModel parameters);

        /// <summary>
        /// Get scholarship type by given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ScholarshipTypeViewModel GetById(Guid id);

        /// <summary>
        /// Update scholarship type.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        ScholarshipTypeViewModel Update(ScholarshipTypeViewModel request, Guid userId);

        /// <summary>
        /// Delete scholarship type by given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);
    }
}