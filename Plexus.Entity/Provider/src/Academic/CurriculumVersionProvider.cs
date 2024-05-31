using Microsoft.EntityFrameworkCore;
using Plexus.Database;
using Plexus.Database.Model.Academic.Curriculum;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Academic.Curriculum;
using Plexus.Entity.Exception;
using Plexus.Utility.ViewModel;
using Plexus.Utility.Extensions;
using Plexus.Database.Model.Localization.Academic.Curriculum;
using ServiceStack;

namespace Plexus.Entity.Provider.src.Academic
{
    public class CurriculumVersionProvider : ICurriculumVersionProvider
    {
        private readonly DatabaseContext _dbContext;

        public CurriculumVersionProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public CurriculumVersionDTO Create(CreateCurriculumVersionDTO request, string requester)
        {
            var model = new CurriculumVersion
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
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = requester
            };

            var localizes = MapLocalizationDTOToModel(request.Localizations, model).ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.CurriculumVersions.Add(model);

                if (localizes.Any())
                {
                    _dbContext.CurriculumVersionLocalizations.AddRange(localizes);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(model, localizes);

            return response;
        }

        public IEnumerable<CurriculumVersionDTO> GetAll()
        {
            var curriculumVersions = _dbContext.CurriculumVersions.AsNoTracking()
                                                                  .Include(x => x.Localizations)
                                                                  .ToList();

            var response = (from curriculumVersion in curriculumVersions
                            select MapModelToDTO(curriculumVersion, curriculumVersion.Localizations))
                           .ToList();

            return response;
        }

        public IEnumerable<CurriculumVersionDTO> GetByCurriculumId(Guid curriculumId)
        {
            var curriculumVersions = _dbContext.CurriculumVersions.AsNoTracking()
                                                                  .Include(x => x.Localizations)
                                                                  .Where(x => x.CurriculumId == curriculumId)
                                                                  .ToList();

            var response = (from curriculumVersion in curriculumVersions
                            select MapModelToDTO(curriculumVersion, curriculumVersion.Localizations))
                           .ToList();

            return response;
        }

        public CurriculumVersionDTO GetById(Guid id)
        {
            var curriculumVersion = _dbContext.CurriculumVersions.AsNoTracking()
                                                                 .Include(x => x.Localizations)
                                                                 .SingleOrDefault(x => x.Id == id);

            if (curriculumVersion is null)
            {
                throw new CurriculumException.VersionNotFound(id);
            }

            var response = MapModelToDTO(curriculumVersion, curriculumVersion.Localizations);

            return response;
        }

        public IEnumerable<CurriculumVersionDTO> GetById(IEnumerable<Guid> ids)
        {
            var curriculumVersions = _dbContext.CurriculumVersions.AsNoTracking()
                                                                  .Include(x => x.Localizations)
                                                                  .Where(x => ids.Contains(x.Id))
                                                                  .ToList();

            var response = (from curriculumVersion in curriculumVersions
                            select MapModelToDTO(curriculumVersion, curriculumVersion.Localizations))
                           .ToList();

            return response;
        }

        public PagedViewModel<CurriculumVersionDTO> Search(SearchCurriculumVersionCriteriaDTO parameters, int page, int pageSize)
        {
            var query = GenerateSearchQuery(parameters);

            var pagedCurriculumVersion = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<CurriculumVersionDTO>
            {
                Page = pagedCurriculumVersion.Page,
                TotalPage = pagedCurriculumVersion.TotalPage,
                TotalItem = pagedCurriculumVersion.TotalItem,
                Items = (from curriculumVersion in pagedCurriculumVersion.Items
                         select MapModelToDTO(curriculumVersion, curriculumVersion.Localizations))
                        .ToList()
            };

            return response;
        }

        public IEnumerable<CurriculumVersionDTO> Search(SearchCurriculumVersionCriteriaDTO parameters)
        {
            var query = GenerateSearchQuery(parameters);

            var versions = query.ToList();

            var response = (from version in versions
                            select MapModelToDTO(version, version.Localizations))
                           .ToList();

            return response;
        }

