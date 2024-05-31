using Plexus.Entity.DTO.Academic;
using Plexus.Entity.DTO.SearchFilter;

namespace Plexus.Entity.Provider
{
    public interface IAcademicLevelProvider
    {
        /// <summary>
        /// Create new academic level
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        AcademicLevelDTO Create(CreateAcademicLevelDTO request, string requester);

        /// <summary>
        /// Get all academic levels.
        /// </summary>
        /// <returns></returns>
        IEnumerable<AcademicLevelDTO> GetAll();

        /// <summary>
        /// Get academic level by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        AcademicLevelDTO GetById(Guid id);

        /// <summary>
        /// Get academic levels by given parameters
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<AcademicLevelDTO> Search(SearchAcademicLevelCriteriaDTO? parameters = null);

        /// <summary>
        /// Get academic level by ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        IEnumerable<AcademicLevelDTO> GetById(IEnumerable<Guid> ids);

        /// <summary>
        /// Delete academic level
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);
    }
}