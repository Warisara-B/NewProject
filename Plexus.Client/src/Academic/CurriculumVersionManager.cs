using Plexus.Client.ViewModel.Academic.Curriculum;
using Plexus.Entity.DTO;
using Plexus.Utility.ViewModel;
using Plexus.Entity.Provider;
using Plexus.Entity.DTO.Academic.Curriculum;
using Plexus.Entity.Exception;
using Plexus.Entity.DTO.Academic;
using Plexus.Client.ViewModel.DropDown;
using Plexus.Client.ViewModel.SearchFilter;

namespace Plexus.Client.src.Academic
{
    public class CurriculumVersionManager : ICurriculumVersionManager
    {
        private readonly IAcademicLevelProvider _academicLevelProvider;
        private readonly IFacultyProvider _facultyProvider;
        private readonly IDepartmentProvider _departmentProvider;
        private readonly IAcademicProgramProvider _academicProgramProvider;
        private readonly ICurriculumProvider _curriculumProvider;
        private readonly ICurriculumVersionProvider _curriculumVersionProvider;
        private readonly ICurriculumCourseGroupProvider _curriculumCourseGroupProvider;
        private readonly ICourseProvider _courseProvider;
        private readonly IAcademicSpecializationProvider _academicSpecializationProvider;

        public CurriculumVersionManager(ICurriculumProvider curriculumProvider,
                                        ICurriculumVersionProvider curriculumVersionProvider,
                                        ICurriculumCourseGroupProvider curriculumCourseGroupProvider,
                                        ICourseProvider courseProvider,
                                        IAcademicSpecializationProvider academicSpecializationProvider,
                                        IAcademicLevelProvider academicLevelProvider,
                                        IFacultyProvider facultyProvider,
                                        IDepartmentProvider departmentProvider,
                                        IAcademicProgramProvider academicProgramProvider)
        {
            _curriculumProvider = curriculumProvider;
            _curriculumVersionProvider = curriculumVersionProvider;
            _curriculumCourseGroupProvider = curriculumCourseGroupProvider;
            _courseProvider = courseProvider;
            _academicSpecializationProvider = academicSpecializationProvider;
            _academicLevelProvider = academicLevelProvider;
            _facultyProvider = facultyProvider;
            _departmentProvider = departmentProvider;
            _academicProgramProvider = academicProgramProvider;
        }

        public CurriculumVersionViewModel Create(CreateCurriculumVersionViewModel request, Guid userId)
        {
            var dto = new CreateCurriculumVersionDTO
            {
                CurriculumId = request.CurriculumId,
                AcademicLevelId = request.AcademicLevelId,
                FacultyId = request.FacultyId,
                DepartmentId = request.DepartmentId,
                AcademicProgramId = request.AcademicProgramId,
                Code = request.Code,
                Name = request.Name,
                DegreeName = request.DegreeName,
                Abbreviation = request.Abbreviation,
                TotalCredit = request.TotalCredit,
                TotalYear = request.TotalYear,
                ExpectedGraduatingCredit = request.ExpectedGraduatingCredit,
                ApprovedAt = request.ApprovedAt,
                StartBatchCode = request.StartBatchCode,
                EndBatchCode = request.EndBatchCode,
                CollegeCalendarType = request.CollegeCalendarType,
                Remark = request.Remark,
                IsActive = request.IsActive,
                Localizations = MapLocalizationViewModelToDTO(request.Localizations).ToList()
            };

            var curriculumVersion = _curriculumVersionProvider.Create(dto, userId.ToString());

            var academicLevel = _academicLevelProvider.GetById(request.AcademicLevelId);

            var faculty = _facultyProvider.GetById(request.FacultyId);

            var department = _departmentProvider.GetById(request.DepartmentId);

            var academicProgram = _academicProgramProvider.GetById(request.AcademicProgramId);

            var response = MapDTOToViewModel(curriculumVersion, academicLevel, faculty, department, academicProgram);

            return response;
        }

