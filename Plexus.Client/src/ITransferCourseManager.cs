using Plexus.Client.ViewModel.Academic;
using Plexus.Client.ViewModel.Registration;

namespace Plexus.Client
{
    public interface ITransferCourseManager
    {
        /// <summary>
        /// Create transfer courses record.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<TransferViewModel> Create(Guid id, CreateTransferViewModel request, Guid userId);

        /// <summary>
        /// Get transfer courses by student id.
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        IEnumerable<TransferViewModel> GetByStudent(Guid studentId);
    }
}