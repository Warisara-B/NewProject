using Plexus.Client.ViewModel.DropDown;
using Plexus.Client.ViewModel.Facility;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface IRoomTypeManager
    {
        /// <summary>
        /// Create new room type record
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        RoomTypeViewModel Create(CreateRoomTypeViewModel request, Guid userId);

        /// <summary>
        /// Get room type by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        RoomTypeViewModel GetById(Guid id);

        /// <summary>
        /// Search room type according to given criteria
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<RoomTypeViewModel> Search(SearchRoomTypeCriteriaViewModel criteria, int page, int pageSize);

        /// <summary>
        /// Update room type information
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        RoomTypeViewModel Update(RoomTypeViewModel request, Guid userId);

        /// <summary>
        /// Delete room type information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);

        /// <summary>
        /// Get room type drop down list.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<RoomTypeDropDownViewModel> GetDropDownList(SearchRoomTypeCriteriaViewModel parameters);
    }
}