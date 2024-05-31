using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Payment.Scholarship;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider
{
    public interface IScholarshipProvider
    {
        /// <summary>
        /// Create new scholarship.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        ScholarshipDTO Create(CreateScholarshipDTO request, string requester);

        /// <summary>
        /// Search scholarship by given parameters as paged.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<ScholarshipDTO> Search(SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25);

        /// <summary>
        /// Search scholarship by given parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<ScholarshipDTO> Search(SearchCriteriaViewModel parameters);

        /// <summary>
        /// Get scholarships by given ids.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        IEnumerable<ScholarshipDTO> GetById(IEnumerable<Guid> ids);

        /// <summary>
        /// Get scholarship by given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ScholarshipDTO GetById(Guid id);

        /// <summary>
        /// Update scholarship.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        ScholarshipDTO Update(ScholarshipDTO request, string requester);

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
        IEnumerable<ScholarshipReserveBudgetDTO> GetReserveBudgetByScholarshipId(Guid id);

        /// <summary>
        /// Update scholarship reserve budget.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="budgets"></param>
        /// <returns></returns>
        void UpdateReserveBudgets(Guid id, IEnumerable<ScholarshipReserveBudgetDTO> budgets);

        /// <summary>
        /// Get scholarship fee item by scholarship id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IEnumerable<ScholarshipFeeItemDTO> GetFeeItemByScholarshipId(Guid id);
        
        /// <summary>
        /// Update scholarship fee item.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="items"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        void UpdateFeeItems(Guid id, IEnumerable<ScholarshipFeeItemDTO> items, string requester);
    }
}