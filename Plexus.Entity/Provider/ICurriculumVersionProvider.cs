using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Academic.Curriculum;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider
{
    public interface ICurriculumVersionProvider
    {
        /// <summary>
        /// Create curriculum version record.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        CurriculumVersionDTO Create(CreateCurriculumVersionDTO request, string requester);

        /// <summary>
        /// Get all curriculum versions.
        /// </summary>
        /// <returns></returns>
        IEnumerable<CurriculumVersionDTO> GetAll();

        /// <summary>
        /// Get curriculum version by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        CurriculumVersionDTO GetById(Guid id);

        /// <summary>
        /// Get curriculum version by ids
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IEnumerable<CurriculumVersionDTO> GetById(IEnumerable<Guid> ids);

        /// <summary>
        /// Get curriculum versions by curriculum id.
        /// </summary>
        /// <param name="curriculumId"></param>
        /// <returns></returns>
        IEnumerable<CurriculumVersionDTO> GetByCurriculumId(Guid curriculumId);

        /// <summary>
        /// Search curriculum version as paged.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<CurriculumVersionDTO> Search(SearchCurriculumVersionCriteriaDTO parameters, int page, int pageSize);

        /// <summary>
        /// Search curriculum version by given parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<CurriculumVersionDTO> Search(SearchCurriculumVersionCriteriaDTO parameters);

        /// <summary>
        /// Update curriculum version information.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        CurriculumVersionDTO Update(CurriculumVersionDTO request, string requester);

        /// <summary>
        /// Delete curriculum version by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);

        /// <summary>
        /// Set curriculum version course blacklist
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="courseIds"></param>
        /// <returns></returns>
        void UpdateBlackListCourses(Guid versionId, IEnumerable<Guid> courseIds);

        /// <summary>
        /// Get curriculum version course blacklist
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns></returns>
        IEnumerable<CurriculumCourseBlackListDTO> GetBlackListCourses(Guid versionId);

        /// <summary>
        /// Set curriculum version academic specialization.
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="specializationIds"></param>
        /// <returns></returns>
        void UpdateAcademicSpecialization(Guid versionId, IEnumerable<Guid> specializationIds);

        /// <summary>
        /// Get curriculum version academic specialization.
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns></returns>
        IEnumerable<CurriculumVersionSpecializationDTO> GetAcademicSpecializations(Guid versionId);

        /// <summary>
        /// Get curriculum version corequisites.
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns></returns>
        IEnumerable<CorequisiteDTO> GetCorequisites(Guid versionId);

        /// <summary>
        /// Set curriculum version corequisites.
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="corequisites"></param>
        /// <returns></returns>
        void UpdateCorequisite(Guid versionId, IEnumerable<CreateCorequisiteDTO> corequisites);

        /// <summary>
        /// Get list of courses from curriculum course and specialization courses inside curriculum version
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns></returns>
        IEnumerable<CurriculumVersionCourseDTO> GetCoursesList(Guid versionId);

        /// <summary>
        /// Get curriculum version equivalent courses.
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns></returns>
        IEnumerable<EquivalentCourseDTO> GetEquivalentCourses(Guid versionId);

        /// <summary>
        /// Set curriculum version equivalent courses.
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="equivalences"></param>
        /// <returns></returns>
        void UpdateEquivalentCourses(Guid versionId, IEnumerable<CreateEquivalentCourseDTO> equivalences);
    }
}