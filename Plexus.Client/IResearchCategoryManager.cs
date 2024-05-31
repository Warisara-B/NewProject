using Plexus.Client.ViewModel.Research;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface IResearchCategoryManager
    {
        /// <summary>
        /// Create article type.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        ResearchCategoryViewModel Create(CreateResearchCategoryViewModel request, Guid userId);

        /// <summary>
        /// Search research category by given parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<ResearchCategoryViewModel> Search(SearchResearchCategoryCriteriaViewModel parameters, int page = 1, int pageSize = 25);

        /// <summary>
        /// Get article type by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ResearchCategoryViewModel GetById(Guid id);

        /// <summary>
        /// Update article type.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        ResearchCategoryViewModel Update(Guid id, CreateResearchCategoryViewModel request, Guid userId);

        /// <summary>
        /// Delete article type.
        /// </summary>
        /// <param name="id"></param>
        void Delete(Guid id);
    }
}