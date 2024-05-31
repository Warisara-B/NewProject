using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Academic;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider
{ 
    public interface IGradeProvider
    {
        /// <summary>
        /// Create new grade record
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        GradeDTO Create(CreateGradeDTO request, string requester);

        /// <summary>
        /// Get all grade records
        /// </summary>  
        /// <returns></returns>
        IEnumerable<GradeDTO> GetAll();

        /// <summary>
        /// Get grade by id
        /// </summary>
        /// <param name="gradeId"></param>
        /// <returns></returns>
        GradeDTO GetById(Guid gradeId);

        /// <summary>
        /// Get grade by ids
        /// </summary>
        /// <param name="gradeIds"></param>
        /// <returns></returns>
        IEnumerable<GradeDTO> GetById(IEnumerable<Guid> gradeIds);

        /// <summary>
        /// Get grade list by letters
        /// </summary>
        /// <param name="gradeLetter"></param>
        /// <returns></returns>
        IEnumerable<GradeDTO> GetByLetter(string gradeLetter);

        /// <summary>
        /// Get grade by given parameters as paged response
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<GradeDTO> Search(SearchCriteriaViewModel parameters, int page, int pageSize);

        /// <summary>
        /// Update grade information
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        GradeDTO Update(GradeDTO request, string requester);

        /// <summary>
        /// Delete grade
        /// </summary>
        /// <param name="gradeId"></param>
        void Delete(Guid gradeId);
    }
}