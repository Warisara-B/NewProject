using Microsoft.AspNetCore.Http;
using Plexus.Client.ViewModel;
using Plexus.Database.Model;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Academic;
using Plexus.Entity.DTO.Academic.Curriculum;
using Plexus.Entity.DTO.Payment;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface IStudentManager
    {
        /// <summary>
        /// Create student info.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        StudentViewModel Create(CreateStudentViewModel request, Guid userId);

        /// <summary>
        /// Search student with given parameters as paging.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<StudentViewModel> Search(SearchCriteriaViewModel parameters, int page, int pageSize);

        /// <summary>
        /// Get student by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        StudentViewModel GetById(Guid id);

        /// <summary>
        /// Get student by code.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        StudentViewModel GetByCode(string code);

        /// <summary>
        /// Get student by file.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        IEnumerable<StudentViewModel> GetByFile(IFormFile file);

        /// <summary>
        /// Update student info.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        StudentViewModel Update(StudentViewModel request, Guid userId);

        /// <summary>
        /// Delete student by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);

        /// <summary>
        /// Upload student card image.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cardImage"></param>
        /// <returns></returns>
        Task UploadCardImageAsync(Guid id, IFormFile cardImage);

        /// <summary>
        /// Map student dto to model.
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="academicLevel"></param>
        /// <param name="faculty"></param>
        /// <param name="department"></param>
        /// <param name="version"></param>
        /// <param name="studentFeeType"></param>
        /// <returns></returns>
        StudentViewModel MapDTOToViewModel(StudentDTO dto, AcademicLevelDTO? academicLevel = null, FacultyDTO? faculty = null,
                                           DepartmentDTO? department = null, CurriculumVersionDTO? version = null,
                                           StudentFeeTypeDTO? studentFeeType = null);

        /// <summary>
        /// Get student card by id.
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        StudentCardViewModel GetStudentCard(Guid studentId);

        /// <summary>
        /// Get student's general information by id.
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        StudentGeneralInfoViewModel GetStudentGeneralInfo(Guid studentId);

        /// <summary>
        /// Update student general information by student id.
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        StudentGeneralInfoViewModel UpdateStudentGeneralInfo(Guid studentId, CreateStudentGeneralInfoViewModel request);

        /// <summary>
        /// Get student's contact information by student id.
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        StudentContactViewModel GetStudentContact(Guid studentId);

        /// <summary>
        /// Update student's contact information by student id.
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        StudentContactViewModel UpdateStudentContact(Guid studentId, CreateStudentContactViewModel request);

        /// <summary>
        /// Get student academic information by student id.
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        StudentAcademicInfoViewModel GetStudentAcademicInfo(Guid studentId);

        /// <summary>
        /// Update student academic information by student id.
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        StudentAcademicInfoViewModel UpdateStudentAcademicInfo(Guid studentId, CreateStudentAcademicInfoViewModel request);

        /// <summary>
        /// Get student's curriculum information by student id.
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        StudentCurriculumViewModel GetStudentCurriculumInfo(Guid studentId);

        /// <summary>
        /// Update student's curriculum information by student id.
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        StudentCurriculumViewModel UpdateStudentCurriculumInfo(Guid studentId, CreateStudentCurriculumViewModel request);

        /// <summary>
        /// Get student's curriculum information log by student id.
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        IEnumerable<StudentCurriculumViewModel> GetStudentCurriculumLog(Guid studentId);
    }
}