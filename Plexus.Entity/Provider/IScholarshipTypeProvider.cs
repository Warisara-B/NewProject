using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Payment.Scholarship;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider
{
    public interface IScholarshipTypeProvider
    {
        /// <summary>
        /// Create new scholarship type.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        ScholarshipTypeDTO Create(CreateScholarshipTypeDTO request, string requester);

        /// <summary>
        /// Search scholarship type by given parameters as paged.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<ScholarshipTypeDTO> Search(SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25);

        /// <summary>
        /// Search scholarship type by given parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<ScholarshipTypeDTO> Search(SearchCriteriaViewModel parameters);

        /// <summary>
        /// Get scholarship types by given ids.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        IEnumerable<ScholarshipTypeDTO> GetById(IEnumerable<Guid> ids);

        /// <summary>
        /// Get scholarship type by given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ScholarshipTypeDTO GetById(Guid id);

        /// <summary>
        /// Update scholarship type.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        ScholarshipTypeDTO Update(ScholarshipTypeDTO request, string requester);

        /// <summary>
        /// Delete scholarship type by given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);
    }
}