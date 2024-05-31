using Microsoft.EntityFrameworkCore;
using Plexus.Database;
using Plexus.Database.Model.Payment.Scholarship;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Payment.Scholarship;
using Plexus.Entity.Exception;
using Plexus.Utility.Extensions;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider.src.Payment.Scholarship
{
    public class ScholarshipTypeProvider : IScholarshipTypeProvider
    {
        private readonly DatabaseContext _dbContext;
        
        public ScholarshipTypeProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ScholarshipTypeDTO Create(CreateScholarshipTypeDTO request, string requester)
        {
            var model = new ScholarshipType
            {
                Name = request.Name,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = requester
            };

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.ScholarshipTypes.Add(model);

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(model);

            return response;
        }

        public PagedViewModel<ScholarshipTypeDTO> Search(SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var query = GenerateSearchQuery(parameters);

            var pagedScholarshipType = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<ScholarshipTypeDTO>
            {
                Page = pagedScholarshipType.Page,
                TotalPage = pagedScholarshipType.TotalPage,
                TotalItem = pagedScholarshipType.TotalItem,
                Items = (from scholarshipType in pagedScholarshipType.Items
                         select MapModelToDTO(scholarshipType))
                        .ToList()
            };

            return response;
        }

        public IEnumerable<ScholarshipTypeDTO> Search(SearchCriteriaViewModel parameters)
        {
            var query = GenerateSearchQuery(parameters);

            var scholarshipTypes = query.ToList();

            var response = (from scholarshipType in scholarshipTypes
                	        select MapModelToDTO(scholarshipType))
                           .ToList();
            
            return response;
        }

        public IEnumerable<ScholarshipTypeDTO> GetById(IEnumerable<Guid> ids)
        {
            var scholarshipTypes = _dbContext.ScholarshipTypes.AsNoTracking()
                                                              .Where(x => ids.Contains(x.Id))
                                                              .ToList();
            
            var response = (from scholarshipType in scholarshipTypes
                	        select MapModelToDTO(scholarshipType))
                           .ToList();
            
            return response;
        }

        public ScholarshipTypeDTO GetById(Guid id)
        {
            var scholarshipType = _dbContext.ScholarshipTypes.AsNoTracking()
                                                             .SingleOrDefault(x => x.Id == id);
            
            if (scholarshipType is null) 
            {
                throw new ScholarshipException.ItemNotFound(id);
            }

            var response = MapModelToDTO(scholarshipType);

            return response;
        }

        public ScholarshipTypeDTO Update(ScholarshipTypeDTO request, string requester)
        {
            var scholarshipType = _dbContext.ScholarshipTypes.SingleOrDefault(x => x.Id == request.Id);

            if (scholarshipType is null)
            {
                throw new ScholarshipException.ItemNotFound(request.Id);
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                scholarshipType.Name = request.Name;
                scholarshipType.IsActive = request.IsActive;
                scholarshipType.UpdatedAt = DateTime.UtcNow;
                scholarshipType.UpdatedBy = requester;

                transaction.Commit();
            }

            _dbContext.SaveChanges();
            
            var response = MapModelToDTO(scholarshipType);

            return response;
        }

        public void Delete(Guid id)
        {
            var scholarshipType = _dbContext.ScholarshipTypes.SingleOrDefault(x => x.Id == id);

            if (scholarshipType is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.ScholarshipTypes.Remove(scholarshipType);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        private static ScholarshipTypeDTO MapModelToDTO(ScholarshipType model)
        {
            var response = new ScholarshipTypeDTO
            {
                Id = model.Id,
                Name = model.Name,
                IsActive = model.IsActive,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt
            };

            return response;
        }

        private IQueryable<ScholarshipType> GenerateSearchQuery(SearchCriteriaViewModel? parameters = null)
        {
            var query = _dbContext.ScholarshipTypes.AsNoTracking();

            if (parameters is not null)
            {
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