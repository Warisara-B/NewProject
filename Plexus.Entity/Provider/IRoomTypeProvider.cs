using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Facility;
using Plexus.Entity.DTO.SearchFilter;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider
{
    public interface IRoomTypeProvider
    {
        /// <summary>
        /// Create new room type
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        RoomTypeDTO Create(CreateRoomTypeDTO request, string requester);

        /// <summary>
        /// Get all room type
        /// </summary>
        /// <returns></returns>
        IEnumerable<RoomTypeDTO> GetAll();

        /// <summary>
        /// Get room type by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        RoomTypeDTO GetById(Guid id);

        /// <summary>
        /// Get room type by ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        IEnumerable<RoomTypeDTO> GetById(IEnumerable<Guid> ids);

        /// <summary>
        /// Search room type as paging
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<RoomTypeDTO> Search(SearchRoomTypeCriteriaDTO criteria, int page, int pageSize);

        /// <summary>
        /// Search room type by given criteria.
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IEnumerable<RoomTypeDTO> Search(SearchRoomTypeCriteriaDTO criteria);

        /// <summary>
        /// Update room type
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        RoomTypeDTO Update(RoomTypeDTO request, string requester);

        /// <summary>
        /// Delete room type
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);
    }
}