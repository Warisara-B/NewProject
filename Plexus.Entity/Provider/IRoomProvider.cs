using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Facility;
using Plexus.Entity.DTO.SearchFilter;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider
{
    public interface IRoomProvider
    {
        /// <summary>
        /// Create new room
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        RoomDTO Create(CreateRoomDTO request, string requester);

        /// <summary>
        /// Get all room
        /// </summary>
        /// <returns></returns>
        IEnumerable<RoomDTO> GetAll();

        /// <summary>
        /// Get room by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        RoomDTO GetById(Guid id);

        /// <summary>
        /// Get room by ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        IEnumerable<RoomDTO> GetById(IEnumerable<Guid> ids);

        /// <summary>
        /// Get rooms by building id
        /// </summary>
        /// <param name="buildingId"></param>
        /// <returns></returns>
        IEnumerable<RoomDTO> GetByBuildingId(Guid? buildingId);

        /// <summary>
        /// Search room as paging
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<RoomDTO> Search(SearchRoomCriteriaDTO criteria, int page, int pageSize);

        /// <summary>
        /// Search room by given criteria.
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IEnumerable<RoomDTO> Search(SearchRoomCriteriaDTO criteria);

        /// <summary>
        /// Update room
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        RoomDTO Update(RoomDTO request, string requester);

        /// <summary>
        /// Delete room
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);

        /// <summary>
        /// Get facilities by room id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IEnumerable<RoomFacilityDTO> GetFacilityByRoomId(Guid id);
    }
}