using Plexus.Database.Enum.Registration;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Registration;
using Plexus.Entity.DTO.SearchFilter;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider
{
    public interface IPeriodAndSlotProvider
    {
        /// <summary>
        /// Create new period.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        PeriodDTO CreatePeriod(CreatePeriodDTO request, string requester);

        /// <summary>
        /// Get all periods.
        /// </summary>
        /// <returns></returns>
        IEnumerable<PeriodDTO> GetAllPeriods();

        /// <summary>
        /// Get period by given id.
        /// </summary>
        /// <param name="periodId"></param>
        /// <returns></returns>
        PeriodDTO GetPeriodById(Guid periodId);

        /// <summary>
        /// Get periods by given term id.
        /// </summary>
        /// <param name="termId"></param>
        /// <returns></returns>
        IEnumerable<PeriodDTO> GetPeriodsByTermId(Guid termId);

        /// <summary>
        /// Update period info.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        PeriodDTO UpdatePeriod(PeriodDTO request, string requester);

        /// <summary>
        /// Delete period by id.
        /// </summary>
        /// <param name="periodId"></param>
        /// <returns></returns>
        void DeletePeriod(Guid periodId);

        /// <summary>
        /// Create new slot.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        SlotDTO CreateSlot(CreateSlotDTO request, string requester);

        /// <summary>
        /// Get slot by id.
        /// </summary>
        /// <param name="slotId"></param>
        /// <returns></returns>
        SlotDTO GetSlotById(Guid slotId);

        /// <summary>
        /// Get slots by ids.
        /// </summary>
        /// <param name="slotIds"></param>
        /// <returns></returns>
        IEnumerable<SlotDTO> GetSlotById(IEnumerable<Guid> slotIds);

        /// <summary>
        /// Get slots by period id.
        /// </summary>
        /// <param name="periodId"></param>
        /// <returns></returns>
        IEnumerable<SlotDTO> GetSlotByPeriodId(Guid periodId);

        /// <summary>
        /// Get paged slot by period id.
        /// </summary>
        /// <param name="periodId"></param>
        /// <returns></returns>
        PagedViewModel<SlotDTO> GetPagedSlotByPeriodId(Guid periodId, int page = 1, int pageSize = 25);

        /// <summary>
        /// Update slot info.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        SlotDTO UpdateSlot(SlotDTO request, string requester);

        /// <summary>
        /// Delete slot by id.
        /// </summary>
        /// <param name="slotId"></param>
        /// <returns></returns>
        void DeleteSlot(Guid slotId);

        /// <summary>
        /// Check stdent period by student id, term id and period type.
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="termId"></param>
        /// <param name="periodType"></param>
        /// <returns></returns>
        bool IsStudentHasAvailablePeriod(Guid studentId, Guid termId, PeriodType periodType);
    }
}