        public CurriculumVersionViewModel GetById(Guid id)
        {
            var curriculumVersion = _curriculumVersionProvider.GetById(id);

            var academicLevel = _academicLevelProvider.GetById(curriculumVersion.AcademicLevelId);

            var faculty = _facultyProvider.GetById(curriculumVersion.FacultyId);

            var department = _departmentProvider.GetById(curriculumVersion.DepartmentId);

            var academicProgram = _academicProgramProvider.GetById(curriculumVersion.AcademicProgramId);

            var response = MapDTOToViewModel(curriculumVersion, academicLevel, faculty, department, academicProgram);

            return response;
        }

        public PagedViewModel<CurriculumVersionViewModel> Search(SearchCurriculumVersionCriteriaViewModel parameters, int page, int pageSize)
        {
            var dto = new SearchCurriculumVersionCriteriaDTO
            {
                Name = parameters.Name,
                Code = parameters.Code,
                CurriculumId = parameters.CurriculumId,
                IsActive = parameters.IsActive,
                SortBy = parameters.SortBy,
                OrderBy = parameters.OrderBy
            };

            var pagedCurriculumVersion = _curriculumVersionProvider.Search(dto, page, pageSize);

            var curriculums = _curriculumProvider.GetAll()
                                                 .ToList();

            var response = new PagedViewModel<CurriculumVersionViewModel>
            {
                Page = pagedCurriculumVersion.Page,
                TotalPage = pagedCurriculumVersion.TotalPage,
                TotalItem = pagedCurriculumVersion.TotalItem,
                Items = (from curriculumVersion in pagedCurriculumVersion.Items
                         let curriculum = curriculums.SingleOrDefault(x => x.Id == curriculumVersion.CurriculumId)
                         let academicLevel = _academicLevelProvider.GetById(curriculumVersion.AcademicLevelId)
                         let faculty = _facultyProvider.GetById(curriculumVersion.FacultyId)
                         let department = _departmentProvider.GetById(curriculumVersion.DepartmentId)
                         let academicProgram = _academicProgramProvider.GetById(curriculumVersion.AcademicProgramId)
                         select MapDTOToViewModel(curriculumVersion, academicLevel, faculty, department, academicProgram, curriculum))
                        .ToList()
            };

            return response;
        }

        public IEnumerable<CurriculumVersionDropDownViewModel> GetDropDownList(SearchCurriculumVersionCriteriaViewModel parameters)
        {
            var dto = new SearchCurriculumVersionCriteriaDTO
            {
                Name = parameters.Name,
                Code = parameters.Code,
                CurriculumId = parameters.CurriculumId,
                IsActive = parameters.IsActive,
                SortBy = parameters.SortBy,
                OrderBy = parameters.OrderBy
            };

            var curriculumVersions = _curriculumVersionProvider.Search(dto)
                                                               .ToList();

            var response = (from curriculumVersion in curriculumVersions
                            select MapDTOToDropDown(curriculumVersion))
                           .ToList();

            return response;
        }

        public CurriculumVersionViewModel Update(Guid id, CreateCurriculumVersionViewModel request, Guid userId)
        {
            var curriculumVersions = _curriculumVersionProvider.GetAll()
                                                               .ToList();

            var curriculumVersion = curriculumVersions.SingleOrDefault(x => x.Id == id);

            var academicLevel = _academicLevelProvider.GetById(request.AcademicLevelId);

            var faculty = _facultyProvider.GetById(request.FacultyId);

            var department = _departmentProvider.GetById(request.DepartmentId);

            var academicProgram = _academicProgramProvider.GetById(request.AcademicProgramId);

            if (curriculumVersion is null)
            {
                throw new CurriculumException.VersionNotFound(id);
            }

            curriculumVersion.CurriculumId = request.CurriculumId;
            curriculumVersion.AcademicLevelId = request.AcademicLevelId;
            curriculumVersion.FacultyId = request.FacultyId;
            curriculumVersion.DepartmentId = request.DepartmentId;
            curriculumVersion.AcademicProgramId = request.AcademicProgramId;
            curriculumVersion.Code = request.Code;
            curriculumVersion.Name = request.Name;
            curriculumVersion.DegreeName = request.DegreeName;
            curriculumVersion.Description = request.Description;
            curriculumVersion.Abbreviation = request.Abbreviation;
            curriculumVersion.TotalCredit = request.TotalCredit;
            curriculumVersion.TotalYear = request.TotalYear;
            curriculumVersion.ExpectedGraduatingCredit = request.ExpectedGraduatingCredit;
            curriculumVersion.ApprovedAt = request.ApprovedAt;
            curriculumVersion.StartBatchCode = request.StartBatchCode;
            curriculumVersion.EndBatchCode = request.EndBatchCode;
            curriculumVersion.CollegeCalendarType = request.CollegeCalendarType;
            curriculumVersion.Remark = request.Remark;
            curriculumVersion.IsActive = request.IsActive;
            curriculumVersion.Localizations = MapLocalizationViewModelToDTO(request.Localizations).ToList();

            var updatedCurriculumVersion = _curriculumVersionProvider.Update(curriculumVersion, userId.ToString());

            var response = MapDTOToViewModel(updatedCurriculumVersion, academicLevel, faculty, department, academicProgram);

            return response;
        }

