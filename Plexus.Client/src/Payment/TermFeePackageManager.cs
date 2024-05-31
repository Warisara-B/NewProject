using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Plexus.Client.ViewModel.DropDown;
using Plexus.Client.ViewModel.Payment;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Database;
using Plexus.Database.Model.Academic;
using Plexus.Database.Model.Academic.Curriculum;
using Plexus.Database.Model.Academic.Faculty;
using Plexus.Database.Model.Payment;
using Plexus.Entity.Exception;
using Plexus.Utility.Extensions;
using Plexus.Utility.ViewModel;

namespace Plexus.Client.src.Payment
{
    public class TermFeePackageManager : ITermFeePackageManager
    {
        private readonly IUnitOfWork _uow;
        private readonly IAsyncRepository<TermFeePackage> _termFeePackageRepository;
        private readonly IAsyncRepository<TermFeeItem> _termFeeItemRepository;
        private readonly IAsyncRepository<AcademicLevel> _academicLevelRepository;
        private readonly IAsyncRepository<Faculty> _facultyRepository;
        private readonly IAsyncRepository<Department> _departmentRepository;
        private readonly IAsyncRepository<CurriculumVersion> _curriculumVersionRepository;

        public TermFeePackageManager(IUnitOfWork uow,
                                     IAsyncRepository<TermFeePackage> termFeePackageRepository,
                                     IAsyncRepository<TermFeeItem> termFeeItemRepository,
                                     IAsyncRepository<AcademicLevel> academicLevelRepository,
                                     IAsyncRepository<Faculty> facultyRepository,
                                     IAsyncRepository<Department> departmentRepository,
                                     IAsyncRepository<CurriculumVersion> curriculumVersionRepository)
        {
            _uow = uow;
            _termFeePackageRepository = termFeePackageRepository;
            _termFeeItemRepository = termFeeItemRepository;
            _academicLevelRepository = academicLevelRepository;
            _facultyRepository = facultyRepository;
            _departmentRepository = departmentRepository;
            _curriculumVersionRepository = curriculumVersionRepository;
        }

        public TermFeePackageViewModel Create(CreateTermFeePackageViewModel request)
        {
            var termFeePackage = new TermFeePackage
            {
                Name = request.Name,
                Type = request.Type,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "", // TODO : Add requester.
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = "" // TODO : Add requester.
            };

            var termFeeItems = MapTermFeeItems(termFeePackage, request.Items);

            termFeePackage.Items = termFeeItems;

            _uow.BeginTran();
            _termFeePackageRepository.Add(termFeePackage);
            _termFeeItemRepository.AddRange(termFeeItems);
            _uow.Complete();
            _uow.CommitTran();

            var response = MapModelToViewModel(termFeePackage, termFeeItems);

            return response;
        }

        public TermFeePackageViewModel GetById(Guid id)
        {
            var package = _termFeePackageRepository.Query()
                                                   .FirstOrDefault(x => x.Id == id);

            if (package is null)
            {
                throw new TermFeeException.PackageNotFound(id);
            }

            var items = _termFeeItemRepository.Query()
                                              .Where(x => x.TermFeePackageId == package.Id)
                                              .Include(x => x.FeeItem)
                                                .ThenInclude(x => x.Localizations)
                                              .ToList();

            var response = MapModelToViewModel(package, items);

            return response;
        }

        public IEnumerable<BaseDropDownViewModel> GetDropDownList(SearchTermFeePackageCriteriaViewModel parameters)
        {
            var packages = Search(parameters).ToList();

            var response = (from package in packages
                            select MapViewModelToDropDown(package))
                           .ToList();

            return response;
        }

        public IEnumerable<TermFeePackageViewModel> Search(SearchTermFeePackageCriteriaViewModel parameters)
        {
            var query = GenerateSearchQuery(parameters);

            var packages = query.ToList();

            var response = (from package in packages
                            select MapModelToViewModel(package, package.Items))
                           .ToList();

            return response;
        }

        public PagedViewModel<TermFeePackageViewModel> Search(SearchTermFeePackageCriteriaViewModel parameters, int page, int pageSize)
        {
            var query = GenerateSearchQuery(parameters);

            var pagedPackage = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<TermFeePackageViewModel>
            {
                Page = pagedPackage.Page,
                PageSize = pagedPackage.PageSize,
                TotalPage = pagedPackage.TotalPage,
                TotalItem = pagedPackage.TotalItem,
                Items = (from package in pagedPackage.Items
                         select MapModelToViewModel(package, package.Items))
                        .ToList()
            };

            return response;
        }

        public TermFeePackageViewModel Update(Guid id, CreateTermFeePackageViewModel request)
        {
            var package = _termFeePackageRepository.Query()
                                                   .FirstOrDefault(x => x.Id == id);

            if (package is null)
            {
                throw new TermFeeException.PackageNotFound(id);
            }

            // UPDATE PACKAGE INFORMATION
            package.Name = request.Name;
            package.Type = request.Type;
            package.IsActive = request.IsActive;
            package.UpdatedAt = DateTime.UtcNow;
            package.UpdatedBy = ""; // TODO : Add requester.

            var packageItems = MapTermFeeItems(package, request.Items);

            _uow.BeginTran();
            _termFeePackageRepository.Update(package);
            _termFeeItemRepository.DeleteRange(package.Items.ToList());

            if (packageItems.Any())
            {
                _termFeeItemRepository.AddRange(packageItems);
            }

            _uow.Complete();
            _uow.CommitTran();

            var response = MapModelToViewModel(package, packageItems);

            return response;
        }

