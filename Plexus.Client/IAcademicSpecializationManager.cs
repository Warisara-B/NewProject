using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Client.ViewModel.Academic.Curriculum;
using Plexus.Client.ViewModel.DropDown;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface IAcademicSpecializationManager
    {
        /// <summary>
        /// Create academic specialization.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        AcademicSpecializationViewModel Create(CreateAcademicSpecializationViewModel request, Guid userId);

        /// <summary>
        /// Search academic specialization according to given parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<AcademicSpecializationViewModel> Search(SearchAcademicSpecializationCriteriaViewModel parameters, int page, int pageSize);

        /// <summary>
        /// Get academic specialization dropdown list by given parameters.
        /// </summary>
        /// <returns></returns>
        IEnumerable<BaseDropDownViewModel> GetDropdownList(SearchAcademicSpecializationCriteriaViewModel parameters);

        /// <summary>
        /// Get academic specialization by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        AcademicSpecializationViewModel GetById(Guid id);

        /// <summary>
        /// Update academic specialization.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        AcademicSpecializationViewModel Update(Guid id, CreateAcademicSpecializationViewModel request, Guid userId);

        /// <summary>
        /// Delete academic specialization by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);

        /// <summary>
        /// Get courses under academic specialization.
        /// </summary>
        /// <param name="specializationId"></param>
        /// <returns></returns>
        IEnumerable<SpecializationCourseViewModel> GetCourses(Guid specializationId);

        /// <summary>
        /// Update courses under academic specialization.
        /// </summary>
        /// <param name="specializationId"></param>
        /// <param name="requests"></param>
        /// <returns></returns>
        IEnumerable<SpecializationCourseViewModel> UpdateCourses(Guid specializationId, IEnumerable<CreateSpecializationCourseViewModel> requests);
    }
}