
using Plexus.Client.ViewModel.Academic.Section;
using Plexus.Entity.DTO.Academic;
using Plexus.Entity.DTO.Academic.Curriculum;
using Plexus.Entity.DTO.Academic.Section;
using Plexus.Entity.Exception;
using Plexus.Entity.Provider;
using Plexus.Utility.Extensions;

namespace Plexus.Client.src.Academic
{
    public class ExclusionConditionManager : IExclusionConditionManager
    {
        private readonly IExclusionConditionProvider _exclusionConditionProvider;
        private readonly ISectionProvider _sectionProvider;
        private readonly IFacultyProvider _facultyProvider;
        private readonly IDepartmentProvider _departmentProvider;
        private readonly ICurriculumProvider _curriculumProvider;
        private readonly ICurriculumVersionProvider _curriculumVersionProvider;

        public ExclusionConditionManager(IExclusionConditionProvider exclusionConditionProvider,
                                          ISectionProvider sectionProvider,
                                         IFacultyProvider facultyProvider,
                                         IDepartmentProvider departmentProvider,
                                         ICurriculumProvider curriculumProvider,
                                         ICurriculumVersionProvider curriculumVersionProvider)
        {
            _exclusionConditionProvider = exclusionConditionProvider;
            _sectionProvider = sectionProvider;
            _facultyProvider = facultyProvider;
            _departmentProvider = departmentProvider;
            _curriculumProvider = curriculumProvider;
            _curriculumVersionProvider = curriculumVersionProvider;
        }

        public ExclusionConditionViewModel Create(Guid sectionId, CreateExclusionConditionViewModel request, Guid userId)
        {
            var section = _sectionProvider.GetById(sectionId);

            var conditions = ValidateConditions(request.Conditions);

            var dto = new CreateExclusionConditionDTO
            {
                SectionId = sectionId,
                Name = request.Name,
                Description = request.Description,
                Conditions = (from condition in conditions
                              select new SectionConditionDTO
                              {
                                  FacultyId = condition.FacultyId,
                                  DepartmentId = condition.DepartmentId,
                                  CurriculumId = condition.CurriculumId,
                                  CurriculumVersionId = condition.CurriculumVersionId,
                                  Batches = condition.Batches is not null && condition.Batches.Any() ? (from batch in condition.Batches
                                                                                                        select batch)
                                                                                                       .ToList()
                                                                                                     : null,
                                  Codes = condition.Codes.SplitWithCommaSeparator()
                              })
                             .ToList()
            };

            var exclusionCondition = _exclusionConditionProvider.Create(dto, userId.ToString());

            var response = MapDTOToViewModel(exclusionCondition, section, conditions);

            return response;
        }

        public IEnumerable<ExclusionConditionViewModel> GetBySectionId(Guid sectionId)
        {
            var section = _sectionProvider.GetById(sectionId);

            var exclusionConditions = _exclusionConditionProvider.GetBySectionId(sectionId)
                                                                 .ToList();

            var validConditions = exclusionConditions.SelectMany(x => x.Conditions)
                                                     .ToList();

            var facultyIds = validConditions.Where(x => x.FacultyId.HasValue)
                                            .Select(x => x.FacultyId!.Value)
                                            .Distinct()
                                            .ToList();

            var faculties = _facultyProvider.GetById(facultyIds)
                                            .ToList();

            var departmentIds = validConditions.Where(x => x.DepartmentId.HasValue)
                                               .Select(x => x.DepartmentId!.Value)
                                               .Distinct()
                                               .ToList();

            var departments = _departmentProvider.GetById(departmentIds)
                                                 .ToList();

            var curriculumIds = validConditions.Where(x => x.CurriculumId.HasValue)
                                               .Select(x => x.CurriculumId!.Value)
                                               .Distinct()
                                               .ToList();

            var curriculums = _curriculumProvider.GetById(curriculumIds)
                                                 .ToList();

            var curriculumVersionIds = validConditions.Where(x => x.CurriculumVersionId.HasValue)
                                                      .Select(x => x.CurriculumVersionId!.Value)
                                                      .Distinct()
                                                      .ToList();

            var curriculumVersions = _curriculumVersionProvider.GetById(curriculumVersionIds)
                                                               .ToList();

            var response = (from exclusionCondition in exclusionConditions
                            let conditions = (from condition in exclusionCondition.Conditions
                                              let faculty = condition.FacultyId.HasValue ? faculties.SingleOrDefault(x => x.Id == condition.FacultyId!.Value)
                                                                                         : null
                                              let department = condition.DepartmentId.HasValue ? departments.SingleOrDefault(x => x.Id == condition.DepartmentId!.Value)
                                                                                               : null
                                              let curriculum = condition.CurriculumId.HasValue ? curriculums.SingleOrDefault(x => x.Id == condition.CurriculumId!.Value)
                                                                                               : null
                                              let curriculumVersion = condition.CurriculumVersionId.HasValue ? curriculumVersions.SingleOrDefault(x => x.Id == condition.CurriculumVersionId!.Value)
                                                                                                             : null
                                              select MapConditionDTOToModel(condition, faculty, department, curriculum, curriculumVersion))
                                             .ToList()
                            select MapDTOToViewModel(exclusionCondition, section, conditions))
                           .ToList();

            return response;
        }

