using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Academic;
using Plexus.Entity.DTO.SearchFilter;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider
{
    public interface IAcademicProgramProvider
    {
        /// <summary>
        /// Create new academic program.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        AcademicProgramDTO Create(CreateAcademicProgramDTO request, string requester);

        /// <summary>
        /// Get rate types by given ids.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        IEnumerable<AcademicProgramDTO> GetById(IEnumerable<Guid> ids);

        /// <summary>
        /// Get rate types by given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        AcademicProgramDTO GetById(Guid id);

        /// <summary>
        /// Update academic program.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        AcademicProgramDTO Update(AcademicProgramDTO request, string requester);

        /// <summary>
        /// Delete academic program by given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);
    }
}