using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Payment.Scholarship;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider
{
    public interface IStudentScholarshipProvider
    {
        /// <summary>
        /// Add single student scholarship
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        StudentScholarshipDTO Create(CreateStudentScholarshipDTO request, string requester);

        /// <summary>
        /// Add multiple student scholarship
        /// </summary>
        /// <param name="requests"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        IEnumerable<StudentScholarshipDTO> Create(IEnumerable<CreateStudentScholarshipDTO> requests, string requester);

        /// <summary>
        /// Search student scholarship by given parameters as paged.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<StudentScholarshipDTO> Search(SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25);

        /// <summary>
        /// Get student scholarship information by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        StudentScholarshipDTO GetById(Guid id);

        /// <summary>
        /// Get student scholarship information by multiple id
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        IEnumerable<StudentScholarshipDTO> GetById(IEnumerable<Guid> ids);

        /// <summary>
        /// Get list of student scholarship by scholarship id
        /// </summary>
        /// <param name="scholarshipId"></param>
        /// <returns></returns>
        IEnumerable<StudentScholarshipDTO> GetByScholarshipId(Guid scholarshipId);

        /// <summary>
        /// Get list of scholarship by student id
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        IEnumerable<StudentScholarshipDTO> GetByStudentId(Guid studentId);

        /// <summary>
        /// Search budgets by student scholarship id by given parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<StudentScholarshipReserveBudgetDTO> SearchBudgets(SearchCriteriaViewModel parameters);

        /// <summary>
        /// Get student scholarship budgets by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        StudentScholarshipReserveBudgetDTO GetBudgetById(Guid id);

        /// <summary>
        /// Search scholarship usages by student id and given parameters.
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<StudentScholarshipUsageDTO> SearchUsages(Guid studentId, SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25);

        /// <summary>
        /// Update student scholarship
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        StudentScholarshipDTO Update(StudentScholarshipDTO request, string requester);
        
        /// <summary>
        /// Update student scholarship active flag.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isActive"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        void Acitve(Guid id, bool isActive, string requester);

        /// <summary>
        /// Approve student scholarship by id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="approvedAt"></param>
        /// <param name="approvedBy"></param>
        /// <param name="remark"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        void Approve(Guid id, DateTime approvedAt, Guid approvedBy, string? remark, string requester);

        /// <summary>
        /// Delete student scholarship
        /// </summary>
        /// <param name="id"></param>
        void Delete(Guid id);

        /// <summary>
        /// Create new reserved budget for student scholarship
        /// </summary>
        /// <param name="studentScholarshipId"></param>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        void CreateReservedBudget(Guid studentScholarshipId, CreateStudentScholarshipReserveBudgetDTO request, string requester);

        /// <summary>
        /// Update reserved budget balance
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        void UpdateReservedBudgetBalance(UpdateScholarshipReservedBudgetDTO request, string requester);

        /// <summary>
        /// Insert adjustment balance to student scholarship
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        void AdjustScholarshipBudget(AdjustScholarshipBudgetDTO request, string requester);

        /// <summary>
        /// Use student scholarship budget.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        void UseScholarshipBudget(UseScholarshipBudgetDTO request, string requester);
    }
}