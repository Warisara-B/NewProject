using Microsoft.AspNetCore.Http;
using Plexus.Client.ViewModel;
using Plexus.Client.ViewModel.Academic;
using Plexus.Entity.DTO;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface IStudentTermManager
    {
        /// <summary>
        /// Create student term.
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        StudentTermViewModel Create(Guid studentId, UpdateStudentTermViewModel request, Guid userId);

        /// <summary>
        /// Create student terms by student id.
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        IEnumerable<StudentTermViewModel> GetByStudentId(Guid studentId);

        /// <summary>
        /// Create student terms by student id and term id.
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="termId"></param>
        /// <returns></returns>
        StudentTermViewModel GetByStudentIdAndTermId(Guid studentId, Guid termId);

        /// <summary>
        /// Update student term.
        /// Flags, min and max credit only.
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        StudentTermViewModel Update(Guid studentId, UpdateStudentTermViewModel request, Guid userId);
    }
}