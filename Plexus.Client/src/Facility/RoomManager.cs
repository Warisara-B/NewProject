using Plexus.Client.ViewModel.DropDown;
using Plexus.Client.ViewModel.Facility;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Facility;
using Plexus.Entity.DTO.SearchFilter;
using Plexus.Entity.Exception;
using Plexus.Entity.Provider;
using Plexus.Entity.Provider.src.Facility;
using Plexus.Utility.ViewModel;

namespace Plexus.Client.src.Facility
{
    public class RoomManager : IRoomManager
    {
        private readonly IRoomProvider _roomProvider;
        private readonly IBuildingProvider _buildingProvider;
        private readonly IRoomTypeProvider _roomTypeProvider;
        private readonly IFacilityProvider _facilityProvider;

        public RoomManager(IRoomProvider roomProvider,
                           IBuildingProvider buildingProvider,
                           IRoomTypeProvider roomTypeProvider,
                           IFacilityProvider facilityProvider)
        {
            _roomProvider = roomProvider;
            _buildingProvider = buildingProvider;
            _roomTypeProvider = roomTypeProvider;
            _facilityProvider = facilityProvider;
        }

        public RoomViewModel Create(CreateRoomViewModel request, Guid userId)
        {
            var building = request.BuildingId.HasValue ? _buildingProvider.GetById(request.BuildingId.Value)
                                                       : null;

            var roomType = request.RoomTypeId.HasValue ? _roomTypeProvider.GetById(request.RoomTypeId.Value)
                                                       : null;

            var rooms = _roomProvider.GetAll()
                                     .ToList();

            var duplicateRooms = rooms.Where(x => string.Equals(x.Code, request.Code, StringComparison.InvariantCultureIgnoreCase))
                                      .ToList();

            if (duplicateRooms.Any())
            {
                throw new RoomException.Duplicate(request.Code);
            }

            //TODO : Refactor Validation
            var facilityIds = request.Facilities is null ? Enumerable.Empty<Guid>()
                                                         : request.Facilities.Select(x => x.FacilityId)
                                                                             .ToList();

            var duplicateFacility = facilityIds.GroupBy(x => x)
                                               .Where(x => x.Count() > 1)
                                               .ToList();

            if (duplicateFacility.Any())
            {
                throw new RoomException.DuplicateFacility();
            }

            var facilities = _facilityProvider.GetById(facilityIds)
                                              .ToList();

            foreach (var facilityId in facilityIds)
            {
                var facility = facilities.SingleOrDefault(x => x.Id == facilityId);

                if (facility is null)
                {
                    throw new FacilityException.NotFound(facilityId);
                }
            }

            var dto = new CreateRoomDTO
            {
                BuildingId = request.BuildingId,
                Name = request.Name,
                Code = request.Code,
                Floor = request.Floor,
                Capacity = request.Capacity,
                ExaminationCapacity = request.ExaminationCapacity,
                RoomTypeId = request.RoomTypeId,
                IsActive = request.IsActive,
                IsReservable = request.IsReservable,
                Localizations = MapLocalizationViewModelToDTO(request.Localizations).ToList(),
                Facilities = MapFacilityViewModelToDTO(request.Facilities).ToList()
            };

            var room = _roomProvider.Create(dto, userId.ToString());

            var response = MapDTOToViewModel(room, facilities, building, roomType);

            return response;
        }

        public RoomViewModel GetById(Guid id)
        {
            var room = _roomProvider.GetById(id);

            var building = room.BuildingId.HasValue ? _buildingProvider.GetById(room.BuildingId.Value)
                                                    : null;

            var roomType = room.RoomTypeId.HasValue ? _roomTypeProvider.GetById(room.RoomTypeId.Value)
                                                    : null;

            var facilityIds = room.Facilities is null ? Enumerable.Empty<Guid>()
                                                      : room.Facilities.Select(x => x.FacilityId)
                                                                       .ToList();

            var facilities = _facilityProvider.GetById(facilityIds)
                                              .ToList();

            var response = MapDTOToViewModel(room, facilities, building, roomType);

            return response;
        }

