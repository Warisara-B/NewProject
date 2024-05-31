using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Academic;
using Plexus.Entity.DTO.Academic.Curriculum;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider
{
    public interface ICurriculumProvider
    {
        /// <summary>
        /// Create curriculum record.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        CurriculumDTO Create(CreateCurriculumDTO request, string requester);

        /// <summary>
        /// Get all curriculums.
        /// </summary>
        /// <returns></returns>
        IEnumerable<CurriculumDTO> GetAll();

        /// <summary>
        /// Get curriculum by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        CurriculumDTO GetById(Guid id);

        /// <summary>
        /// Get curriculum by ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        IEnumerable<CurriculumDTO> GetById(IEnumerable<Guid> ids);

        /// <summary>
        /// Search curriculum as paged.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<CurriculumDTO> Search(SearchCriteriaViewModel parameters, int page, int pageSize);

        /// <summary>
        /// Search curriculums by given parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<CurriculumDTO> Search(SearchCriteriaViewModel parameters);

        /// <summary>
        /// Update curriculum information.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        CurriculumDTO Update(CurriculumDTO request, string requester);

        /// <summary>
        /// Delete curriculum by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);
    }
}