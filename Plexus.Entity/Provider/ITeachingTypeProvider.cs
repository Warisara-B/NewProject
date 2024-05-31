using Plexus.Entity.DTO.Academic;

namespace Plexus.Entity.Provider
{
    public interface ITeachingTypeProvider
    {
        /// <summary>
        /// Create new teaching type.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        TeachingTypeDTO Create(CreateTeachingTypeDTO request, string requester);

        /// <summary>
        /// Get teaching type by given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TeachingTypeDTO GetById(Guid id);

        /// <summary>
        /// Update teaching type.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        TeachingTypeDTO Update(TeachingTypeDTO request, string requester);

        /// <summary>
        /// Delete teaching type by given id.
        /// </summary>
        /// <param name="id"></param>
        void Delete(Guid id);
    }
}