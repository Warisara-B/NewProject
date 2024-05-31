using Plexus.Client.ViewModel.DropDown;
using Plexus.Client.ViewModel.Payment.Scholarship;
using Plexus.Entity.DTO;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface IScholarshipManager
    {
        /// <summary>
        /// Create new scholarship.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        ScholarshipViewModel Create(CreateScholarshipViewModel request, Guid userId);

        /// <summary>
        /// Search scholarship by given parameters as paged.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<ScholarshipViewModel> Search(SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25);

        /// <summary>
        /// Search scholarship by given parameters as drop down list.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<ScholarshipDropDownViewModel> GetDropDownList(SearchCriteriaViewModel parameters);

        /// <summary>
        /// Get scholarship by given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ScholarshipViewModel GetById(Guid id);

        /// <summary>
        /// Update scholarship.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        ScholarshipViewModel Update(ScholarshipViewModel request, Guid userId);
        
        /// <summary>
        /// Delete scholarship by given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);

        /// <summary>
        /// Get scholarship reserve budget by scholarship id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IEnumerable<ScholarshipReserveBudgetViewModel> GetReserveBudgetByScholarshipId(Guid id);

        /// <summary>
        /// Update scholarship reserve budget.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="budgets"></param>
        /// <returns></returns>
        IEnumerable<ScholarshipReserveBudgetViewModel> UpdateReserveBudgets(Guid id, IEnumerable<ScholarshipReserveBudgetViewModel> budgets);

        /// <summary>
        /// Get scholarship fee item by scholarship id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IEnumerable<ScholarshipFeeItemViewModel> GetFeeItemByScholarshipId(Guid id);
        
        /// <summary>
        /// Update scholarship fee item.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="items"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<ScholarshipFeeItemViewModel> UpdateFeeItems(Guid id, IEnumerable<UpdateScholarshipFeeItemViewModel> items, Guid userId);
    }
}