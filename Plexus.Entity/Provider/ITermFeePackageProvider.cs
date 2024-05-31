using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Payment;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider
{
    public interface ITermFeePackageProvider
    {
        /// <summary>
        /// Create new term fee package.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        TermFeePackageDTO Create(CreateTermFeePackageDTO request, string requester);

        /// <summary>
        /// Get term fee packages by given parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<TermFeePackageDTO> Search(SearchCriteriaViewModel? parameters = null);

        /// <summary>
        /// Search term fee packages as paging.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<TermFeePackageDTO> Search(SearchCriteriaViewModel? parameters, int page, int pageSize);

        /// <summary>
        /// Get term fee package by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TermFeePackageDTO GetById(Guid id);

        /// <summary>
        /// Get term fee packages by ids.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        IEnumerable<TermFeePackageDTO> GetById(IEnumerable<Guid> ids);

        /// <summary>
        /// Update term fee package.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        TermFeePackageDTO Update(TermFeePackageDTO request, string requester);

        /// <summary>
        /// Delete term fee package by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);
    }
}