using Plexus.Entity.DTO.Academic;

namespace Plexus.Entity.Provider
{
    public interface IStudentTermProvider
    {
        /// <summary>
        /// Create student term.
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        StudentTermDTO Create(Guid studentId, UpdateStudentTermDTO request, string requester);

        /// <summary>
        /// Get student terms by student id.
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        IEnumerable<StudentTermDTO> GetByStudentId(Guid studentId);

        /// <summary>
        /// Get student term by student id and term id.
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="termId"></param>
        /// <returns></returns>
        StudentTermDTO GetByStudentIdAndTermId(Guid studentId, Guid termId);

        /// <summary>
        /// Update student term.
        /// Flags, min and max credit only.
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        StudentTermDTO Update(Guid studentId, StudentTermDTO request, string requester);

        /// <summary>
        /// Update student term.
        /// Total credit, total registration credit and gpax.
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        StudentTermDTO UpdateGPAXAndCredit(Guid studentId, StudentTermDTO request, string requester);
    }
}