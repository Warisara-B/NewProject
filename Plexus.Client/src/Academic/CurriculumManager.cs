using Plexus.Client.ViewModel.Academic.Curriculum;
using Plexus.Entity.DTO;
using Plexus.Entity.Provider;
using Plexus.Utility.ViewModel;
using Plexus.Entity.DTO.Academic.Curriculum;
using Plexus.Entity.DTO.Academic;
using Plexus.Entity.Exception;
using Plexus.Client.ViewModel.DropDown;

namespace Plexus.Client.src.Academic
{
    public class CurriculumManager : ICurriculumManager
    {
        private readonly IAcademicLevelProvider _academicLevelProvider;
        private readonly IFacultyProvider _facultyProvider;
        private readonly IDepartmentProvider _departmentProvider;
        private readonly ICurriculumProvider _curriculumProvider;
        private readonly ICurriculumVersionProvider _curriculumVersionProvider;

        public CurriculumManager(IAcademicLevelProvider academicLevelProvider,
                                 IFacultyProvider facultyProvider,
                                 IDepartmentProvider departmentProvider,
                                 ICurriculumProvider curriculumProvider,
                                 ICurriculumVersionProvider curriculumVersionProvider)
        {
            _academicLevelProvider = academicLevelProvider;
            _facultyProvider = facultyProvider;
            _departmentProvider = departmentProvider;
            _curriculumProvider = curriculumProvider;
            _curriculumVersionProvider = curriculumVersionProvider;
        }

        public CurriculumViewModel Create(CreateCurriculumViewModel request, Guid userId)
        {
            var academicLevel = _academicLevelProvider.GetById(request.AcademicLevelId);

            var faculty = _facultyProvider.GetById(request.FacultyId);

            var department = request.DepartmentId.HasValue ? _departmentProvider.GetById(request.DepartmentId.Value)
                                                           : null;

            var dto = new CreateCurriculumDTO
            {
                AcademicLevelId = request.AcademicLevelId,
                FacultyId = request.FacultyId,
                DepartmentId = request.DepartmentId,
                Code = request.Code,
                Name = request.Name,
                FormalName = request.FormalName,
                Abbreviation = request.Abbreviation,
                Description = request.Description,
                IsActive = request.IsActive,
                Localizations = MapLocalizationViewModelToDTO(request.Localizations).ToList()
            };

            var curriculum = _curriculumProvider.Create(dto, userId.ToString());

            var response = MapDTOToViewModel(curriculum, academicLevel, faculty, department);

            return response;
        }

        public CurriculumViewModel GetById(Guid id)
        {
            var curriculum = _curriculumProvider.GetById(id);

            var academicLevel = _academicLevelProvider.GetById(curriculum.AcademicLevelId);

            var faculty = _facultyProvider.GetById(curriculum.FacultyId);
            
            var department = curriculum.DepartmentId.HasValue ? _departmentProvider.GetById(curriculum.DepartmentId.Value)
                                                              : null;

            var response = MapDTOToViewModel(curriculum, academicLevel, faculty, department);

            return response;
        }

        public PagedViewModel<CurriculumViewModel> Search(SearchCriteriaViewModel parameters, int page, int pageSize)
        {
            var pagedCurriculum = _curriculumProvider.Search(parameters, page, pageSize);

            var academicLevels = _academicLevelProvider.Search()
                                                       .ToList();
            
            var faculties = _facultyProvider.GetAll()
                                            .ToList();

            var departments = _departmentProvider.GetAll()
                                                 .ToList();

            var response = new PagedViewModel<CurriculumViewModel>
            {
                Page = pagedCurriculum.Page,
                TotalPage = pagedCurriculum.TotalPage,
                TotalItem = pagedCurriculum.TotalItem,
                Items = (from curriculum in pagedCurriculum.Items
                         let academicLevel = academicLevels.SingleOrDefault(x => x.Id == curriculum.AcademicLevelId)
                         let faculty = faculties.SingleOrDefault(x => x.Id == curriculum.FacultyId)
                         let department = curriculum.DepartmentId.HasValue ? departments.SingleOrDefault(x => x.Id == curriculum.DepartmentId.Value)
                                                                           : null
                         select MapDTOToViewModel(curriculum, academicLevel, faculty, department))
                        .ToList()
            };

            return response;
        }

