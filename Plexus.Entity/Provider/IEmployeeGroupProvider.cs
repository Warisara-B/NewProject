using Plexus.Entity.DTO;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider
{
    public interface IEmployeeGroupProvider
    {
        /// <summary>
        /// Create new instructor rank.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        EmployeeGroupDTO Create(CreateEmployeeGroupDTO request, string requester);

        /// <summary>
        /// Search instructor rank by given parameters as paged.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<EmployeeGroupDTO> Search(SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25);

        /// <summary>
        /// Search instructor rank by given parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<EmployeeGroupDTO> Search(SearchCriteriaViewModel parameters);

        /// <summary>
        /// Get instructor rank by given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        EmployeeGroupDTO GetById(Guid id);

        /// <summary>
        /// Get instructor ranks by given ids.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        IEnumerable<EmployeeGroupDTO> GetById(IEnumerable<Guid> ids);

        /// <summary>
        /// Update instructor rank.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        EmployeeGroupDTO Update(EmployeeGroupDTO request, string requester);

        /// <summary>
        /// Delete instructor rank by given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);
    }
}