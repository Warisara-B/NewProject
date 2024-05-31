using Plexus.Database.Model;
using Plexus.Entity.DTO;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider
{
    public interface IEmployeeProvider
    {
        /// <summary>
        /// Create new instructor.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        EmployeeDTO Create(CreateEmployeeDTO request, string requester);

        /// <summary>
        /// Search instructor by given parameter as paged.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<EmployeeDTO> Search(SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25);

        /// <summary>
        /// Search instructor by given parameter.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<EmployeeDTO> Search(SearchCriteriaViewModel parameters);

        /// <summary>
        /// Search instructor by ids.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        IEnumerable<EmployeeDTO> GetById(IEnumerable<Guid> ids);

        /// <summary>
        /// Get instructor by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        EmployeeDTO GetById(Guid id);

        /// <summary>
        /// Update instructor information.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        EmployeeDTO Update(EmployeeDTO request, string requester);

        /// <summary>
        /// Delete instructor by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);

        /// <summary>
        /// Upload instructor card image.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cardImagePath"></param>
        /// <returns></returns>
        void UpdateCardImage(Guid id, string cardImagePath);
    }
}