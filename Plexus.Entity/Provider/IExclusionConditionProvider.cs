using Plexus.Entity.DTO.Academic.Section;

namespace Plexus.Entity.Provider
{
    public interface IExclusionConditionProvider
    {
        /// <summary>
        /// Create new exclusion condition.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        ExclusionConditionDTO Create(CreateExclusionConditionDTO request, string requester);

        /// <summary>
        /// Get exclusion conditions by section id.
        /// </summary>
        /// <param name="sectionId"></param>
        /// <returns></returns>
        IEnumerable<ExclusionConditionDTO> GetBySectionId(Guid sectionId);

        /// <summary>
        /// Get exclusion condition by given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ExclusionConditionDTO GetById(Guid id);

        /// <summary>
        /// Get exclusion condition by given ids.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        IEnumerable<ExclusionConditionDTO> GetById(IEnumerable<Guid> ids);

        /// <summary>
        /// Update exclusion condition.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        ExclusionConditionDTO Update(ExclusionConditionDTO request, string requester);

        /// <summary>
        /// Delete exclusion condition by given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);
    }
}