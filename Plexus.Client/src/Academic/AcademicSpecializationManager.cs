using Plexus.Client.ViewModel;
using Plexus.Client.ViewModel.Academic.Curriculum;
using Plexus.Client.ViewModel.DropDown;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Academic;
using Plexus.Entity.DTO.Academic.Curriculum;
using Plexus.Entity.DTO.SearchFilter;
using Plexus.Entity.Exception;
using Plexus.Entity.Provider;
using Plexus.Utility.ViewModel;

namespace Plexus.Client.src.Academic
{
    public class AcademicSpecializationManager : IAcademicSpecializationManager
    {
        private readonly IAcademicSpecializationProvider _academicSpecializationProvider;
        private readonly ICourseProvider _courseProvider;
        private readonly IGradeProvider _gradeProvider;

        public AcademicSpecializationManager(IAcademicSpecializationProvider academicSpecializationProvider,
                                             ICourseProvider courseProvider,
                                             IGradeProvider gradeProvider)
        {
            _academicSpecializationProvider = academicSpecializationProvider;
            _courseProvider = courseProvider;
            _gradeProvider = gradeProvider;
        }

        public AcademicSpecializationViewModel Create(CreateAcademicSpecializationViewModel request, Guid userId)
        {
            if (request.ParentAcademicSpecializationId.HasValue)
            {
                var parent = _academicSpecializationProvider.GetById(request.ParentAcademicSpecializationId.Value);
            }

            var dto = new CreateAcademicSpecializationDTO
            {
                ParentAcademicSpecializationId = request.ParentAcademicSpecializationId,
                Name = request.Name,
                Code = request.Code,
                Abbreviation = request.Abbreviation,
                Type = request.Type,
                Description = request.Description,
                RequiredCredit = request.RequiredCredit,
                Remark = request.Remark,
                Level = request.Level,
                Localizations = MapLocalizationViewModelToDTO(request.Localizations).ToList()
            };

            var academicSpecialization = _academicSpecializationProvider.Create(dto, userId.ToString());

            var response = MapCourseGroupDTOToViewModel(academicSpecialization);

            return response;
        }

        public PagedViewModel<AcademicSpecializationViewModel> Search(SearchAcademicSpecializationCriteriaViewModel parameters, int page, int pageSize)
        {
            var dto = new SearchAcademicSpecializationCriteriaDTO
            {
                Keyword = parameters.Keyword,
                Code = parameters.Code,
                Name = parameters.Name,
                Level = parameters.Level,
                SortBy = parameters.SortBy,
                OrderBy = parameters.OrderBy
            };

            var pagedAcademicSpecializations = _academicSpecializationProvider.Search(dto, page, pageSize);

            var academicSpecializations = _academicSpecializationProvider.GetAll()
                                                                         .ToList();

            var courseIds = academicSpecializations.SelectMany(x => x.Courses)
                                                   .Select(x => x.CourseId)
                                                   .Distinct()
                                                   .ToList();

            var courses = !courseIds.Any() ? Enumerable.Empty<CourseDTO>()
                                           : _courseProvider.GetById(courseIds)
                                                            .ToList();

            var grades = !courses.Any() ? Enumerable.Empty<GradeDTO>()
                                        : _gradeProvider.GetAll()
                                                        .ToList();

            var response = new PagedViewModel<AcademicSpecializationViewModel>
            {
                Page = pagedAcademicSpecializations.Page,
                TotalPage = pagedAcademicSpecializations.TotalPage,
                TotalItem = pagedAcademicSpecializations.TotalItem,
                Items = (from academicSpecialization in pagedAcademicSpecializations.Items
                         select MapCourseGroupDTOToViewModel(academicSpecialization, academicSpecializations, courses, grades))
                        .ToList()
            };

            return response;
        }

        public AcademicSpecializationViewModel GetById(Guid id)
        {
            var academicSpecialization = _academicSpecializationProvider.GetById(id);

            var academicSpecializations = _academicSpecializationProvider.GetAll()
                                                                         .ToList();

            var courseIds = academicSpecializations.SelectMany(x => x.Courses)
                                                   .Select(x => x.CourseId)
                                                   .Distinct()
                                                   .ToList();

            var courses = !courseIds.Any() ? Enumerable.Empty<CourseDTO>()
                                           : _courseProvider.GetById(courseIds)
                                                            .ToList();

            var grades = !courses.Any() ? Enumerable.Empty<GradeDTO>()
                                        : _gradeProvider.GetAll()
                                                        .ToList();

            var response = MapCourseGroupDTOToViewModel(academicSpecialization, academicSpecializations, courses, grades);

            return response;
        }

