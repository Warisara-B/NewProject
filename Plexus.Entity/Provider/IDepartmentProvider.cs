using Plexus.Entity.DTO.Academic;

namespace Plexus.Entity.Provider
{
    public interface IDepartmentProvider
    {
        /// <summary>
        /// Create new department.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        DepartmentDTO Create(CreateDepartmentDTO request, string requester);

        /// <summary>
        /// Get all department.
        /// </summary>
        /// <returns></returns>
        IEnumerable<DepartmentDTO> GetAll();

        /// <summary>
        /// Get department by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DepartmentDTO GetById(Guid id);

        /// <summary>
        /// Get department by ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        IEnumerable<DepartmentDTO> GetById(IEnumerable<Guid> ids);

        /// <summary>
        /// Get department by faculty id.
        /// </summary>
        /// <param name="facultyId"></param>
        /// <returns></returns>
        IEnumerable<DepartmentDTO> GetByFacultyId(Guid facultyId);

        /// <summary>
        /// Update department information.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        DepartmentDTO Update(DepartmentDTO request, string requester);

        /// <summary>
        /// Delete department.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);
    }
}