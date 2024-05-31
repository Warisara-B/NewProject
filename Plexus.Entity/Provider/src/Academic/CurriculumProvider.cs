using Microsoft.EntityFrameworkCore;
using Plexus.Database;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Academic.Curriculum;
using Plexus.Entity.Exception;
using Plexus.Utility.ViewModel;
using Plexus.Utility.Extensions;
using Plexus.Database.Model.Localization.Academic.Curriculum;

namespace Plexus.Entity.Provider.src.Academic
{
    public class CurriculumProvider : ICurriculumProvider
    {
        private readonly DatabaseContext _dbContext;

        public CurriculumProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public CurriculumDTO Create(CreateCurriculumDTO request, string requester)
        {
            var model = new Database.Model.Academic.Curriculum.Curriculum
            {
                AcademicLevelId = request.AcademicLevelId,
                FacultyId = request.FacultyId,
                DepartmentId = request.DepartmentId,
                Name = request.Name,
                Code = request.Code,
                FormalName = request.FormalName,
                Abbreviation = request.Abbreviation,
                Description = request.Abbreviation,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = requester
            };

            var localizes = MapLocalizationDTOToModel(request.Localizations, model).ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Curriculums.Add(model);

                if (localizes.Any())
                {
                    _dbContext.CurriculumLocalizations.AddRange(localizes);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(model, localizes); ;

            return response;
        }

        public IEnumerable<CurriculumDTO> GetAll()
        {
            var curriculums = _dbContext.Curriculums.AsNoTracking()
                                                    .Include(x => x.Localizations)
                                                    .ToList();

            var response = (from curriculum in curriculums
                            select MapModelToDTO(curriculum, curriculum.Localizations))
                           .ToList();

            return response;
        }

        public CurriculumDTO GetById(Guid id)
        {
            var curriculum = _dbContext.Curriculums.AsNoTracking()
                                                   .Include(x => x.Localizations)
                                                   .SingleOrDefault(x => x.Id == id);

            if (curriculum is null)
            {
                throw new CurriculumException.NotFound(id);
            }

            var response = MapModelToDTO(curriculum, curriculum.Localizations);

            return response;
        }

        public IEnumerable<CurriculumDTO> GetById(IEnumerable<Guid> ids)
        {
            var curriculums = _dbContext.Curriculums.AsNoTracking()
                                                    .Include(x => x.Localizations)
                                                    .Where(x => ids.Contains(x.Id))
                                                    .ToList();

            var response = (from curriculum in curriculums
                            select MapModelToDTO(curriculum, curriculum.Localizations))
                           .ToList();

            return response;
        }

        public PagedViewModel<CurriculumDTO> Search(SearchCriteriaViewModel parameters, int page, int pageSize)
        {
            var query = GenerateSearchQuery(parameters);

            var pagedCurriculum = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<CurriculumDTO>
            {
                Page = pagedCurriculum.Page,
                TotalPage = pagedCurriculum.TotalPage,
                TotalItem = pagedCurriculum.TotalItem,
                Items = (from curriculum in pagedCurriculum.Items
                         select MapModelToDTO(curriculum, curriculum.Localizations))
                        .ToList()
            };

            return response;
        }

        public IEnumerable<CurriculumDTO> Search(SearchCriteriaViewModel parameters)
        {
            var query = GenerateSearchQuery(parameters);

            var curriculums = query.ToList();

            var response = (from curriculum in curriculums
                            select MapModelToDTO(curriculum, curriculum.Localizations))
                           .ToList();

            return response;
        }

        public CurriculumDTO Update(CurriculumDTO request, string requester)
        {
            var curriculum = _dbContext.Curriculums.Include(x => x.Localizations)
                                                   .SingleOrDefault(x => x.Id == request.Id);

            if (curriculum is null)
            {
                throw new CurriculumException.NotFound(request.Id);
            }

            var localizes = MapLocalizationDTOToModel(request.Localizations, curriculum).ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                curriculum.AcademicLevelId = request.AcademicLevelId;
                curriculum.FacultyId = request.FacultyId;
                curriculum.DepartmentId = request.DepartmentId;
                curriculum.Code = request.Code;
                curriculum.Name = request.Name;
                curriculum.FormalName = request.FormalName;
                curriculum.Abbreviation = request.Abbreviation;
                curriculum.Description = request.Description;
                curriculum.IsActive = request.IsActive;
                curriculum.UpdatedAt = DateTime.UtcNow;
                curriculum.UpdatedBy = requester;

                _dbContext.CurriculumLocalizations.RemoveRange(curriculum.Localizations);

                if (localizes.Any())
                {
                    _dbContext.CurriculumLocalizations.AddRange(localizes);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(curriculum, localizes);

            return response;
        }

        public void Delete(Guid id)
        {
            var curriculum = _dbContext.Curriculums.SingleOrDefault(x => x.Id == id);
            if (curriculum is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Curriculums.Remove(curriculum);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        private IQueryable<Database.Model.Academic.Curriculum.Curriculum> GenerateSearchQuery(SearchCriteriaViewModel? parameters)
        {
            var query = _dbContext.Curriculums.Include(x => x.Localizations)
                                              .AsNoTracking();

            if (parameters is not null)
            {
                if (parameters.AcademicLevelId.HasValue)
                {
                    query = query.Where(x => x.AcademicLevelId == parameters.AcademicLevelId.Value);
                }

                if (parameters.FacultyId.HasValue)
                {
                    query = query.Where(x => x.FacultyId == parameters.FacultyId.Value);
                }

                if (parameters.DepartmentId.HasValue)
                {
                    query = query.Where(x => x.DepartmentId == parameters.DepartmentId.Value);
                }

                if (!string.IsNullOrEmpty(parameters.Code))
                {
                    query = query.Where(x => x.Code.Contains(parameters.Code));
                }

                if (!string.IsNullOrEmpty(parameters.Name))
                {
                    query = query.Where(x => x.Name.Contains(parameters.Name));
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

        private static CurriculumDTO MapModelToDTO(Database.Model.Academic.Curriculum.Curriculum model,
                                                   IEnumerable<CurriculumLocalization> localizations)
        {
            var response = new CurriculumDTO
            {
                Id = model.Id,
                AcademicLevelId = model.AcademicLevelId,
                FacultyId = model.FacultyId,
                DepartmentId = model.DepartmentId,
                Code = model.Code,
                Name = model.Name,
                FormalName = model.FormalName,
                Abbreviation = model.Abbreviation,
                Description = model.Description,
                IsActive = model.IsActive,
                Localizations = localizations is null ? Enumerable.Empty<CurriculumLocalizationDTO>()
                                                      : (from locale in localizations
                                                         orderby locale.Language
                                                         select new CurriculumLocalizationDTO
                                                         {
                                                             Language = locale.Language,
                                                             Name = locale.Name,
                                                             FormalName = locale.FormalName,
                                                             Abbreviation = locale.Abbreviation
                                                         })
                                                        .ToList(),
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt
            };

            return response;
        }

        private static IEnumerable<CurriculumLocalization> MapLocalizationDTOToModel(
            IEnumerable<CurriculumLocalizationDTO>? localizations,
            Database.Model.Academic.Curriculum.Curriculum model)
        {
            if (localizations is null)
            {
                return Enumerable.Empty<CurriculumLocalization>();
            }

            var response = (from locale in localizations
                            select new CurriculumLocalization
                            {
                                Curriculum = model,
                                Language = locale.Language,
                                Name = locale.Name,
                                FormalName = locale.FormalName,
                                Abbreviation = locale.Abbreviation
                            })
                            .ToList();

            return response;
        }
    }
}

