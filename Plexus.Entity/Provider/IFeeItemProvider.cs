using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Payment;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider
{
    public interface IFeeItemProvider
    {
        /// <summary>
        /// Get fee item by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        FeeItemDTO GetById(Guid id);

        /// <summary>
        /// Get fee item by ids.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        IEnumerable<FeeItemDTO> GetById(IEnumerable<Guid> ids);
    }
}