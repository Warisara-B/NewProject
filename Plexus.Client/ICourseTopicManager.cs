using Plexus.Client.ViewModel.Academic;
using Plexus.Entity.DTO;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface ICourseTopicManager
    {
        /// <summary>
        /// Create new course topic.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        CourseTopicViewModel Create(Guid courseId, CreateCourseTopicViewModel request, Guid userId);

        /// <summary>
        /// Get course topic by course id.
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        IEnumerable<CourseTopicViewModel> GetByCourseId(Guid courseId);

        /// <summary>
        /// Get course topic by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        CourseTopicViewModel GetById(Guid courseTopicId);

        /// <summary>
        /// Get course topic as paged.
        /// </summary>
        /// <returns></returns>
        PagedViewModel<CourseTopicViewModel> Search(Guid courseId, int page = 1, int pageSize = 25);

        /// <summary>
        /// Update course topic information.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        CourseTopicViewModel Update(Guid courseId, Guid courseTopicId, CreateCourseTopicViewModel request, Guid userId);

        /// <summary>
        /// Delete course topic by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid courseTopicId);
    }
}