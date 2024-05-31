using Plexus.Client.ViewModel.Payment;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Academic;
using Plexus.Entity.DTO.Academic.Curriculum;
using Plexus.Entity.DTO.Payment;
using Plexus.Entity.Provider;
using Plexus.Utility.ViewModel;

namespace Plexus.Client.src.Payment
{
    public class TermFeeItemManager : ITermFeeItemManager
    {
        private readonly ITermFeePackageProvider _termFeePackageProvider;
        private readonly ITermFeeItemProvider _termFeeItemProvider;
        private readonly IFeeItemProvider _feeItemProvider;
        private readonly IAcademicLevelProvider _academicLevelProvider;
        private readonly IFacultyProvider _facultyProvider;
        private readonly IDepartmentProvider _departmentProvider;
        private readonly ICurriculumProvider _curriculumProvider;
        private readonly ICurriculumVersionProvider _curriculumVersionProvider;
        private readonly IStudentFeeTypeProvider _studentFeeTypeProvider;

        public TermFeeItemManager(ITermFeePackageProvider termFeePackageProvider,
                                  ITermFeeItemProvider termFeeItemProvider,
                                  IFeeItemProvider feeItemProvider,
                                  IAcademicLevelProvider academicLevelProvider,
                                  IFacultyProvider facultyProvider,
                                  IDepartmentProvider departmentProvider,
                                  ICurriculumProvider curriculumProvider,
                                  ICurriculumVersionProvider curriculumVersionProvider,
                                  IStudentFeeTypeProvider studentFeeTypeProvider)
        {
            _termFeePackageProvider = termFeePackageProvider;
            _termFeeItemProvider = termFeeItemProvider;
            _feeItemProvider = feeItemProvider;
            _academicLevelProvider = academicLevelProvider;
            _facultyProvider = facultyProvider;
            _departmentProvider = departmentProvider;
            _curriculumProvider = curriculumProvider;
            _curriculumVersionProvider = curriculumVersionProvider;
            _studentFeeTypeProvider = studentFeeTypeProvider;
        }

        public TermFeeItemViewModel Create(CreateTermFeeItemViewModel request, Guid userId)
        {
            throw new NotImplementedException();
            // var package = _termFeePackageProvider.GetById(request.TermFeePackageId);

            // var feeItem = _feeItemProvider.GetById(request.FeeItemId);

            // var (academicLevel, faculty, department, version) = ValidateCondition(request.Condition);

            // var dto = new CreateTermFeeItemDTO
            // {
            //     TermFeePackageId = request.TermFeePackageId,
            //     FeeItemId = request.FeeItemId,
            //     RecurringType = request.RecurringType,
            //     Condition = MapConditionViewModelToDTO(request.Condition),
            //     Amount = request.Amount,
            //     IsActive = request.IsActive
            // };

            // var item = _termFeeItemProvider.Create(dto, userId.ToString());

            // var response = MapDTOToViewModel(item, package, feeItem, academicLevel, faculty, department, version);

            // return response;
        }

