using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Client.ViewModel.Registration;
using Plexus.Entity.DTO.Academic;
using Plexus.Entity.DTO.Registration;
using Plexus.Entity.DTO.SearchFilter;
using Plexus.Entity.Exception;
using Plexus.Entity.Provider;
using Plexus.Utility.Extensions;
using Plexus.Utility.ViewModel;

namespace Plexus.Client.src.Registration
{
    public class SlotConditionManager : ISlotConditionManager
    {
        private readonly IPeriodAndSlotProvider _periodAndSlotProvider;
        private readonly ISlotConditionProvider _slotConditionProvider;
        private readonly IFacultyProvider _facultyProvider;
        private readonly IDepartmentProvider _departmentProvider;

        public SlotConditionManager(IPeriodAndSlotProvider periodAndSlotProvider,
                                    ISlotConditionProvider slotConditionProvider,
                                    IFacultyProvider facultyProvider,
                                    IDepartmentProvider departmentProvider)
        {
            _periodAndSlotProvider = periodAndSlotProvider;
            _slotConditionProvider = slotConditionProvider;
            _facultyProvider = facultyProvider;
            _departmentProvider = departmentProvider;
        }

        public SlotConditionViewModel Create(Guid slotId, CreateSlotConditionViewModel request, Guid userId)
        {
            var conditions = ValidateConditions(request.Conditions).ToList();

            var dto = MapViewModelToDTO(request);

            var slotCondition = _slotConditionProvider.Create(slotId, dto, userId.ToString());

            var response = MapDTOToViewModel(slotCondition, conditions);

            return response;
        }

        public PagedViewModel<SlotConditionViewModel> Search(SearchSlotConditionCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var dto = new SearchSlotConditionCriteriaDTO
            {
                Keyword = parameters.Keyword,
                SlotId = parameters.SlotId,
                IsActive = parameters.IsActive,
                SortBy = parameters.SortBy,
                OrderBy = parameters.OrderBy
            };

            var pagedSlotCondition = _slotConditionProvider.Search(dto, page, pageSize);

            var conditions = pagedSlotCondition.Items.SelectMany(x => x.Conditions)
                                                     .ToList();

            var faculties = Enumerable.Empty<FacultyDTO>();
            var departments = Enumerable.Empty<DepartmentDTO>();

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

            var response = new PagedViewModel<SlotConditionViewModel>
            {
                Page = pagedSlotCondition.Page,
                TotalPage = pagedSlotCondition.TotalPage,
                TotalItem = pagedSlotCondition.TotalItem,
                Items = (from slotCondition in pagedSlotCondition.Items
                         select MapDTOToViewModel(slotCondition, faculties, departments))
                        .ToList()
            };

            return response;
        }

        public SlotConditionViewModel GetById(Guid id)
        {
            var slotCondition = _slotConditionProvider.GetById(id);

            var faculties = Enumerable.Empty<FacultyDTO>();
            var departments = Enumerable.Empty<DepartmentDTO>();

            var facultyIds = slotCondition.Conditions.Where(x => x.FacultyId.HasValue)
                                                     .Select(x => x.FacultyId!.Value)
                                                     .Distinct()
                                                     .ToList();

            if (facultyIds.Any())
            {
                faculties = _facultyProvider.GetById(facultyIds)
                                            .ToList();
            }

            var departmentIds = slotCondition.Conditions.Where(x => x.DepartmentId.HasValue)
                                                        .Select(x => x.DepartmentId!.Value)
                                                        .Distinct()
                                                        .ToList();

            if (departmentIds.Any())
            {
                departments = _departmentProvider.GetById(departmentIds)
                                                 .ToList();
            }

            var response = MapDTOToViewModel(slotCondition, faculties, departments);

            return response;
        }

        public SlotConditionViewModel Update(Guid slotId, Guid id, CreateSlotConditionViewModel request, Guid userId)
        {
            var slot = _periodAndSlotProvider.GetSlotById(slotId);

            var slotConditions = _slotConditionProvider.GetBySlotId(slotId)
                                                       .ToList();

            var slotCondition = slotConditions.SingleOrDefault(x => x.Id == id);

            if (slotCondition is null)
            {
                throw new SlotConditionException.NotFound(id);
            }

            var conditions = ValidateConditions(request.Conditions).ToList();

            slotCondition.Conditions = (from condition in request.Conditions
                                        select new ConditionDTO
                                        {
                                            FacultyId = condition.FacultyId,
                                            DepartmentId = condition.DepartmentId,
                                            Codes = condition.Codes.SplitWithCommaSeparator(),
                                            StartedCode = condition.StartedCode,
                                            EndedCode = condition.EndedCode,
                                            StartedBatch = condition.StartedBatch,
                                            EndedBatch = condition.EndedBatch,
                                        })
                                       .ToList();

            slotCondition.IsActive = request.IsActive;

            var updatedAudienceGroup = _slotConditionProvider.Update(slotCondition, userId.ToString());

            var response = MapDTOToViewModel(updatedAudienceGroup, conditions);

            return response;
        }

