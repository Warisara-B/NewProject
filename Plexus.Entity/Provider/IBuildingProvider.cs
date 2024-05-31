using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Facility;
using Plexus.Entity.DTO.SearchFilter;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider
{
    public interface IBuildingProvider
    {
        /// <summary>
        /// Create building
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        BuildingDTO Create(CreateBuildingDTO request, string requester);

        /// <summary>
        /// Get all building
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        IEnumerable<BuildingDTO> GetAll();

        /// <summary>
        /// Get building by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BuildingDTO GetById(Guid id);

        /// <summary>
        /// Search building return as paged
        /// </summary>
        /// <param name="parameters">Search by campus id</param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<BuildingDTO> Search(SearchBuildingCriteriaDTO parameters, int page, int pageSize);

        /// <summary>
        /// Search building return as list
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<BuildingDTO> Search(SearchBuildingCriteriaDTO parameters);

        /// <summary>
        /// Update building information
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        BuildingDTO Update(BuildingDTO request, string requester);

        /// <summary>
        /// Delete building
        /// </summary>
        /// <param name="id"></param>
        void Delete(Guid id);

        /// <summary>
        /// Get building available times
        /// </summary>
        /// <param name="buildingId"></param>
        /// <returns></returns>
        IEnumerable<BuildingAvailableTimeDTO> GetAvailableTime(Guid buildingId);

        /// <summary>
        /// Update building available times
        /// </summary>
        /// <param name="buildingId"></param>
        /// <param name="requests"></param>
        /// <returns></returns>
        IEnumerable<BuildingAvailableTimeDTO> UpdateAvailableTime(Guid buildingId, IEnumerable<BuildingAvailableTimeDTO> requests);
    }
}