        public ExclusionConditionViewModel GetById(Guid id)
        {
            var exclusionCondition = _exclusionConditionProvider.GetById(id);

            var section = _sectionProvider.GetById(exclusionCondition.SectionId);

            var facultyIds = exclusionCondition.Conditions.Where(x => x.FacultyId.HasValue)
                                                          .Select(x => x.FacultyId!.Value)
                                                          .Distinct()
                                                          .ToList();

            var faculties = _facultyProvider.GetById(facultyIds)
                                            .ToList();

            var departmentIds = exclusionCondition.Conditions.Where(x => x.DepartmentId.HasValue)
                                                             .Select(x => x.DepartmentId!.Value)
                                                             .Distinct()
                                                             .ToList();

            var departments = _departmentProvider.GetById(departmentIds)
                                                 .ToList();

            var curriculumIds = exclusionCondition.Conditions.Where(x => x.CurriculumId.HasValue)
                                                             .Select(x => x.CurriculumId!.Value)
                                                             .Distinct()
                                                             .ToList();

            var curriculums = _curriculumProvider.GetById(curriculumIds)
                                                 .ToList();

            var curriculumVersionIds = exclusionCondition.Conditions.Where(x => x.CurriculumVersionId.HasValue)
                                                                    .Select(x => x.CurriculumVersionId!.Value)
                                                                    .Distinct()
                                                                    .ToList();

            var curriculumVersions = _curriculumVersionProvider.GetById(curriculumVersionIds)
                                                               .ToList();

            var conditions = (from condition in exclusionCondition.Conditions
                              let faculty = condition.FacultyId.HasValue ? faculties.SingleOrDefault(x => x.Id == condition.FacultyId!.Value)
                                                                         : null
                              let department = condition.DepartmentId.HasValue ? departments.SingleOrDefault(x => x.Id == condition.DepartmentId!.Value)
                                                                               : null
                              let curriculum = condition.CurriculumId.HasValue ? curriculums.SingleOrDefault(x => x.Id == condition.CurriculumId!.Value)
                                                                               : null
                              let curriculumVersion = condition.CurriculumVersionId.HasValue ? curriculumVersions.SingleOrDefault(x => x.Id == condition.CurriculumVersionId!.Value)
                                                                                             : null
                              select MapConditionDTOToModel(condition, faculty, department, curriculum, curriculumVersion))
                             .ToList();

            var response = MapDTOToViewModel(exclusionCondition, section, conditions);

            return response;
        }

