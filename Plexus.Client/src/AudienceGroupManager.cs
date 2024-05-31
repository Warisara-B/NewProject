using Plexus.Client.ViewModel;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Academic;
using Plexus.Entity.Provider;
using Plexus.Utility.ViewModel;
using Plexus.Utility.Extensions;
using Plexus.Entity.Exception;

namespace Plexus.Client.src
{
    public class AudienceGroupManager : IAudienceGroupManager
    {
        private readonly IAudienceGroupProvider _audienceGroupProvider;
        private readonly IAcademicLevelProvider _academicLevelProvider;
        private readonly IAcademicProgramProvider _academicProgramProvider;
        private readonly IFacultyProvider _facultyProvider;
        private readonly IDepartmentProvider _departmentProvider;

        public AudienceGroupManager(IAudienceGroupProvider audienceGroupProvider,
                                    IAcademicLevelProvider academicLevelProvider,
                                    IAcademicProgramProvider academicProgramProvider,
                                    IFacultyProvider facultyProvider,
                                    IDepartmentProvider departmentProvider)
        {
            _audienceGroupProvider = audienceGroupProvider;
            _academicLevelProvider = academicLevelProvider;
            _academicProgramProvider = academicProgramProvider;
            _facultyProvider = facultyProvider;
            _departmentProvider = departmentProvider;
        }

        public AudienceGroupViewModel Create(CreateAudienceGroupViewModel request, Guid userId)
        {
            var conditions = ValidateConditions(request.Conditions).ToList();

            var dto = MapViewModelToDTO(request);

            var audienceGroup = _audienceGroupProvider.Create(dto, userId.ToString());

            var response = MapDTOToViewModel(audienceGroup, conditions);

            return response;
        }

        public PagedViewModel<AudienceGroupViewModel> Search(SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var pagedAudienceGroup = _audienceGroupProvider.Search(parameters, page, pageSize);

            var conditions = pagedAudienceGroup.Items.SelectMany(x => x.Conditions)
                                                     .ToList();

            var academicLevels = Enumerable.Empty<AcademicLevelDTO>();
            var academicPrograms = Enumerable.Empty<AcademicProgramDTO>();
            var faculties = Enumerable.Empty<FacultyDTO>();
            var departments = Enumerable.Empty<DepartmentDTO>();

            var academicLevelIds = conditions.Where(x => x.AcademicLevelId.HasValue)
                                             .Select(x => x.AcademicLevelId!.Value)
                                             .Distinct()
                                             .ToList();
            
            if (academicLevelIds.Any())
            {
                academicLevels = _academicLevelProvider.GetById(academicLevelIds)
                                                       .ToList();
            }

            var academicProgramIds = conditions.Where(x => x.AcademicProgramId.HasValue)
                                               .Select(x => x.AcademicProgramId!.Value)
                                               .Distinct()
                                               .ToList();
            
            if (academicProgramIds.Any())
            {
                academicPrograms = _academicProgramProvider.GetById(academicProgramIds)
                                                           .ToList();
            }

            var facultyIds = conditions.Where(x => x.FacultyId.HasValue)
                                       .Select(x => x.FacultyId!.Value)
                                       .Distinct()
                                       .ToList();
            
            if (facultyIds.Any())
            {
                faculties = _facultyProvider.GetById(facultyIds)
                                            .ToList();
            }

            var departmentIds = conditions.Where(x => x.DepartmentId.HasValue)
                                          .Select(x => x.DepartmentId!.Value)
                                          .Distinct()
                                          .ToList();
            
            if (departmentIds.Any())
            {
                departments = _departmentProvider.GetById(departmentIds)
                                                 .ToList();
            }

            var response = new PagedViewModel<AudienceGroupViewModel>
            {
                Page = pagedAudienceGroup.Page,
                TotalPage = pagedAudienceGroup.TotalPage,
                TotalItem = pagedAudienceGroup.TotalItem,
                Items = (from audienceGroup in pagedAudienceGroup.Items
                         select MapDTOToViewModel(audienceGroup, academicLevels, academicPrograms, faculties, departments))
                        .ToList()
            };

            return response;
        }

