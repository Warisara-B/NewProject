using Plexus.Entity.DTO;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider
{
    public interface IStudentProvider
    {
        /// <summary>
        /// Create new student.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        StudentDTO Create(CreateStudentDTO request, string requester);

        /// <summary>
        /// Search student as paging.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<StudentDTO> Search(SearchCriteriaViewModel parameters, int page, int pageSize);

        /// <summary>
        /// Search student return as list.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<StudentDTO> Search(SearchCriteriaViewModel parameters);

        /// <summary>
        /// Get student by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        StudentDTO GetById(Guid id);

        /// <summary>
        /// Get student by code.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        StudentDTO GetByCode(string code);

        /// <summary>
        /// Get student by ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        IEnumerable<StudentDTO> GetById(IEnumerable<Guid> ids);

        /// <summary>
        /// Get student by codes.
        /// </summary>
        /// <param name="codes"></param>
        /// <returns></returns>
        IEnumerable<StudentDTO> GetByCode(IEnumerable<string> codes);

        /// <summary>
        /// Update student info.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        StudentDTO Update(StudentDTO request, string requester);

        /// <summary>
        /// Delete student by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);

        /// <summary>
        /// Upload student card image.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cardImagePath"></param>
        /// <returns></returns>
        void UploadCardImage(Guid id, string cardImagePath);

        /// <summary>
        /// Get next student code in batch.
        /// </summary>
        /// <param name="batchCode"></param>
        /// <param name="cardImagePath"></param>
        /// <returns></returns>
        int GetNextCode(int batchCode);

        /// <summary>
        /// Get student mini card by id.
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        StudentProfileCardDTO GetStudentCardById(Guid studentId);

        /// <summary>
        /// Update student's general information by student id.
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        StudentGeneralInfoDTO UpdateGeneralInfo(Guid studentId, UpdateStudentGeneralInfoDTO request);

        /// <summary>
        /// Get student full profile by id.
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        StudentFullProfileDTO GetStudentFullProfileById(Guid studentId);
    }
}