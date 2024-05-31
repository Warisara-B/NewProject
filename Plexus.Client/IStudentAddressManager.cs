using Plexus.Client.ViewModel;

namespace Plexus.Client
{
    public interface IStudentAddressManager
    {
        /// <summary>
        /// Create student address by student id.
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        StudentAddressViewModel Create(Guid studentId, CreateStudentAddressViewModel request, Guid userId);

        /// <summary>
        /// Get student address by student id.
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        IEnumerable<StudentAddressViewModel> GetByStudentId(Guid studentId);

        /// <summary>
        /// Get student address by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        StudentAddressViewModel GetById(Guid id);

        /// <summary>
        /// Update student address by id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        StudentAddressViewModel Update(Guid id, CreateStudentAddressViewModel request, Guid userId);

        /// <summary>
        /// Delete student address by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);
    }
}