        public PagedViewModel<TermFeeItemViewModel> Search(SearchCriteriaViewModel parameters, int page, int pageSize)
        {
            var pagedItems = _termFeeItemProvider.Search(parameters, page, pageSize);

            var termFeePackageIds = pagedItems.Items.Select(x => x.TermFeePackageId)
                                                    .Distinct()
                                                    .ToList();

            var packages = _termFeePackageProvider.GetById(termFeePackageIds)
                                                  .ToList();

            var feeItemIds = pagedItems.Items.Select(x => x.FeeItemId)
                                             .Distinct()
                                             .ToList();

            var feeItems = _feeItemProvider.GetById(feeItemIds)
                                           .ToList();

            var academicLevelIds = pagedItems.Items.Where(x => x.Condition != null
                                                               && x.Condition.AcademicLevelId.HasValue)
                                                   .Select(x => x.Condition!.AcademicLevelId!.Value)
                                                   .Distinct()
                                                   .ToList();

            var academicLevels = _academicLevelProvider.GetById(academicLevelIds)
                                                       .ToList();

            var facultyIds = pagedItems.Items.Where(x => x.Condition != null
                                                         && x.Condition.FacultyId.HasValue)
                                             .Select(x => x.Condition!.FacultyId!.Value)
                                             .Distinct()
                                             .ToList();

            var faculties = _facultyProvider.GetById(facultyIds)
                                            .ToList();

            var departmentIds = pagedItems.Items.Where(x => x.Condition != null
                                                            && x.Condition.DepartmentId.HasValue)
                                                .Select(x => x.Condition!.DepartmentId!.Value)
                                                .Distinct()
                                                .ToList();

            var departments = _departmentProvider.GetById(departmentIds)
                                                 .ToList();

            var curriculumIds = pagedItems.Items.Where(x => x.Condition != null
                                                            && x.Condition.CurriculumId.HasValue)
                                                .Select(x => x.Condition!.CurriculumId!.Value)
                                                .Distinct()
                                                .ToList();

            var curriculums = _curriculumProvider.GetById(curriculumIds)
                                                 .ToList();

            var versionIds = pagedItems.Items.Where(x => x.Condition != null
                                                         && x.Condition.CurriculumVersionId.HasValue)
                                             .Select(x => x.Condition!.CurriculumVersionId!.Value)
                                             .Distinct()
                                             .ToList();

            var versions = _curriculumVersionProvider.GetById(versionIds)
                                                     .ToList();

            var studentFeeTypeIds = pagedItems.Items.Where(x => x.Condition != null
                                                                && x.Condition.StudentFeeTypeId.HasValue)
                                                    .Select(x => x.Condition!.StudentFeeTypeId!.Value)
                                                    .Distinct()
                                                    .ToList();

            var studentFeeTypes = _studentFeeTypeProvider.GetById(studentFeeTypeIds)
                                                         .ToList();

            var response = new PagedViewModel<TermFeeItemViewModel>
            {
                Page = pagedItems.Page,
                TotalPage = pagedItems.TotalPage,
                TotalItem = pagedItems.TotalItem,
                Items = (from item in pagedItems.Items
                         let package = packages.SingleOrDefault(x => x.Id == item.TermFeePackageId)
                         let feeItem = feeItems.SingleOrDefault(x => x.Id == item.FeeItemId)
                         let academicLevel = item.Condition is null
                             || !item.Condition!.AcademicLevelId.HasValue ? null
                                                                          : academicLevels.SingleOrDefault(x => x.Id == item.Condition!.AcademicLevelId!.Value)
                         let faculty = item.Condition is null
                                       || !item.Condition!.FacultyId.HasValue ? null
                                                                              : faculties.Single(x => x.Id == item.Condition!.FacultyId!.Value)
                         let department = item.Condition is null
                                          || !item.Condition!.DepartmentId.HasValue ? null
                                                                                    : departments.SingleOrDefault(x => x.Id == item.Condition!.DepartmentId!.Value)
                         let curriculum = item.Condition is null
                                          || !item.Condition!.CurriculumId.HasValue ? null
                                                                                    : curriculums.SingleOrDefault(x => x.Id == item.Condition!.CurriculumId!.Value)
                         let version = item.Condition is null
                                       || !item.Condition!.CurriculumVersionId.HasValue ? null
                                                                                        : versions.SingleOrDefault(x => x.Id == item.Condition!.CurriculumVersionId!.Value)
                         let studentFeeType = item.Condition is null
                                              || !item.Condition!.StudentFeeTypeId.HasValue ? null
                                                                                            : studentFeeTypes.SingleOrDefault(x => x.Id == item.Condition!.StudentFeeTypeId!.Value)
                         select MapDTOToViewModel(item, package, feeItem, academicLevel, faculty, department, version))
                        .ToList()
            };

            return response;
        }

        public TermFeeItemViewModel GetById(Guid id)
        {
            var item = _termFeeItemProvider.GetById(id);

            var package = _termFeePackageProvider.GetById(item.TermFeePackageId);

            var feeItem = _feeItemProvider.GetById(item.FeeItemId);

            if (item.Condition is null)
            {
                var model = MapDTOToViewModel(item, package, feeItem);

                return model;
            }

            var condition = item.Condition!;

            var academicLevel = condition.AcademicLevelId.HasValue ? _academicLevelProvider.GetById(condition.AcademicLevelId.Value)
                                                                   : null;

            var faculty = condition.FacultyId.HasValue ? _facultyProvider.GetById(condition.FacultyId.Value)
                                                       : null;

            var department = condition.DepartmentId.HasValue ? _departmentProvider.GetById(condition.DepartmentId.Value)
                                                             : null;

            var curriculum = condition.CurriculumId.HasValue ? _curriculumProvider.GetById(condition.CurriculumId.Value)
                                                             : null;

            var version = condition.CurriculumVersionId.HasValue ? _curriculumVersionProvider.GetById(condition.CurriculumVersionId.Value)
                                                                 : null;

            var studentFeeType = condition.StudentFeeTypeId.HasValue ? _studentFeeTypeProvider.GetById(condition.StudentFeeTypeId.Value)
                                                                     : null;

            var response = MapDTOToViewModel(item, package, feeItem, academicLevel, faculty, department, version);

            return response;
        }

