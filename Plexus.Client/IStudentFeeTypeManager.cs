using Plexus.Client.ViewModel.DropDown;
using Plexus.Client.ViewModel.Payment;
using Plexus.Entity.DTO;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface IStudentFeeTypeManager
    {
        /// <summary>
        /// Create new student fee type.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        StudentFeeTypeViewModel Create(CreateStudentFeeTypeViewModel request, Guid userId);

        /// <summary>
        /// Search student fee type by given parameters as paged.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<StudentFeeTypeViewModel> Search(SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25);

        /// <summary>
        /// Search student fee type by given parameters as drop down.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<BaseDropDownViewModel> GetDropDownList(SearchCriteriaViewModel parameters);
        
        /// <summary>
        /// Get student fee type by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        StudentFeeTypeViewModel GetById(Guid id);

        /// <summary>
        /// Update student fee type info.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        StudentFeeTypeViewModel Update(StudentFeeTypeViewModel request, Guid userId);

        /// <summary>
        /// Delete student fee type by id.
        /// </summary>
        /// <param name="id"></param>
        void Delete(Guid id);
    }
}