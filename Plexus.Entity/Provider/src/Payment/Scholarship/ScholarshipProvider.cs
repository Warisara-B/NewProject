using Microsoft.EntityFrameworkCore;
using Plexus.Database;
using Plexus.Database.Model.Payment.Scholarship;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Payment.Scholarship;
using Plexus.Entity.Exception;
using Plexus.Utility.Extensions;
using Plexus.Utility.ViewModel;
using ScholarshipModel = Plexus.Database.Model.Payment.Scholarship.Scholarship;

namespace Plexus.Entity.Provider.src.Payment.Scholarship
{
    public class ScholarshipProvider : IScholarshipProvider
    {
        private readonly DatabaseContext _dbContext;

        public ScholarshipProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ScholarshipDTO Create(CreateScholarshipDTO request, string requester)
        {
            var model = new ScholarshipModel
            {
                ScholarshipTypeId = request.ScholarshipTypeId,
                Sponsor = request.Sponsor,
                Name = request.Name,
                TotalBudget = request.TotalBudget,
                LimitBalance = request.LimitBalance,
                YearDuration = request.YearDuration,
                MinGPA = request.MinGPA,
                MaxGPA = request.MaxGPA,
                IsRepeatCourseApplied = request.IsRepeatCourseApplied,
                IsAllCoverage = request.IsAllCoverage,
                IsActive = request.IsActive,
                Remark = request.Remark,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = requester
            };

            var newBudgets = (from budget in request.Budgets
                              select new ScholarshipReserveBudget
                              {
                                  Scholarship = model,
                                  Name = budget.Name,
                                  Amount = budget.Amount
                              })
                             .ToList();

            var newItems = (from item in request.FeeItems
                            select new ScholarshipFeeItem
                            {
                                Scholarship = model,
                                FeeItemId = item.FeeItemId,
                                Percentage = item.Percentage,
                                Amount = item.Amount
                            })
                           .ToList();
            
            var feeItemTransactions = (from item in newItems
                                       select new ScholarshipFeeItemTransaction
                                       {
                                           Scholarship = model,
                                           FeeItemId = item.FeeItemId,
                                           Percentage = item.Percentage,
                                           Amount = item.Amount,
                                           UpdatedAt = DateTime.UtcNow,
                                           UpdatedBy = requester
                                       })
                                      .ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Scholarships.Add(model);

                if (newBudgets.Any())
                {
                    _dbContext.ScholarshipReservedBudgets.AddRange(newBudgets);
                }

                if (newItems.Any())
                {
                    _dbContext.ScholarshipFeeItems.AddRange(newItems);
                    
                    _dbContext.ScholarshipFeeItemTransactions.AddRange(feeItemTransactions);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(model, newBudgets, newItems);

            return response;
        }

        public PagedViewModel<ScholarshipDTO> Search(SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var query = GenerateSearchQuery(parameters);

            var pagedScholarship = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<ScholarshipDTO>
            {
                Page = pagedScholarship.Page,
                TotalPage = pagedScholarship.TotalPage,
                TotalItem = pagedScholarship.TotalItem,
                Items = (from scholarship in pagedScholarship.Items
                         select MapModelToDTO(scholarship))
                        .ToList()
            };

            return response;
        }

        public IEnumerable<ScholarshipDTO> Search(SearchCriteriaViewModel parameters)
        {
            var query = GenerateSearchQuery(parameters);

            var scholarshipList = query.ToList();

            var response = (from scholarship in scholarshipList
                            select MapModelToDTO(scholarship))
                           .ToList();
            
            return response;
        }

        public IEnumerable<ScholarshipDTO> GetById(IEnumerable<Guid> ids)
        {
            var scholarshipList = _dbContext.Scholarships.AsNoTracking()
                                                         .Where(x => ids.Contains(x.Id))
                                                         .ToList();

            var response = (from scholarship in scholarshipList
                            select MapModelToDTO(scholarship))
                           .ToList();
            
            return response;
        }

        public ScholarshipDTO GetById(Guid id)
        {
            var scholarship = _dbContext.Scholarships.AsNoTracking()
                                                     .SingleOrDefault(x => x.Id == id);
            
            if (scholarship is null)
            {
                throw new ScholarshipException.NotFound(id);
            }

            var budgets = _dbContext.ScholarshipReservedBudgets.AsNoTracking()
                                                               .Where(x => x.ScholarshipId == id)
                                                               .ToList();
            
            var items = _dbContext.ScholarshipFeeItems.AsNoTracking()
                                                      .Where(x => x.ScholarshipId == id)
                                                      .ToList();

            var response = MapModelToDTO(scholarship, budgets, items);

            return response;
        }

        public ScholarshipDTO Update(ScholarshipDTO request, string requester)
        {
            var scholarship = _dbContext.Scholarships.SingleOrDefault(x => x.Id == request.Id);

            if (scholarship is null)
            {
                throw new ScholarshipException.NotFound(request.Id);
            }

            var existingBudgets = _dbContext.ScholarshipReservedBudgets.Where(x => x.ScholarshipId == request.Id)
                                                                       .ToList();

            var newBudgets = (from budget in request.Budgets
                              select new ScholarshipReserveBudget
                              {
                                  ScholarshipId = request.Id,
                                  Name = budget.Name,
                                  Amount = budget.Amount
                              })
                             .ToList();

            var existingFeeItems = _dbContext.ScholarshipFeeItems.Where(x => x.ScholarshipId == request.Id)
                                                                 .ToList();

            var newItems = (from item in request.FeeItems
                            select new ScholarshipFeeItem
                            {
                                ScholarshipId = request.Id,
                                FeeItemId = item.FeeItemId,
                                Percentage = item.Percentage,
                                Amount = item.Amount
                            })
                           .ToList();

            var feeItemTransactions = (from item in newItems
                                       select new ScholarshipFeeItemTransaction
                                       {
                                           ScholarshipId = item.ScholarshipId,
                                           FeeItemId = item.FeeItemId,
                                           Percentage = item.Percentage,
                                           Amount = item.Amount,
                                           UpdatedAt = DateTime.UtcNow,
                                           UpdatedBy = requester
                                       })
                                      .ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                scholarship.ScholarshipTypeId = request.ScholarshipTypeId;
                scholarship.Sponsor = request.Sponsor;
                scholarship.Name = request.Name;
                scholarship.TotalBudget = request.TotalBudget;
                scholarship.LimitBalance = request.LimitBalance;
                scholarship.YearDuration = request.YearDuration;
                scholarship.MinGPA = request.MinGPA;
                scholarship.MaxGPA = request.MaxGPA;
                scholarship.IsRepeatCourseApplied = request.IsRepeatCourseApplied;
                scholarship.IsAllCoverage = request.IsAllCoverage;
                scholarship.Remark = request.Remark;
                scholarship.IsActive = request.IsActive;
                scholarship.UpdatedAt = DateTime.UtcNow;
                scholarship.UpdatedBy = requester;

                if (existingBudgets.Any())
                {
                    _dbContext.ScholarshipReservedBudgets.RemoveRange(existingBudgets);
                }
                
                if (newBudgets.Any())
                {
                    _dbContext.ScholarshipReservedBudgets.AddRange(newBudgets);
                }

                if (existingFeeItems.Any())
                {
                    _dbContext.ScholarshipFeeItems.RemoveRange(existingFeeItems);   
                }
                
                if (newItems.Any())
                {
                    _dbContext.ScholarshipFeeItems.AddRange(newItems);
                    
                    _dbContext.ScholarshipFeeItemTransactions.AddRange(feeItemTransactions);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(scholarship, newBudgets, newItems);

            return response;
        }

        public void Delete(Guid id)
        {
            var scholarship = _dbContext.Scholarships.SingleOrDefault(x => x.Id == id);

            if (scholarship is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Scholarships.Remove(scholarship);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        public IEnumerable<ScholarshipReserveBudgetDTO> GetReserveBudgetByScholarshipId(Guid id)
        {
            var budgets = _dbContext.ScholarshipReservedBudgets.AsNoTracking()
                                                               .Where(x => x.ScholarshipId == id)
                                                               .ToList();
            
            var response = (from budget in budgets
                            select MapBudgetModelToDTO(budget))
                           .ToList();

            return response;
        }

        public void UpdateReserveBudgets(Guid id, IEnumerable<ScholarshipReserveBudgetDTO> budgets)
        {
            var existingBudgets = _dbContext.ScholarshipReservedBudgets.Where(x => x.ScholarshipId == id)
                                                                       .ToList();
            
            var newBudgets = budgets is null ? Enumerable.Empty<ScholarshipReserveBudget>()
                                             : (from budget in budgets
                                                select new ScholarshipReserveBudget
                                                {
                                                    ScholarshipId = id,
                                                    Name = budget.Name,
                                                    Amount = budget.Amount
                                                })
                                               .ToList();
            
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                if (existingBudgets.Any())
                {
                    _dbContext.ScholarshipReservedBudgets.RemoveRange(existingBudgets);
                }
                
                if (newBudgets.Any())
                {
                    _dbContext.ScholarshipReservedBudgets.AddRange(newBudgets);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        public IEnumerable<ScholarshipFeeItemDTO> GetFeeItemByScholarshipId(Guid id)
        {
            var feeItems = _dbContext.ScholarshipFeeItems.AsNoTracking()
                                                         .Where(x => x.ScholarshipId == id)
                                                         .ToList();
            
            var response = (from feeItem in feeItems
                            select MapItemModelToDTO(feeItem))
                           .ToList();
            
            return response;
        }

        public void UpdateFeeItems(Guid id, IEnumerable<ScholarshipFeeItemDTO> items, string requester)
        {
            var existingFeeItems = _dbContext.ScholarshipFeeItems.Where(x => x.ScholarshipId == id)
                                                                 .ToList();
            
            var newItems = items is null ? Enumerable.Empty<ScholarshipFeeItem>()
                                         : (from item in items
                                            select new ScholarshipFeeItem
                                            {
                                                ScholarshipId = id,
                                                FeeItemId = item.FeeItemId,
                                                Percentage = item.Percentage,
                                                Amount = item.Amount
                                            })
                                           .ToList();
            
            var feeItemTransactions = (from item in newItems
                                       select new ScholarshipFeeItemTransaction
                                       {
                                           ScholarshipId = item.ScholarshipId,
                                           FeeItemId = item.FeeItemId,
                                           Percentage = item.Percentage,
                                           Amount = item.Amount,
                                           UpdatedAt = DateTime.UtcNow,
                                           UpdatedBy = requester
                                       })
                                      .ToList();
            
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                if (existingFeeItems.Any())
                {
                    _dbContext.ScholarshipFeeItems.RemoveRange(existingFeeItems);   
                }
                
                if (newItems.Any())
                {
                    _dbContext.ScholarshipFeeItems.AddRange(newItems);
                    
                    _dbContext.ScholarshipFeeItemTransactions.AddRange(feeItemTransactions);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        private static ScholarshipDTO MapModelToDTO(ScholarshipModel model, IEnumerable<ScholarshipReserveBudget>? budgets = null, IEnumerable<ScholarshipFeeItem>? items = null)
        {
            var response = new ScholarshipDTO
            {
                Id = model.Id,
                ScholarshipTypeId = model.ScholarshipTypeId,
                Sponsor = model.Sponsor,
                Name = model.Name,
                TotalBudget = model.TotalBudget,
                LimitBalance = model.LimitBalance,
                YearDuration = model.YearDuration,
                MinGPA = model.MinGPA,
                MaxGPA = model.MaxGPA,
                IsRepeatCourseApplied = model.IsRepeatCourseApplied,
                IsAllCoverage = model.IsAllCoverage,
                IsActive = model.IsActive,
                Remark = model.Remark,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                Budgets = budgets is null || !budgets.Any() ? Enumerable.Empty<ScholarshipReserveBudgetDTO>()
                                                            : (from budget in budgets
                                                               select MapBudgetModelToDTO(budget))
                                                              .ToList(),
                FeeItems = items is null || !items.Any() ? Enumerable.Empty<ScholarshipFeeItemDTO>()
                                                         : (from item in items
                                                            select MapItemModelToDTO(item))
                                                           .ToList()
            };

            return response;
        }

        private static ScholarshipReserveBudgetDTO MapBudgetModelToDTO(ScholarshipReserveBudget model)
        {
            var response = new ScholarshipReserveBudgetDTO
            {
                Name = model.Name,
                Amount = model.Amount
            };

            return response;
        }

        private static ScholarshipFeeItemDTO MapItemModelToDTO(ScholarshipFeeItem model)
        {
            var response = new ScholarshipFeeItemDTO
            {
                FeeItemId = model.FeeItemId,
                Percentage = model.Percentage,
                Amount = model.Amount
            };

            return response;
        }

        private IQueryable<Plexus.Database.Model.Payment.Scholarship.Scholarship> GenerateSearchQuery(SearchCriteriaViewModel? parameters = null)
        {
            var query = _dbContext.Scholarships.AsNoTracking();

            if (parameters is not null)
            {
                if (parameters.ScholarshipTypeId.HasValue)
                {
                    query = query.Where(x => x.ScholarshipTypeId == parameters.ScholarshipTypeId.Value);
                }

                if (!string.IsNullOrEmpty(parameters.Sponsor))
                {
                    query = query.Where(x => x.Sponsor.Contains(parameters.Sponsor));
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