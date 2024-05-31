using Plexus.Entity.DTO;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider
{
    public interface IInstructorTypeProvider
    {
        /// <summary>
        /// Create new instructor type.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        InstructorTypeDTO Create(CreateInstructorTypeDTO request, string requester);

        /// <summary>
        /// Search instructor type by given parameters as paged.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<InstructorTypeDTO> Search(SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25);

        /// <summary>
        /// Search instructor type by given parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<InstructorTypeDTO> Search(SearchCriteriaViewModel parameters);

        /// <summary>
        /// Get instructor type by given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        InstructorTypeDTO GetById(Guid id);

        /// <summary>
        /// Get instructor types by given ids.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        IEnumerable<InstructorTypeDTO> GetById(IEnumerable<Guid> ids);

        /// <summary>
        /// Update instructor type.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        InstructorTypeDTO Update(InstructorTypeDTO request, string requester);

        /// <summary>
        /// Delete instructor type by given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);
    }
}