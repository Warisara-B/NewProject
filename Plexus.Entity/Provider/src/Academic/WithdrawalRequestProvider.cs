using System;
using Microsoft.EntityFrameworkCore;
using Plexus.Database;
using Plexus.Database.Enum.Academic;
using Plexus.Database.Model.Academic;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Academic;
using Plexus.Entity.Exception;
using Plexus.Utility.Extensions;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider.src.Academic
{
    public class WithdrawalRequestProvider : IWithdrawalRequestProvider
    {
        private readonly DatabaseContext _dbContext;

        public WithdrawalRequestProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<WithdrawalRequestDTO> Create(IEnumerable<Guid> studyCourseIds, string remark, string requester)
        {
            if (!studyCourseIds.Any())
            {
                return Enumerable.Empty<WithdrawalRequestDTO>();
            }

            var models = (from studyCourseId in studyCourseIds
                          select new WithdrawalRequest
                          {
                              StudyCourseId = studyCourseId,
                              Status = WithdrawalStatus.REQUESTED,
                              Remark = remark,
                              CreatedAt = DateTime.UtcNow,
                              CreatedBy = requester,
                              UpdatedAt = DateTime.UtcNow,
                              UpdatedBy = requester
                          })
                        .ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.WithdrawalRequests.AddRange(models);

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var studyCourses = _dbContext.StudyCourses.AsNoTracking()
                                                      .Where(x => studyCourseIds.Contains(x.Id))
                                                      .ToList();

            var response = (from model in models
                            let studyCourse = studyCourses.SingleOrDefault(x => x.Id == model.StudyCourseId)
                            select MapModeltToDTO(model, studyCourse))
                           .ToList();

            return response;
        }

        public WithdrawalRequestDTO GetById(Guid id)
        {
            var withdrawalRequest = _dbContext.WithdrawalRequests.AsNoTracking()
                                                                 .Include(x => x.StudyCourse)
                                                                 .SingleOrDefault(x => x.Id == id);

            if (withdrawalRequest is null)
            {
                throw new WithdrawalException.NotFound(id);
            }

            var response = MapModeltToDTO(withdrawalRequest, withdrawalRequest.StudyCourse);

            return response;
        }

        public IEnumerable<WithdrawalRequestDTO> GetById(IEnumerable<Guid> ids)
        {
            var withdrawalRequests = _dbContext.WithdrawalRequests.AsNoTracking()
                                                                 .Include(x => x.StudyCourse)
                                                                 .Where(x => ids.Contains(x.Id))
                                                                 .ToList();

            var response = (from withdrawalRequest in withdrawalRequests
                            orderby withdrawalRequest.CreatedAt
                            select MapModeltToDTO(withdrawalRequest, withdrawalRequest.StudyCourse))
                           .ToList();

            return response;
        }

        public IEnumerable<WithdrawalRequestDTO> GetPendingRequestByStudyCourseId(IEnumerable<Guid> studyCourseIds)
        {
            if(studyCourseIds is null || !studyCourseIds.Any())
            {
                return Enumerable.Empty<WithdrawalRequestDTO>();
            }

            var withdrawalRequests = _dbContext.WithdrawalRequests.AsNoTracking()
                                                                  .Include(x => x.StudyCourse)
                                                                  .Where(x => studyCourseIds.Contains(x.StudyCourseId)
                                                                              && x.Status == WithdrawalStatus.REQUESTED)
                                                                  .ToList();

            var response = (from withdrawalRequest in withdrawalRequests
                            orderby withdrawalRequest.CreatedAt
                            select MapModeltToDTO(withdrawalRequest, withdrawalRequest.StudyCourse))
                           .ToList();

            return response;
        }

        public PagedViewModel<WithdrawalRequestDTO> Search(SearchCriteriaViewModel? parameter, int page, int pageSize)
        {
            var query = GenerateSeachQuery(parameter);

            var pagedResult = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<WithdrawalRequestDTO>
            {
                TotalPage = pagedResult.TotalPage,
                Page = pagedResult.Page,
                TotalItem = pagedResult.TotalItem,
                Items = (from withdrawalRequest in pagedResult.Items
                         select MapModeltToDTO(withdrawalRequest, withdrawalRequest.StudyCourse))
                        .ToList()
            };

            return response;
        }

        public IEnumerable<WithdrawalRequestDTO> Search(SearchCriteriaViewModel? parameter)
        {
            var query = GenerateSeachQuery(parameter).ToList();

            var response = (from withdrawRequest in query
                            select MapModeltToDTO(withdrawRequest, withdrawRequest.StudyCourse))
                           .ToList();

            return response;
        }

        public void UpdateWithdrawalStatus(IEnumerable<Guid> ids, WithdrawalStatus status, string remark, string requester)
        {
            var requests = _dbContext.WithdrawalRequests.Where(x => ids.Contains(x.Id))
                                                        .ToList();

            foreach (var request in requests)
            {
                if (request.Status == status)
                    continue;

                request.Status = status;
                request.UpdatedAt = DateTime.UtcNow;
                request.UpdatedBy = requester;

                if (status == WithdrawalStatus.APPROVED)
                {
                    request.ApprovedAt = DateTime.UtcNow;
                    request.ApprovedBy = requester;
                }
            }

            // IF UPDATED STATUS IS REJECTED, JUST UPDATE REQUEST, NO NEED TO UPDATE STUDY COURSE + GRADE LOGS
            if (status == WithdrawalStatus.REJECTED)
            {
                _dbContext.SaveChanges();

                return;
            }

            var studyCourseIds = requests.Select(x => x.StudyCourseId)
                                         .ToList();

            var studyCourses = _dbContext.StudyCourses.Include(x => x.Grade)
                                                      .Where(x => studyCourseIds.Contains(x.Id))
                                                      .ToList();

            foreach (var studyCourse in studyCourses)
            {
                // APPROVE -> MOVE STATUS TO WITHDRAWN
                if (status == WithdrawalStatus.APPROVED)
                    studyCourse.Status = StudyCourseStatus.WITHDRAWN;

                // CANCELD -> REVERT TO ACTIVE / REGISTER
                if (status == WithdrawalStatus.CANCELED)
                    studyCourse.Status = studyCourse.PaidAt.HasValue ? StudyCourseStatus.ACTIVE
                                                                     : StudyCourseStatus.REGISTERED;

            }

            var gradeLogs = (from studyCourse in studyCourses
                             select new GradeLog
                             {
                                 StudyCourseId = studyCourse.Id,
                                 FromGrade = status == WithdrawalStatus.APPROVED ? studyCourse.Grade?.Letter
                                                                                 : "W",
                                 ToGrade = status == WithdrawalStatus.CANCELED ? studyCourse.Grade?.Letter
                                                                                 : "W",
                                 CreatedAt = DateTime.UtcNow,
                                 CreatedBy = requester
                             })
                            .ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                if (gradeLogs.Any())
                    _dbContext.GradeLogs.AddRange(gradeLogs);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        private IQueryable<WithdrawalRequest> GenerateSeachQuery(SearchCriteriaViewModel? parameters)
        {
            var query = _dbContext.WithdrawalRequests.Include(x => x.StudyCourse)
                                                     .AsNoTracking();

            if (parameters is not null)
            {
                if (parameters.TermId.HasValue)
                {
                    query = query.Where(x => x.StudyCourse.TermId == parameters.TermId.Value);
                }

                if (!string.IsNullOrEmpty(parameters.Code))
                {
                    query = query.Where(x => x.StudyCourse.Student.Code == parameters.Code);
                }

                if (!string.IsNullOrEmpty(parameters.Keyword))
                {
                    query = query.Where(x => x.StudyCourse.Course.Code.Contains(parameters.Keyword)
                                             || x.StudyCourse.Course.Name.Contains(parameters.Keyword));
                }

                if (parameters.WithdrawalStatus.HasValue)
                {
                    query = query.Where(x => x.Status == parameters.WithdrawalStatus.Value);
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

        private static WithdrawalRequestDTO MapModeltToDTO(WithdrawalRequest model, StudyCourse studyCourse)
        {
            return new WithdrawalRequestDTO
            {
                Id = model.Id,
                StudyCourseId = model.StudyCourseId,
                Status = model.Status,
                StudentId = studyCourse.StudentId,
                CourseId = studyCourse.CourseId,
                SectionId = studyCourse.SectionId,
                TermId = studyCourse.TermId,
                Remark = model.Remark,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                ApprovedAt = model.ApprovedAt
            };
        }
    }
}