        public void Delete(Guid id)
        {
            _curriculumVersionProvider.Delete(id);
        }

        public CurriculumVersionViewModel Copy(Guid baseVersionId, CopyCurriculumVersionViewModel request, Guid userId)
        {
            var baseVersion = _curriculumVersionProvider.GetById(baseVersionId);

            var curriculumCourseGroups = _curriculumCourseGroupProvider.GetByCurriculumVersionId(baseVersionId)
                                                                       .ToList();

            var specialization = _curriculumVersionProvider.GetAcademicSpecializations(baseVersionId)
                                                           .ToList();

            var coRequisite = _curriculumVersionProvider.GetCorequisites(baseVersionId)
                                                        .ToList();

            var courseEquivalent = _curriculumVersionProvider.GetEquivalentCourses(baseVersionId)
                                                             .ToList();

            var blackListCourses = _curriculumVersionProvider.GetBlackListCourses(baseVersionId)
                                                             .ToList();

            var dto = new CreateCurriculumVersionDTO
            {
                CurriculumId = baseVersion.CurriculumId,
                AcademicLevelId = baseVersion.AcademicLevelId,
                FacultyId = baseVersion.FacultyId,
                DepartmentId = baseVersion.DepartmentId,
                AcademicProgramId = baseVersion.AcademicProgramId,
                Code = request.Code,
                Name = request.Name,
                DegreeName = request.DegreeName,
                Abbreviation = request.Abbreviation,
                TotalCredit = request.TotalCredit,
                TotalYear = request.TotalYear,
                ExpectedGraduatingCredit = request.ExpectedGraduatingCredit,
                ApprovedAt = request.ApprovedAt,
                StartBatchCode = request.StartBatchCode,
                EndBatchCode = request.EndBatchCode,
                CollegeCalendarType = request.CollegeCalendarType,
                Remark = request.Remark,
                IsActive = request.IsActive,
                Localizations = MapLocalizationViewModelToDTO(request.Localizations).ToList()
            };

            var curriculumVersion = _curriculumVersionProvider.Create(dto, userId.ToString());

            var academicLevel = _academicLevelProvider.GetById(curriculumVersion.AcademicLevelId);

            var faculty = _facultyProvider.GetById(curriculumVersion.FacultyId);

            var deparment = _departmentProvider.GetById(curriculumVersion.DepartmentId);

            var academicProgram = _academicProgramProvider.GetById(curriculumVersion.AcademicProgramId);

            if (curriculumCourseGroups.Any())
            {
                CopyCurriculumCourseGroup(curriculumVersion.Id, null, null, curriculumCourseGroups, userId);
            }

            if (specialization.Any())
            {
                _curriculumVersionProvider.UpdateAcademicSpecialization(curriculumVersion.Id, specialization.Select(x => x.AcademicSpecializationId).ToList());
            }

            if (coRequisite.Any())
            {
                _curriculumVersionProvider.UpdateCorequisite(curriculumVersion.Id, coRequisite);
            }

            if (courseEquivalent.Any())
            {
                _curriculumVersionProvider.UpdateEquivalentCourses(curriculumVersion.Id, courseEquivalent);
            }

            if (blackListCourses.Any())
            {
                _curriculumVersionProvider.UpdateBlackListCourses(curriculumVersion.Id, blackListCourses.Select(x => x.CourseId).ToList());
            }

            var response = MapDTOToViewModel(curriculumVersion, academicLevel, faculty, deparment, academicProgram);

            return response;
        }