        public IEnumerable<AudienceGroupViewModel> Search(SearchCriteriaViewModel parameters)
        {
            var audienceGroups = _audienceGroupProvider.Search(parameters)
                                                       .ToList();

            var conditions = audienceGroups.SelectMany(x => x.Conditions)
                                           .ToList();

            var academicLevels = Enumerable.Empty<AcademicLevelDTO>();
            var academicPrograms = Enumerable.Empty<AcademicProgramDTO>();
            var faculties = Enumerable.Empty<FacultyDTO>();
            var departments = Enumerable.Empty<DepartmentDTO>();

            var academicLevelIds = conditions.Where(x => x.AcademicLevelId.HasValue)
                                             .Select(x => x.AcademicLevelId!.Value)
                                             .Distinct()
                                             .ToList();
            
            if (academicLevelIds.Any())
            {
                academicLevels = _academicLevelProvider.GetById(academicLevelIds)
                                                       .ToList();
            }

            var academicProgramIds = conditions.Where(x => x.AcademicProgramId.HasValue)
                                               .Select(x => x.AcademicProgramId!.Value)
                                               .Distinct()
                                               .ToList();
            
            if (academicProgramIds.Any())
            {
                academicPrograms = _academicProgramProvider.GetById(academicProgramIds)
                                                           .ToList();
            }

            var facultyIds = conditions.Where(x => x.FacultyId.HasValue)
                                       .Select(x => x.FacultyId!.Value)
                                       .Distinct()
                                       .ToList();
            
            if (facultyIds.Any())
            {
                faculties = _facultyProvider.GetById(facultyIds)
                                            .ToList();
            }

            var departmentIds = conditions.Where(x => x.DepartmentId.HasValue)
                                          .Select(x => x.DepartmentId!.Value)
                                          .Distinct()
                                          .ToList();
            
            if (departmentIds.Any())
            {
                departments = _departmentProvider.GetById(departmentIds)
                                                 .ToList();
            }

            var response = (from audienceGroup in audienceGroups
                            select MapDTOToViewModel(audienceGroup, academicLevels, academicPrograms, faculties, departments))
                           .ToList();

            return response;
        }

        public AudienceGroupViewModel GetById(Guid id)
        {
            var audienceGroup = _audienceGroupProvider.GetById(id);

            var academicLevels = Enumerable.Empty<AcademicLevelDTO>();
            var academicPrograms = Enumerable.Empty<AcademicProgramDTO>();
            var faculties = Enumerable.Empty<FacultyDTO>();
            var departments = Enumerable.Empty<DepartmentDTO>();

            var academicLevelIds = audienceGroup.Conditions.Where(x => x.AcademicLevelId.HasValue)
                                                           .Select(x => x.AcademicLevelId!.Value)
                                                           .Distinct()
                                                           .ToList();
            
            if (academicLevelIds.Any())
            {
                academicLevels = _academicLevelProvider.GetById(academicLevelIds)
                                                       .ToList();
            }

            var academicProgramIds = audienceGroup.Conditions.Where(x => x.AcademicProgramId.HasValue)
                                                             .Select(x => x.AcademicProgramId!.Value)
                                                             .Distinct()
                                                             .ToList();
            
            if (academicProgramIds.Any())
            {
                academicPrograms = _academicProgramProvider.GetById(academicProgramIds)
                                                           .ToList();
            }

            var facultyIds = audienceGroup.Conditions.Where(x => x.FacultyId.HasValue)
                                                     .Select(x => x.FacultyId!.Value)
                                                     .Distinct()
                                                     .ToList();
            
            if (facultyIds.Any())
            {
                faculties = _facultyProvider.GetById(facultyIds)
                                            .ToList();
            }

            var departmentIds = audienceGroup.Conditions.Where(x => x.DepartmentId.HasValue)
                                                        .Select(x => x.DepartmentId!.Value)
                                                        .Distinct()
                                                        .ToList();
            
            if (departmentIds.Any())
            {
                departments = _departmentProvider.GetById(departmentIds)
                                                 .ToList();
            }

            var response = MapDTOToViewModel(audienceGroup, academicLevels, academicPrograms, faculties, departments);

            return response;
        }

        public AudienceGroupViewModel Update(AudienceGroupViewModel request, Guid userId)
        {
            var audienceGroup = _audienceGroupProvider.GetById(request.Id);

            var conditions = ValidateConditions(request.Conditions).ToList();

            audienceGroup.Name = request.Name;
            audienceGroup.Description = request.Description;
            audienceGroup.Conditions = (from condition in request.Conditions
                                        select new AudienceGroupConditionDTO
                                        {
                                            AcademicLevelId = condition.AcademicLevelId,
                                            AcademicProgramId = condition.AcademicProgramId,
                                            FacultyId = condition.FacultyId,
                                            DepartmentId = condition.DepartmentId,
                                            Codes = condition.Codes.SplitWithCommaSeparator(),
                                            StartedCode = condition.StartedCode,
                                            EndedCode = condition.EndedCode,
                                            StartedBatch = condition.StartedBatch,
                                            EndedBatch = condition.EndedBatch,
                                            StartedLastDigit = condition.StartedLastDigit,
                                            EndedLastDigit = condition.EndedLastDigit
                                        })
                                       .ToList();

            var updatedAudienceGroup = _audienceGroupProvider.Update(audienceGroup, userId.ToString());

            var response = MapDTOToViewModel(updatedAudienceGroup, conditions);

            return response;
        }

