using Plexus.Database.Enum.Facility.Reservation;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Facility.Reservation;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider
{
    public interface IRoomReservationProvider
    {
        /// <summary>
        /// Create room reserve request.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        RoomReserveRequestDTO Create(CreateRoomReserveRequestDTO request, string requester);

        /// <summary>
        /// Search room reserve requests by given parameters as paged.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<RoomReserveRequestDTO> Search(SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25);

        /// <summary>
        /// Get room reserve requests by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        RoomReserveRequestDTO GetById(Guid id);

        /// <summary>
        /// Get room reserve slot by request id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IEnumerable<RoomReserveSlotDTO> GetReserveSlotByRequestId(Guid id);

        /// <summary>
        /// Get room reserve slot by ids.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        IEnumerable<RoomReserveSlotDTO> GetReserveSlotById(IEnumerable<Guid> ids);

        /// <summary>
        /// Update room reserve requests status.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="remark"></param>
        /// <param name="requester"></param>
        void UpdateStatus(Guid id, ReservationStatus status, string? remark, string requester);

        /// <summary>
        /// Cancel room reserve slots.
        /// </summary>
        /// <param name="requests"></param>
        /// <param name="requester"></param>
        void CancelReserveSlots(IEnumerable<UpdateRoomReserveSlotDTO> requests, string requester);
    }   
}