        public CurriculumVersionDTO Update(CurriculumVersionDTO request, string requester)
        {
            var curriculumVersion = _dbContext.CurriculumVersions.Include(x => x.Localizations)
                                                                 .SingleOrDefault(x => x.Id == request.Id);

            if (curriculumVersion is null)
            {
                throw new CurriculumException.VersionNotFound(request.Id);
            }

            var localizes = MapLocalizationDTOToModel(request.Localizations, curriculumVersion).ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
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
                curriculumVersion.UpdatedAt = DateTime.UtcNow;
                curriculumVersion.UpdatedBy = requester;

                _dbContext.CurriculumVersionLocalizations.RemoveRange(curriculumVersion.Localizations);

                if (localizes.Any())
                {
                    _dbContext.CurriculumVersionLocalizations.AddRange(localizes);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(curriculumVersion, localizes);

            return response;
        }

        public void Delete(Guid id)
        {
            var curriculumVersion = _dbContext.CurriculumVersions.SingleOrDefault(x => x.Id == id);
            if (curriculumVersion is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.CurriculumVersions.Remove(curriculumVersion);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        public IEnumerable<CurriculumCourseBlackListDTO> GetBlackListCourses(Guid versionId)
        {
            var blackListCourses = _dbContext.CurriculumCourseBlackLists.AsNoTracking()
                                                                        .Where(x => x.CurriculumVersionId == versionId)
                                                                        .ToList();

            var response = (from data in blackListCourses
                            select MapCourseBlacklistModelToDTO(data))
                           .ToList();

            return response;
        }

        public void UpdateBlackListCourses(Guid versionId, IEnumerable<Guid> courseIds)
        {
            var existingBlackListCourses = _dbContext.CurriculumCourseBlackLists.Where(x => x.CurriculumVersionId == versionId)
                                                                                .ToList();

            var newBlackListCourses = (from courseId in courseIds
                                       select new CurriculumCourseBlackList
                                       {
                                           CurriculumVersionId = versionId,
                                           CourseId = courseId
                                       })
                                      .ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.CurriculumCourseBlackLists.RemoveRange(existingBlackListCourses);

                if (newBlackListCourses.Any())
                {
                    _dbContext.CurriculumCourseBlackLists.AddRange(newBlackListCourses);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        public IEnumerable<CurriculumVersionSpecializationDTO> GetAcademicSpecializations(Guid versionId)
        {
            var academicSpecializations = _dbContext.CurriculumVersionSpecializations.AsNoTracking()
                                                                                     .Where(x => x.CurriculumVersionId == versionId)
                                                                                     .ToList();

            var response = (from data in academicSpecializations
                            select MapVersionSpecializationToDTO(data))
                           .ToList();

            return response;
        }

        public void UpdateAcademicSpecialization(Guid versionId, IEnumerable<Guid> specializationIds)
        {
            var existintAcademicSpecialization = _dbContext.CurriculumVersionSpecializations.Where(x => x.CurriculumVersionId == versionId)
                                                                                            .ToList();

            var newAcademicSpecializations = (from specializationId in specializationIds
                                              select new CurriculumVersionSpecialization
                                              {
                                                  CurriculumVersionId = versionId,
                                                  AcademicSpecializationId = specializationId
                                              })
                                             .ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.CurriculumVersionSpecializations.RemoveRange(existintAcademicSpecialization);

                if (newAcademicSpecializations.Any())
                {
                    _dbContext.CurriculumVersionSpecializations.AddRange(newAcademicSpecializations);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        public IEnumerable<CorequisiteDTO> GetCorequisites(Guid versionId)
        {
            var corequisites = _dbContext.Corequisites.AsNoTracking()
                                                      .Where(x => x.CurriculumVersionId == versionId)
                                                      .ToList();

            var response = (from corequisite in corequisites
                            select MapCorequisiteToDTO(corequisite))
                           .ToList();

            return response;
        }

        public void UpdateCorequisite(Guid versionId, IEnumerable<CreateCorequisiteDTO> corequisites)
        {
            var existingCorequisites = _dbContext.Corequisites.Where(x => x.CurriculumVersionId == versionId)
                                                              .ToList();

            var newCorequisites = (from corequisite in corequisites
                                   select new Corequisite
                                   {
                                       CurriculumVersionId = versionId,
                                       CourseId = corequisite.CourseId,
                                       CorequisiteCourseId = corequisite.CorequisiteCourseId
                                   })
                                  .ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Corequisites.RemoveRange(existingCorequisites);

                if (newCorequisites.Any())
                {
                    _dbContext.Corequisites.AddRange(newCorequisites);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        public IEnumerable<CurriculumVersionCourseDTO> GetCoursesList(Guid versionId)
        {
            var courseGroups = _dbContext.CurriculumCourseGroups.AsNoTracking()
                                                                .Include(x => x.CurriculumCourses)
                                                                .Where(x => x.CurriculumVersionId == versionId)
                                                                .ToList();

            var specializeGroups = _dbContext.CurriculumVersionSpecializations.AsNoTracking()
                                                                              .Include(x => x.AcademicSpecialization)
                                                                              .ThenInclude(x => x.SpecializationCourses)
                                                                              .Where(x => x.CurriculumVersionId == versionId)
                                                                              .ToList();

            var curriculumCourses = courseGroups.SelectMany(x => x.CurriculumCourses)
                                                .Select(x => new CurriculumVersionCourseDTO
                                                {
                                                    CourseId = x.CourseId,
                                                    CourseGroupId = x.CourseGroupId,
                                                    RequiredGradeId = x.RequiredGradeId
                                                })
                                                .ToList();

            var specializeCourses = specializeGroups.SelectMany(x => x.AcademicSpecialization.SpecializationCourses)
                                                    .Select(x => new CurriculumVersionCourseDTO
                                                    {
                                                        CourseId = x.CourseId,
                                                        SpecialzationId = x.AcademicSpecializationId,
                                                        RequiredGradeId = x.RequiredGradeId
                                                    })
                                                    .ToList();

            var response = curriculumCourses.Concat(specializeCourses)
                                            .ToList();

            return response;
        }

        public IEnumerable<EquivalentCourseDTO> GetEquivalentCourses(Guid versionId)
        {
            var equivalences = _dbContext.EquivalentCourses.AsNoTracking()
                                                           .Where(x => x.CurriculumVersionId == versionId)
                                                           .ToList();

            var response = (from equivalence in equivalences
                            select MapEquivalentCourseToDTO(equivalence))
                           .ToList();

            return response;
        }

        public void UpdateEquivalentCourses(Guid versionId, IEnumerable<CreateEquivalentCourseDTO> equivalences)
        {
            var existingEquivalences = _dbContext.EquivalentCourses.Where(x => x.CurriculumVersionId == versionId)
                                                                   .ToList();

            var newEquivalences = (from equivalence in equivalences
                                   select new EquivalentCourse
                                   {
                                       CurriculumVersionId = versionId,
                                       CourseId = equivalence.CourseId,
                                       EquivalenceCourseId = equivalence.EquivalentCourseId
                                   })
                                  .ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.EquivalentCourses.RemoveRange(existingEquivalences);

                if (newEquivalences.Any())
                {
                    _dbContext.EquivalentCourses.AddRange(newEquivalences);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        private IQueryable<CurriculumVersion> GenerateSearchQuery(SearchCurriculumVersionCriteriaDTO? parameters)
        {
            var query = _dbContext.CurriculumVersions.Include(x => x.Localizations)
                                                     .AsNoTracking();

            if (parameters is not null)
            {
                if (!string.IsNullOrEmpty(parameters.Name))
                {
                    query = query.Where(x => (!string.IsNullOrEmpty(x.Name)
                                                 && x.Name.Contains(parameters.Name))
                                              || (!string.IsNullOrEmpty(x.DegreeName)
                                                 && x.DegreeName.Contains(parameters.Name))
                                              || (!string.IsNullOrEmpty(x.Abbreviation)
                                                 && x.Abbreviation.Contains(parameters.Name)));
                }

                if (!string.IsNullOrEmpty(parameters.Code))
                {
                    query = query.Where(x => !string.IsNullOrEmpty(x.Code)
                                                && x.Code.Contains(parameters.Code));
                }

                if (parameters.CurriculumId.HasValue)
                {
                    query = query.Where(x => x.CurriculumId == parameters.CurriculumId.Value);
                }

                if (parameters.IsActive.HasValue)
                {
                    query = query.Where(x => x.IsActive == parameters.IsActive.Value);
                }
            }

            query = query.OrderBy(x => x.Code);

            if (parameters is not null)
            {
                if (!string.IsNullOrEmpty(parameters.SortBy))
                {
                    try
                    {
                        query = query.OrderBy(parameters.SortBy, parameters.OrderBy);
                    }
                    catch (System.Exception)
                    {
                        // invalid property name
                    }
                }
            }

            return query;
        }

        private static CurriculumVersionDTO MapModelToDTO(CurriculumVersion model, IEnumerable<CurriculumVersionLocalization> localizations)
        {
            var response = new CurriculumVersionDTO
            {
                Id = model.Id,
                CurriculumId = model.CurriculumId,
                AcademicLevelId = model.AcademicLevelId,
                FacultyId = model.FacultyId,
                DepartmentId = model.DepartmentId,
                AcademicProgramId = model.AcademicProgramId,
                Code = model.Code,
                Name = model.Name,
                DegreeName = model.DegreeName,
                Description = model.Description,
                Abbreviation = model.Abbreviation,
                TotalCredit = model.TotalCredit,
                TotalYear = model.TotalYear,
                ExpectedGraduatingCredit = model.ExpectedGraduatingCredit,
                ApprovedAt = model.ApprovedAt,
                StartBatchCode = model.StartBatchCode,
                EndBatchCode = model.EndBatchCode,
                CollegeCalendarType = model.CollegeCalendarType,
                Remark = model.Remark,
                IsActive = model.IsActive,
                Localizations = localizations is null ? Enumerable.Empty<CurriculumVersionLocalizationDTO>()
                                                      : (from locale in localizations
                                                         orderby locale.Language
                                                         select new CurriculumVersionLocalizationDTO
                                                         {
                                                             Language = locale.Language,
                                                             Name = locale.Name,
                                                             DegreeName = locale.DegreeName,
                                                             Description = locale.Description,
                                                             Abbreviation = locale.Abbreviation
                                                         })
                                                         .ToList(),
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt
            };

            return response;
        }

        private static IEnumerable<CurriculumVersionLocalization> MapLocalizationDTOToModel(
            IEnumerable<CurriculumVersionLocalizationDTO>? localizations,
            CurriculumVersion model)
        {
            if (localizations is null)
            {
                return Enumerable.Empty<CurriculumVersionLocalization>();
            }

            var response = (from locale in localizations
                            select new CurriculumVersionLocalization
                            {
                                CurriculumVersion = model,
                                Language = locale.Language,
                                Name = locale.Name,
                                DegreeName = locale.DegreeName,
                                Description = locale.Description,
                                Abbreviation = locale.Abbreviation
                            })
                            .ToList();

            return response;
        }

        private static CurriculumCourseBlackListDTO MapCourseBlacklistModelToDTO(CurriculumCourseBlackList model)
        {
            return new CurriculumCourseBlackListDTO
            {
                CurriculumVersionId = model.CurriculumVersionId,
                CourseId = model.CourseId
            };
        }

        private static CurriculumVersionSpecializationDTO MapVersionSpecializationToDTO(CurriculumVersionSpecialization model)
        {
            return new CurriculumVersionSpecializationDTO
            {
                CurriculumVersionId = model.CurriculumVersionId,
                AcademicSpecializationId = model.AcademicSpecializationId
            };
        }

        private static CorequisiteDTO MapCorequisiteToDTO(Corequisite model)
        {
            return new CorequisiteDTO
            {
                CurriculumVersionId = model.CurriculumVersionId,
                CourseId = model.CourseId,
                CorequisiteCourseId = model.CorequisiteCourseId
            };
        }

        private static EquivalentCourseDTO MapEquivalentCourseToDTO(EquivalentCourse model)
        {
            return new EquivalentCourseDTO
            {
                CurriculumVersionId = model.CurriculumVersionId,
                CourseId = model.CourseId,
                EquivalentCourseId = model.EquivalenceCourseId
            };
        }
    }
}