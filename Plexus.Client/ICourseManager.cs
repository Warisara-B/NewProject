using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Client.ViewModel.Academic;
using Plexus.Client.ViewModel.DropDown;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface ICourseManager
    {
        /// <summary>
        /// Create new course into system
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        CourseViewModel Create(CreateCourseViewModel request, Guid userId);

        /// <summary>
        /// Get course information
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        CourseViewModel GetById(Guid courseId);

        /// <summary>
        /// Get course dropdown list by given parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<CourseDropDownViewModel> GetDropDownList(SearchCourseCriteriaViewModel parameters);

        /// <summary>
        /// Search course according to parameters, return as paged object
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<CourseViewModel> Search(SearchCourseCriteriaViewModel parameters, int page, int pageSize);

        /// <summary>
        /// Update course record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        CourseViewModel Update(Guid id, CreateCourseViewModel request, Guid userId);

        /// <summary>
        /// Delete course record
        /// </summary>
        /// <param name="courseId"></param>
        void Delete(Guid courseId);
    }
}