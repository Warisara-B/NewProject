using Plexus.Client.ViewModel.DropDown;
using Plexus.Client.ViewModel.Payment;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Entity.DTO;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface ITermFeePackageManager
    {
        /// <summary>
        /// Create new term fee package.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        TermFeePackageViewModel Create(CreateTermFeePackageViewModel request);

        /// <summary>
        /// Get term fee package by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TermFeePackageViewModel GetById(Guid id);

        /// <summary>
        /// Get all term fee package as drop down by givien parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<BaseDropDownViewModel> GetDropDownList(SearchTermFeePackageCriteriaViewModel parameters);

        /// <summary>
        /// Search term fee package according to given parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<TermFeePackageViewModel> Search(SearchTermFeePackageCriteriaViewModel parameters);

        /// <summary>
        /// Search term fee package according to given parameters as paged.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<TermFeePackageViewModel> Search(SearchTermFeePackageCriteriaViewModel parameters, int page, int pageSize);

        /// <summary>
        /// Update term fee package information.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        TermFeePackageViewModel Update(Guid id, CreateTermFeePackageViewModel request);

        /// <summary>
        /// Delete term fee package by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);
    }
}