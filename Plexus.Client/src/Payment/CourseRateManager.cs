using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
using ServiceStack;

namespace Plexus.Client.src.Payment
{
    public class CourseRateManager : ICourseRateManager
    {
        private readonly IUnitOfWork _uow;
        private readonly IAsyncRepository<CourseRate> _courseRateRepository;
        private readonly IAsyncRepository<RateType> _rateTypeRepository;
        private readonly IAsyncRepository<AcademicLevel> _academicLevelRepository;
        private readonly IAsyncRepository<Faculty> _facultyRepository;
        private readonly IAsyncRepository<Department> _departmentRepository;
        private readonly IAsyncRepository<CurriculumVersion> _curriculumVersionRepository;

        public CourseRateManager(IUnitOfWork uow,
                                 IAsyncRepository<CourseRate> courseRateRepository,
                                 IAsyncRepository<RateType> rateTypeRepository,
                                 IAsyncRepository<AcademicLevel> academicLevelRepository,
                                 IAsyncRepository<Faculty> facultyRepository,
                                 IAsyncRepository<Department> departmentRepository,
                                 IAsyncRepository<CurriculumVersion> curriculumVersionRepository)
        {
            _uow = uow;
            _courseRateRepository = courseRateRepository;
            _rateTypeRepository = rateTypeRepository;
            _academicLevelRepository = academicLevelRepository;
            _facultyRepository = facultyRepository;
            _departmentRepository = departmentRepository;
            _curriculumVersionRepository = curriculumVersionRepository;
        }

        public CourseRateViewModel Create(CreateCourseRateViewModel request)
        {
            var conditions = ValidateConditions(request.Conditions);

            var existingCourseRates = _courseRateRepository.Query()
                                                           .ToList();

            var rateType = _rateTypeRepository.Query()
                                              .FirstOrDefault(x => x.Id == request.RateTypeId);

            if (rateType is null)
            {
                throw new RateTypeException.NotFound(request.RateTypeId);
            }


            var duplicateIndexes = existingCourseRates.GroupBy(x => new { x.RateTypeId, x.Index })
                                                      .Where(x => x.Count() > 1)
                                                      .ToList();

            if (duplicateIndexes.Any())
            {
                throw new CourseRateException.DuplicateIndexes();
            }

            var model = new CourseRate
            {
                Name = request.Name,
                IsActive = request.IsActive,
                Conditions = request.Conditions is null ? null
                                                        : JsonConvert.SerializeObject(request.Conditions),
                RateTypeId = request.RateTypeId,
                Index = request.Index,
                Amount = request.Amount,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "", // TODO : Add requester.
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = "" // TODO : Add requester.
            };

            _uow.BeginTran();
            _courseRateRepository.Add(model);
            _uow.Complete();
            _uow.CommitTran();

            var response = MapModelToViewModel(model, rateType);

            return response;
        }

        public CourseRateViewModel GetById(Guid id)
        {
            var courseRate = _courseRateRepository.Query()
                                                  .FirstOrDefault(x => x.Id == id);

            if (courseRate is null)
            {
                throw new CourseRateException.NotFound(id);
            }

            var rateType = _rateTypeRepository.Query()
                                              .Include(x => x.Localizations)
                                              .FirstOrDefault(x => x.Id == courseRate.RateTypeId)!;

            var response = MapModelToViewModel(courseRate, rateType);

            return response;
        }

        public IEnumerable<CourseRateViewModel> Search(SearchCourseRateCriteriaViewModel? parameters)
        {
            var query = GenerateSearchQuery(parameters);

            var courseRates = query.ToList();

            var response = (from rate in courseRates
                            select MapModelToViewModel(rate, rate.RateType))
                           .ToList();

            return response;
        }

        public PagedViewModel<CourseRateViewModel> Search(SearchCourseRateCriteriaViewModel? parameters, int page, int pageSize)
        {
            var query = GenerateSearchQuery(parameters);

            var pagedCourseRate = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<CourseRateViewModel>
            {
                Page = pagedCourseRate.Page,
                PageSize = pagedCourseRate.PageSize,
                TotalPage = pagedCourseRate.TotalPage,
                TotalItem = pagedCourseRate.TotalItem,
                Items = (from courseRate in pagedCourseRate.Items
                         select MapModelToViewModel(courseRate, courseRate.RateType))
                        .ToList()
            };

            return response;
        }

        public CourseRateViewModel Update(Guid id, CreateCourseRateViewModel request)
        {
            var courseRate = _courseRateRepository.Query()
                                                  .FirstOrDefault(x => x.Id == id);

            if (courseRate is null)
            {
                throw new CourseRateException.NotFound(id);
            }

            var condition = ValidateConditions(request.Conditions);

            var existingCourseRates = _courseRateRepository.Query()
                                                           .ToList();

            var rateType = _rateTypeRepository.Query()
                                              .FirstOrDefault(x => x.Id == request.RateTypeId);

            if (rateType is null)
            {
                throw new RateTypeException.NotFound(request.RateTypeId);
            }


            var duplicateIndexes = existingCourseRates.GroupBy(x => new { x.RateTypeId, x.Index })
                                                      .Where(x => x.Count() > 1)
                                                      .ToList();

            courseRate.Name = request.Name;
            courseRate.IsActive = request.IsActive;
            courseRate.Conditions = request.Conditions is null ? null
                                                               : JsonConvert.SerializeObject(request.Conditions);
            courseRate.RateTypeId = request.RateTypeId;
            courseRate.Index = request.Index;
            courseRate.Amount = request.Amount;

            _uow.BeginTran();
            _courseRateRepository.Update(courseRate);
            _uow.Complete();
            _uow.CommitTran();

            var response = MapModelToViewModel(courseRate, rateType);

            return response;
        }

