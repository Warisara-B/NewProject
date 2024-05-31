using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Academic.Curriculum;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider
{
    public interface IAcademicSpecializationProvider
    {
        /// <summary>
        /// Create academic specialization.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        AcademicSpecializationDTO Create(CreateAcademicSpecializationDTO request, string requester);

        /// <summary>
        /// Search academic specialization with given parameters.
        /// </summary>
        /// <returns></returns>
        IEnumerable<AcademicSpecializationDTO> Search(SearchAcademicSpecializationCriteriaDTO parameters);

        /// <summary>
        /// Search academic specialization with given parameters as paged.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<AcademicSpecializationDTO> Search(SearchAcademicSpecializationCriteriaDTO parameters, int page, int pageSize);

        /// <summary>
        /// Get all academic specializations.
        /// </summary>
        /// <returns></returns>
        IEnumerable<AcademicSpecializationDTO> GetAll();

        /// <summary>
        /// Get academic specialization by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        AcademicSpecializationDTO GetById(Guid id);

        /// <summary>
        /// Get academic specializations by ids.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        IEnumerable<AcademicSpecializationDTO> GetById(IEnumerable<Guid> ids);

        /// <summary>
        /// Update academic specialization.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        AcademicSpecializationDTO Update(AcademicSpecializationDTO request, string requester);

        /// <summary>
        /// Delete academic specialization by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);

        /// <summary>
        /// Get courses by academic specialization id.
        /// </summary>
        /// <param name="specializationId"></param>
        /// <returns></returns>
        IEnumerable<SpecializationCourseDTO> GetCourses(Guid specializationId);

        /// <summary>
        /// Update specialization courses under academic specialization.
        /// </summary>
        /// <param name="specializationId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        void UpdateCourses(Guid specializationId, IEnumerable<SpecializationCourseDTO> request);
    }
}