        public ExclusionConditionViewModel Update(ExclusionConditionViewModel request, Guid userId)
        {
            var exclusionCondition = _exclusionConditionProvider.GetById(request.Id);

            var section = _sectionProvider.GetById(request.SectionId);

            var conditions = ValidateConditions(request.Conditions);

            exclusionCondition.Name = request.Name;
            exclusionCondition.Description = request.Description;
            exclusionCondition.Conditions = (from condition in conditions
                                             select new SectionConditionDTO
                                             {
                                                 FacultyId = condition.FacultyId,
                                                 DepartmentId = condition.DepartmentId,
                                                 CurriculumId = condition.CurriculumId,
                                                 CurriculumVersionId = condition.CurriculumVersionId,
                                                 Batches = condition.Batches is not null && condition.Batches.Any() ? (from batch in condition.Batches
                                                                                                                       select batch)
                                                                                                                      .ToList()
                                                                                                                    : null,
                                                 Codes = condition.Codes.SplitWithCommaSeparator()
                                             })
                                            .ToList();

            var updatedExclusionCondition = _exclusionConditionProvider.Update(exclusionCondition, userId.ToString());

            var response = MapDTOToViewModel(updatedExclusionCondition, section, conditions);

            return response;
        }

        public void Delete(Guid id)
        {
            _exclusionConditionProvider.Delete(id);
        }

        private static ExclusionConditionViewModel MapDTOToViewModel(ExclusionConditionDTO dto,
                                                                     SectionDTO section,
                                                                     IEnumerable<SectionConditionViewModel> conditions)
        {
            var response = new ExclusionConditionViewModel
            {
                Id = dto.Id,
                SectionId = dto.SectionId,
                SectionNumber = section.Number,
                Name = dto.Name,
                Description = dto.Description,
                Conditions = conditions,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt
            };

            return response;
        }

        private SectionConditionViewModel MapConditionDTOToModel(SectionConditionDTO dto,
                                                                            FacultyDTO? faculty = null,
                                                                            DepartmentDTO? department = null,
                                                                            CurriculumDTO? curriculum = null,
                                                                            CurriculumVersionDTO? curriculumVersion = null)
        {
            var response = new SectionConditionViewModel
            {
                FacultyId = dto.FacultyId,
                FacultyName = faculty?.Name,
                DepartmentId = dto.DepartmentId,
                DepartmentName = department?.Name,
                CurriculumId = dto.CurriculumId,
                CurriculumName = curriculum?.Name,
                CurriculumVersionId = dto.CurriculumVersionId,
                CurriculumVersionName = curriculumVersion?.Name,
                Batches = dto.Batches,
                Codes = dto.Codes.ToStringWithCommaSeparator()
            };

            return response;
        }

        private IEnumerable<SectionConditionViewModel> ValidateConditions(IEnumerable<CreateSectionConditionViewModel> conditions)
        {
            if (conditions is not null && conditions.Any())
            {
                var results = new List<SectionConditionViewModel>();

                foreach (var condition in conditions)
                {
                    var isValid = false;

                    var response = new SectionConditionViewModel
                    {
                        FacultyId = condition.FacultyId,
                        DepartmentId = condition.DepartmentId,
                        CurriculumId = condition.CurriculumId,
                        CurriculumVersionId = condition.CurriculumVersionId
                    };

                    if (condition.FacultyId.HasValue)
                    {
                        isValid = true;

                        var faculty = _facultyProvider.GetById(condition.FacultyId.Value);
                        response.FacultyName = faculty.Name;
                    }

                    if (condition.DepartmentId.HasValue)
                    {
                        isValid = true;

                        var department = _departmentProvider.GetById(condition.DepartmentId.Value);
                        response.DepartmentName = department.Name;
                    }

                    if (condition.CurriculumId.HasValue)
                    {
                        isValid = true;

                        var curriculum = _curriculumProvider.GetById(condition.CurriculumId.Value);
                        response.CurriculumName = curriculum.Name;
                    }

                    if (condition.CurriculumVersionId.HasValue)
                    {
                        isValid = true;

                        var curriculumVersion = _curriculumVersionProvider.GetById(condition.CurriculumVersionId.Value);
                        response.CurriculumVersionName = curriculumVersion.Name;
                    }

                    if (condition.Batches is not null && condition.Batches.Any())
                    {
                        isValid = true;

                        response.Batches = (from batch in condition.Batches
                                            select batch)
                                           .ToList();
                    }

                    response.Codes = condition.Codes;

                    if (!isValid)
                    {
                        throw new ExclusionConditionException.ConditionNotSpecify();
                    }

                    results.Add(response);
                }

                return results;
            }

            throw new ExclusionConditionException.ConditionNotSpecify();
        }
    }
}