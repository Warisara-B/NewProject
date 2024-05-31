using Microsoft.AspNetCore.Http;
using Plexus.Client.ViewModel.Academic;
using Plexus.Client.ViewModel.DropDown;
using Plexus.Database.Model.Academic.Curriculum;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Academic;
using Plexus.Entity.Exception;
using Plexus.Entity.Provider;
using Plexus.Entity.Provider.src;
using Plexus.Entity.Provider.src.Academic;
using Plexus.Integration;
using Plexus.Integration.src;
using Plexus.Utility.ViewModel;
using ServiceStack;

namespace Plexus.Client.src.Academic
{
    public class GradeMaintenanceManager : IGradeMaintenanceManager
    {
        private readonly IGradeMaintenanceProvider _gradeMaintenanceProvider;
        private readonly IBlobStorageProvider _blobStorageProvider;
        private readonly IStudentProvider _studentProvider;

        public GradeMaintenanceManager(IGradeMaintenanceProvider gradeMaintenanceProvider, IBlobStorageProvider blobStorageProvider, IStudentProvider studentProvider)
        {
            _blobStorageProvider = blobStorageProvider;
            _gradeMaintenanceProvider = gradeMaintenanceProvider;
            _studentProvider = studentProvider;
        }

        public GradeMaintenanceViewModel GetById(Guid gradeId)
        {
            var grade = _gradeMaintenanceProvider.GetById(gradeId);

            var response = MapDTOToViewModel(grade);

            return response;
        }

        public GradeMaintenanceViewModel Create(CreateGradeMaintenanceViewModel request, Guid userId)
        {
            var file = request.File;
            var fileName = string.Empty;
            if (file != null && file.Length > 0)
            {
                var fileExtension = Path.GetExtension(file.FileName);
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
                fileName = $"GradeMainten/{fileNameWithoutExtension}_{DateTime.UtcNow:yyyyMMddHHmmss}{fileExtension}";
                _blobStorageProvider.UploadFileAsync(fileName, file.OpenReadStream());
            }

            var dto = new CreateGradeMaintenanceDTO
            {
                StudentCode = request.StudentCode,
                CoursesCode = request.CoursesCode,
                Grade = request.Grade,
                Remark = request.Remark,
                PathFile = fileName,
                IsActive = request.IsActive,
            };

            var grade = _gradeMaintenanceProvider.Create(dto, userId.ToString());
            var response = MapDTOToViewModel(grade);

            return response;
        }

        public PagedViewModel<GradeMaintenanceViewModel> Search(SearchGradeMaintenanceViewModel parameters, int page, int pageSize)
        {
            var pagedGrade = _gradeMaintenanceProvider.Search(parameters, page, pageSize);

            var response = new PagedViewModel<GradeMaintenanceViewModel>
            {
                Page = pagedGrade.Page,
                Items = (from grade in pagedGrade.Items
                         select MapDTOToViewModel(grade))
                        .ToList()
            };

            return response;
        }

        public PagedViewModel<CourseGradeMaintenViewModel> SearchCouse(SearchGradeMaintenanceViewModel parameters, int page, int pageSize)
        {
            var pagedGrade = _gradeMaintenanceProvider.SearchCouse(parameters, page, pageSize);

            var response = new PagedViewModel<CourseGradeMaintenViewModel>
            {
                Page = pagedGrade.Page,
                Items = (from grade in pagedGrade.Items
                         select MapCourseDTOToViewModel(grade))
                        .ToList()
            };
            return response;
        }

        public static CourseGradeMaintenViewModel MapCourseDTOToViewModel(CourseGradeMaintenanceDTO dto)
        {
            return new CourseGradeMaintenViewModel
            {
                StudentCode = dto.StudentCode,
                StudentName = dto.StudentName,
                AcademicLevelFormalName = dto.AcademicLevelFormalName,
                DepartmentName = dto.DepartmentName,
                CurriculumVersionName = dto.CurriculumVersionName,
                GPAX = dto.GPAX,
                StudyPlans = dto.StudyPlans,
                Course = dto.Course,
                Section = dto.Section,
                Instructor = dto.Instructor,
                Grade = dto.Grade,
                Status = dto.Status
            };
        }

        public GradeMaintenanceViewModel Update(GradeMaintenanceViewModel request, Guid userId)
        {
            var grade = _gradeMaintenanceProvider.GetById(request.Id);

            var duplicateGradeName = _gradeMaintenanceProvider.GetByName(request.Grade);

            if (duplicateGradeName.Any(x => x.Id != request.Id))
            {
                throw new GradeException.LetterDuplicate(request.Grade);
            }

            grade.Grade = request.Grade;
            grade.Remark = request.Remark;
            grade.PathFile = request.PathFile;

            var updatedGrade = _gradeMaintenanceProvider.Update(grade, userId.ToString());

            var response = MapDTOToViewModel(updatedGrade);

            return response;
        }

        public static GradeMaintenanceViewModel MapDTOToViewModel(GradeMaintenanceDTO dto)
        {
            return new GradeMaintenanceViewModel
            {
                Id = dto.Id,
                Grade = dto.Grade,
                Remark = dto.Remark,
                PathFile = dto.PathFile,
                IsActive = dto.IsActive,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt
            };
        }

        public void Delete(Guid id)
        {
            _gradeMaintenanceProvider.Delete(id);
        }
    }
}
