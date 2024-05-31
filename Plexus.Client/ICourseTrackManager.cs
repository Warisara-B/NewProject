using Plexus.Client.ViewModel.Academic;
using Plexus.Client.ViewModel.DropDown;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Academic;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface ICourseTrackManager
    {
        /// <summary>
        /// Create new course track.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        CourseTrackViewModel Create(CreateCourseTrackViewModel request, Guid userId);

        /// <summary>
        /// Search course track by given parameters as paged.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<CourseTrackViewModel> Search(SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25);
        
        /// <summary>
        /// Search course track by given parameters as drop down list.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<BaseDropDownViewModel> GetDropDownList(SearchCriteriaViewModel parameters);

        /// <summary>
        /// Get course track by given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        CourseTrackViewModel GetById(Guid id);

        /// <summary>
        /// Update course track.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        CourseTrackViewModel Update(CourseTrackViewModel request, Guid userId);

        /// <summary>
        /// Delete course track by given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);

        /// <summary>
        /// Get course track details by course track id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IEnumerable<CourseTrackDetailViewModel> GetDetailByCourseTrackId(Guid id);

        /// <summary>
        /// Update course track details.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="details"></param>
        /// <returns></returns>
        IEnumerable<CourseTrackDetailViewModel> UpdateDetails(Guid id, IEnumerable<UpdateCourseTrackDetailViewModel> details);
    }   
}