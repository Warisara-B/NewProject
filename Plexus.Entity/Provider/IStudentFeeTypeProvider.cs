using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Payment;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider
{
    public interface IStudentFeeTypeProvider
    {
        /// <summary>
        /// Create new student fee type.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        StudentFeeTypeDTO Create(CreateStudentFeeTypeDTO request, string requester);

        /// <summary>
        /// Search student fee type by given parameters as paged.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<StudentFeeTypeDTO> Search(SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25);

        /// <summary>
        /// Search student fee type by given parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<StudentFeeTypeDTO> Search(SearchCriteriaViewModel? parameters = null);

        /// <summary>
        /// Get student fee type by ids.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        IEnumerable<StudentFeeTypeDTO> GetById(IEnumerable<Guid> ids);
        
        /// <summary>
        /// Get student fee type by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        StudentFeeTypeDTO GetById(Guid id);

        /// <summary>
        /// Update student fee type info.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        StudentFeeTypeDTO Update(StudentFeeTypeDTO request, string requester);

        /// <summary>
        /// Delete student fee type by id.
        /// </summary>
        /// <param name="id"></param>
        void Delete(Guid id);
    }
}