        private void CopyCurriculumCourseGroup(Guid newCurriculumVersionId, Guid? baseParentId, Guid? newParentId, List<CurriculumCourseGroupDTO> courseGroups, Guid userId)
        {
            if (courseGroups is null || !courseGroups.Any())
            {
                return;
            }

            var matchingParentGroups = courseGroups.Where(x => x.ParentCourseGroupId == baseParentId).ToList();

            if (!matchingParentGroups.Any())
            {
                return;
            }

            foreach (var courseGroupDTO in matchingParentGroups)
            {
                courseGroupDTO.CurriculumVersionId = newCurriculumVersionId;
                courseGroupDTO.ParentCourseGroupId = newParentId;

                var courseGroup = _curriculumCourseGroupProvider.Create(courseGroupDTO, userId.ToString());

                var curriculumCourses = new List<CurriculumCourseDTO>();

                foreach (var course in courseGroupDTO.Courses)
                {
                    course.CourseGroupId = courseGroup.Id;

                    curriculumCourses.Add(course);
                }

                _curriculumCourseGroupProvider.UpdateCourses(courseGroup.Id, curriculumCourses);

                var ignoreCourseIds = courseGroup.IgnoreCourses.Select(x => x.CourseId)
                                                               .ToList();

                _curriculumCourseGroupProvider.UpdateIgnoreCourses(courseGroup.Id, ignoreCourseIds);

                CopyCurriculumCourseGroup(newCurriculumVersionId, courseGroupDTO.Id, courseGroup.Id, courseGroups, userId);
            }
        }

        public IEnumerable<CurriculumCourseBlackListViewModel> GetBlackListCourses(Guid versionId)
        {
            var version = _curriculumVersionProvider.GetById(versionId);

            var blackListCourses = _curriculumVersionProvider.GetBlackListCourses(versionId).ToList();

            if (!blackListCourses.Any())
            {
                return Enumerable.Empty<CurriculumCourseBlackListViewModel>();
            }

            var courseIds = blackListCourses.Select(x => x.CourseId)
                                            .Distinct()
                                            .ToList();

            var courses = _courseProvider.GetById(courseIds)
                                         .ToList();

            var response = (from course in courses
                            orderby course.Code
                            select MapCourseDTOToBlackListViewModel(course))
                           .ToList();

            return response;
        }

        public IEnumerable<CurriculumCourseBlackListViewModel> UpdateBlackListCourses(Guid versionId, IEnumerable<Guid> blackListCourseIds)
        {
            var version = _curriculumVersionProvider.GetById(versionId);

            var curriculumCourses = _curriculumVersionProvider.GetCoursesList(versionId);

            var courseIds = blackListCourseIds is null ? Enumerable.Empty<Guid>()
                                                       : blackListCourseIds.Distinct()
                                                                           .ToList();

            var courses = _courseProvider.GetById(courseIds)
                                         .ToList();

            foreach (var courseId in courseIds)
            {
                var matchingCourse = courses.SingleOrDefault(x => x.Id == courseId);

                if (matchingCourse is null)
                {
                    throw new CourseException.NotFound(courseId);
                }

                var isInCurriculum = curriculumCourses.Any(x => x.CourseId == courseId);

                if (isInCurriculum)
                {
                    throw new CurriculumException.InCurriculumCourseException(courseId);
                }
            }

            _curriculumVersionProvider.UpdateBlackListCourses(versionId, courseIds);

            var response = (from course in courses
                            orderby course.Code
                            select MapCourseDTOToBlackListViewModel(course))
                           .ToList();

            return response;
        }

