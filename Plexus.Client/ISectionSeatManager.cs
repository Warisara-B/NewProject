using Plexus.Client.ViewModel.Academic.Section;

namespace Plexus.Client
{
    public interface ISectionSeatManager
    {
        /// <summary>
        /// Get section seats by section id.
        /// </summary>
        /// <param name="sectionIds"></param>
        /// <returns></returns>
        IEnumerable<SectionSeatViewModel> GetBySectionId(IEnumerable<Guid> sectionIds);
    }
}