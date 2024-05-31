using Plexus.Client.ViewModel.Facility.Reservation;
using Plexus.Database.Enum.Facility.Reservation;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Facility;
using Plexus.Entity.DTO.Facility.Reservation;
using Plexus.Entity.Exception;
using Plexus.Entity.Provider;
using Plexus.Utility.ViewModel;

namespace Plexus.Client.src.Facility.Reservation
{
    public class RoomReservationManager : IRoomReservationManager
    {
        private readonly IRoomReservationProvider _roomReservationProvider;
        private readonly IRoomProvider _roomProvider;

        public RoomReservationManager(IRoomReservationProvider roomReservationProvider,
                                      IRoomProvider roomProvider)
        {
            _roomReservationProvider = roomReservationProvider;
            _roomProvider = roomProvider;
        }

        public RoomReserveRequestViewModel Create(CreateRoomReserveRequestViewModel request, Guid userId)
        {
            if (request.ToDate.HasValue
               && request.FromDate > request.ToDate.Value)
            {
                throw new RoomReservationException.GivenToDateInvalid();
            }

            if ((!request.ToDate.HasValue
                 || request.FromDate == request.ToDate.Value)
                && request.StartedAt > request.EndedAt)
            {
                throw new RoomReservationException.GivenTimeInvalid();
            }

            var room = _roomProvider.GetById(request.RoomId);

            var dto = new CreateRoomReserveRequestDTO
            {
                Title = request.Title,
                SenderType = request.SenderType,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                FromDate = request.FromDate,
                ToDate = request.ToDate,
                StartedAt = request.StartedAt,
                EndedAt = request.EndedAt,
                RepeatedOn = request.RepeatedOn,
                RoomId = request.RoomId,
                UsageType = request.UsageType,
                Description = request.Description,
                Remark = request.Remark,
                RequesterName = request.RequesterName
            };

            var reserveRequest = _roomReservationProvider.Create(dto, userId.ToString());

            var response = MapDTOToViewModel(reserveRequest, room);

            return response;
        }

        public PagedViewModel<RoomReserveRequestViewModel> Search(SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var pagedReserveRequest = _roomReservationProvider.Search(parameters, page, pageSize);

            var roomIds = pagedReserveRequest.Items.Select(x => x.RoomId)
                                                   .Distinct()
                                                   .ToList();

            var rooms = _roomProvider.GetById(roomIds)
                                     .ToList();

            var response = new PagedViewModel<RoomReserveRequestViewModel>
            {
                Page = pagedReserveRequest.Page,
                TotalPage = pagedReserveRequest.TotalPage,
                TotalItem = pagedReserveRequest.TotalItem,
                Items = (from reserveRequest in pagedReserveRequest.Items
                         let room = rooms.SingleOrDefault(x => x.Id == reserveRequest.RoomId)
                         select MapDTOToViewModel(reserveRequest, room))
                        .ToList()
            };

            return response;
        }

        public RoomReserveRequestViewModel GetById(Guid id)
        {
            var reserveRequest = _roomReservationProvider.GetById(id);

            var room = _roomProvider.GetById(reserveRequest.RoomId);

            var response = MapDTOToViewModel(reserveRequest, room);

            return response;
        }

        public IEnumerable<RoomReserveSlotViewModel> GetReserveSlotByRequestId(Guid id)
        {
            var reserveSlots = _roomReservationProvider.GetReserveSlotByRequestId(id)
                                                       .ToList();
            
            var roomIds = reserveSlots.Select(x => x.RoomId)
                                      .Distinct()
                                      .ToList();

            var rooms = _roomProvider.GetById(roomIds)
                                     .ToList();

            var response = (from slot in reserveSlots
                            let room = rooms.SingleOrDefault(x => x.Id == slot.RoomId)
                            select MapSlotDTOToViewModel(slot, room))
                           .ToList();
            
            return response;
        }

        public void UpdateStatus(Guid id, ReservationStatus status, string? remark, Guid userId)
        {
            _roomReservationProvider.UpdateStatus(id, status, remark, userId.ToString());
        }
        
        public void CancelReserveSlots(IEnumerable<UpdateRoomReserveSlotViewModel> requests, Guid userId)
        {
            var ids = requests.Select(x => x.Id)
                              .ToList();

            var reserveSlots = _roomReservationProvider.GetReserveSlotById(ids)
                                                       .ToList();

            foreach (var slot in requests)
            {
                var reserveSlot = reserveSlots.SingleOrDefault(x => x.Id == slot.Id);

                if (reserveSlot is null)
                {
                    throw new RoomReservationException.SlotNotFound(slot.Id);
                }

                reserveSlot.Remark = slot.Remark;
            }

            _roomReservationProvider.CancelReserveSlots(reserveSlots, userId.ToString());
        }

        private static RoomReserveRequestViewModel MapDTOToViewModel(RoomReserveRequestDTO dto, RoomDTO room)
        {
            var response = new RoomReserveRequestViewModel
            {
                Id = dto.Id,
                Title = dto.Title,
                SenderType = dto.SenderType,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                FromDate = dto.FromDate,
                ToDate = dto.ToDate,
                StartedAt = dto.StartedAt,
                EndedAt = dto.EndedAt,
                RepeatedOn = dto.RepeatedOn,
                RoomId = dto.RoomId,
                UsageType = dto.UsageType,
                Description = dto.Description,
                Remark = dto.Remark,
                RequesterName = dto.RequesterName,
                Status = dto.Status,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt,
                RoomName = room.Name,
                Slots = (from slot in dto.Slots
                         select MapSlotDTOToViewModel(slot, room))
                        .ToList()
            };

            return response;
        }

        private static RoomReserveSlotViewModel MapSlotDTOToViewModel(RoomReserveSlotDTO dto, RoomDTO room)
        {
            var response = new RoomReserveSlotViewModel
            {
                Id = dto.Id,
                RoomId = dto.RoomId,
                FromDate = dto.FromDate,
                ToDate = dto.ToDate,
                Status = dto.Status,
                Remark = dto.Remark,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt,
                RoomName = room.Name
            };

            return response;
        }
    }
}