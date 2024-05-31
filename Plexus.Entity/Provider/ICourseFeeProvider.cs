using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Payment;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider
{
    public interface ICourseFeeProvider
    {
        /// <summary>
        /// Create new course fee.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        CourseFeeDTO Create(CreateCourseFeeDTO request, string requester);

        /// <summary>
        /// Get course fee by course id.
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        IEnumerable<CourseFeeDTO> GetByCourseId(Guid courseId);

        /// <summary>
        /// Get course fee by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        CourseFeeDTO GetById(Guid id);

        /// <summary>
        /// Update course fee information.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        CourseFeeDTO Update(CourseFeeDTO request, string requester);

        /// <summary>
        /// Delete course fee by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);
    }
}