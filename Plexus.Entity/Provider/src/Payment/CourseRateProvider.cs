using System;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Plexus.Database;
using Plexus.Database.Model.Payment;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Payment;
using Plexus.Entity.Exception;
using Plexus.Utility.Extensions;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider.src.Payment
{
    public class CourseRateProvider : ICourseRateProvider
    {
        private readonly DatabaseContext _dbContext;

        public CourseRateProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public CourseRateDTO Create(CreateCourseRateDTO request, string requester)
        {
            var model = new CourseRate
            {
                Name = request.Description,
                Conditions = request.Condition is null ? null
                                                       : JsonConvert.SerializeObject(request.Condition),
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = requester
            };

            var indexes = request.Indexes is null ? Enumerable.Empty<CourseRateIndex>()
                                                  : (from index in request.Indexes
                                                     orderby index.Index
                                                     select new CourseRateIndex
                                                     {
                                                         CourseRate = model,
                                                         RateTypeId = index.RateTypeId,
                                                         Index = index.Index,
                                                         Amount = index.Amount,
                                                         CalculationType = index.CalculationType
                                                     })
                                                    .ToList();

            var indexTransactions = request.Indexes is null ? Enumerable.Empty<CourseRateIndexTransaction>()
                                                           : (from index in request.Indexes
                                                              orderby index.Index
                                                              select new CourseRateIndexTransaction
                                                              {
                                                                  CourseRate = model,
                                                                  RateTypeId = index.RateTypeId,
                                                                  Index = index.Index,
                                                                  Amount = index.Amount,
                                                                  CalculationType = index.CalculationType,
                                                                  UpdatedAt = DateTime.UtcNow,
                                                                  UpdatedBy = requester
                                                              })
                                                             .ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.CourseRates.Add(model);

                if (indexes.Any())
                {
                    _dbContext.CourseRateIndexes.AddRange(indexes);
                    _dbContext.CourseRateIndexTransactions.AddRange(indexTransactions);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(model, indexes);

            return response;
        }

        public CourseRateDTO GetById(Guid id)
        {
            var courseRate = _dbContext.CourseRates.AsNoTracking()
                                                   .SingleOrDefault(x => x.Id == id);

            if (courseRate is null)
            {
                throw new CourseRateException.NotFound(id);
            }

            var response = MapModelToDTO(courseRate, null);

            return response;
        }

        public IEnumerable<CourseRateDTO> GetByIds(IEnumerable<Guid> ids)
        {
            var courseRates = _dbContext.CourseRates.Where(x => ids.Contains(x.Id))
                                                    .AsNoTracking()
                                                    .ToList();

            var response = (from rate in courseRates
                            select MapModelToDTO(rate, null))
                           .ToList();

            return response;
        }

        public IEnumerable<CourseRateDTO> GetAll()
        {
            var courseRates = _dbContext.CourseRates.AsNoTracking()
                                                    .ToList();

            var response = (from rate in courseRates
                            select MapModelToDTO(rate, null))
                           .ToList();

            return response;
        }

        public PagedViewModel<CourseRateDTO> Search(SearchCriteriaViewModel parameters, int page, int pageSize)
        {
            var query = GenerateSearchQuery(parameters);

            var pagedCourseRate = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<CourseRateDTO>
            {
                Page = pagedCourseRate.Page,
                TotalPage = pagedCourseRate.TotalPage,
                TotalItem = pagedCourseRate.TotalItem,
                Items = (from rate in pagedCourseRate.Items
                         select MapModelToDTO(rate, null))
                        .ToList()
            };

            return response;
        }

        public CourseRateDTO Update(CourseRateDTO request, string requester)
        {
            var existingRate = _dbContext.CourseRates.SingleOrDefault(x => x.Id == request.Id);

            if (existingRate is null)
            {
                throw new CourseRateException.NotFound(request.Id);
            }

            existingRate.Name = request.Description;
            existingRate.Conditions = request.Condition is null ? null
                                                                : JsonConvert.SerializeObject(request.Condition);
            existingRate.IsActive = request.IsActive;
            existingRate.UpdatedAt = DateTime.UtcNow;
            existingRate.UpdatedBy = requester;

            var indexes = request.Indexes is null ? Enumerable.Empty<CourseRateIndex>()
                                                  : (from index in request.Indexes
                                                     orderby index.Index
                                                     select new CourseRateIndex
                                                     {
                                                         CourseRateId = existingRate.Id,
                                                         RateTypeId = index.RateTypeId,
                                                         Index = index.Index,
                                                         Amount = index.Amount,
                                                         CalculationType = index.CalculationType
                                                     })
                                                    .ToList();

            var indexTransactions = request.Indexes is null ? Enumerable.Empty<CourseRateIndexTransaction>()
                                                           : (from index in request.Indexes
                                                              orderby index.Index
                                                              select new CourseRateIndexTransaction
                                                              {
                                                                  CourseRateId = existingRate.Id,
                                                                  RateTypeId = index.RateTypeId,
                                                                  Index = index.Index,
                                                                  Amount = index.Amount,
                                                                  CalculationType = index.CalculationType,
                                                                  UpdatedAt = DateTime.UtcNow,
                                                                  UpdatedBy = requester
                                                              })
                                                             .ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                if (indexes.Any())
                {
                    _dbContext.CourseRateIndexes.AddRange(indexes);
                    _dbContext.CourseRateIndexTransactions.AddRange(indexTransactions);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(existingRate, indexes);

            return response;
        }

        public void Delete(Guid id)
        {
            var existingRate = _dbContext.CourseRates.SingleOrDefault(x => x.Id == id);

            if (existingRate is null)
            {
                return;
            }

            using (var tranasction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.CourseRates.Remove(existingRate);

                tranasction.Commit();
            }

            _dbContext.SaveChanges();
        }

        private static CourseRateDTO MapModelToDTO(CourseRate model, IEnumerable<CourseRateIndex>? indexes)
        {
            var conditions = string.IsNullOrEmpty(model.Conditions) ? null
                                                                    : JsonConvert.DeserializeObject<CourseRateConditionDTO>(model.Conditions);

            var response = new CourseRateDTO
            {
                Id = model.Id,
                Description = model.Name,
                IsActive = model.IsActive,
                Condition = conditions,
                Indexes = indexes is null ? Enumerable.Empty<CourseRateIndexDTO>()
                                        : (from index in indexes
                                           orderby index.RateTypeId, index.Index
                                           select new CourseRateIndexDTO
                                           {
                                               Index = index.Index,
                                               RateTypeId = index.RateTypeId,
                                               Amount = index.Amount,
                                               CalculationType = index.CalculationType
                                           })
                                          .ToList(),
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt
            };

            return response;
        }

        private IQueryable<CourseRate> GenerateSearchQuery(SearchCriteriaViewModel? parameters = null)
        {
            var query = _dbContext.CourseRates.AsNoTracking();

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

