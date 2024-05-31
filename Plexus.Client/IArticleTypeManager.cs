using Plexus.Client.ViewModel.DropDown;
using Plexus.Client.ViewModel.Research;
using Plexus.Entity.DTO.SearchFilter;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface IArticleTypeManager
    {
        /// <summary>
        /// Create article type.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        ArticleTypeViewModel Create(CreateArticleTypeViewModel request, Guid userId);

        /// <summary>
        /// Search article type by given parameters as paged.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<ArticleTypeViewModel> Search(SearchArticleTypeCriteriaViewModel parameters, int page = 1, int pageSize = 25);

        /// <summary>
        /// Search article type by given parameters as drop down list.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<BaseDropDownViewModel> GetDropDownList(SearchArticleTypeCriteriaViewModel parameters);

        /// <summary>
        /// Get article type by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ArticleTypeViewModel GetById(Guid id);

        /// <summary>
        /// Update article type.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        ArticleTypeViewModel Update(Guid id, CreateArticleTypeViewModel request, Guid userId);

        /// <summary>
        /// Delete article type.
        /// </summary>
        /// <param name="id"></param>
        void Delete(Guid id);
    }
}