        public void Delete(Guid id)
        {
            var courseRate = _courseRateRepository.Query()
                                                  .FirstOrDefault(x => x.Id == id);

            if (courseRate is null)
            {
                return;
            }

            _uow.BeginTran();
            _courseRateRepository.Delete(courseRate);
            _uow.Complete();
            _uow.CommitTran();
        }

        private CourseRateConditionViewModel? ValidateConditions(CreateCourseRateConditionViewModel? conditions)
        {
            if (conditions is null)
            {
                return null;
            }

            var conditionViewModel = new CourseRateConditionViewModel();

            if (conditions.AcademicLevelId.HasValue)
            {
                var academicLevel = _academicLevelRepository.Query()
                                                            .Include(x => x.Localizations)
                                                            .FirstOrDefault(x => x.Id == conditions.AcademicLevelId.Value);

                if (academicLevel is null)
                {
                    throw new AcademicLevelException.NotFound(conditions.AcademicLevelId.Value);
                }

                conditionViewModel.AcademicLevelId = academicLevel.Id;
                conditionViewModel.AcademicLevelName = academicLevel.Name;
            }

            if (conditions.FacultyId.HasValue)
            {
                var faculty = _facultyRepository.Query()
                                                .Include(x => x.Localizations)
                                                .FirstOrDefault(x => x.Id == conditions.FacultyId.Value);

                if (faculty is null)
                {
                    throw new FacultyException.NotFound(conditions.FacultyId.Value);
                }

                conditionViewModel.FacultyId = faculty.Id;
                conditionViewModel.FacultyName = faculty.Name;
            }

            if (conditions.DepartmentId.HasValue)
            {
                var department = _departmentRepository.Query()
                                                      .Include(x => x.Localizations)
                                                      .FirstOrDefault(x => x.Id == conditions.DepartmentId.Value);

                if (department is null)
                {
                    throw new DepartmentException.NotFound(conditions.DepartmentId.Value);
                }

                if (conditions.FacultyId.HasValue
                    && department.FacultyId != conditions.FacultyId.Value)
                {
                    throw new DepartmentException.NotFound(conditions.DepartmentId.Value);
                }

                conditionViewModel.DepartmentId = department.Id;
                conditionViewModel.DepartmentName = department.Name;
            }

            if (conditions.CurriculumVersionId.HasValue)
            {
                var version = _curriculumVersionRepository.Query()
                                                          .Include(x => x.Localizations)
                                                          .FirstOrDefault(x => x.Id == conditions.CurriculumVersionId.Value);

                if (version is null)
                {
                    throw new CurriculumVersionException.NotFound(conditions.CurriculumVersionId.Value);
                }

                conditionViewModel.CurriculumVersionId = version.Id;
                conditionViewModel.CurriculumVersionName = version.Name;
            }

            return conditionViewModel;
        }

        private CourseRateViewModel MapModelToViewModel(CourseRate model, RateType rateType)
        {
            var response = new CourseRateViewModel
            {
                Id = model.Id,
                IsActive = model.IsActive,
                Conditions = null,
                RateTypeId = model.RateTypeId,
                RateTypeName = rateType.Name,
                Index = model.Index,
                Amount = model.Amount,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt
            };

            AcademicLevel? academicLevel = null;
            Faculty? faculty = null;
            Department? department = null;
            CurriculumVersion? curriculumVersion = null;

            if (model.Conditions is not null)
            {
                var condition = JsonConvert.DeserializeObject<CourseRateConditionViewModel>(model.Conditions)!;

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

                var conditionViewModel = new CourseRateConditionViewModel
                {
                    AcademicLevelId = condition.AcademicLevelId,
                    AcademicLevelName = academicLevel is null ? null
                                                              : academicLevel.Name,
                    FacultyId = condition.FacultyId,
                    FacultyName = faculty is null ? null
                                                  : faculty.Name,
                    DepartmentId = condition.DepartmentId,
                    DepartmentName = department is null ? null
                                                        : department.Name,
                    CurriculumVersionId = condition.CurriculumVersionId,
                    CurriculumVersionName = curriculumVersion is null ? null
                                                                      : curriculumVersion.Name,
                    FromBatch = condition.FromBatch,
                    ToBatch = condition.ToBatch
                };

                response.Conditions = conditionViewModel;
            }

            return response;
        }

        private IQueryable<CourseRate> GenerateSearchQuery(SearchCourseRateCriteriaViewModel? parameters = null)
        {
            var query = _courseRateRepository.Query()
                                             .Include(x => x.RateType)
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

            query.OrderBy(x => x.CreatedAt);

            return query;
        }
    }
}

