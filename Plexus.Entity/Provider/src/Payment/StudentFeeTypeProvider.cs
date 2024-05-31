using Microsoft.EntityFrameworkCore;
using Plexus.Database;
using Plexus.Database.Model.Payment;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Payment;
using Plexus.Utility.ViewModel;
using Plexus.Utility.Extensions;
using Plexus.Entity.Exception;
using Plexus.Database.Model.Localization.Payment;

namespace Plexus.Entity.Provider.src.Payment
{
    public class StudentFeeTypeProvider : IStudentFeeTypeProvider
    {
        private readonly DatabaseContext _dbContext;

        public StudentFeeTypeProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public StudentFeeTypeDTO Create(CreateStudentFeeTypeDTO request, string requester)
        {
            var model = new StudentFeeType
            {
                Name = request.Name,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = requester
            };

            var localizations = request.Localizations is null ? Enumerable.Empty<StudentFeeTypeLocalization>()
                                                              : (from localize in request.Localizations
                                                                 select new StudentFeeTypeLocalization
                                                                 {
                                                                     StudentFeeType = model,
                                                                     Language = localize.Language,
                                                                     Name = localize.Name
                                                                 })
                                                                .ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.StudentFeeTypes.Add(model);

                if (localizations.Any())
                {
                    _dbContext.StudentFeeTypeLocalizations.AddRange(localizations);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(model, localizations);

            return response;
        }

        public PagedViewModel<StudentFeeTypeDTO> Search(SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var query = GenerateSearchQuery(parameters);

            var pagedStudentFeeType = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<StudentFeeTypeDTO>
            {
                Page = pagedStudentFeeType.Page,
                TotalPage = pagedStudentFeeType.TotalPage,
                TotalItem = pagedStudentFeeType.TotalItem,
                Items = (from studentFeeType in pagedStudentFeeType.Items
                         select MapModelToDTO(studentFeeType, studentFeeType.Localizations))
                        .ToList()
            };

            return response;
        }

        public IEnumerable<StudentFeeTypeDTO> Search(SearchCriteriaViewModel? parameters = null)
        {
            var query = GenerateSearchQuery(parameters);

            var studentFeeTypes = query.ToList();

            var response = (from studentFeeType in studentFeeTypes
                            select MapModelToDTO(studentFeeType, studentFeeType.Localizations))
                           .ToList();
            
            return response;
        }

        public IEnumerable<StudentFeeTypeDTO> GetById(IEnumerable<Guid> ids)
        {
            var studentFeeTypes = _dbContext.StudentFeeTypes.Include(x => x.Localizations)
                                                            .AsNoTracking()
                                                            .Where(x => ids.Contains(x.Id))
                                                            .ToList();

            var response = (from studentFeeType in studentFeeTypes
                            select MapModelToDTO(studentFeeType, studentFeeType.Localizations))
                           .ToList();
            
            return response;                                                
        }

        public StudentFeeTypeDTO GetById(Guid id)
        {
            var studentFeeType = _dbContext.StudentFeeTypes.Include(x => x.Localizations)
                                                           .AsNoTracking()
                                                           .SingleOrDefault(x => x.Id == id);
            
            if (studentFeeType is null)
            {
                throw new StudentFeeTypeException.NotFound(id);
            }

            var response = MapModelToDTO(studentFeeType, studentFeeType.Localizations);

            return response;
        }

        public StudentFeeTypeDTO Update(StudentFeeTypeDTO request, string requester)
        {
            var studentFeeType = _dbContext.StudentFeeTypes.Include(x => x.Localizations)
                                                           .SingleOrDefault(x => x.Id == request.Id);

            if (studentFeeType is null)
            {
                throw new StudentFeeTypeException.NotFound(request.Id);
            }

            var localizations = request.Localizations is null ? Enumerable.Empty<StudentFeeTypeLocalization>()
                                                              : (from localize in request.Localizations
                                                                 select new StudentFeeTypeLocalization
                                                                 {
                                                                     StudentFeeTypeId = studentFeeType.Id,
                                                                     Language = localize.Language,
                                                                     Name = localize.Name
                                                                 })
                                                                .ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                studentFeeType.Name = request.Name;
                studentFeeType.IsActive = request.IsActive;
                studentFeeType.UpdatedAt = DateTime.UtcNow;
                studentFeeType.UpdatedBy = requester;

                _dbContext.StudentFeeTypeLocalizations.RemoveRange(studentFeeType.Localizations);

                if (localizations.Any())
                {
                    _dbContext.StudentFeeTypeLocalizations.AddRange(localizations);
                }
                
                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(studentFeeType, localizations);

            return response;
        }

        public void Delete(Guid id)
        {
            var studentFeeType = _dbContext.StudentFeeTypes.SingleOrDefault(x => x.Id == id);

            if (studentFeeType is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.StudentFeeTypes.Remove(studentFeeType);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        private static StudentFeeTypeDTO MapModelToDTO(StudentFeeType model, IEnumerable<StudentFeeTypeLocalization> localizations)
        {
            var response = new StudentFeeTypeDTO
            {
                Id = model.Id,
                Name = model.Name,
                IsActive = model.IsActive,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                Localizations = localizations is null ? Enumerable.Empty<StudentFeeTypeLocalizationDTO>()
                                                      : (from localize in localizations
                                                         orderby localize.Language
                                                         select new StudentFeeTypeLocalizationDTO
                                                         {
                                                             Language = localize.Language,
                                                             Name = localize.Name
                                                         })
                                                        .ToList()
            };
            
            return response;
        }

        private IQueryable<StudentFeeType> GenerateSearchQuery(SearchCriteriaViewModel? parameters = null)
        {
            var query = _dbContext.StudentFeeTypes.Include(x => x.Localizations)
                                                  .AsNoTracking();

            if (parameters is not null)
            {
                if (!string.IsNullOrEmpty(parameters.Keyword))
                {
                    query = query.Where(x => x.Name.Contains(parameters.Keyword));
                }

                if (parameters.IsActive.HasValue)
                {
                    query = query.Where(x => x.IsActive == parameters.IsActive.Value);
                }
            }

            query = query.OrderBy(x => x.CreatedAt);

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
    }
}