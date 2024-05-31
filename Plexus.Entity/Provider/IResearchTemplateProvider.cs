using Plexus.Entity.DTO.Research;
using Plexus.Entity.DTO.SearchFilter;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider
{
    public interface IResearchTemplateProvider
    {
        /// <summary>
        /// Create research template
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        ResearchTemplateDTO Create(CreateResearchTemplateDTO request, string requester);

        /// <summary>
        /// Search research template as paging
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<ResearchTemplateDTO> Search(SearchResearchTemplateCriteriaDTO? parameters, int page = 1, int pageSize = 25);

        /// <summary>
        /// Get research template by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ResearchTemplateDTO GetById(Guid id);

        /// <summary>
        /// Update research template
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        ResearchTemplateDTO Update(ResearchTemplateDTO request, string requester);

        /// <summary>
        /// Delete research template by id.
        /// </summary>
        /// <param name="id"></param>
        void Delete(Guid id);

        /// <summary>
        /// Delete research template sequence by given id.
        /// </summary>
        /// <param name="sequenceId"></param>
        void DeleteSequence(Guid sequenceId);
    }
}