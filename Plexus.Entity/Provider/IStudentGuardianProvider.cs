using Plexus.Entity.DTO;

namespace Plexus.Entity.Provider
{
    public interface IStudentGuardianProvider
    {
        /// <summary>
        /// Create new student guardian.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        StudentGuardianDTO Create(CreateStudentGuardianDTO request, string requester);

        /// <summary>
        /// Get guardian by student id.
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        IEnumerable<StudentGuardianDTO> GetByStudentId(Guid studentId);

        /// <summary>
        /// Get guardian by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        StudentGuardianDTO GetById(Guid id);

        /// <summary>
        /// Update stduent guardian info.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        StudentGuardianDTO Update(StudentGuardianDTO request, string requester);

        /// <summary>
        /// Delete guardian by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);
    }
}