using Plexus.Entity.DTO.Academic;

namespace Plexus.Entity.Provider
{
    public interface IFacultyProvider
    {
        /// <summary>
        /// Create new faculty.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        FacultyDTO Create(CreateFacultyDTO request, string requester);

        /// <summary>
        /// Get all faculties.
        /// </summary>
        /// <returns></returns>
        IEnumerable<FacultyDTO> GetAll();

        /// <summary>
        /// Get faculty by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        FacultyDTO GetById(Guid id);

        /// <summary>
        /// Get faculty by ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        IEnumerable<FacultyDTO> GetById(IEnumerable<Guid> ids);

        /// <summary>
        /// Update faculty information.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        FacultyDTO Update(FacultyDTO request, string requester);

        /// <summary>
        /// Delete faculty.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);
    }
}