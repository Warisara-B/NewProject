using Plexus.Entity.DTO;
using Plexus.Entity.DTO.SearchFilter;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider
{
    public interface IInstructorRoleProvider
    {
        /// <summary>
        /// Create instructor role.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        public InstructorRoleDTO Create(CreateInstructorRoleDTO request, string requester);

        /// <summary>
        /// Get all instructor roles.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IEnumerable<InstructorRoleDTO> Search(SearchInstructorRoleCriteriaDTO parameters);

        /// <summary>
        /// Search instructor role as paged.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public PagedViewModel<InstructorRoleDTO> Search(SearchInstructorRoleCriteriaDTO parameters, int page = 1, int pageSize = 5);

        /// <summary>
        /// Get instructor role by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public InstructorRoleDTO GetById(Guid id);

        /// <summary>
        /// Update instructor role.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        public InstructorRoleDTO Update(InstructorRoleDTO request, string requester);

        /// <summary>
        /// Delete instructor role.
        /// </summary>
        /// <param name="id"></param>
        public void Delete(Guid id);
    }
}