using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Client.ViewModel.Academic.Curriculum;
using Plexus.Client.ViewModel.DropDown;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface ICurriculumVersionManager
    {
        /// <summary>
        /// Create new curriculum version.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        CurriculumVersionViewModel Create(CreateCurriculumVersionViewModel request, Guid userId);

        /// <summary>
        /// Get curriculum version by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        CurriculumVersionViewModel GetById(Guid id);

        /// <summary>
        /// Search curriculum version according to given parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<CurriculumVersionViewModel> Search(SearchCurriculumVersionCriteriaViewModel parameters, int page, int pageSize);

        /// <summary>
        /// Search curriculum version according to given parameters as drop down.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IEnumerable<CurriculumVersionDropDownViewModel> GetDropDownList(SearchCurriculumVersionCriteriaViewModel parameters);

        /// <summary>
        /// Update curriculum version information.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        CurriculumVersionViewModel Update(Guid id, CreateCurriculumVersionViewModel request, Guid userId);

        /// <summary>
        /// Delete curriculum version by id.
        /// </summary>
        /// <param name="id"></param>
        void Delete(Guid id);

        /// <summary>
        /// Copy curriculum version with given base version id
        /// </summary>
        /// <param name="baseVersionId"></param>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        CurriculumVersionViewModel Copy(Guid baseVersionId, CopyCurriculumVersionViewModel request, Guid userId);

        /// <summary>
        /// Get curriculum version blacklist courses
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns></returns>
        IEnumerable<CurriculumCourseBlackListViewModel> GetBlackListCourses(Guid versionId);

        /// <summary>
        /// Set curriculum version blacklist courses
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="blackListCourseIds"></param>
        /// <returns></returns>
        IEnumerable<CurriculumCourseBlackListViewModel> UpdateBlackListCourses(Guid versionId, IEnumerable<Guid> blackListCourseIds);

        /// <summary>
        /// Get curriculum version academic specializations.
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns></returns>
        IEnumerable<CurriculumVersionSpecializationViewModel> GetAcademicSpecializations(Guid versionId);

        /// <summary>
        /// Set curriculum version academic specializations.
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="specializationIds"></param>
        /// <returns></returns>
        IEnumerable<CurriculumVersionSpecializationViewModel> UpdateAcademicSpecializations(Guid versionId, IEnumerable<Guid> specializationIds);

        /// <summary>
        /// Get curriculum version corequisites.
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns></returns>
        IEnumerable<CorequisiteViewModel> GetCorequisites(Guid versionId);

        /// <summary>
        /// Set curriculum version corequisites.
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="corequisites"></param>
        /// <returns></returns>
        IEnumerable<CorequisiteViewModel> UpdateCorequisites(Guid versionId, IEnumerable<CreateCorequisiteViewModel> corequisites);

        /// <summary>
        /// Get course list inside curriculum as dropdown
        /// </summary>
        /// <param name="curriculumVersionId"></param>
        /// <returns></returns>
        IEnumerable<CourseDropDownViewModel> GetCurriculumVersionCourseDropdownLists(Guid curriculumVersionId);

        /// <summary>
        /// Get curriculum version equivalent courses.
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns></returns>
        IEnumerable<EquivalentCourseViewModel> GetEquivalentCourses(Guid versionId);

        /// <summary>
        /// Set curriculum version equivalent courses.
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="equivalences"></param>
        /// <returns></returns>
        IEnumerable<EquivalentCourseViewModel> UpdateEquivalentCourses(Guid versionId, IEnumerable<CreateEquivalentCourseViewModel> equivalences);
    }
}