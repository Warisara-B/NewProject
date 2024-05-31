using Plexus.Client.ViewModel.Research;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface IResearchTemplateManager
    {
        /// <summary>
        /// Create research template record.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        ResearchTemplateViewModel Create(UpsertResearchTemplateViewModel request, Guid userId);

        /// <summary>
        /// Search research template according to given criteria.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<ResearchTemplateListViewModel> Search(SearchResearchTemplateCriteriaViewModel parameters, int page = 1, int pageSize = 25);

        /// <summary>
        /// Get by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ResearchTemplateViewModel GetById(Guid id);

        /// <summary>
        /// Update research template by id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        ResearchTemplateViewModel Update(Guid id, UpsertResearchTemplateViewModel request, Guid userId);

        /// <summary>
        /// Delete research template by id.
        /// </summary>
        /// <param name="id"></param>
        void Delete(Guid id);
    }
}