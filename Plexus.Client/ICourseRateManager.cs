using Plexus.Client.ViewModel.Payment;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Entity.DTO;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface ICourseRateManager
    {
        /// <summary>
        /// Create course rate
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        CourseRateViewModel Create(CreateCourseRateViewModel request);

        /// <summary>
        /// Get course rate by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        CourseRateViewModel GetById(Guid id);

        /// <summary>
        /// Get course rate by given parameters
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<CourseRateViewModel> Search(SearchCourseRateCriteriaViewModel? parameters);

        /// <summary>
        /// Get course rate as paged by given parameters
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<CourseRateViewModel> Search(SearchCourseRateCriteriaViewModel? parameters, int page, int pageSize);

        /// <summary>
        /// Update course rate
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        CourseRateViewModel Update(Guid id, CreateCourseRateViewModel request);

        /// <summary>
        /// Delete course rate by id
        /// </summary>
        /// <param name="id"></param>
        void Delete(Guid id);
    }
}