        public IEnumerable<CurriculumVersionSpecializationViewModel> GetAcademicSpecializations(Guid versionId)
        {
            var version = _curriculumVersionProvider.GetById(versionId);

            var versionSpecializations = _curriculumVersionProvider.GetAcademicSpecializations(versionId)
                                                                   .ToList();

            if (!versionSpecializations.Any())
            {
                return Enumerable.Empty<CurriculumVersionSpecializationViewModel>();
            }

            var academicSpecializationIds = versionSpecializations.Select(x => x.AcademicSpecializationId)
                                                                  .Distinct()
                                                                  .ToList();

            var academicSpecializations = _academicSpecializationProvider.GetById(academicSpecializationIds)
                                                                         .ToList();

            var response = (from academicSpecialization in academicSpecializations
                            orderby academicSpecialization.Code, academicSpecialization.Level, academicSpecialization.Name
                            select MapSpecializationToVersionSpecializationViewModel(academicSpecialization))
                           .ToList();

            return response;
        }

        public IEnumerable<CurriculumVersionSpecializationViewModel> UpdateAcademicSpecializations(Guid versionId, IEnumerable<Guid> specializationIds)
        {
            var version = _curriculumVersionProvider.GetById(versionId);

            var academicSpecializationIds = specializationIds is null ? Enumerable.Empty<Guid>()
                                                                      : specializationIds.Distinct()
                                                                                         .ToList();

            var academicSpecializations = _academicSpecializationProvider.GetById(academicSpecializationIds)
                                                                         .ToList();

            foreach (var academicSpecializationId in academicSpecializationIds)
            {
                var matchingSpecialization = academicSpecializations.SingleOrDefault(x => x.Id == academicSpecializationId);

                if (matchingSpecialization is null)
                {
                    throw new AcademicSpecializationException.NotFound(academicSpecializationId);
                }
            }

            _curriculumVersionProvider.UpdateAcademicSpecialization(versionId, academicSpecializationIds);

            var response = (from academicSpecialization in academicSpecializations
                            orderby academicSpecialization.Code, academicSpecialization.Level, academicSpecialization.Name
                            select MapSpecializationToVersionSpecializationViewModel(academicSpecialization))
                           .ToList();

            return response;
        }

        public IEnumerable<CorequisiteViewModel> GetCorequisites(Guid versionId)
        {
            var version = _curriculumVersionProvider.GetById(versionId);

            var corequisites = _curriculumVersionProvider.GetCorequisites(versionId)
                                                         .ToList();

            if (!corequisites.Any())
            {
                return Enumerable.Empty<CorequisiteViewModel>();
            }

            var courseIds = corequisites.SelectMany(x => new List<Guid> { x.CourseId, x.CorequisiteCourseId })
                                        .Distinct()
                                        .ToList();

            var courses = _courseProvider.GetById(courseIds)
                                         .ToList();

            var response = (from corequisite in corequisites
                            select MapCorequisiteToViewModel(corequisite.CourseId, corequisite.CorequisiteCourseId, courses))
                           .ToList();

            response = response.OrderBy(x => x.CourseCode)
                               .ThenBy(x => x.CorequisiteCourseCode)
                               .ToList();

            return response;
        }

