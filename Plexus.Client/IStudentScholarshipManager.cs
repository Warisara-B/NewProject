using Plexus.Client.ViewModel;
using Plexus.Client.ViewModel.DropDown;
using Plexus.Client.ViewModel.Payment.Scholarship;
using Plexus.Entity.DTO;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface IStudentScholarshipManager
    {
        /// <summary>
        /// Create student scholarship
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        StudentScholarshipViewModel Create(Guid studentId, CreateStudentScholarshipViewModel request, Guid userId);

        /// <summary>
        /// Create multiple student scholarship.
        /// </summary>
        /// <param name="scholarshipId"></param>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<StudentScholarshipViewModel> Create(Guid scholarshipId, CreateMultipleScholarshipViewModel request, Guid userId);

        /// <summary>
        /// Get information by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        StudentScholarshipViewModel GetById(Guid id);

        /// <summary>
        /// Get list of scholarship by student id
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        IEnumerable<StudentScholarshipViewModel> GetByStudentId(Guid studentId);

        /// <summary>
        /// Search student scholarship by given parameters as paged.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<StudentScholarshipViewModel> Search(SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25);

        /// <summary>
        /// Get student budget by given parameters as drop down.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<StudentScholarshipBudgetDropDownViewModel> GetBudgetDropDownList(SearchCriteriaViewModel parameters);

        /// <summary>
        /// Get student scholarship budgets by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        StudentReservedBudgetViewModel GetBudgetById(Guid id);

        /// <summary>
        /// Search scholarship usages by student id and given parameters.
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<StudentScholarshipUsageViewModel> SearchUsages(Guid studentId, SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25);

        /// <summary>
        /// Update student scholarship information
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        StudentScholarshipViewModel Update(UpdateStudentScholarShipViewModel request, Guid userId);

        /// <summary>
        /// Update student scholarship active flag.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isActive"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        void Active(Guid id, bool isActive, Guid userId);

        /// <summary>
        /// Approve student scholarship by id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        void Approve(Guid id, ApproveStudentScholarshipViewModel request, Guid userId);

        /// <summary>
        /// Delete student scholarship
        /// </summary>
        /// <param name="id"></param>
        void Delete(Guid id);

        /// <summary>
        /// Add new reserve budget to student scholarship
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        void CreateNewReserveBudget(Guid id, CreateStudentReservedBudgetViewModel request, Guid userId);

        /// <summary>
        /// Update reserve budget balance
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        void UpdateReserveBudgetBalance(Guid id, UpdateStudentReservedBudgetViewModel request, Guid userId);

        /// <summary>
        /// Add balance adjustment record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        void AddAdjustmentBalance(Guid id, CreateStudentReservedBudgetViewModel request, Guid userId);

        /// <summary>
        /// Use reserved budget.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        void UseReserveBudget(Guid id, UpdateStudentReservedBudgetViewModel request, Guid userId);
    }
}