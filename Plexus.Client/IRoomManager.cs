using Plexus.Client.ViewModel.DropDown;
using Plexus.Client.ViewModel.Facility;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface IRoomManager
    {
        /// <summary>
        /// Create new room record
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        RoomViewModel Create(CreateRoomViewModel request, Guid userId);

        /// <summary>
        /// Get room by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        RoomViewModel GetById(Guid id);

        /// <summary>
        /// Search room according to given criteria
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<RoomViewModel> Search(SearchRoomCriteriaViewModel criteria, int page, int pageSize);

        /// <summary>
        /// Update room information
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        RoomViewModel Update(RoomViewModel request, Guid userId);

        /// <summary>
        /// Delete room information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);

        /// <summary>
        /// Get room drop down list.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<RoomDropDownViewModel> GetDropDownList(SearchRoomCriteriaViewModel parameters);

        /// <summary>
        /// Get facilities by room id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IEnumerable<RoomFacilityViewModel> GetFacilityByRoomId(Guid id);
    }
}