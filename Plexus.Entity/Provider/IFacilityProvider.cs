using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Facility;
using Plexus.Entity.DTO.SearchFilter;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider
{
    public interface IFacilityProvider
    {
        /// <summary>
        /// Create new facility.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        FacilityDTO Create(CreateFacilityDTO request, string requester);

        /// <summary>
        /// Search facility by given parameters as paged.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<FacilityDTO> Search(SearchFacilityCriteriaDTO parameters, int page = 1, int pageSize = 25);

        /// <summary>
        /// Search facility by given parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<FacilityDTO> Search(SearchFacilityCriteriaDTO parameters);

        /// <summary>
        /// Get facilities by given ids.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        IEnumerable<FacilityDTO> GetById(IEnumerable<Guid> ids);

        /// <summary>
        /// Get facility by given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        FacilityDTO GetById(Guid id);

        /// <summary>
        /// Update facility.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        FacilityDTO Update(FacilityDTO request, string requester);

        /// <summary>
        /// Delete rate type by given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);
    }
}