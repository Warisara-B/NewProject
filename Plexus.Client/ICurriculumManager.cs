using Plexus.Client.ViewModel.Academic.Curriculum;
using Plexus.Client.ViewModel.DropDown;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Academic;
using Plexus.Entity.DTO.Academic.Curriculum;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface ICurriculumManager
    {
        /// <summary>
        /// Create new curriculum.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        CurriculumViewModel Create(CreateCurriculumViewModel request, Guid userId);

        /// <summary>
        /// Get curriculum by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        CurriculumViewModel GetById(Guid id);

        /// <summary>
        /// Search curriculum according to given parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<CurriculumViewModel> Search(SearchCriteriaViewModel parameters, int page, int pageSize);

        /// <summary>
        /// Search curriculum as drop down list by given parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<CurriculumDropDownViewModel> GetDropDownList(SearchCriteriaViewModel parameters);

        /// <summary>
        /// Update curriculum information.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        CurriculumViewModel Update(CurriculumViewModel request, Guid userId);

        /// <summary>
        /// Delete curriculum by id.
        /// </summary>
        /// <param name="id"></param>
        void Delete(Guid id);

        /// <summary>
        /// Map curriculum dto with academic level, faculty and department to view model.
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="academicLevel"></param>
        /// <param name="faculty"></param>
        /// <param name="department"></param>
        /// <returns></returns>
        CurriculumViewModel MapDTOToViewModel(CurriculumDTO dto, AcademicLevelDTO? academicLevel = null, 
                                              FacultyDTO? faculty = null, DepartmentDTO? department = null);
    }
}