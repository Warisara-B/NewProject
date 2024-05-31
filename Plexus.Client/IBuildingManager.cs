using Plexus.Client.ViewModel.DropDown;
using Plexus.Client.ViewModel.Facility;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Entity.DTO;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface IBuildingManager
    {
        /// <summary>
        /// Create new building
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        BuildingViewModel Create(CreateBuildingViewModel request, Guid userId);

        /// <summary>
        /// Get building by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BuildingViewModel GetById(Guid id);

        /// <summary>
        /// Search building according to given parameters
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<BuildingViewModel> Search(SearchBuildingCriteriaViewModel parameters, int page, int pageSize);

        /// <summary>
        /// Update building information
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        BuildingViewModel Update(BuildingViewModel request, Guid userId);

        /// <summary>
        /// Delete building by id
        /// </summary>
        /// <param name="id"></param>
        void Delete(Guid id);

        /// <summary>
        /// Get building available times
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IEnumerable<BuildingAvailableTimeViewModel> GetAvailableTimes(Guid id);

        /// <summary>
        /// Update available times
        /// </summary>
        /// <param name="buildingId"></param>
        /// <param name="requests"></param>
        /// <returns></returns>
        IEnumerable<BuildingAvailableTimeViewModel> UpdateAvailableTimes(Guid buildingId, IEnumerable<BuildingAvailableTimeViewModel> requests);

        /// <summary>
        /// Get building manager drop down list
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<BuildingDropDownViewModel> GetDropDownList(SearchBuildingCriteriaViewModel parameters);
    }
}