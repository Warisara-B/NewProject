using Plexus.Client.ViewModel;
using Plexus.Client.ViewModel.DropDown;
using Plexus.Entity.DTO;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface IInstructorTypeManager
    {
        /// <summary>
        /// Create new instructor type.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        InstructorTypeViewModel Create(CreateInstructorTypeViewModel request, Guid userId);

        /// <summary>
        /// Search instructor type by given parameters as paged.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<InstructorTypeViewModel> Search(SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25);
        
        /// <summary>
        /// Search instructor type by given parameters as drop down list.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<BaseDropDownViewModel> GetDropDownList(SearchCriteriaViewModel parameters);

        /// <summary>
        /// Get instructor type by given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        InstructorTypeViewModel GetById(Guid id);

        /// <summary>
        /// Update instructor type.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        InstructorTypeViewModel Update(InstructorTypeViewModel request, Guid userId);

        /// <summary>
        /// Delete instructor type by given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);
    }
}