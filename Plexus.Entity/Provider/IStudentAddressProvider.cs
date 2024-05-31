using Plexus.Entity.DTO;

namespace Plexus.Entity.Provider
{
    public interface IStudentAddressProvider
    {
        /// <summary>
        /// Add new student contact address.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        StudentAddressDTO Create(CreateStudentAddressDTO request, string requester);

        /// <summary>
        /// Get student contact address by student id.
        /// </summary>
        /// <param name="stduentId"></param>
        /// <returns></returns>
        IEnumerable<StudentAddressDTO> GetByStudentId(Guid stduentId);

        /// <summary>
        /// Get student contact address by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        StudentAddressDTO GetById(Guid id);

        /// <summary>
        /// Update student contact address.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        StudentAddressDTO Update(StudentAddressDTO request, string requester);

        // /// <summary>
        // /// Clear main address flag by student id.
        // /// </summary>
        // /// <param name="studentId"></param>
        // /// <param name="requester"></param>
        // /// <returns></returns>
        // void ClearMainAddress(Guid studentId, string requester);

        /// <summary>
        /// Delete student contact address by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);
    }
}