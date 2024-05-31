using Plexus.Client.ViewModel.Academic;
using Plexus.Client.ViewModel.DropDown;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface ITeachingTypeManager
    {
        /// <summary>
        /// Create new teaching type.
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        TeachingTypeViewModel Create(CreateTeachingTypeViewModel request, Guid userId);

        /// <summary>
        /// Search teaching type as paged by given parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<TeachingTypeViewModel> Search(SearchTeachingTypeCriteriaViewModel parameters, int page = 1, int pageSize = 25);

        /// <summary>
        /// Search teaching type by given parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<TeachingTypeViewModel> Search(SearchTeachingTypeCriteriaViewModel? parameters);

        /// <summary>
        /// Get teaching type drop down.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<BaseDropDownViewModel> GetDropDownList(SearchTeachingTypeCriteriaViewModel parameters);

        /// <summary>
        /// Get teaching type by given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TeachingTypeViewModel GetById(Guid id);

        /// <summary>
        /// Update teaching type.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        TeachingTypeViewModel Update(Guid id, CreateTeachingTypeViewModel request, Guid userId);

        /// <summary>
        /// Delete teaching type by given id.
        /// </summary>
        /// <param name="id"></param>
        void Delete(Guid id);
    }
}