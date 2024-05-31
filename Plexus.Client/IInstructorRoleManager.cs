using Plexus.Client.ViewModel;
using Plexus.Client.ViewModel.DropDown;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface IInstructorRoleManager
    {
        /// <summary>
        /// Create instructor role.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public InstructorRoleViewModel Create(CreateInstructorRoleViewModel request, Guid userId);

        /// <summary>
        /// Search instructor roles.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IEnumerable<InstructorRoleViewModel> Search(SearchInstructorRoleCriteriaViewModel? parameters);

        /// <summary>
        /// Get instructor roles as dropdown.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BaseDropDownViewModel> GetDropDownList(SearchInstructorRoleCriteriaViewModel parameters);

        /// <summary>
        /// Search instructor role as paged.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public PagedViewModel<InstructorRoleViewModel> Search(SearchInstructorRoleCriteriaViewModel parameters, int page = 1, int pageSize = 5);

        /// <summary>
        /// Get instructor role by given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public InstructorRoleViewModel GetById(Guid id);

        /// <summary>
        /// Update instructor role by given id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public InstructorRoleViewModel Update(Guid id, CreateInstructorRoleViewModel request, Guid userId);

        /// <summary>
        /// Delete instructor role by given id.
        /// </summary>
        /// <param name="id"></param>
        public void Delete(Guid id);
    }
}