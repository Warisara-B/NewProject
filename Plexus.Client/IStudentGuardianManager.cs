using Plexus.Client.ViewModel;

namespace Plexus.Client
{
    public interface IStudentGuardianManager
    {
        /// <summary>
        /// Create student guardian by student id.
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        StudentGuardianViewModel Create(Guid studentId, CreateStudentGuardianViewModel request, Guid userId);

        /// <summary>
        /// Get guardian by student id.
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        IEnumerable<StudentGuardianViewModel> GetByStudentId(Guid studentId);

        /// <summary>
        /// Get guardian by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        StudentGuardianViewModel GetById(Guid id);

        /// <summary>
        /// Update guardian informationby id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        StudentGuardianViewModel Update(Guid id, CreateStudentGuardianViewModel request, Guid userId);

        /// <summary>
        /// Delete guardian information.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);
    }
}