        public AcademicSpecializationViewModel Update(Guid id, CreateAcademicSpecializationViewModel request, Guid userId)
        {
            var academicSpecialization = _academicSpecializationProvider.GetById(id);

            var academicSpecializations = _academicSpecializationProvider.GetAll()
                                                                         .ToList();

            if (request.ParentAcademicSpecializationId.HasValue)
            {
                var parent = academicSpecializations.SingleOrDefault(x => x.Id == request.ParentAcademicSpecializationId.Value);
                if (parent is null)
                {
                    throw new AcademicSpecializationException.NotFound(request.ParentAcademicSpecializationId.Value);
                }
            }

            academicSpecialization.ParentAcademicSpecializationId = request.ParentAcademicSpecializationId;
            academicSpecialization.Name = request.Name;
            academicSpecialization.Abbreviation = request.Abbreviation;
            academicSpecialization.Code = request.Code;
            academicSpecialization.Description = request.Description;
            academicSpecialization.Type = request.Type;
            academicSpecialization.Level = request.Level;
            academicSpecialization.RequiredCredit = request.RequiredCredit;
            academicSpecialization.Remark = request.Remark;
            academicSpecialization.Localizations = MapLocalizationViewModelToDTO(request.Localizations).ToList();

            var updatedAcademicSepecialization = _academicSpecializationProvider.Update(academicSpecialization, userId.ToString());

            var courseIds = academicSpecializations.SelectMany(x => x.Courses)
                                                   .Select(x => x.CourseId)
                                                   .Distinct()
                                                   .ToList();

            var courses = !courseIds.Any() ? Enumerable.Empty<CourseDTO>()
                                           : _courseProvider.GetById(courseIds)
                                                            .ToList();

            var grades = !courses.Any() ? Enumerable.Empty<GradeDTO>()
                                        : _gradeProvider.GetAll()
                                                        .ToList();

            var response = MapCourseGroupDTOToViewModel(updatedAcademicSepecialization, academicSpecializations, courses, grades);

            return response;
        }

        public void Delete(Guid id)
        {
            _academicSpecializationProvider.Delete(id);
        }

        public IEnumerable<SpecializationCourseViewModel> GetCourses(Guid specializationId)
        {
            var currCourses = _academicSpecializationProvider.GetCourses(specializationId);

            if (!currCourses.Any())
            {
                return Enumerable.Empty<SpecializationCourseViewModel>();
            }

            var courseIds = currCourses.Select(x => x.CourseId)
                                   .ToList();

            var courses = _courseProvider.GetById(courseIds)
                                         .ToList();

            var grades = _gradeProvider.GetAll()
                                       .ToList();

            var response = (from currCourse in currCourses
                            let course = courses.SingleOrDefault(x => x.Id == currCourse.CourseId)
                            let grade = !currCourse.RequiredGradeId.HasValue ? null
                                                                             : grades.SingleOrDefault(x => x.Id == currCourse.RequiredGradeId.Value)
                            where course is not null
                            orderby currCourse.IsRequiredCourse descending, course.Code
                            select MapCurriculumCourseViewModel(currCourse, course, grade))
                           .ToList();

            return response;
        }

        public IEnumerable<SpecializationCourseViewModel> UpdateCourses(Guid specializationId, IEnumerable<CreateSpecializationCourseViewModel> requests)
        {
            var courseIds = requests.Select(x => x.CourseId)
                                    .ToList();

            var courses = _courseProvider.GetById(courseIds)
                                         .ToList();

            var grades = _gradeProvider.GetAll()
                                       .ToList();

            var academicSpecialization = _academicSpecializationProvider.GetById(specializationId);

            foreach (var currCourse in requests)
            {
                var matchingCourses = courses.SingleOrDefault(x => x.Id == currCourse.CourseId);
                var matchingGrade = !currCourse.RequiredGradeId.HasValue ? null
                                                                         : grades.SingleOrDefault(x => x.Id == currCourse.RequiredGradeId.Value);

                if (matchingCourses is null)
                {
                    throw new CourseException.NotFound(currCourse.CourseId);
                }

                if (currCourse.RequiredGradeId.HasValue && matchingGrade is null)
                {
                    throw new GradeException.NotFound(currCourse.RequiredGradeId.Value);
                }
            }

            var dtos = (from request in requests
                        select new SpecializationCourseDTO
                        {
                            AcademicSpecializationId = specializationId,
                            CourseId = request.CourseId,
                            IsRequiredCourse = request.IsRequiredCourse,
                            RequiredGradeId = request.RequiredGradeId
                        })
                       .ToList();

            _academicSpecializationProvider.UpdateCourses(specializationId, dtos);

            var response = (from currCourse in dtos
                            let course = courses.Single(x => x.Id == currCourse.CourseId)
                            let grade = !currCourse.RequiredGradeId.HasValue ? null
                                                                             : grades.SingleOrDefault(x => x.Id == currCourse.RequiredGradeId.Value)
                            where course is not null
                            orderby currCourse.IsRequiredCourse descending, course.Code
                            select MapCurriculumCourseViewModel(currCourse, course, grade))
                           .ToList();

            return response;
        }

