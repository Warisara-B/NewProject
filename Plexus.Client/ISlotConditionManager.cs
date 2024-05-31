using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Client.ViewModel.Registration;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface ISlotConditionManager
    {
        /// <summary>
        /// Create new slot condition.
        /// </summary>
        /// <param name="slotId"></param>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        SlotConditionViewModel Create(Guid slotId, CreateSlotConditionViewModel request, Guid userId);

        /// <summary>
        /// Search slot condition by given parameters as paged.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<SlotConditionViewModel> Search(SearchSlotConditionCriteriaViewModel parameters, int page = 1, int pageSize = 25);

        /// <summary>
        /// Get slot condition by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        SlotConditionViewModel GetById(Guid id);

        /// <summary>
        /// Update slot condition info.
        /// </summary>
        /// <param name="slotId"></param>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        SlotConditionViewModel Update(Guid slotId, Guid id, CreateSlotConditionViewModel request, Guid userId);

        /// <summary>
        /// Delete slot condition by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);
    }
}