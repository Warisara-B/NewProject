using Plexus.Client.ViewModel.DropDown;
using Plexus.Client.ViewModel.Facility;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Entity.DTO;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface IFacilityManager
    {
        /// <summary>
        /// Create new facility.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        FacilityViewModel Create(CreateFacilityViewModel request, Guid userId);

        /// <summary>
        /// Search facility by given parameters as paged.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<FacilityViewModel> Search(SearchFacilityCriteriaViewModel parameters, int page = 1, int pageSize = 25);

        /// <summary>
        /// Search facility by given parameters as drop down.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<BaseDropDownViewModel> GetDropDownList(SearchFacilityCriteriaViewModel parameters);

        /// <summary>
        /// Get facility by given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        FacilityViewModel GetById(Guid id);

        /// <summary>
        /// Update facility.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        FacilityViewModel Update(Guid id, CreateFacilityViewModel request, Guid userId);

        /// <summary>
        /// Delete rate type by given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);
    }
}