        public PagedViewModel<RoomViewModel> Search(SearchRoomCriteriaViewModel criteria, int page, int pageSize)
        {
            var dto = new SearchRoomCriteriaDTO
            {
                Keyword = criteria.Keyword,
                BuildingId = criteria.BuildingId,
                CampusId = criteria.CampusId,
                IsActive = criteria.IsActive,
                SortBy = criteria.SortBy,
                OrderBy = criteria.OrderBy
            };

            var pagedRooms = _roomProvider.Search(dto, page, pageSize);

            var buildings = _buildingProvider.GetAll();

            var roomTypes = _roomTypeProvider.GetAll();

            var facilityIds = pagedRooms.Items.Where(x => x.Facilities is not null)
                                              .SelectMany(x => x.Facilities!.Select(x => x.FacilityId))
                                              .Distinct()
                                              .ToList();

            var facilities = _facilityProvider.GetById(facilityIds)
                                              .ToList();

            var response = new PagedViewModel<RoomViewModel>
            {
                Page = pagedRooms.Page,
                TotalPage = pagedRooms.TotalPage,
                TotalItem = pagedRooms.TotalItem,
                Items = (from room in pagedRooms.Items
                         let building = room.BuildingId.HasValue ? buildings.SingleOrDefault(x => x.Id == room.BuildingId)
                                                                 : null
                         let roomType = room.RoomTypeId.HasValue ? roomTypes.SingleOrDefault(x => x.Id == room.RoomTypeId)
                                                                 : null
                         select MapDTOToViewModel(room, facilities, building, roomType))
                        .ToList()
            };

            return response;
        }

        public RoomViewModel Update(RoomViewModel request, Guid userId)
        {
            var building = request.BuildingId.HasValue ? _buildingProvider.GetById(request.BuildingId.Value)
                                                       : null;

            var roomType = request.RoomTypeId.HasValue ? _roomTypeProvider.GetById(request.RoomTypeId.Value)
                                                       : null;

            var rooms = _roomProvider.GetAll()
                                     .ToList();

            var room = rooms.SingleOrDefault(x => x.Id == request.Id);

            if (room is null)
            {
                throw new RoomException.NotFound(request.Id);
            }

            var duplicateRooms = rooms.Where(x => x.Id != request.Id
                                                  && string.Equals(x.Code, request.Code, StringComparison.InvariantCultureIgnoreCase))
                                      .ToList();

            if (duplicateRooms.Any())
            {
                throw new RoomException.Duplicate(request.Code);
            }

            var facilityIds = request.Facilities is null ? Enumerable.Empty<Guid>()
                                                         : request.Facilities.Select(x => x.FacilityId)
                                                                             .ToList();

            var duplicateFacility = facilityIds.GroupBy(x => x)
                                               .Where(x => x.Count() > 1)
                                               .ToList();

            if (duplicateFacility.Any())
            {
                throw new RoomException.DuplicateFacility();
            }

            var facilities = _facilityProvider.GetById(facilityIds)
                                              .ToList();

            foreach (var facilityId in facilityIds)
            {
                var facility = facilities.SingleOrDefault(x => x.Id == facilityId);

                if (facility is null)
                {
                    throw new FacilityException.NotFound(facilityId);
                }
            }

            room.BuildingId = request.BuildingId;
            room.Name = request.Name;
            room.Code = request.Code;
            room.Floor = request.Floor;
            room.Capacity = request.Capacity;
            room.ExaminationCapacity = request.ExaminationCapacity;
            room.RoomTypeId = request.RoomTypeId;
            room.IsActive = request.IsActive;
            room.IsReservable = request.IsReservable;
            room.Localizations = MapLocalizationViewModelToDTO(request.Localizations).ToList();
            room.Facilities = MapFacilityViewModelToDTO(request.Facilities).ToList();

            var updatedRoom = _roomProvider.Update(room, userId.ToString());

            var response = MapDTOToViewModel(updatedRoom, facilities, building, roomType);

            return response;
        }

        public void Delete(Guid id)
        {
            _roomProvider.Delete(id);
        }

