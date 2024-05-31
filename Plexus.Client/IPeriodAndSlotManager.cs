using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Client.ViewModel.Registration;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface IPeriodAndSlotManager
    {
        /// <summary>
        /// Create a new period with all related
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        PeriodViewModel CreatePeriod(CreatePeriodViewModel request, Guid userId);

        /// <summary>
        /// Get period by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        PeriodViewModel GetPeriodById(Guid id);

        /// <summary>
        /// Search section by given parameters as paged.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<PeriodViewModel> GetPagedPeriod(int page, int pageSize);

        /// <summary>
        /// Update period with all related.
        /// </summary>
        /// <param name="periodId"></param>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        PeriodViewModel UpdatePeriod(Guid periodId, CreatePeriodViewModel request, Guid userId);

        /// <summary>
        /// Delete period by id.
        /// </summary>
        /// <param name="periodId"></param>
        void DeletePeriod(Guid periodId);

        /// <summary>
        /// Create slot with all related.
        /// </summary>
        /// <param name="periodId"></param>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        SlotViewModel CreateSlot(Guid periodId, CreateSlotViewModel request, Guid userId);

        /// <summary>
        /// Get slot by slot id.
        /// </summary>
        /// <param name="slotId"></param>
        /// <returns></returns>
        SlotViewModel GetSlotById(Guid slotId);

        /// <summary>
        /// Get slot by period id.
        /// </summary>
        /// <param name="periodId"></param>
        /// <returns></returns>
        IEnumerable<SlotViewModel> GetSlotByPeriodId(Guid periodId);

        /// <summary>
        /// Get slots by period id as paged.
        /// </summary>
        /// <param name="periodId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<SlotViewModel> GetPagedSlotByPeriodId(Guid periodId, int page, int pageSize);

        /// <summary>
        /// Update slot with all related.
        /// </summary>
        /// <param name="periodId"></param>
        /// <param name="slotId"></param>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        SlotViewModel UpdateSlot(Guid periodId, Guid slotId, CreateSlotViewModel request, Guid userId);

        /// <summary>
        /// Delete slot.
        /// </summary>
        /// <param name="periodId"></param>
        /// <param name="slotId"></param>
        void DeleteSlot(Guid periodId, Guid slotId);
    }
}