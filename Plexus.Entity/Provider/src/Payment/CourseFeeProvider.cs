using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Plexus.Database;
using Plexus.Database.Model.Payment;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Payment;
using Plexus.Entity.Exception;
using Plexus.Utility.Extensions;

namespace Plexus.Entity.Provider.src.Payment
{
    public class CourseFeeProvider : ICourseFeeProvider
    {
        private readonly DatabaseContext _dbContext;

        public CourseFeeProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public CourseFeeDTO Create(CreateCourseFeeDTO request, string requester)
        {
            var model = new CourseFee
            {
                CourseId = request.CourseId,
                FeeItemId = request.FeeItemId,
                CalculationType = request.CalculationType,
                Conditions = JsonConvert.SerializeObject(request.Condition),
                RateTypeId = request.RateTypeId,
                RateIndex = request.RateIndex,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = requester
            };

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.CourseFees.Add(model);

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(model);

            return response;
        }

        public IEnumerable<CourseFeeDTO> GetByCourseId(Guid courseId)
        {
            var courseFees = _dbContext.CourseFees.AsNoTracking()
                                                  .Where(x => x.CourseId == courseId)
                                                  .ToList();
            
            var response = (from courseFee in courseFees
                            select MapModelToDTO(courseFee))
                           .ToList();
            
            return response;
        }

        public CourseFeeDTO GetById(Guid id)
        {
            var courseFee = _dbContext.CourseFees.AsNoTracking()
                                                 .SingleOrDefault(x => x.Id == id);
            
            if (courseFee is null)
            {
                throw new CourseFeeException.NotFound(id);
            }

            var response = MapModelToDTO(courseFee);

            return response;
        }

        public CourseFeeDTO Update(CourseFeeDTO request, string requester)
        {
            var courseFee = _dbContext.CourseFees.SingleOrDefault(x => x.Id == request.Id);

            if (courseFee is null)
            {
                throw new CourseFeeException.NotFound(request.Id);
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                courseFee.CourseId = request.CourseId;
                courseFee.FeeItemId = request.FeeItemId;
                courseFee.CalculationType = request.CalculationType;
                courseFee.Conditions = JsonConvert.SerializeObject(request.Condition);
                courseFee.RateTypeId = request.RateTypeId;
                courseFee.RateIndex = request.RateIndex;
                courseFee.UpdatedAt = DateTime.UtcNow;
                courseFee.UpdatedBy = requester;

                transaction.Commit();
            }
            
            _dbContext.SaveChanges();

            var response = MapModelToDTO(courseFee);

            return response;
        }

        public void Delete(Guid id)
        {
            var courseFee = _dbContext.CourseFees.SingleOrDefault(x => x.Id == id);

            if (courseFee is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.CourseFees.Remove(courseFee);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        private CourseFeeDTO MapModelToDTO(CourseFee model)
        {
            var response = new CourseFeeDTO
            {
                Id = model.Id,
                CourseId = model.CourseId,
                FeeItemId = model.FeeItemId,
                CalculationType = model.CalculationType,
                Condition = string.IsNullOrEmpty(model.Conditions) ? null
                                                                   : JsonConvert.DeserializeObject<CourseFeeConditionDTO>(model.Conditions),
                RateTypeId = model.RateTypeId,
                RateIndex = model.RateIndex,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt
            };

            return response;
        }

        private IQueryable<CourseFee> GenerateSearchQuery(SearchCriteriaViewModel? parameters = null)
        {
            var query = _dbContext.CourseFees.AsNoTracking();

            if (parameters is not null)
            {
                if (parameters.CourseId.HasValue)
                {
                    query = query.Where(x => x.CourseId == parameters.CourseId);
                }
            }

            query = query.OrderBy(x => x.CreatedAt);

            return query;
        }
    }
}