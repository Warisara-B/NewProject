using Microsoft.EntityFrameworkCore;
using Plexus.Database;
using Plexus.Database.Model.Facility;
using Plexus.Database.Model.Localization.Facility;
using Plexus.Entity.DTO.Facility;
using Plexus.Entity.DTO.SearchFilter;
using Plexus.Entity.Exception;
using Plexus.Utility.Extensions;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider.src.Facility
{
    public class RoomProvider : IRoomProvider
    {
        private readonly DatabaseContext _dbContext;

        public RoomProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public RoomDTO Create(CreateRoomDTO request, string requester)
        {
            var model = new Room
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
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = requester
            };

            var localizes = MapLocalizationDTOToModel(request.Localizations, model).ToList();

            var facilities = MapFacilityDTOToModel(request.Facilities, requester, model).ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Rooms.Add(model);

                if (localizes.Any())
                {
                    _dbContext.RoomLocalizations.AddRange(localizes);
                }

                if (facilities.Any())
                {
                    _dbContext.RoomFacilities.AddRange(facilities);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(model, localizes, facilities);

            return response;
        }

        public IEnumerable<RoomDTO> GetAll()
        {
            var rooms = _dbContext.Rooms.AsNoTracking()
                                        .Include(x => x.Localizations)
                                        .Include(x => x.Facilities)
                                        .ToList();

            var response = (from room in rooms
                            orderby room.Code
                            select MapModelToDTO(room, room.Localizations, room.Facilities))
                           .ToList();

            return response;
        }

        public IEnumerable<RoomDTO> GetByBuildingId(Guid? buildingId)
        {
            var rooms = _dbContext.Rooms.AsNoTracking()
                                        .Include(x => x.Localizations)
                                        .Include(x => x.Facilities)
                                        .Where(x => x.BuildingId == buildingId)
                                        .ToList();

            var response = (from room in rooms
                            orderby room.Code
                            select MapModelToDTO(room, room.Localizations, room.Facilities))
                           .ToList();

            return response;
        }

        public RoomDTO GetById(Guid id)
        {
            var room = _dbContext.Rooms.AsNoTracking()
                                       .Include(x => x.Localizations)
                                       .Include(x => x.Facilities)
                                       .SingleOrDefault(x => x.Id == id);

            if (room is null)
            {
                throw new RoomException.NotFound(id);
            }

            var response = MapModelToDTO(room, room.Localizations, room.Facilities);

            return response;
        }

        public IEnumerable<RoomDTO> GetById(IEnumerable<Guid> ids)
        {
            var rooms = _dbContext.Rooms.AsNoTracking()
                                        .Include(x => x.Localizations)
                                        .Include(x => x.Facilities)
                                        .Where(x => ids.Contains(x.Id))
                                        .ToList();

            var response = (from room in rooms
                            orderby room.Code
                            select MapModelToDTO(room, room.Localizations, room.Facilities))
                           .ToList();

            return response;
        }

        public PagedViewModel<RoomDTO> Search(SearchRoomCriteriaDTO parameters, int page, int pageSize)
        {
            var query = GenerateSearchQuery(parameters);

            var pagedRoom = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<RoomDTO>
            {
                Page = pagedRoom.Page,
                TotalPage = pagedRoom.TotalPage,
                TotalItem = pagedRoom.TotalItem,
                Items = (from room in pagedRoom.Items
                         select MapModelToDTO(room, room.Localizations, room.Facilities))
                        .ToList()
            };

            return response;
        }

        public IEnumerable<RoomDTO> Search(SearchRoomCriteriaDTO parameters)
        {
            var query = GenerateSearchQuery(parameters);

            var rooms = query.ToList();

            var response = (from room in rooms
                            select MapModelToDTO(room, room.Localizations, room.Facilities))
                           .ToList();

            return response;
        }

        public RoomDTO Update(RoomDTO request, string requester)
        {
            var room = _dbContext.Rooms.Include(x => x.Localizations)
                                       .Include(x => x.Facilities)
                                       .SingleOrDefault(x => x.Id == request.Id);

            if (room is null)
            {
                throw new RoomException.NotFound(request.Id);
            }

            var localizes = MapLocalizationDTOToModel(request.Localizations, room).ToList();

            var facilities = MapFacilityDTOToModel(request.Facilities, requester, room);

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                room.BuildingId = request.BuildingId;
                room.Name = request.Name;
                room.Code = request.Code;
                room.Floor = request.Floor;
                room.Capacity = request.Capacity;
                room.ExaminationCapacity = request.ExaminationCapacity;
                room.RoomTypeId = request.RoomTypeId;
                room.IsActive = request.IsActive;
                room.IsReservable = request.IsReservable;
                room.UpdatedAt = DateTime.UtcNow;
                room.UpdatedBy = requester;

                _dbContext.RoomLocalizations.RemoveRange(room.Localizations);

                _dbContext.RoomFacilities.RemoveRange(room.Facilities);

                if (localizes.Any())
                {
                    _dbContext.RoomLocalizations.AddRange(localizes);
                }

                if (facilities.Any())
                {
                    _dbContext.RoomFacilities.AddRange(facilities);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(room, localizes, facilities);

            return response;
        }

        public void Delete(Guid id)
        {
            var room = _dbContext.Rooms.SingleOrDefault(x => x.Id == id);

            if (room is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Rooms.Remove(room);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        public IEnumerable<RoomFacilityDTO> GetFacilityByRoomId(Guid id)
        {
            var facilities = _dbContext.RoomFacilities.AsNoTracking()
                                                      .Where(x => x.RoomId == id)
                                                      .ToList();

            var response = (from facility in facilities
                            select MapFacilityModelToDTO(facility))
                           .ToList();

            return response;
        }

        private IQueryable<Room> GenerateSearchQuery(SearchRoomCriteriaDTO? parameters)
        {
            var query = _dbContext.Rooms.Include(x => x.Localizations)
                                        .Include(x => x.Facilities)
                                        .Include(x => x.Building)
                                        .AsNoTracking();

            if (parameters != null)
            {
                if (parameters.CampusId.HasValue)
                {
                    query = query.Where(x => x.BuildingId.HasValue
                                             && x.Building!.CampusId == parameters.CampusId.Value);
                }

                if (parameters.BuildingId.HasValue)
                {
                    query = query.Where(x => x.BuildingId.HasValue
                                             && x.BuildingId == parameters.BuildingId.Value);
                }

                if (!string.IsNullOrEmpty(parameters.Keyword))
                {
                    query = query.Where(x => x.Name.Contains(parameters.Keyword)
                                       || x.Code.Contains(parameters.Keyword));
                }

                if (parameters.IsActive.HasValue)
                {
                    query = query.Where(x => x.IsActive == parameters.IsActive.Value);
                }
            }

            query = query.OrderBy(x => x.Code);

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

        private static RoomDTO MapModelToDTO(Room model, IEnumerable<RoomLocalization> localizations, IEnumerable<RoomFacility> facilities)
        {
            return new RoomDTO
            {
                Id = model.Id,
                BuildingId = model.BuildingId,
                Name = model.Name,
                Code = model.Code,
                Floor = model.Floor,
                Capacity = model.Capacity,
                ExaminationCapacity = model.ExaminationCapacity,
                RoomTypeId = model.RoomTypeId,
                IsActive = model.IsActive,
                IsReservable = model.IsReservable,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                Localizations = localizations is null ? Enumerable.Empty<RoomLocalizationDTO>()
                                                      : (from localize in localizations
                                                         select new RoomLocalizationDTO
                                                         {
                                                             Language = localize.Language,
                                                             Name = localize.Name
                                                         })
                                                        .ToList(),
                Facilities = facilities is null ? Enumerable.Empty<RoomFacilityDTO>()
                                                : (from facility in facilities
                                                   select new RoomFacilityDTO
                                                   {
                                                       FacilityId = facility.FacilityId,
                                                       Amount = facility.Amount,
                                                       IsActive = facility.IsActive,
                                                       CreatedAt = facility.CreatedAt
                                                   })
                                                  .ToList()
            };
        }

        private static RoomFacilityDTO MapFacilityModelToDTO(RoomFacility model)
        {
            var response = new RoomFacilityDTO
            {
                FacilityId = model.FacilityId,
                Amount = model.Amount,
                IsActive = model.IsActive,
                CreatedAt = model.CreatedAt
            };

            return response;
        }

        private static IEnumerable<RoomLocalization> MapLocalizationDTOToModel(
            IEnumerable<RoomLocalizationDTO>? localizations,
            Room model)
        {
            if (localizations is null)
            {
                return Enumerable.Empty<RoomLocalization>();
            }

            var response = (from locale in localizations
                            select new RoomLocalization
                            {
                                Room = model,
                                Language = locale.Language,
                                Name = locale.Name
                            })
                            .ToList();

            return response;
        }

        private static IEnumerable<RoomFacility> MapFacilityDTOToModel(IEnumerable<UpdateRoomFacilityDTO>? facilities, string requester, Room model)
        {
            if (facilities is null)
            {
                return Enumerable.Empty<RoomFacility>();
            }

            var response = (from facility in facilities
                            select new RoomFacility
                            {
                                Room = model,
                                FacilityId = facility.FacilityId,
                                Amount = facility.Amount,
                                IsActive = facility.IsActive,
                                CreatedAt = DateTime.UtcNow,
                                CreatedBy = requester
                            })
                            .ToList();

            return response;
        }
    }
}