        public IEnumerable<CurriculumDropDownViewModel> GetDropDownList(SearchCriteriaViewModel parameters)
        {
            var curriculums = _curriculumProvider.Search(parameters);
            
            var response = (from curriculum in curriculums
                            select MapDTOToDropDown(curriculum))
                           .ToList();
            
            return response;
        }

        public CurriculumViewModel Update(CurriculumViewModel request, Guid userId)
        {
            var curriculums = _curriculumProvider.GetAll()
                                                 .ToList();

            var curriculum = curriculums.SingleOrDefault(x => x.Id == request.Id);

            if (curriculum is null)
            {
                throw new CurriculumException.NotFound(request.Id);
            }

            var academicLevel = _academicLevelProvider.GetById(request.AcademicLevelId);

            var faculty = _facultyProvider.GetById(request.FacultyId);
            
            var department = request.DepartmentId.HasValue ? _departmentProvider.GetById(request.DepartmentId.Value)
                                                           : null;

            curriculum.AcademicLevelId = request.AcademicLevelId;
            curriculum.FacultyId = request.FacultyId;
            curriculum.DepartmentId = request.DepartmentId;
            curriculum.Code = request.Code;
            curriculum.Name = request.Name;
            curriculum.FormalName = request.FormalName;
            curriculum.Abbreviation = request.Abbreviation;
            curriculum.Description = request.Description;
            curriculum.IsActive = request.IsActive;
            curriculum.Localizations = MapLocalizationViewModelToDTO(request.Localizations).ToList();

            var updatedCurriculum = _curriculumProvider.Update(curriculum, userId.ToString());

            var response = MapDTOToViewModel(updatedCurriculum, academicLevel, faculty, department);

            return response;
        }

        public void Delete(Guid id)
        {
            _curriculumProvider.Delete(id);
        }

        public CurriculumViewModel MapDTOToViewModel(CurriculumDTO dto, AcademicLevelDTO? academicLevel = null, 
                                                     FacultyDTO? faculty = null, DepartmentDTO? department = null)
        {
            var response = new CurriculumViewModel
            {
                Id = dto.Id,
                AcademicLevelId = dto.AcademicLevelId,
                AcademicLevelName = academicLevel?.Name,
                FacultyId = dto.FacultyId,
                FacultyName = faculty?.Name,
                DepartmentId = dto.DepartmentId,
                DepartmentName = department?.Name,
                Code = dto.Code,
                Name = dto.Name,
                FormalName = dto.FormalName,
                Abbreviation = dto.Abbreviation,
                Description = dto.Abbreviation,
                IsActive = dto.IsActive,
                Localizations = (from locale in dto.Localizations
                                orderby locale.Language
                                select new CurriculumLocalizationViewModel
                                {
                                    Language = locale.Language,
                                    Name = locale.Name,
                                    FormalName = locale.FormalName,
                                    Abbreviation = locale.Abbreviation
                                })
                                .ToList(),
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt
            };

            return response;
        }

        private static IEnumerable<CurriculumLocalizationDTO> MapLocalizationViewModelToDTO(
            IEnumerable<CurriculumLocalizationViewModel>? localizations)
        {
            if (localizations is null)
            {
                return Enumerable.Empty<CurriculumLocalizationDTO>();
            }

            var response = (from locale in localizations
                            select new CurriculumLocalizationDTO
                            {
                                Language = locale.Language,
                                Name = locale.Name,
                                FormalName = locale.FormalName,
                                Abbreviation = locale.Abbreviation
                            })
                            .ToList();

            return response;
        }

        private CurriculumDropDownViewModel MapDTOToDropDown(CurriculumDTO dto)
        {
            var response = new CurriculumDropDownViewModel
            {
                Id = dto.Id.ToString(),
                Name = dto.Name,
                Code = dto.Code,
                AcademicLevelId = dto.AcademicLevelId,
                FacultyId = dto.FacultyId,
                DepartmentId = dto.DepartmentId
            };

            return response;
        }
    }
}