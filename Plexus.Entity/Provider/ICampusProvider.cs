using Plexus.Entity.DTO.Facility;

namespace Plexus.Entity.Provider
{
    public interface ICampusProvider
    {
        /// <summary>
        /// Create new campus
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        CampusDTO Create(CreateCampusDTO request, string requester);

        /// <summary>
        /// Get all campus
        /// </summary>
        /// <returns></returns>
        IEnumerable<CampusDTO> GetAll();

        /// <summary>
        /// Get campus by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        CampusDTO GetById(Guid id);

        /// <summary>
        /// Update campus
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        CampusDTO Update(CampusDTO request, string requester);

        /// <summary>
        /// Delete campus
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);
    }
}