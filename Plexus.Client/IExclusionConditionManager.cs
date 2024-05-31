using Plexus.Client.ViewModel.Academic.Section;

namespace Plexus.Client
{
    public interface IExclusionConditionManager
    {
        /// <summary>
        /// Create new exclusion condition.
        /// </summary>
        /// <param name="sectionId"></param>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        ExclusionConditionViewModel Create(Guid sectionId, CreateExclusionConditionViewModel request, Guid userId);

        /// <summary>
        /// Get exclusion conditions by section id.
        /// </summary>
        /// <param name="sectionId"></param>
        /// <returns></returns>
        IEnumerable<ExclusionConditionViewModel> GetBySectionId(Guid sectionId);

        /// <summary>
        /// Get exclusion condition by given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ExclusionConditionViewModel GetById(Guid id);

        /// <summary>
        /// Update exclusion condition.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        ExclusionConditionViewModel Update(ExclusionConditionViewModel request, Guid userId);

        /// <summary>
        /// Delete exclusion condition by given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);
    }
}