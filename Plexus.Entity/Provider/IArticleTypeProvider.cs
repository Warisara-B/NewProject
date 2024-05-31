using Plexus.Entity.DTO.Research;
using Plexus.Entity.DTO.SearchFilter;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider
{
    public interface IArticleTypeProvider
    {
        /// <summary>
        /// Create article type.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        ArticleTypeDTO Create(CreateArticleTypeDTO request, string requester);

        /// <summary>
        /// Search academic program by given parameters as paged.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<ArticleTypeDTO> Search(SearchArticleTypeCriteriaDTO parameters, int page = 1, int pageSize = 25);

        /// <summary>
        /// Search academic program by given parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<ArticleTypeDTO> Search(SearchArticleTypeCriteriaDTO parameters);

        /// <summary>
        /// Get article type by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ArticleTypeDTO GetById(Guid id);

        /// <summary>
        /// Update article type.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        ArticleTypeDTO Update(ArticleTypeDTO request, string requester);

        /// <summary>
        /// Delete article type by id.
        /// </summary>
        /// <param name="id"></param>
        void Delete(Guid id);
    }
}