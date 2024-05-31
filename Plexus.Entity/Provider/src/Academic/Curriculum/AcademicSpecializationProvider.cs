using Microsoft.EntityFrameworkCore;
using Plexus.Database;
using Plexus.Database.Model.Academic.Curriculum;
using Plexus.Database.Model.Localization.Academic.Curriculum;
using Plexus.Entity.DTO.Academic.Curriculum;
using Plexus.Entity.Exception;
using Plexus.Utility.ViewModel;
using Plexus.Utility.Extensions;
using Plexus.Entity.DTO;

namespace Plexus.Entity.Provider.src.Academic.Curriculum
{
    public class AcademicSpecializationProvider : IAcademicSpecializationProvider
    {
        private readonly DatabaseContext _dbContext;

        public AcademicSpecializationProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public AcademicSpecializationDTO Create(CreateAcademicSpecializationDTO request, string requester)
        {
            var model = new AcademicSpecialization
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
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = requester
            };

            var localizes = MapLocalizationDTOToModel(request.Localizations, model).ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.AcademicSpecializations.Add(model);

                if (localizes.Any())
                {
                    _dbContext.AcademicSpecializationLocalizations.AddRange(localizes);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(model, localizes);

            return response;
        }

        public IEnumerable<AcademicSpecializationDTO> Search(SearchAcademicSpecializationCriteriaDTO parameters)
        {
            var query = GenerateSearchQuery(parameters);

            var academicSpecializations = query.ToList();

            var response = (from academicSpecialization in academicSpecializations
                            select MapModelToDTO(academicSpecialization, academicSpecialization.Localizations, academicSpecialization.SpecializationCourses))
                           .ToList();

            return response;
        }

        public PagedViewModel<AcademicSpecializationDTO> Search(SearchAcademicSpecializationCriteriaDTO parameters, int page, int pageSize)
        {
            var query = GenerateSearchQuery(parameters);

            var pagedAcademicSpecializations = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<AcademicSpecializationDTO>
            {
                Page = pagedAcademicSpecializations.Page,
                TotalPage = pagedAcademicSpecializations.TotalPage,
                TotalItem = pagedAcademicSpecializations.TotalItem,
                Items = (from academicSpecialization in pagedAcademicSpecializations.Items
                         select MapModelToDTO(academicSpecialization, academicSpecialization.Localizations, academicSpecialization.SpecializationCourses))
                        .ToList()
            };

            return response;
        }

        public IEnumerable<AcademicSpecializationDTO> GetAll()
        {
            var academicSpecializations = _dbContext.AcademicSpecializations.AsNoTracking()
                                                                            .Include(x => x.Localizations)
                                                                            .Include(x => x.SpecializationCourses)
                                                                            .ToList();

            var response = (from academicSpecialization in academicSpecializations
                            select MapModelToDTO(academicSpecialization, academicSpecialization.Localizations, academicSpecialization.SpecializationCourses))
                           .ToList();

            return response;
        }

        public AcademicSpecializationDTO GetById(Guid id)
        {
            var academicSpecialization = _dbContext.AcademicSpecializations.AsNoTracking()
                                                                           .Include(x => x.Localizations)
                                                                           .Include(x => x.SpecializationCourses)
                                                                           .SingleOrDefault(x => x.Id == id);

            if (academicSpecialization is null)
            {
                throw new AcademicSpecializationException.NotFound(id);
            }

            var response = MapModelToDTO(academicSpecialization, academicSpecialization.Localizations, academicSpecialization.SpecializationCourses);

            return response;
        }

        public IEnumerable<AcademicSpecializationDTO> GetById(IEnumerable<Guid> ids)
        {
            var academicSpecializations = _dbContext.AcademicSpecializations.AsNoTracking()
                                                                            .Include(x => x.Localizations)
                                                                            .Where(x => ids.Contains(x.Id))
                                                                            .ToList();

            var response = (from academicSpecialization in academicSpecializations
                            select MapModelToDTO(academicSpecialization, academicSpecialization.Localizations))
                           .ToList();

            return response;
        }