        public IEnumerable<BaseDropDownViewModel> GetDropdownList(SearchAcademicSpecializationCriteriaViewModel parameters)
        {
            var dto = new SearchAcademicSpecializationCriteriaDTO
            {
                Keyword = parameters.Keyword,
                Code = parameters.Code,
                Name = parameters.Name,
                Level = parameters.Level,
                SortBy = parameters.SortBy,
                OrderBy = parameters.OrderBy
            };

            var academicSpecializations = _academicSpecializationProvider.Search(dto);

            var response = (from academicSpecialization in academicSpecializations
                            select new BaseDropDownViewModel
                            {
                                Id = academicSpecialization.Id.ToString(),
                                Name = academicSpecialization.Name
                            })
                           .ToList();

            return response;
        }

        private static AcademicSpecializationViewModel MapCourseGroupDTOToViewModel(AcademicSpecializationDTO baseAcademicSpecialization,
                                                                                    IEnumerable<AcademicSpecializationDTO>? academicSpecializations = null,
                                                                                    IEnumerable<CourseDTO>? courses = null,
                                                                                    IEnumerable<GradeDTO>? grades = null)
        {
            var response = new AcademicSpecializationViewModel
            {
                Id = baseAcademicSpecialization.Id,
                Code = baseAcademicSpecialization.Code,
                Type = baseAcademicSpecialization.Type,
                Level = baseAcademicSpecialization.Level,
                RequiredCredit = baseAcademicSpecialization.RequiredCredit,
                Remark = baseAcademicSpecialization.Remark,
                ParentAcademicSpecializationId = baseAcademicSpecialization.ParentAcademicSpecializationId,
                Localizations = (from locale in baseAcademicSpecialization.Localizations
                                 orderby locale.Language
                                 select new AcademicSpecializationLocalizationViewModel
                                 {
                                     Language = locale.Language,
                                     Name = locale.Name,
                                     Abbreviation = locale.Abbreviation,
                                     Description = locale.Description
                                 })
                                .ToList(),
                SubGroups = academicSpecializations is null
                                ? Enumerable.Empty<AcademicSpecializationViewModel>()
                                : (from specialization in academicSpecializations
                                   where specialization.ParentAcademicSpecializationId.HasValue
                                          && specialization.ParentAcademicSpecializationId.Value == baseAcademicSpecialization.Id
                                   orderby specialization.Code, specialization.Level, specialization.Name
                                   select MapCourseGroupDTOToViewModel(specialization, academicSpecializations, courses, grades))
                                  .ToList(),
                Courses = baseAcademicSpecialization.Courses is null || courses is null
                                ? Enumerable.Empty<SpecializationCourseViewModel>()
                                : (from curriculumCourse in baseAcademicSpecialization.Courses
                                   let course = courses.SingleOrDefault(x => x.Id == curriculumCourse.CourseId)
                                   let grade = !curriculumCourse.RequiredGradeId.HasValue ? null
                                                                                          : grades?.SingleOrDefault(x => x.Id == curriculumCourse.RequiredGradeId.Value)
                                   where course is not null && curriculumCourse.AcademicSpecializationId == baseAcademicSpecialization.Id
                                   orderby curriculumCourse.IsRequiredCourse descending, course.Code
                                   select MapCurriculumCourseViewModel(curriculumCourse, course, grade))
                                  .ToList(),
                CreatedAt = baseAcademicSpecialization.CreatedAt,
                UpdatedAt = baseAcademicSpecialization.UpdatedAt,
            };

            return response;
        }

        private static SpecializationCourseViewModel MapCurriculumCourseViewModel(SpecializationCourseDTO dto, CourseDTO course, GradeDTO grade)
        {
            var response = new SpecializationCourseViewModel
            {
                CourseId = dto.CourseId,
                CourseCode = course.Code,
                CourseName = (from data in course.Localizations
                              orderby data.Language
                              select new CourseLocalizationDTO
                              {
                                  Language = data.Language,
                                  Name = data.Name
                              }).SingleOrDefault()?.Name,
                Credit = course.Credit,
                IsRequiredCourse = dto.IsRequiredCourse,
                RequiredGradeId = dto.RequiredGradeId,
                RequiredGradeLetter = !dto.RequiredGradeId.HasValue ? null
                                                                    : grade.Letter
            };

            return response;
        }

        private static IEnumerable<AcademicSpecializationLocalizationDTO> MapLocalizationViewModelToDTO(
                    IEnumerable<AcademicSpecializationLocalizationViewModel>? localizations)
        {
            if (localizations is null)
            {
                return Enumerable.Empty<AcademicSpecializationLocalizationDTO>();
            }

            var response = (from locale in localizations
                            select new AcademicSpecializationLocalizationDTO
                            {
                                Language = locale.Language,
                                Name = locale.Name,
                                Abbreviation = locale.Abbreviation,
                                Description = locale.Description
                            })
                           .ToList();

            return response;
        }
    }
}