        public TermFeeItemViewModel Update(TermFeeItemViewModel request, Guid userId)
        {
            var item = _termFeeItemProvider.GetById(request.Id);

            var package = _termFeePackageProvider.GetById(request.TermFeePackageId);

            var feeItem = _feeItemProvider.GetById(request.FeeItemId);

            var (academicLevel, faculty, department, version) = ValidateCondition(request.Condition);

            item.TermFeePackageId = request.TermFeePackageId;
            item.FeeItemId = request.FeeItemId;
            item.RecurringType = request.RecurringType;
            item.Condition = MapConditionViewModelToDTO(request.Condition);
            item.Amount = request.Amount;
            item.IsActive = request.IsActive;

            var updatedItem = _termFeeItemProvider.Update(item, userId.ToString());

            var response = MapDTOToViewModel(updatedItem, package, feeItem, academicLevel, faculty, department, version);

            return response;
        }

        public void Delete(Guid id)
        {
            _termFeeItemProvider.Delete(id);
        }

        private static TermFeeItemViewModel MapDTOToViewModel(TermFeeItemDTO dto,
                                                                   TermFeePackageDTO package,
                                                                   FeeItemDTO feeItem,
                                                                   AcademicLevelDTO? academicLevel = null,
                                                                   FacultyDTO? faculty = null,
                                                                   DepartmentDTO? department = null,
                                                                   CurriculumVersionDTO? version = null)
        {
            var condition = dto.Condition is null ? null
                                                  : new TermFeeItemConditionViewModel
                                                  {
                                                      AcademicLevelId = dto.Condition.AcademicLevelId,
                                                      AcademicLevelName = academicLevel?.Name,
                                                      FacultyId = dto.Condition.FacultyId,
                                                      FacultyName = faculty?.Name,
                                                      DepartmentId = dto.Condition.DepartmentId,
                                                      DepartmentName = department?.Name,
                                                      CurriculumVersionId = dto.Condition.CurriculumVersionId,
                                                      CurriculumVersionName = version?.Name,
                                                      FromBatch = dto.Condition.FromBatch,
                                                      ToBatch = dto.Condition.ToBatch
                                                  };

            var response = new TermFeeItemViewModel
            {
                Id = dto.Id,
                TermFeePackageId = dto.TermFeePackageId,
                FeeItemId = dto.FeeItemId,
                RecurringType = dto.RecurringType,
                Condition = condition,
                Amount = dto.Amount,
                IsActive = dto.IsActive,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt,
                FeeItemName = feeItem.Name,
                TermFeePackageName = package.Name
            };

            return response;
        }

        private (AcademicLevelDTO?, FacultyDTO?, DepartmentDTO?, CurriculumVersionDTO?) ValidateCondition(CreateTermFeeItemConditionViewModel? condition)
        {
            if (condition is null)
            {
                return (null, null, null, null);
            }

            var academicLevel = condition.AcademicLevelId.HasValue ? _academicLevelProvider.GetById(condition.AcademicLevelId.Value)
                                                                   : null;

            var faculty = condition.FacultyId.HasValue ? _facultyProvider.GetById(condition.FacultyId.Value)
                                                       : null;

            var department = condition.DepartmentId.HasValue ? _departmentProvider.GetById(condition.DepartmentId.Value)
                                                             : null;

            var version = condition.CurriculumVersionId.HasValue ? _curriculumVersionProvider.GetById(condition.CurriculumVersionId.Value)
                                                                 : null;

            return (academicLevel, faculty, department, version);
        }

        private static TermFeeItemConditionDTO? MapConditionViewModelToDTO(CreateTermFeeItemConditionViewModel? condition)
        {
            if (condition is null)
            {
                return null;
            }

            var response = new TermFeeItemConditionDTO
            {
                AcademicLevelId = condition.AcademicLevelId,
                FacultyId = condition.FacultyId,
                DepartmentId = condition.DepartmentId,
                CurriculumVersionId = condition.CurriculumVersionId,
                FromBatch = condition.FromBatch,
                ToBatch = condition.ToBatch
            };

            return response;
        }
    }
}