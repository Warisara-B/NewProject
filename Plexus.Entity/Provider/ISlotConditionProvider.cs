using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Registration;
using Plexus.Entity.DTO.SearchFilter;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider
{
    public interface ISlotConditionProvider
    {
        /// <summary>
        /// Create new slot condition.
        /// </summary>
        /// <param name="slotId"></param>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        SlotConditionDTO Create(Guid slotId, CreateSlotConditionDTO request, string requester);

        /// <summary>
        /// Search slot condition by given parameters as paged.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<SlotConditionDTO> Search(SearchSlotConditionCriteriaDTO parameters, int page = 1, int pageSize = 25);

        /// <summary>
        /// Get slot conditions by slot id.
        /// </summary>
        /// <param name="slotId"></param>
        /// <returns></returns>
        IEnumerable<SlotConditionDTO> GetBySlotId(Guid slotId);

        /// <summary>
        /// Get slot condition by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        SlotConditionDTO GetById(Guid id);

        /// <summary>
        /// Update slot condition info.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        SlotConditionDTO Update(SlotConditionDTO request, string requester);

        /// <summary>
        /// Delete slot condition by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);
    }
}