        public IEnumerable<CorequisiteViewModel> UpdateCorequisites(Guid versionId, IEnumerable<CreateCorequisiteViewModel> corequisites)
        {
            var version = _curriculumVersionProvider.GetById(versionId);

            var blackListCourses = _curriculumVersionProvider.GetBlackListCourses(versionId)
                                                             .ToList();

            var versionCorequisites = corequisites is null ? Enumerable.Empty<CreateCorequisiteViewModel>()
                                                           : corequisites;

            var courseIds = versionCorequisites.SelectMany(x => new List<Guid> { x.CourseId, x.CorequisiteCourseId })
                                               .Distinct()
                                               .ToList();

            var curriculumCourses = _curriculumVersionProvider.GetCoursesList(versionId)
                                                              .ToList();

            foreach (var courseId in courseIds)
            {
                if (!curriculumCourses.Any(x => x.CourseId == courseId))
                {
                    throw new CurriculumException.NotInCurriculumVersionException(courseId);
                }

                if (blackListCourses.Any(x => x.CourseId == courseId))
                {
                    throw new CurriculumException.InBlackListCourseException(courseId);
                }
            }

            foreach (var versionCorequisite in versionCorequisites)
            {
                var reversedCorequisite = versionCorequisites.Where(x => x.CourseId == versionCorequisite.CorequisiteCourseId
                                                                         && x.CorequisiteCourseId == versionCorequisite.CourseId)
                                                             .ToList();

                if (reversedCorequisite.Any())
                {
                    throw new CurriculumException.ReversedCorequisiteException(versionCorequisite.CourseId, versionCorequisite.CorequisiteCourseId);
                }
            }

            var dto = (from versionCorequisite in versionCorequisites
                       select new CreateCorequisiteDTO
                       {
                           CourseId = versionCorequisite.CourseId,
                           CorequisiteCourseId = versionCorequisite.CorequisiteCourseId
                       })
                      .ToList();

            _curriculumVersionProvider.UpdateCorequisite(versionId, dto);

            var courses = _courseProvider.GetById(courseIds)
                                         .ToList();

            var response = (from versionCorequisite in versionCorequisites
                            select MapCorequisiteToViewModel(versionCorequisite.CourseId, versionCorequisite.CorequisiteCourseId, courses))
                           .ToList();

            response = response.OrderBy(x => x.CourseCode)
                               .ThenBy(x => x.CorequisiteCourseCode)
                               .ToList();

            return response;
        }

        public IEnumerable<CourseDropDownViewModel> GetCurriculumVersionCourseDropdownLists(Guid curriculumVersionId)
        {
            _curriculumVersionProvider.GetById(curriculumVersionId);

            var curriculumVersionCourses = _curriculumVersionProvider.GetCoursesList(curriculumVersionId)
                                                                     .ToList();

            var courseBlackLists = _curriculumVersionProvider.GetBlackListCourses(curriculumVersionId)
                                                             .ToList();

            var blackListCourseIds = courseBlackLists.Select(x => x.CourseId)
                                                     .ToList();

            var courseIds = curriculumVersionCourses.Select(x => x.CourseId)
                                                    .Distinct()
                                                    .Except(blackListCourseIds)
                                                    .ToList();

            if (!courseIds.Any())
            {
                return Enumerable.Empty<CourseDropDownViewModel>();
            }

            var courses = _courseProvider.GetById(courseIds)
                                         .ToList();

            var response = (from course in courses
                            orderby course.Code, course.Name
                            select new CourseDropDownViewModel
                            {
                                Id = course.Id.ToString(),
                                Name = course.Name,
                                Code = course.Code,
                                AcademicLevelId = course.AcademicLevelId,
                                FacultyId = course.FacultyId,
                                DepartmentId = course.DepartmentId
                            })
                           .ToList();

            return response;
        }

        public IEnumerable<EquivalentCourseViewModel> GetEquivalentCourses(Guid versionId)
        {
            var version = _curriculumVersionProvider.GetById(versionId);

            var equivalences = _curriculumVersionProvider.GetEquivalentCourses(versionId)
                                                         .ToList();

            if (!equivalences.Any())
            {
                return Enumerable.Empty<EquivalentCourseViewModel>();
            }

            var courseIds = equivalences.SelectMany(x => new List<Guid> { x.CourseId, x.EquivalentCourseId })
                                        .Distinct()
                                        .ToList();

            var courses = _courseProvider.GetById(courseIds)
                                         .ToList();

            var response = (from equivalence in equivalences
                            select MapEquivalentCourseToViewModel(equivalence.CourseId, equivalence.EquivalentCourseId, courses))
                           .ToList();

            response = response.OrderBy(x => x.CourseCode)
                               .ThenBy(x => x.EquivalentCourseCode)
                               .ToList();

            return response;
        }