        public void Delete(Guid id)
        {
            _audienceGroupProvider.Delete(id);
        }

        private AudienceGroupViewModel MapDTOToViewModel(AudienceGroupDTO dto, IEnumerable<AudienceGroupConditionViewModel> conditions)
        {
            var response = new AudienceGroupViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                IsActive = dto.IsActive,
                Conditions = conditions,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt
            };

            return response;
        }

        private static AudienceGroupViewModel MapDTOToViewModel(AudienceGroupDTO dto,
            IEnumerable<AcademicLevelDTO> academicLevels,
            IEnumerable<AcademicProgramDTO> academicPrograms,
            IEnumerable<FacultyDTO> faculties,
            IEnumerable<DepartmentDTO> departments)
        {
            var response = new AudienceGroupViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                IsActive = dto.IsActive,
                Conditions = (from condition in dto.Conditions
                              let academicLevel = condition.AcademicLevelId.HasValue ? academicLevels.SingleOrDefault(x => x.Id == condition.AcademicLevelId.Value)
                                                                                     : null
                              let academicProgram = condition.AcademicProgramId.HasValue ? academicPrograms.SingleOrDefault(x => x.Id == condition.AcademicProgramId.Value)
                                                                                         : null
                              let faculty = condition.FacultyId.HasValue ? faculties.SingleOrDefault(x => x.Id == condition.FacultyId.Value)
                                                                         : null
                              let department = condition.DepartmentId.HasValue ? departments.SingleOrDefault(x => x.Id == condition.DepartmentId.Value)
                                                                               : null
                              select new AudienceGroupConditionViewModel
                              {
                                  AcademicLevelId = condition.AcademicLevelId,
                                  AcademicLevelName = academicLevel?.Name,
                                  AcademicProgramId = condition.AcademicProgramId,
                                  AcademicProgramName = academicProgram?.Name,
                                  FacultyId = condition.FacultyId,
                                  FacultyName = faculty?.Name,
                                  DepartmentId = condition.DepartmentId,
                                  DepartmentName = department?.Name,
                                  Codes = condition.Codes.ToStringWithCommaSeparator(),
                                  StartedCode = condition.StartedCode,
                                  EndedCode = condition.EndedCode,
                                  StartedBatch = condition.StartedBatch,
                                  EndedBatch = condition.EndedBatch,
                                  StartedLastDigit = condition.StartedLastDigit,
                                  EndedLastDigit = condition.EndedLastDigit
                              })
                             .ToList(),
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt
            };