        public IEnumerable<RoomDropDownViewModel> GetDropDownList(SearchRoomCriteriaViewModel parameters)
        {
            var dto = new SearchRoomCriteriaDTO
            {
                Keyword = parameters.Keyword,
                BuildingId = parameters.BuildingId,
                CampusId = parameters.CampusId,
                IsActive = parameters.IsActive,
                SortBy = parameters.SortBy,
                OrderBy = parameters.OrderBy
            };

            var rooms = _roomProvider.Search(dto)
                                     .ToList();

            var response = (from room in rooms
                            orderby room.Code, room.Name
                            select new RoomDropDownViewModel
                            {
                                Id = room.Id.ToString(),
                                Name = room.Name,
                                BuildingId = room.BuildingId
                            })
                           .ToList();

            return response;
        }

        public IEnumerable<RoomFacilityViewModel> GetFacilityByRoomId(Guid id)
        {
            var roomFacilities = _roomProvider.GetFacilityByRoomId(id)
                                              .ToList();

            var facilityIds = roomFacilities.Select(x => x.FacilityId)
                                            .ToList();

            var facilities = _facilityProvider.GetById(facilityIds)
                                              .ToList();

            var response = (from roomFacility in roomFacilities
                            let facility = facilities.SingleOrDefault(x => x.Id == roomFacility.FacilityId)
                            select MapFacilityDTOToViewModel(roomFacility, facility))
                           .ToList();

            return response;
        }

        private static RoomViewModel MapDTOToViewModel(RoomDTO dto, IEnumerable<FacilityDTO> facilities, BuildingDTO? building = null, RoomTypeDTO? roomType = null)
        {
            var response = new RoomViewModel
            {
                Id = dto.Id,
                BuildingId = dto.BuildingId,
                BuildingName = building?.Name,
                Code = dto.Code,
                Floor = dto.Floor,
                Capacity = dto.Capacity,
                ExaminationCapacity = dto.ExaminationCapacity,
                RoomTypeId = dto.RoomTypeId,
                RoomTypeName = roomType?.Name,
                IsActive = dto.IsActive,
                IsReservable = dto.IsReservable,
                Localizations = (from locale in dto.Localizations
                                 orderby locale.Language
                                 select new RoomLocalizationViewModel
                                 {
                                     Language = locale.Language,
                                     Name = locale.Name
                                 })
                                .ToList(),
                Facilities = (from roomFacility in dto.Facilities
                              let facility = facilities.SingleOrDefault(x => x.Id == roomFacility.FacilityId)
                              orderby roomFacility.CreatedAt
                              select MapFacilityDTOToViewModel(roomFacility, facility))
                             .ToList(),
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt,
            };

            return response;
        }

        private static IEnumerable<RoomLocalizationDTO> MapLocalizationViewModelToDTO(
            IEnumerable<RoomLocalizationViewModel>? localizations)
        {
            if (localizations is null)
            {
                return Enumerable.Empty<RoomLocalizationDTO>();
            }

            var response = (from locale in localizations
                            select new RoomLocalizationDTO
                            {
                                Language = locale.Language,
                                Name = locale.Name
                            })
                            .ToList();

            return response;
        }

        private static IEnumerable<RoomFacilityDTO> MapFacilityViewModelToDTO(IEnumerable<UpdateRoomFacilityViewModel>? facilities)
        {
            if (facilities is null)
            {
                return Enumerable.Empty<RoomFacilityDTO>();
            }

            var response = (from facility in facilities
                            select new RoomFacilityDTO
                            {
                                FacilityId = facility.FacilityId,
                                Amount = facility.Amount,
                                IsActive = facility.IsActive
                            })
                           .ToList();

            return response;
        }

        private static RoomFacilityViewModel MapFacilityDTOToViewModel(RoomFacilityDTO dto, FacilityDTO facility)
        {
            var response = new RoomFacilityViewModel
            {
                FacilityId = dto.FacilityId,
                Amount = dto.Amount,
                IsActive = dto.IsActive,
                CreatedAt = dto.CreatedAt,
                FacilityName = facility?.Name
            };

            return response;
        }
    }
}