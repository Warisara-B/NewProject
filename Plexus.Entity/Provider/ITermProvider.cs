using Plexus.Entity.DTO.Academic;

namespace Plexus.Entity.Provider
{
    public interface ITermProvider
    {
        /// <summary>
        /// Add new term record
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        TermDTO Create(CreateTermDTO request, string requester);

        /// <summary>
        /// Get term by id
        /// </summary>
        /// <param name="termId"></param>
        /// <returns></returns>
        TermDTO GetById(Guid termId);

        /// <summary>
        /// Get term by ids
        /// </summary>
        /// <param name="termIds"></param>
        /// <returns></returns>
        IEnumerable<TermDTO> GetById(IEnumerable<Guid> termIds);

        /// <summary>
        /// Get term by academic level
        /// </summary>
        /// <param name="academicLevelId"></param>
        /// <returns></returns>
        IEnumerable<TermDTO> GetByAcademiceLevel(Guid academicLevelId);

        /// <summary>
        /// Update term information
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        TermDTO Update(TermDTO request, string requester);

        /// <summary>
        /// Delete term information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);

        /// <summary>
        /// Get student current registration term
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        TermDTO? GetRegistrationTerm(Guid studentId);
    }
}