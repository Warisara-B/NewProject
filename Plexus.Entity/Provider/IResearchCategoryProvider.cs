using Plexus.Entity.DTO.Research;
using Plexus.Entity.DTO.SearchFilter;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider
{
    public interface IResearchCategoryProvider
    {
        /// <summary>
        /// Create research category.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        ResearchCategoryDTO Create(CreateResearchCategoryDTO request, string requester);

        /// <summary>
        /// Search research category by given parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<ResearchCategoryDTO> Search(SearchResearchCategoryCriteriaDTO parameters, int page = 1, int pageSize = 25);

        /// <summary>
        /// Get research category by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ResearchCategoryDTO GetById(Guid id);

        /// <summary>
        /// Update research category.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        ResearchCategoryDTO Update(ResearchCategoryDTO request, string requester);

        /// <summary>
        /// Delete research category by id.
        /// </summary>
        /// <param name="id"></param>
        void Delete(Guid id);
    }
}