        public IEnumerable<EquivalentCourseViewModel> UpdateEquivalentCourses(Guid versionId, IEnumerable<CreateEquivalentCourseViewModel> equivalences)
        {
            var version = _curriculumVersionProvider.GetById(versionId);

            var blackListCourses = _curriculumVersionProvider.GetBlackListCourses(versionId)
                                                             .ToList();

            var versionEquivalences = equivalences is null ? Enumerable.Empty<CreateEquivalentCourseViewModel>()
                                                           : equivalences;

            var courseIds = versionEquivalences.SelectMany(x => new List<Guid> { x.CourseId, x.EquivalentCourseId })
                                               .ToList();

            var curriculumCourses = _curriculumVersionProvider.GetCoursesList(versionId);

            var courses = _courseProvider.GetById(courseIds)
                                         .ToList();

            foreach (var equivalence in versionEquivalences)
            {
                if (!curriculumCourses.Any(x => x.CourseId == equivalence.CourseId))
                {
                    throw new CurriculumException.NotInCurriculumVersionException(equivalence.CourseId);
                }

                if (!courses.Any(x => x.Id == equivalence.EquivalentCourseId))
                {
                    throw new CourseException.NotFound(equivalence.EquivalentCourseId);
                }

                if (blackListCourses.Any(x => x.CourseId == equivalence.CourseId || x.CourseId == equivalence.EquivalentCourseId))
                {
                    throw new CurriculumException.InBlackListCourseException(equivalence.CourseId);
                }
            }

            var dto = (from equivalence in versionEquivalences
                       select new CreateEquivalentCourseDTO
                       {
                           CourseId = equivalence.CourseId,
                           EquivalentCourseId = equivalence.EquivalentCourseId
                       })
                      .ToList();

            _curriculumVersionProvider.UpdateEquivalentCourses(versionId, dto);

            var response = (from equivalence in versionEquivalences
                            select MapEquivalentCourseToViewModel(equivalence.CourseId, equivalence.EquivalentCourseId, courses))
                           .ToList();

            return response;
        }

        private CurriculumVersionViewModel MapDTOToViewModel(CurriculumVersionDTO dto, AcademicLevelDTO academicLevel,
                                                             FacultyDTO faculty, DepartmentDTO department,
                                                             AcademicProgramDTO academicProgram, CurriculumDTO? curriculum = null)
        {
            var response = new CurriculumVersionViewModel
            {
                Id = dto.Id,
                CurriculumId = dto.CurriculumId,
                AcademicLevelId = dto.AcademicLevelId,
                AcademicLevelName = (from data in academicLevel.Localizations
                                     orderby data.Language
                                     select new AcademicLevelLocalizationDTO
                                     {
                                         Language = data.Language,
                                         Name = data.Name
                                     }).FirstOrDefault()?.Name,
                FacultyId = dto.FacultyId,
                FacultyName = (from data in faculty.Localizations
                               orderby data.Language
                               select new FacultyLocalizationDTO
                               {
                                   Language = data.Language,
                                   Name = data.Name
                               }).FirstOrDefault()?.Name,
                DepartmentId = dto.DepartmentId,
                DepartmentName = (from data in department.Localizations
                                  orderby data.Language
                                  select new DepartmentLocalizationDTO
                                  {
                                      Language = data.Language,
                                      Name = data.Name
                                  }).FirstOrDefault()?.Name,
                AcademicProgramId = dto.AcademicProgramId,
                AcademicProgramName = (from data in academicProgram.Localizations
                                       orderby data.Language
                                       select new AcademicLevelLocalizationDTO
                                       {
                                           Language = data.Language,
                                           Name = data.Name
                                       }).FirstOrDefault()?.Name,
                Code = dto.Code,
                TotalCredit = dto.TotalCredit,
                TotalYear = dto.TotalYear,
                ExpectedGraduatingCredit = dto.ExpectedGraduatingCredit,
                ApprovedAt = dto.ApprovedAt,
                StartBatchCode = dto.StartBatchCode,
                EndBatchCode = dto.EndBatchCode,
                CollegeCalendarType = dto.CollegeCalendarType,
                Remark = dto.Remark,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt,
                IsActive = dto.IsActive,
                CurriculumName = curriculum?.Name,
                Localizations = (from locale in dto.Localizations
                                 orderby locale.Language
                                 select new CurriculumVersionLocalizationViewModel
                                 {
                                     Language = locale.Language,
                                     Name = locale.Name,
                                     DegreeName = locale.DegreeName,
                                     Description = locale.Description,
                                     Abbreviation = locale.Abbreviation
                                 })
                                .ToList()
            };

            return response;
        }

