using Plexus.Client.ViewModel.Payment;
using Plexus.Entity.DTO;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface ICourseFeeManager
    {
        /// <summary>
        /// Create new course fee.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        CourseFeeViewModel Create(Guid id, CreateCourseFeeViewModel request, Guid userId);

        /// <summary>
        /// Get course fee by course id.
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        IEnumerable<CourseFeeViewModel> GetByCourseId(Guid courseId);

        /// <summary>
        /// Get course fee by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        CourseFeeViewModel GetById(Guid id);

        /// <summary>
        /// Update course fee information.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        CourseFeeViewModel Update(Guid id, Guid courseFeeId, CreateCourseFeeViewModel request, Guid userId);

        /// <summary>
        /// Delete course fee by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);
    }
}