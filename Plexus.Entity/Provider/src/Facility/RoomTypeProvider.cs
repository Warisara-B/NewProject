using Microsoft.EntityFrameworkCore;
using Plexus.Database;
using Plexus.Database.Model.Facility;
using Plexus.Entity.DTO.Facility;
using Plexus.Entity.DTO.SearchFilter;
using Plexus.Entity.Exception;
using Plexus.Utility.Extensions;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider.src.Facility
{
    public class RoomTypeProvider : IRoomTypeProvider
    {
        private readonly DatabaseContext _dbContext;

        public RoomTypeProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public RoomTypeDTO Create(CreateRoomTypeDTO request, string requester)
        {
            var model = new RoomType
            {
                Name = request.Name,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = requester
            };

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.RoomTypes.Add(model);
                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(model);

            return response;
        }

        public IEnumerable<RoomTypeDTO> GetAll()
        {
            var roomTypes = _dbContext.RoomTypes.AsNoTracking()
                                                .ToList();

            var response = (from roomType in roomTypes
                            orderby roomType.Name
                            select MapModelToDTO(roomType))
                           .ToList();

            return response;
        }

        public RoomTypeDTO GetById(Guid id)
        {
            var roomType = _dbContext.RoomTypes.AsNoTracking()
                                               .SingleOrDefault(x => x.Id == id);

            if (roomType is null)
            {
                throw new RoomException.NotFound(id);
            }

            var response = MapModelToDTO(roomType);

            return response;
        }

        public IEnumerable<RoomTypeDTO> GetById(IEnumerable<Guid> ids)
        {
            var roomTypes = _dbContext.RoomTypes.AsNoTracking()
                                                .Where(x => ids.Contains(x.Id))
                                                .ToList();

            var response = (from roomType in roomTypes
                            orderby roomType.Name
                            select MapModelToDTO(roomType))
                           .ToList();

            return response;
        }

        public PagedViewModel<RoomTypeDTO> Search(SearchRoomTypeCriteriaDTO parameters, int page, int pageSize)
        {
            var query = GenerateSearchQuery(parameters);

            var pagedRoomType = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<RoomTypeDTO>
            {
                Page = pagedRoomType.Page,
                TotalPage = pagedRoomType.TotalPage,
                TotalItem = pagedRoomType.TotalItem,
                Items = (from roomType in pagedRoomType.Items
                         select MapModelToDTO(roomType))
                        .ToList()
            };

            return response;
        }

        public IEnumerable<RoomTypeDTO> Search(SearchRoomTypeCriteriaDTO parameters)
        {
            var query = GenerateSearchQuery(parameters);

            var roomTypes = query.ToList();

            var response = (from roomType in roomTypes
                            select MapModelToDTO(roomType))
                           .ToList();

            return response;
        }

        public RoomTypeDTO Update(RoomTypeDTO request, string requester)
        {
            var roomType = _dbContext.RoomTypes.SingleOrDefault(x => x.Id == request.Id);

            if (roomType is null)
            {
                throw new RoomException.NotFound(request.Id);
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                roomType.Name = request.Name;
                roomType.UpdatedAt = DateTime.UtcNow;
                roomType.UpdatedBy = requester;

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(roomType);

            return response;
        }

        public void Delete(Guid id)
        {
            var roomType = _dbContext.RoomTypes.SingleOrDefault(x => x.Id == id);

            if (roomType is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.RoomTypes.Remove(roomType);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        private IQueryable<RoomType> GenerateSearchQuery(SearchRoomTypeCriteriaDTO? parameters)
        {
            var query = _dbContext.RoomTypes.AsNoTracking();

            if (parameters != null)
            {
                if (!string.IsNullOrEmpty(parameters.Keyword))
                {
                    query = query.Where(x => x.Name.Contains(parameters.Keyword));
                }
            }

            query = query.OrderBy(x => x.Name);

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

        private static RoomTypeDTO MapModelToDTO(RoomType model)
        {
            return new RoomTypeDTO
            {
                Id = model.Id,
                Name = model.Name,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
            };
        }
    }
}