        private static IEnumerable<CurriculumVersionLocalizationDTO> MapLocalizationViewModelToDTO(
            IEnumerable<CurriculumVersionLocalizationViewModel>? localizations)
        {
            if (localizations is null)
            {
                return Enumerable.Empty<CurriculumVersionLocalizationDTO>();
            }

            var response = (from locale in localizations
                            select new CurriculumVersionLocalizationDTO
                            {
                                Language = locale.Language,
                                Name = locale.Name,
                                DegreeName = locale.DegreeName,
                                Description = locale.Description,
                                Abbreviation = locale.Abbreviation
                            })
                            .ToList();

            return response;
        }

        private static CurriculumCourseBlackListViewModel MapCourseDTOToBlackListViewModel(CourseDTO dto)
        {
            var response = new CurriculumCourseBlackListViewModel
            {
                CourseId = dto.Id,
                CourseCode = dto.Code,
                CourseName = (from data in dto.Localizations
                              orderby data.Language
                              select new CourseLocalizationDTO
                              {
                                  Language = data.Language,
                                  Name = data.Name
                              }).SingleOrDefault()?.Name,
                Credit = dto.Credit
            };

            return response;
        }

        private static CurriculumVersionSpecializationViewModel MapSpecializationToVersionSpecializationViewModel(AcademicSpecializationDTO dto)
        {
            var response = new CurriculumVersionSpecializationViewModel
            {
                AcademicSpecializationId = dto.Id,
                AcademicSpecializationCode = dto.Code,
                AcademicSpecializationName = dto.Name
            };

            return response;
        }

        private static CorequisiteViewModel MapCorequisiteToViewModel(Guid courseId,
                                                                      Guid corequisiteCourseId,
                                                                      IEnumerable<CourseDTO>? courses = null)
        {
            var course = courses?.SingleOrDefault(x => x.Id == courseId);
            var corequisiteCourse = courses?.SingleOrDefault(x => x.Id == corequisiteCourseId);
            var response = new CorequisiteViewModel
            {
                CourseId = courseId,
                CourseCode = course?.Code,
                CourseName = (from data in course?.Localizations
                              orderby data.Language
                              select new CourseLocalizationDTO
                              {
                                  Language = data.Language,
                                  Name = data.Name
                              }).SingleOrDefault()?.Name,
                CorequisiteCourseId = corequisiteCourseId,
                CorequisiteCourseCode = corequisiteCourse?.Code,
                CorequisiteCourseName = (from data in corequisiteCourse?.Localizations
                                         orderby data.Language
                                         select new CourseLocalizationDTO
                                         {
                                             Language = data.Language,
                                             Name = data.Name
                                         }).SingleOrDefault()?.Name,
            };

            return response;
        }

        private static EquivalentCourseViewModel MapEquivalentCourseToViewModel(Guid courseId,
                                                                                Guid equivalentCourseId,
                                                                                IEnumerable<CourseDTO>? courses = null)
        {
            var course = courses?.SingleOrDefault(x => x.Id == courseId);
            var equivalence = courses?.SingleOrDefault(x => x.Id == equivalentCourseId);
            var response = new EquivalentCourseViewModel
            {
                CourseId = courseId,
                CourseCode = course?.Code,
                CourseName = (from data in course?.Localizations
                              orderby data.Language
                              select new CourseLocalizationDTO
                              {
                                  Language = data.Language,
                                  Name = data.Name
                              }).SingleOrDefault()?.Name,
                EquivalentCourseId = equivalentCourseId,
                EquivalentCourseCode = equivalence?.Code,
                EquivalentCourseName = (from data in equivalence?.Localizations
                                        orderby data.Language
                                        select new CourseLocalizationDTO
                                        {
                                            Language = data.Language,
                                            Name = data.Name
                                        }).SingleOrDefault()?.Name,
            };

            return response;
        }

        private static CurriculumVersionDropDownViewModel MapDTOToDropDown(CurriculumVersionDTO dto)
        {
            var response = new CurriculumVersionDropDownViewModel
            {
                Id = dto.Id.ToString(),
                Name = dto.Name,
                CurriculumId = dto.CurriculumId,
                Code = dto.Code
            };

            return response;
        }
    }
}