using Plexus.Client.ViewModel.DropDown;
using Plexus.Client.ViewModel.Facility;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Entity.DTO.Facility;
using Plexus.Entity.DTO.SearchFilter;
using Plexus.Entity.Exception;
using Plexus.Entity.Provider;
using Plexus.Utility.ViewModel;

namespace Plexus.Client.src.Facility
{
    public class RoomTypeManager : IRoomTypeManager
    {
        private readonly IRoomTypeProvider _roomTypeProvider;

        public RoomTypeManager(IRoomTypeProvider roomTypeProvider)
        {
            _roomTypeProvider = roomTypeProvider;
        }

        public RoomTypeViewModel Create(CreateRoomTypeViewModel request, Guid userId)
        {
            var roomTypes = _roomTypeProvider.GetAll()
                                             .ToList();

            var duplicateRoomTypes = roomTypes.Where(x => string.Equals(x.Name, request.Name, StringComparison.InvariantCultureIgnoreCase))
                                              .ToList();

            if (duplicateRoomTypes.Any())
            {
                throw new RoomTypeException.Duplicate(request.Name);
            }

            var dto = new CreateRoomTypeDTO
            {
                Name = request.Name
            };

            var roomType = _roomTypeProvider.Create(dto, userId.ToString());

            var response = MapDTOToViewModel(roomType);

            return response;
        }

        public RoomTypeViewModel GetById(Guid id)
        {
            var roomType = _roomTypeProvider.GetById(id);

            var response = MapDTOToViewModel(roomType);

            return response;
        }

        public PagedViewModel<RoomTypeViewModel> Search(SearchRoomTypeCriteriaViewModel criteria, int page, int pageSize)
        {
            var dto = new SearchRoomTypeCriteriaDTO
            {
                Keyword = criteria.Keyword,
                SortBy = criteria.SortBy,
                OrderBy = criteria.OrderBy
            };

            var pagedRooms = _roomTypeProvider.Search(dto, page, pageSize);

            var response = new PagedViewModel<RoomTypeViewModel>
            {
                Page = pagedRooms.Page,
                TotalPage = pagedRooms.TotalPage,
                TotalItem = pagedRooms.TotalItem,
                Items = (from roomType in pagedRooms.Items
                         select MapDTOToViewModel(roomType))
                        .ToList()
            };

            return response;
        }

        public RoomTypeViewModel Update(RoomTypeViewModel request, Guid userId)
        {
            var roomTypes = _roomTypeProvider.GetAll()
                                     .ToList();

            var roomType = roomTypes.SingleOrDefault(x => x.Id == request.Id);

            if (roomType is null)
            {
                throw new RoomTypeException.NotFound(request.Id);
            }

            var duplicateRoomTypes = roomTypes.Where(x => x.Id != request.Id
                                                          && string.Equals(x.Name, request.Name, StringComparison.InvariantCultureIgnoreCase))
                                      .ToList();

            if (duplicateRoomTypes.Any())
            {
                throw new RoomTypeException.Duplicate(request.Name);
            }

            roomType.Name = request.Name;

            var updatedRoomType = _roomTypeProvider.Update(roomType, userId.ToString());

            var response = MapDTOToViewModel(updatedRoomType);

            return response;
        }

        public void Delete(Guid id)
        {
            _roomTypeProvider.Delete(id);
        }

        public IEnumerable<RoomTypeDropDownViewModel> GetDropDownList(SearchRoomTypeCriteriaViewModel parameters)
        {
            var dto = new SearchRoomTypeCriteriaDTO
            {
                Keyword = parameters.Keyword,
                SortBy = parameters.SortBy,
                OrderBy = parameters.OrderBy
            };

            var roomTypes = _roomTypeProvider.Search(dto)
                                             .ToList();

            var response = (from roomType in roomTypes
                            orderby roomType.Name
                            select new RoomTypeDropDownViewModel
                            {
                                Id = roomType.Id.ToString(),
                                Name = roomType.Name
                            })
                           .ToList();

            return response;
        }

        private static RoomTypeViewModel MapDTOToViewModel(RoomTypeDTO dto)
        {
            var response = new RoomTypeViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt,
            };

            return response;
        }
    }
}