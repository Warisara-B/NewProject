using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Plexus.Database;
using Plexus.Database.Model.Academic.Section;
using Plexus.Entity.DTO.Academic.Section;
using Plexus.Entity.Exception;

namespace Plexus.Entity.Provider.src.Academic
{
    public class ExclusionConditionProvider : IExclusionConditionProvider
    {
        private readonly DatabaseContext _dbContext;

        public ExclusionConditionProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ExclusionConditionDTO Create(CreateExclusionConditionDTO request, string requester)
        {
            var model = new ExclusionCondition
            {
                SectionId = request.SectionId,
                Name = request.Name,
                Description = request.Description,
                Conditions = JsonConvert.SerializeObject(request.Conditions),
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = requester
            };

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.ExclusionConditions.Add(model);

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(model);

            return response;
        }

        public IEnumerable<ExclusionConditionDTO> GetBySectionId(Guid sectionId)
        {
            var exclusionConditions = _dbContext.ExclusionConditions.AsNoTracking()
                                                                    .Where(x => x.SectionId == sectionId)
                                                                    .ToList();
            
            var response = (from exclusionCondition in exclusionConditions
                            select MapModelToDTO(exclusionCondition))
                           .ToList();

            return response;
        }

        public IEnumerable<ExclusionConditionDTO> GetById(IEnumerable<Guid> ids)
        {
            var exclusionConditions = _dbContext.ExclusionConditions.AsNoTracking()
                                                                    .Where(x => ids.Contains(x.Id))
                                                                    .ToList();
            
            var response = (from exclusionCondition in exclusionConditions
                            select MapModelToDTO(exclusionCondition))
                           .ToList();

            return response;
        }

        public ExclusionConditionDTO GetById(Guid id)
        {
            var exclusionCondition = _dbContext.ExclusionConditions.AsNoTracking()
                                                                   .SingleOrDefault(x => x.Id == id);

            if (exclusionCondition is null)
            {
                throw new ExclusionConditionException.NotFound(id);
            }
            
            var response = MapModelToDTO(exclusionCondition);

            return response;
        }

        public ExclusionConditionDTO Update(ExclusionConditionDTO request, string requester)
        {
            var exclusionCondition = _dbContext.ExclusionConditions.SingleOrDefault(x => x.Id == request.Id);

            if (exclusionCondition is null)
            {
                throw new ExclusionConditionException.NotFound(request.Id);
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                exclusionCondition.Name = request.Name;
                exclusionCondition.Description = request.Description;
                exclusionCondition.Conditions = JsonConvert.SerializeObject(request.Conditions);
                exclusionCondition.UpdatedAt = DateTime.UtcNow;
                exclusionCondition.UpdatedBy = requester;

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(exclusionCondition);

            return response;
        }

        public void Delete(Guid id)
        {
            var exclusionCondition = _dbContext.ExclusionConditions.SingleOrDefault(x => x.Id == id);

            if (exclusionCondition is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.ExclusionConditions.Remove(exclusionCondition);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        private static ExclusionConditionDTO MapModelToDTO(ExclusionCondition model)
        {
            var conditions = JsonConvert.DeserializeObject<IEnumerable<SectionConditionDTO>>(model.Conditions);

            if (conditions is null)
            {
                throw new ExclusionConditionException.ConditionNotSpecify();
            }

            var response = new ExclusionConditionDTO
            {
                Id = model.Id,
                SectionId = model.SectionId,
                Name = model.Name,
                Description = model.Description,
                Conditions = conditions,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt
            };

            return response;
        }
    }
}