            return response;
        }

        private static CreateAudienceGroupDTO MapViewModelToDTO(CreateAudienceGroupViewModel request)
        {
            var dto = new CreateAudienceGroupDTO
            {
                Name = request.Name,
                Description = request.Description,
                IsActive = request.IsActive,
                Conditions = (from condition in request.Conditions
                              select new AudienceGroupConditionDTO
                              {
                                  AcademicLevelId = condition.AcademicLevelId,
                                  AcademicProgramId = condition.AcademicProgramId,
                                  FacultyId = condition.FacultyId,
                                  DepartmentId = condition.DepartmentId,
                                  Codes = condition.Codes.SplitWithCommaSeparator(),
                                  StartedCode = condition.StartedCode,
                                  EndedCode = condition.EndedCode,
                                  StartedBatch = condition.StartedBatch,
                                  EndedBatch = condition.EndedBatch,
                                  StartedLastDigit = condition.StartedLastDigit,
                                  EndedLastDigit = condition.EndedLastDigit
                              })
                             .ToList()
            };

            return dto;
        }

        private IEnumerable<AudienceGroupConditionViewModel> ValidateConditions(IEnumerable<CreateAudienceGroupConditionViewModel> conditions)
        {
            if (conditions is null || !conditions.Any())
            {
                throw new AudienceGroupException.InvalidConditions();
            }

            var emptyConditions = conditions.Where(x => !x.AcademicLevelId.HasValue
                                                        && !x.AcademicProgramId.HasValue
                                                        && !x.FacultyId.HasValue
                                                        && !x.DepartmentId.HasValue
                                                        && string.IsNullOrEmpty(x.Codes)
                                                        && string.IsNullOrEmpty(x.StartedCode)
                                                        && string.IsNullOrEmpty(x.EndedCode)
                                                        && !x.StartedBatch.HasValue
                                                        && !x.EndedBatch.HasValue
                                                        && string.IsNullOrEmpty(x.StartedLastDigit)
                                                        && string.IsNullOrEmpty(x.EndedLastDigit))
                                            .ToList();

            if (emptyConditions.Any())
            {
                throw new AudienceGroupException.InvalidConditions();
            }

            var academicLevelIds = conditions.Where(x => x.AcademicLevelId.HasValue)
                                             .Select(x => x.AcademicLevelId!.Value)
                                             .Distinct()
                                             .ToList();
            
            var academicProgramIds = conditions.Where(x => x.AcademicProgramId.HasValue)
                                               .Select(x => x.AcademicProgramId!.Value)
                                               .Distinct()
                                               .ToList();
            
            var facultyIds = conditions.Where(x => x.FacultyId.HasValue)
                                       .Select(x => x.FacultyId!.Value)
                                       .Distinct()
                                       .ToList();
            
            var departmentIds = conditions.Where(x => x.DepartmentId.HasValue)
                                          .Select(x => x.DepartmentId!.Value)
                                          .Distinct()
                                          .ToList();

            var academicLevels = Enumerable.Empty<AcademicLevelDTO>();
            var academicPrograms = Enumerable.Empty<AcademicProgramDTO>();
            var faculties = Enumerable.Empty<FacultyDTO>();
            var departments = Enumerable.Empty<DepartmentDTO>();         
            
            if (academicLevelIds.Any())
            {
                academicLevels = _academicLevelProvider.GetById(academicLevelIds)
                                                       .ToList();

                foreach (var id in academicLevelIds)
                {
                    var matchingData = academicLevels.SingleOrDefault(x => x.Id == id);

                    if (matchingData is null)
                    {
                        throw new AcademicLevelException.NotFound(id);
                    }
                }
            }
            
            if (academicProgramIds.Any())
            {
                academicPrograms = _academicProgramProvider.GetById(academicProgramIds)
                                                           .ToList();

                foreach (var id in academicProgramIds)
                {
                    var matchingData = academicPrograms.SingleOrDefault(x => x.Id == id);

                    if (matchingData is null)
                    {
                        throw new AcademicProgramException.NotFound(id);
                    }
                }
            }
            
            if (facultyIds.Any())
            {
                faculties = _facultyProvider.GetById(facultyIds)
                                            .ToList();

                foreach (var id in facultyIds)
                {
                    var matchingData = faculties.SingleOrDefault(x => x.Id == id);

                    if (matchingData is null)
                    {
                        throw new FacultyException.NotFound(id);
                    }
                }
            }
            
            if (departmentIds.Any())
            {
                departments = _departmentProvider.GetById(departmentIds)
                                                 .ToList();

                foreach (var id in departmentIds)
                {
                    var matchingData = departments.SingleOrDefault(x => x.Id == id);

                    if (matchingData is null)
                    {
                        throw new DepartmentException.NotFound(id);
                    }
                }
            }

            var response = (from condition in conditions
                            let academicLevel = condition.AcademicLevelId.HasValue ? academicLevels.SingleOrDefault(x => x.Id == condition.AcademicLevelId.Value)
                                                                                   : null
                            let academicProgram = condition.AcademicProgramId.HasValue ? academicPrograms.SingleOrDefault(x => x.Id == condition.AcademicProgramId.Value)
                                                                                       : null
                            let faculty = condition.FacultyId.HasValue ? faculties.SingleOrDefault(x => x.Id == condition.FacultyId.Value)
                                                                       : null
                            let department = condition.DepartmentId.HasValue ? departments.SingleOrDefault(x => x.Id == condition.DepartmentId.Value)
                                                                             : null
                            select new AudienceGroupConditionViewModel
                            {
                                AcademicLevelId = condition.AcademicLevelId,
                                AcademicLevelName = academicLevel?.Name,
                                AcademicProgramId = condition.AcademicProgramId,
                                AcademicProgramName = academicProgram?.Name,
                                FacultyId = condition.FacultyId,
                                FacultyName = faculty?.Name,
                                DepartmentId = condition.DepartmentId,
                                DepartmentName = department?.Name,
                                Codes = condition.Codes,
                                StartedCode = condition.StartedCode,
                                EndedCode = condition.EndedCode,
                                StartedBatch = condition.StartedBatch,
                                EndedBatch = condition.EndedBatch,
                                StartedLastDigit = condition.StartedLastDigit,
                                EndedLastDigit = condition.EndedLastDigit
                            })
                           .ToList();
            
            return response;
        }
    }
}