using Plexus.Database.Enum.Academic;
using Plexus.Entity.DTO.Academic;
using Plexus.Entity.DTO.Registration;

namespace Plexus.Entity.Provider.src.Academic
{
    public interface IStudyCourseProvider
    {
        /// <summary>
        /// Add new study course record
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        IEnumerable<StudyCourseDTO> Create(IEnumerable<CreateStudyCourseDTO> request, string requester);

        /// <summary>
        /// Add new transfer course record.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        IEnumerable<StudyCourseDTO> CreateTransferCourses(IEnumerable<CreateStudyCourseDTO> request, string requester);

        /// <summary>
        /// Get study course data by specific Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        StudyCourseDTO GetById(Guid id);

        /// <summary>
        /// Get study course data by list of ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        IEnumerable<StudyCourseDTO> GetById(IEnumerable<Guid> ids);

        /// <summary>
        /// Get specific study course for specific student
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="termId">Specify term to get study course</param>
        /// <param name="statuses">Specify status to filter list of study course</param>
        /// <returns></returns>
        IEnumerable<StudyCourseDTO> GetByStudent(Guid studentId, Guid? termId = null, IEnumerable<StudyCourseStatus>? statuses = null);

        /// <summary>
        /// Get study courses for given section id
        /// </summary>
        /// <param name="sectionId"></param>
        /// <param name="statuses"></param>
        /// <returns></returns>
        IEnumerable<StudyCourseDTO> GetBySectionId(Guid sectionId, IEnumerable<StudyCourseStatus>? statuses);

        /// <summary>
        /// Get study course from given sectionIds and student Ids
        /// </summary>
        /// <param name="sectionIds"></param>
        /// <param name="studentIds"></param>
        /// <param name="statuses"></param>
        /// <returns></returns>
        IEnumerable<StudyCourseDTO> GetBySectionIdAndStudentId(
                IEnumerable<Guid> sectionIds, IEnumerable<Guid> studentIds,
                IEnumerable<StudyCourseStatus>? statuses = null);

        /// <summary>
        /// Update study course data
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        IEnumerable<StudyCourseDTO> Update(IEnumerable<UpdateStudyCourseDTO> request, string requester);

        /// <summary>
        /// Update study courses.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        IEnumerable<StudyCourseDTO> Update(RegistrationDTO request, string requester);
    }
}