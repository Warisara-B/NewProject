using Plexus.Client.ViewModel.Facility.Reservation;
using Plexus.Database.Enum.Facility.Reservation;
using Plexus.Entity.DTO;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface IRoomReservationManager
    {
        /// <summary>
        /// Create room reserve request.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        RoomReserveRequestViewModel Create(CreateRoomReserveRequestViewModel request, Guid userId);

        /// <summary>
        /// Search room reserve requests by given parameters as paged.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<RoomReserveRequestViewModel> Search(SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25);

        /// <summary>
        /// Get room reserve requests by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        RoomReserveRequestViewModel GetById(Guid id);

        /// <summary>
        /// Get room reserve slot by request id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IEnumerable<RoomReserveSlotViewModel> GetReserveSlotByRequestId(Guid id);

        /// <summary>
        /// Update room reserve requests status.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="remark"></param>
        /// <param name="userId"></param>
        void UpdateStatus(Guid id, ReservationStatus status, string? remark, Guid userId);

        /// <summary>
        /// Cancel room reserve slot by id.
        /// </summary>
        /// <param name="requests"></param>
        /// <param name="userId"></param>
        void CancelReserveSlots(IEnumerable<UpdateRoomReserveSlotViewModel> requests, Guid userId);
    }
}