        public void Delete(Guid id)
        {
            _slotConditionProvider.Delete(id);
        }

        private static SlotConditionViewModel MapDTOToViewModel(SlotConditionDTO dto,
            IEnumerable<FacultyDTO> faculties,
            IEnumerable<DepartmentDTO> departments)
        {
            var response = new SlotConditionViewModel
            {
                Id = dto.Id,
                SlotId = dto.SlotId,
                IsActive = dto.IsActive,
                Conditions = (from condition in dto.Conditions

                              let faculty = condition.FacultyId.HasValue ? faculties.SingleOrDefault(x => x.Id == condition.FacultyId.Value)
                                                                         : null
                              let department = condition.DepartmentId.HasValue ? departments.SingleOrDefault(x => x.Id == condition.DepartmentId.Value)
                                                                               : null
                              select new ConditionViewModel
                              {
                                  FacultyId = condition.FacultyId,
                                  FacultyName = faculty?.Name,
                                  DepartmentId = condition.DepartmentId,
                                  DepartmentName = department?.Name,
                                  Codes = condition.Codes.ToStringWithCommaSeparator(),
                                  StartedCode = condition.StartedCode,
                                  EndedCode = condition.EndedCode,
                                  StartedBatch = condition.StartedBatch,
                                  EndedBatch = condition.EndedBatch,
                              })
                             .ToList(),
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt
            };

            return response;
        }

        private SlotConditionViewModel MapDTOToViewModel(SlotConditionDTO dto, IEnumerable<ConditionViewModel> conditions)
        {
            var response = new SlotConditionViewModel
            {
                Id = dto.Id,
                SlotId = dto.SlotId,
                IsActive = dto.IsActive,
                Conditions = conditions,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt
            };

            return response;
        }

        private static CreateSlotConditionDTO MapViewModelToDTO(CreateSlotConditionViewModel request)
        {
            var dto = new CreateSlotConditionDTO
            {
                IsActive = request.IsActive,
                Conditions = (from condition in request.Conditions
                              select new ConditionDTO
                              {
                                  FacultyId = condition.FacultyId,
                                  DepartmentId = condition.DepartmentId,
                                  Codes = condition.Codes.SplitWithCommaSeparator(),
                                  StartedCode = condition.StartedCode,
                                  EndedCode = condition.EndedCode,
                                  StartedBatch = condition.StartedBatch,
                                  EndedBatch = condition.EndedBatch,
                              })
                             .ToList()
            };

            return dto;
        }

        private IEnumerable<ConditionViewModel> ValidateConditions(IEnumerable<CreateConditionViewModel> conditions)
        {
            if (conditions is null || !conditions.Any())
            {
                throw new SlotConditionException.InvalidConditions();
            }

            var emptyConditions = conditions.Where(x => !x.FacultyId.HasValue
                                                        && !x.DepartmentId.HasValue
                                                        && string.IsNullOrEmpty(x.Codes)
                                                        && string.IsNullOrEmpty(x.StartedCode)
                                                        && string.IsNullOrEmpty(x.EndedCode)
                                                        && !x.StartedBatch.HasValue
                                                        && !x.EndedBatch.HasValue)
                                            .ToList();

            if (emptyConditions.Any())
            {
                throw new AudienceGroupException.InvalidConditions();
            }

            var facultyIds = conditions.Where(x => x.FacultyId.HasValue)
                                       .Select(x => x.FacultyId!.Value)
                                       .Distinct()
                                       .ToList();

            var departmentIds = conditions.Where(x => x.DepartmentId.HasValue)
                                          .Select(x => x.DepartmentId!.Value)
                                          .Distinct()
                                          .ToList();

            var faculties = Enumerable.Empty<FacultyDTO>();
            var departments = Enumerable.Empty<DepartmentDTO>();

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

                            let faculty = condition.FacultyId.HasValue ? faculties.SingleOrDefault(x => x.Id == condition.FacultyId.Value)
                                                                       : null
                            let department = condition.DepartmentId.HasValue ? departments.SingleOrDefault(x => x.Id == condition.DepartmentId.Value)
                                                                             : null
                            select new ConditionViewModel
                            {
                                FacultyId = condition.FacultyId,
                                FacultyName = faculty?.Name,
                                DepartmentId = condition.DepartmentId,
                                DepartmentName = department?.Name,
                                Codes = condition.Codes,
                                StartedCode = condition.StartedCode,
                                EndedCode = condition.EndedCode,
                                StartedBatch = condition.StartedBatch,
                                EndedBatch = condition.EndedBatch
                            })
                           .ToList();

            return response;
        }
    }
}