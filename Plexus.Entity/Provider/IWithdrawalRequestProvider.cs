using Plexus.Database.Enum.Academic;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Academic;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider
{
    public interface IWithdrawalRequestProvider
    {
        /// <summary>
        /// Create withdrawal requests
        /// </summary>
        /// <param name="studyCourseIds"></param>
        /// <param name="remark"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        IEnumerable<WithdrawalRequestDTO> Create(IEnumerable<Guid> studyCourseIds, string remark, string requester);

        /// <summary>
        /// Get withdrawal request by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        WithdrawalRequestDTO GetById(Guid id);

        /// <summary>
        /// Get withdrawal request by ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        IEnumerable<WithdrawalRequestDTO> GetById(IEnumerable<Guid> ids);

        /// <summary>
        /// Get list of pending request by studyCourse id
        /// </summary>
        /// <param name="studyCourseIds"></param>
        /// <returns></returns>
        IEnumerable<WithdrawalRequestDTO> GetPendingRequestByStudyCourseId(IEnumerable<Guid> studyCourseIds);

        /// <summary>
        /// Search withdrawal request by given parameters and return as paged
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<WithdrawalRequestDTO> Search(SearchCriteriaViewModel? parameter, int page, int pageSize);

        /// <summary>
        /// Search withdrawal request by given parameters and return as list
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        IEnumerable<WithdrawalRequestDTO> Search(SearchCriteriaViewModel? parameter);

        /// <summary>
        /// Update withdrawal status
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="status"></param>
        /// <param name="remark"></param>
        /// <param name="requester"></param>
        void UpdateWithdrawalStatus(IEnumerable<Guid> ids, WithdrawalStatus status, string remark, string requester);
    }
}