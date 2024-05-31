using Plexus.Client.ViewModel.Academic;
using Plexus.Entity.DTO;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface IWithdrawalManager
    {
        /// <summary>
        /// Create withdrawal requests
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        void Create(CreateWithdrawalRequestViewModel request, Guid userId);

        /// <summary>
        /// Get withdrawal request by id
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        WithdrawalRequestViewModel GetById(Guid requestId);

        /// <summary>
        /// Search withdrawal request
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<WithdrawalRequestViewModel> Search(SearchCriteriaViewModel parameter, int page, int pageSize);

        /// <summary>
        /// Update withdrawal status
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        void UpdateWithdrawalStatus(UpdateWithdrawalStatusViewModel request, Guid userId);
    }
}