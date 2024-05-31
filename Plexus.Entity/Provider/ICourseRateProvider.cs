using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Payment;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider
{
    public interface ICourseRateProvider
    {
        /// <summary>
        /// Create new course rate
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        CourseRateDTO Create(CreateCourseRateDTO request, string requester);

        /// <summary>
        /// Get all available course rate
        /// </summary>
        /// <returns></returns>
        IEnumerable<CourseRateDTO> GetAll();

        /// <summary>
        /// Get specific course rate by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        CourseRateDTO GetById(Guid id);

        /// <summary>
        /// Get specific course rate by ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        IEnumerable<CourseRateDTO> GetByIds(IEnumerable<Guid> ids);

        /// <summary>
        /// Get course rate as paging by given parameters
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<CourseRateDTO> Search(SearchCriteriaViewModel parameters, int page, int pageSize);

        /// <summary>
        /// Update course rate information
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        CourseRateDTO Update(CourseRateDTO request, string requester);

        /// <summary>
        /// Delete course rate by id
        /// </summary>
        /// <param name="id"></param>
        void Delete(Guid id);
    }
}