using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Plexus.Database;
using Plexus.Database.Enum.Facility.Reservation;
using Plexus.Database.Model.Facility.Reservation;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Facility.Reservation;
using Plexus.Entity.Exception;
using Plexus.Utility.Extensions;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider.src.Facility.Reservation
{
    public class RoomReservationProvider : IRoomReservationProvider
    {
        private readonly DatabaseContext _dbContext;

        public RoomReservationProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public RoomReserveRequestDTO Create(CreateRoomReserveRequestDTO request, string requester)
        {
            var model = new RoomReserveRequest
            {
                Title = request.Title,
                SenderType = request.SenderType,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                FromDate = request.FromDate,
                ToDate = request.ToDate,
                StartedAt = request.StartedAt,
                EndedAt = request.EndedAt,
                RepeatedOn = request.RepeatedOn is null ? null
                                                        : JsonConvert.SerializeObject(request.RepeatedOn),
                RoomId = request.RoomId,
                UsageType = request.UsageType,
                Description = request.Description,
                Remark = request.Remark,
                RequesterName = request.RequesterName,
                Status = ReservationStatus.PENDING,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = requester
            };

            var reserveSlots = MapReserveSlot(request.RoomId, request.FromDate, request.ToDate, request.RepeatedOn,
                                              request.StartedAt, request.EndedAt, requester, model).ToList();

            if (IsReservationOverlapped(request.RoomId, request.FromDate, request.ToDate ?? request.FromDate, reserveSlots))
            {
                throw new RoomReservationException.ReserveSlotOverlapped();
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.RoomReserveRequests.Add(model);

                if (reserveSlots.Any())
                {
                    _dbContext.RoomReserveSlots.AddRange(reserveSlots);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(model, reserveSlots);

            return response;
        }

        public PagedViewModel<RoomReserveRequestDTO> Search(SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var query = GenerateSeachQuery(parameters);

            var pagedReserveRequest = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<RoomReserveRequestDTO>
            {
                Page = pagedReserveRequest.Page,
                TotalPage = pagedReserveRequest.TotalPage,
                TotalItem = pagedReserveRequest.TotalItem,
                Items = (from reserveRequest in pagedReserveRequest.Items
                         select MapModelToDTO(reserveRequest, reserveRequest.Slots))
                        .ToList()
            };

            return response;
        }

        public RoomReserveRequestDTO GetById(Guid id)
        {
            var reserveRequest = _dbContext.RoomReserveRequests.Include(x => x.Slots)
                                                               .AsNoTracking()
                                                               .SingleOrDefault(x => x.Id == id);

            if (reserveRequest is null)
            {
                throw new RoomReservationException.NotFound(id);
            }

            var response = MapModelToDTO(reserveRequest, reserveRequest.Slots);

            return response;
        }

        public IEnumerable<RoomReserveSlotDTO> GetReserveSlotByRequestId(Guid id)
        {
            var reserveSlots = _dbContext.RoomReserveSlots.AsNoTracking()
                                                          .Where(x => x.RoomReserveRequestId.HasValue
                                                                      && x.RoomReserveRequestId == id)
                                                          .ToList();

            var response = (from slot in reserveSlots
                            orderby slot.FromDate
                            select MapSlotModelToDTO(slot))
                           .ToList();

            return response;
        }

        public IEnumerable<RoomReserveSlotDTO> GetReserveSlotById(IEnumerable<Guid> ids)
        {
            var reserveSlots = _dbContext.RoomReserveSlots.AsNoTracking()
                                                          .Where(x => ids.Contains(x.Id))
                                                          .ToList();

            var response = (from slot in reserveSlots
                            orderby slot.FromDate
                            select MapSlotModelToDTO(slot))
                           .ToList();

            return response;
        }

        public void UpdateStatus(Guid id, ReservationStatus status, string? remark, string requester)
        {
            var reserveRequest = _dbContext.RoomReserveRequests.SingleOrDefault(x => x.Id == id);

            if (reserveRequest is null)
            {
                throw new RoomReservationException.NotFound(id);
            }

            var reserveSlots = _dbContext.RoomReserveSlots.Where(x => x.RoomReserveRequestId.HasValue
                                                                      && x.RoomReserveRequestId == id)
                                                          .ToList();

            if (status == ReservationStatus.APPROVED
                && IsReservationOverlapped(reserveRequest.RoomId, reserveRequest.FromDate, reserveRequest.ToDate ?? reserveRequest.FromDate, reserveSlots))
            {
                throw new RoomReservationException.ReserveSlotOverlapped();
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                reserveRequest.Status = status;
                reserveRequest.Remark = remark;
                reserveRequest.UpdatedAt = DateTime.UtcNow;
                reserveRequest.UpdatedBy = requester;

                foreach (var slot in reserveSlots)
                {
                    slot.Status = status;
                    slot.UpdatedAt = DateTime.UtcNow;
                    slot.UpdatedBy = requester;
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        public void CancelReserveSlots(IEnumerable<UpdateRoomReserveSlotDTO> requests, string requester)
        {
            var ids = requests.Select(x => x.Id)
                              .ToList();

            var reserveSlots = _dbContext.RoomReserveSlots.Where(x => ids.Contains(x.Id))
                                                          .ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                foreach (var slot in requests)
                {
                    var reserveSlot = reserveSlots.SingleOrDefault(x => x.Id == slot.Id);

                    if (reserveSlot is null)
                    {
                        throw new RoomReservationException.SlotNotFound(slot.Id);
                    }

                    reserveSlot.Status = ReservationStatus.CANCELLED;
                    reserveSlot.Remark = slot.Remark;
                    reserveSlot.UpdatedAt = DateTime.UtcNow;
                    reserveSlot.UpdatedBy = requester;
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        private static RoomReserveRequestDTO MapModelToDTO(RoomReserveRequest model, IEnumerable<RoomReserveSlot> slots)
        {
            var response = new RoomReserveRequestDTO
            {
                Id = model.Id,
                Title = model.Title,
                SenderType = model.SenderType,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                FromDate = model.FromDate,
                ToDate = model.ToDate,
                StartedAt = model.StartedAt,
                EndedAt = model.EndedAt,
                RepeatedOn = string.IsNullOrEmpty(model.RepeatedOn) ? Enumerable.Empty<DayOfWeek>()
                                                                    : JsonConvert.DeserializeObject<IEnumerable<DayOfWeek>>(model.RepeatedOn),
                RoomId = model.RoomId,
                UsageType = model.UsageType,
                Description = model.Description,
                Remark = model.Remark,
                RequesterName = model.RequesterName,
                Status = model.Status,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                Slots = slots is null || !slots.Any() ? Enumerable.Empty<RoomReserveSlotDTO>()
                                                      : (from slot in slots
                                                         orderby slot.FromDate
                                                         select MapSlotModelToDTO(slot))
                                                        .ToList()
            };

            return response;
        }

        private static RoomReserveSlotDTO MapSlotModelToDTO(RoomReserveSlot model)
        {
            var response = new RoomReserveSlotDTO
            {
                Id = model.Id,
                RoomId = model.RoomId,
                FromDate = model.FromDate,
                ToDate = model.ToDate,
                Status = model.Status,
                Remark = model.Remark,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt
            };

            return response;
        }

        private IQueryable<RoomReserveRequest> GenerateSeachQuery(SearchCriteriaViewModel? parameters)
        {
            var query = _dbContext.RoomReserveRequests.Include(x => x.Slots)
                                                      .Include(x => x.Room)
                                                      .ThenInclude(x => x.Building)
                                                      .AsNoTracking();

            if (parameters is not null)
            {
                if (parameters.FromDate.HasValue)
                {
                    query = query.Where(x => x.FromDate >= parameters.FromDate.Value.Date);
                }

                if (parameters.ToDate.HasValue)
                {
                    query = query.Where(x => x.ToDate < parameters.ToDate.Value.AddDays(1).Date);
                }

                if (!string.IsNullOrEmpty(parameters.Name))
                {
                    query = query.Where(x => x.Title.Contains(parameters.Name));
                }

                if (parameters.CampusId.HasValue)
                {
                    query = query.Where(x => x.Room.Building != null
                                             && x.Room.Building.CampusId == parameters.CampusId.Value);
                }

                if (parameters.BuildingId.HasValue)
                {
                    query = query.Where(x => x.Room.BuildingId == parameters.BuildingId.Value);
                }

                if (parameters.Floor.HasValue)
                {
                    query = query.Where(x => x.Room.Floor == parameters.Floor.Value);
                }

                if (parameters.RoomId.HasValue)
                {
                    query = query.Where(x => x.RoomId == parameters.RoomId.Value);
                }

                if (parameters.SenderType.HasValue)
                {
                    query = query.Where(x => x.SenderType == parameters.SenderType);
                }

                if (!string.IsNullOrEmpty(parameters.CreatedBy))
                {
                    query = query.Where(x => x.CreatedBy == parameters.CreatedBy);
                }
            }

            query = query.OrderBy(x => x.FromDate);

            if (parameters is not null)
            {
                if (!string.IsNullOrEmpty(parameters.SortBy))
                {
                    try
                    {
                        query = query.OrderBy(parameters.SortBy, parameters.OrderBy);
                    }
                    catch (System.Exception)
                    {
                        // invalid property name
                    }
                }
            }

            return query;
        }

        private static IEnumerable<RoomReserveSlot> MapReserveSlot(Guid roomId, DateTime fromDate, DateTime? toDate, IEnumerable<DayOfWeek>? repeatedOn,
            TimeSpan startedAt, TimeSpan endedAt, string requester, RoomReserveRequest? reserveRequest)
        {
            var response = new List<RoomReserveSlot>();

            var days = repeatedOn is null || !repeatedOn.Any() ? Enum.GetValues<DayOfWeek>()
                                                               : repeatedOn;

            var fromTime = fromDate.Add(startedAt).ToUniversalTime();
            var toTime = fromDate.Add(endedAt).ToUniversalTime();

            // SINGLE DAY.
            if (toDate is null)
            {
                var reserveSlot = new RoomReserveSlot
                {
                    RoomId = roomId,
                    FromDate = fromTime,
                    ToDate = toTime,
                    Status = ReservationStatus.PENDING,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = requester,
                    UpdatedAt = DateTime.UtcNow,
                    UpdatedBy = requester
                };

                if (reserveRequest is not null)
                {
                    reserveSlot.RoomReserveRequest = reserveRequest;
                }

                response.Add(reserveSlot);

                return response;
            }

            var daydiff = (toDate.Value.Date - fromDate.Date).Days;

            // MULTIPLE DAYS.
            for (int i = 0; i < daydiff; i++)
            {
                var slotday = fromDate.AddDays(i);

                if(days.Any() && !days.Contains(slotday.DayOfWeek))
                {
                    continue;
                }

                var reserveSlot = new RoomReserveSlot
                {
                    RoomId = roomId,
                    FromDate = fromTime.AddDays(i),
                    ToDate = toTime.AddDays(i),
                    Status = ReservationStatus.PENDING,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = requester,
                    UpdatedAt = DateTime.UtcNow,
                    UpdatedBy = requester
                };

                if (reserveRequest is not null)
                {
                    reserveSlot.RoomReserveRequest = reserveRequest;
                }

                response.Add(reserveSlot);
            }

            return response;
        }

        private bool IsReservationOverlapped(Guid roomId, DateTime fromDate, DateTime toDate, IEnumerable<RoomReserveSlot> reserveSlots)
        {
            var ids = reserveSlots.Select(x => x.Id)
                                  .ToList();

            var approvedReserveSlots = _dbContext.RoomReserveSlots.AsNoTracking()
                                                                  .Where(x => !ids.Contains(x.Id)
                                                                              && x.RoomId == roomId
                                                                              && x.FromDate >= fromDate.Date
                                                                              && x.ToDate < toDate.AddDays(1).Date
                                                                              && x.Status == ReservationStatus.APPROVED)
                                                                  .ToList();

            foreach (var slot in reserveSlots)
            {
                var overlappedSlot = approvedReserveSlots.Where(x => slot.FromDate < x.ToDate
                                                                     && x.FromDate < slot.ToDate
                                                                     && x.RoomId == slot.RoomId)
                                                         .ToList();

                if (overlappedSlot.Any())
                {
                    return true;
                }
            }

            return false;
        }
    }
}