        public void Delete(Guid id)
        {
            var termFeePackage = _termFeePackageRepository.Query()
                                                          .FirstOrDefault(x => x.Id == id);

            if (termFeePackage is null)
            {
                return;
            }

            _uow.BeginTran();
            _termFeePackageRepository.Delete(termFeePackage);
            _uow.Complete();
            _uow.CommitTran();
        }

        private TermFeePackageViewModel MapModelToViewModel(TermFeePackage package, IEnumerable<TermFeeItem> feeItems)
        {
            var termFeePackage = new TermFeePackageViewModel
            {
                Id = package.Id,
                Name = package.Name,
                Type = package.Type,
            };

            foreach (var item in feeItems)
            {
                var packageFeeItem = new TermFeeItemViewModel
                {
                    Id = item.Id,
                    TermFeePackageId = package.Id,
                    TermFeePackageName = package.Name,
                    FeeItemId = item.FeeItemId,
                    FeeItemName = item.FeeItem.Name,
                    RecurringType = item.RecurringType,
                    Amount = item.Amount,
                    TermNo = item.TermNo,
                    TermRunning = item.TermRunning,
                    TermType = item.TermType.Value,
                    CreatedAt = item.CreatedAt,
                    UpdatedAt = item.UpdatedAt,
                };

                if (item.Conditions is not null)
                {
                    var condition = JsonConvert.DeserializeObject<CreateTermFeeItemConditionViewModel>(item.Conditions)!;

                    AcademicLevel? academicLevel = null;
                    Faculty? faculty = null;
                    Department? department = null;
                    CurriculumVersion? curriculumVersion = null;

                    if (condition.AcademicLevelId.HasValue)
                    {
                        academicLevel = _academicLevelRepository.Query()
                                                                .Include(x => x.Localizations)
                                                                .FirstOrDefault(x => x.Id == condition.AcademicLevelId);
                    }

                    if (condition.FacultyId.HasValue)
                    {
                        faculty = _facultyRepository.Query()
                                                    .Include(x => x.Localizations)
                                                    .FirstOrDefault(x => x.Id == condition.FacultyId);
                    }

                    if (condition.DepartmentId.HasValue)
                    {
                        department = _departmentRepository.Query()
                                                          .Include(x => x.Localizations)
                                                          .FirstOrDefault(x => x.Id == condition.DepartmentId);
                    }

                    if (condition.CurriculumVersionId.HasValue)
                    {
                        curriculumVersion = _curriculumVersionRepository.Query()
                                                                        .Include(x => x.Localizations)
                                                                        .FirstOrDefault(x => x.Id == condition.CurriculumVersionId);
                    }

                    packageFeeItem.Condition = new TermFeeItemConditionViewModel
                    {
                        AcademicLevelId = condition?.AcademicLevelId,
                        AcademicLevelName = academicLevel is null ? null
                                                                  : academicLevel.Name,
                        FacultyId = condition?.FacultyId,
                        FacultyName = faculty is null ? null
                                                      : faculty.Name,
                        DepartmentId = condition?.DepartmentId,
                        DepartmentName = department is null ? null
                                                            : department.Name,
                        CurriculumVersionId = condition?.CurriculumVersionId,
                        CurriculumVersionName = curriculumVersion is null ? null
                                                                          : curriculumVersion.Name,
                        FromBatch = condition?.FromBatch,
                        ToBatch = condition?.ToBatch,
                    };
                }

                termFeePackage.Items.ToList().Add(packageFeeItem);
            }

            return termFeePackage;
        }

        private static IEnumerable<TermFeeItem> MapTermFeeItems(TermFeePackage package, IEnumerable<CreateTermFeeItemViewModel> items)
        {
            var response = (from item in items
                            select new TermFeeItem
                            {
                                TermFeePackage = package,
                                FeeItemId = item.FeeItemId,
                                TermType = item.TermType,
                                RecurringType = item.RecurringType,
                                Conditions = item.Condition is null ? null
                                                                    : JsonConvert.SerializeObject(item.Condition),
                                Amount = item.Amount,
                                IsActive = item.IsActive,
                                CreatedAt = DateTime.UtcNow,
                                CreatedBy = "",
                                UpdatedAt = DateTime.UtcNow,
                                UpdatedBy = ""
                            })
                            .ToList();

            return response;
        }

        private static BaseDropDownViewModel MapViewModelToDropDown(TermFeePackageViewModel viewModel)
        {
            var response = new BaseDropDownViewModel
            {
                Id = viewModel.Id.ToString(),
                Name = viewModel.Name
            };

            return response;
        }

        private IQueryable<TermFeePackage> GenerateSearchQuery(SearchTermFeePackageCriteriaViewModel? parameters = null)
        {
            var query = _termFeePackageRepository.Query()
                                                 .Include(x => x.Items)
                                                    .ThenInclude(x => x.FeeItem)
                                                        .ThenInclude(x => x.Localizations);

            if (parameters is not null)
            {
                if (!string.IsNullOrEmpty(parameters.Name))
                {
                    query.Where(x => x.Name.Contains(parameters.Name));
                }

                if (parameters.IsActive.HasValue)
                {
                    query.Where(x => x.IsActive == parameters.IsActive.Value);
                }

                if (!string.IsNullOrEmpty(parameters.SortBy))
                {
                    try
                    {
                        query.OrderBy(parameters.SortBy, parameters.OrderBy);
                    }
                    catch (System.Exception)
                    {
                        // invalid property name
                    }
                }
            }

            query.OrderBy(x => x.Name);

            return query;
        }
    }
}