        public AcademicSpecializationDTO Update(AcademicSpecializationDTO request, string requester)
        {
            var academicSpecialization = _dbContext.AcademicSpecializations.Include(x => x.SpecializationCourses)
                                                                           .Include(x => x.Localizations)
                                                                           .SingleOrDefault(x => x.Id == request.Id);

            if (academicSpecialization is null)
            {
                throw new AcademicSpecializationException.NotFound(request.Id);
            }

            var localizes = MapLocalizationDTOToModel(request.Localizations, academicSpecialization).ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                academicSpecialization.Name = request.Name;
                academicSpecialization.Code = request.Code;
                academicSpecialization.Abbreviation = request.Abbreviation;
                academicSpecialization.Type = request.Type;
                academicSpecialization.Description = request.Description;
                academicSpecialization.RequiredCredit = request.RequiredCredit;
                academicSpecialization.Remark = request.Remark;
                academicSpecialization.Level = request.Level;
                academicSpecialization.UpdatedAt = DateTime.UtcNow;
                academicSpecialization.UpdatedBy = requester;

                _dbContext.AcademicSpecializationLocalizations.RemoveRange(academicSpecialization.Localizations);

                if (localizes.Any())
                {
                    _dbContext.AcademicSpecializationLocalizations.AddRange(localizes);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(academicSpecialization, localizes, academicSpecialization.SpecializationCourses);

            return response;
        }

        public void Delete(Guid id)
        {
            var academicSpecialization = _dbContext.AcademicSpecializations.SingleOrDefault(x => x.Id == id);

            if (academicSpecialization is null)
            {
                return;
            }

            var academicSpecializations = _dbContext.AcademicSpecializations.ToList();

            var deleteSpecializationIds = new List<Guid> { id };

            var subGroupIds = FindSubGroupId(deleteSpecializationIds, academicSpecializations).ToList();

            deleteSpecializationIds.AddRange(subGroupIds);

            do
            {
                subGroupIds = FindSubGroupId(subGroupIds, academicSpecializations).ToList();

                if (subGroupIds.Any())
                {
                    deleteSpecializationIds.AddRange(subGroupIds);
                }

            } while (subGroupIds.Any());

            var deleteAcademicSpecializations = academicSpecializations.Where(x => deleteSpecializationIds.Contains(x.Id))
                                                                       .ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.AcademicSpecializations.RemoveRange(deleteAcademicSpecializations);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        public IEnumerable<SpecializationCourseDTO> GetCourses(Guid specializationId)
        {
            var courses = _dbContext.SpecializationCourses.AsNoTracking()
                                                          .Where(x => x.AcademicSpecializationId == specializationId)
                                                          .ToList();

            var response = (from course in courses
                            select MapCourseModelToDTO(course))
                           .ToList();

            return response;
        }

        public void UpdateCourses(Guid specializationId, IEnumerable<SpecializationCourseDTO> request)
        {
            var existingCourses = _dbContext.SpecializationCourses.Where(x => x.AcademicSpecializationId == specializationId)
                                                                  .ToList();

            var newCourses = (from course in request
                              select new SpecializationCourse
                              {
                                  CourseId = course.CourseId,
                                  AcademicSpecializationId = specializationId,
                                  IsRequiredCourse = course.IsRequiredCourse,
                                  RequiredGradeId = course.RequiredGradeId
                              })
                             .ToList();

            using (var transcation = _dbContext.Database.BeginTransaction())
            {
                if (existingCourses.Any())
                {
                    _dbContext.SpecializationCourses.RemoveRange(existingCourses);
                }

                if (newCourses.Any())
                {
                    _dbContext.SpecializationCourses.AddRange(newCourses);
                }

                transcation.Commit();
            }

            _dbContext.SaveChanges();
        }

        private AcademicSpecializationDTO MapModelToDTO(AcademicSpecialization model,
                                                        IEnumerable<AcademicSpecializationLocalization> localizations,
                                                        IEnumerable<SpecializationCourse>? courses = null)
        {
            var response = new AcademicSpecializationDTO
            {
                Id = model.Id,
                Code = model.Code,
                Abbreviation = model.Abbreviation,
                Name = model.Name,
                Description = model.Description,
                Type = model.Type,
                RequiredCredit = model.RequiredCredit,
                Level = model.Level,
                Remark = model.Remark,
                ParentAcademicSpecializationId = model.ParentAcademicSpecializationId,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                Courses = courses is null ? Enumerable.Empty<SpecializationCourseDTO>()
                                          : (from course in courses
                                             select MapCourseModelToDTO(course))
                                            .ToList(),
                Localizations = localizations is null ? Enumerable.Empty<AcademicSpecializationLocalizationDTO>()
                                                      : (from locale in localizations
                                                         orderby locale.Language
                                                         select new AcademicSpecializationLocalizationDTO
                                                         {
                                                             Language = locale.Language,
                                                             Name = locale.Name,
                                                             Abbreviation = locale.Abbreviation,
                                                             Description = locale.Description
                                                         })
                                                        .ToList()
            };

            return response;
        }

        private static SpecializationCourseDTO MapCourseModelToDTO(SpecializationCourse model)
        {
            return new SpecializationCourseDTO
            {
                AcademicSpecializationId = model.AcademicSpecializationId,
                CourseId = model.CourseId,
                RequiredGradeId = model.RequiredGradeId,
                IsRequiredCourse = model.IsRequiredCourse
            };
        }

        private static IEnumerable<AcademicSpecializationLocalization> MapLocalizationDTOToModel(
                    IEnumerable<AcademicSpecializationLocalizationDTO> localizations,
                    AcademicSpecialization model)
        {
            if (localizations is null)
            {
                return Enumerable.Empty<AcademicSpecializationLocalization>();
            }

            var response = (from locale in localizations
                            select new AcademicSpecializationLocalization
                            {
                                AcademicSpecialization = model,
                                Language = locale.Language,
                                Name = locale.Name,
                                Abbreviation = locale.Abbreviation,
                                Description = locale.Description
                            })
                           .ToList();

            return response;
        }

        private IQueryable<AcademicSpecialization> GenerateSearchQuery(SearchAcademicSpecializationCriteriaDTO? parameters = null)
        {
            var query = _dbContext.AcademicSpecializations.Include(x => x.Localizations)
                                                          .Where(x => !x.ParentAcademicSpecializationId.HasValue)
                                                          .AsNoTracking();

            if (parameters is not null)
            {
                if (!string.IsNullOrEmpty(parameters.Code))
                {
                    query = query.Where(x => x.Code.Contains(parameters.Code));
                }

                if (!string.IsNullOrEmpty(parameters.Name))
                {
                    query = query.Where(x => x.Name.Contains(parameters.Name));
                }

                if (parameters.Level != null)
                {
                    query = query.Where(x => x.Level == parameters.Level);
                }
            }

            query = query.OrderBy(x => x.Code)
                         .ThenBy(x => x.Level)
                         .ThenBy(x => x.Name);

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

        private static IEnumerable<Guid> FindSubGroupId(IEnumerable<Guid> baseSpecializationIds, IEnumerable<AcademicSpecialization> academicSpecializations)
        {
            if (academicSpecializations is null || !academicSpecializations.Any())
            {
                return Enumerable.Empty<Guid>();
            }

            var subGroup = academicSpecializations.Where(x => x.ParentAcademicSpecializationId.HasValue
                                                              && baseSpecializationIds.Contains(x.ParentAcademicSpecializationId.Value))
                                                  .Select(x => x.Id)
                                                  .Distinct()
                                                  .ToList();

            return subGroup;
        }
    }
}