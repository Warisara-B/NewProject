using System;
using Microsoft.EntityFrameworkCore;
using Plexus.Database;
using Plexus.Database.Enum.Payment.Scholarship;
using Plexus.Database.Model.Payment.Scholarship;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Payment.Scholarship;
using Plexus.Entity.Exception;
using Plexus.Utility.ViewModel;
using Plexus.Utility.Extensions;

namespace Plexus.Entity.Provider.src.Payment.Scholarship
{
    public class StudentScholarshipProvider : IStudentScholarshipProvider
    {
        private readonly DatabaseContext _dbContext;

        public StudentScholarshipProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public StudentScholarshipDTO Create(CreateStudentScholarshipDTO request, string requester)
        {
            var (model, reservedBudgets, usages) = MapDTOToModel(request, requester);

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.StudentScholarships.Add(model);

                if (reservedBudgets.Any())
                {
                    _dbContext.StudentScholarshipReserveBudgets.AddRange(reservedBudgets);
                }

                _dbContext.StudentScholarshipUsages.AddRange(usages);

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapScholarshipDTO(model, reservedBudgets, usages);

            return response;
        }

        public IEnumerable<StudentScholarshipDTO> Create(IEnumerable<CreateStudentScholarshipDTO> requests, string requester)
        {
            var studentScholarships = new List<StudentScholarship>();
            
            var budgets = new List<StudentScholarshipReserveBudget>();

            var usages = new List<StudentScholarshipUsage>();

            foreach (var request in requests)
            {
                var (model, reservedBudgets, budgetUsages) = MapDTOToModel(request, requester);

                studentScholarships.Add(model);
                
                budgets.AddRange(reservedBudgets);

                usages.AddRange(budgetUsages);
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                if (studentScholarships.Any())
                {
                    _dbContext.StudentScholarships.AddRange(studentScholarships);
                }

                if (budgets.Any())
                {
                    _dbContext.StudentScholarshipReserveBudgets.AddRange(budgets);
                }

                if (usages.Any())
                {
                    _dbContext.StudentScholarshipUsages.AddRange(usages);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = (from studentScholarship in studentScholarships
                            let studentUsages = usages.Where(x => x.StudentScholarshipId == studentScholarship.Id)
                            let studentBudgets = budgets.Where(x => x.StudentScholarshipId == studentScholarship.Id)
                            select MapScholarshipDTO(studentScholarship, studentBudgets, studentUsages))
                           .ToList();
            
            return response;
        }

        public PagedViewModel<StudentScholarshipDTO> Search(SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var query = GenerateSearchQuery(parameters);

            var pagedStudentScholarship = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<StudentScholarshipDTO>
            {
                Page = pagedStudentScholarship.Page,
                TotalPage = pagedStudentScholarship.TotalPage,
                TotalItem = pagedStudentScholarship.TotalItem,
                Items = (from scholarship in pagedStudentScholarship.Items
                         select MapScholarshipDTO(scholarship, scholarship.ReserveBudgets, scholarship.Usages))
                        .ToList()
            };

            return response;
        }

        public StudentScholarshipDTO GetById(Guid id)
        {
            var scholarShip = _dbContext.StudentScholarships.AsNoTracking()
                                                            .Include(x => x.ReserveBudgets)
                                                            .Include(x => x.Usages)
                                                            .SingleOrDefault(x => x.Id == id);

            if (scholarShip is null)
            {
                throw new StudentScholarshipException.NotFound(id);
            }

            var response = MapScholarshipDTO(scholarShip, scholarShip.ReserveBudgets, scholarShip.Usages);

            return response;
        }

        public IEnumerable<StudentScholarshipDTO> GetById(IEnumerable<Guid> ids)
        {
            if (ids is null || !ids.Any())
            {
                return Enumerable.Empty<StudentScholarshipDTO>();
            }

            var scholarShips = _dbContext.StudentScholarships.AsNoTracking()
                                                             .Include(x => x.ReserveBudgets)
                                                             .Include(x => x.Usages)
                                                             .Where(x => ids.Contains(x.Id))
                                                             .ToList();

            var response = (from scholarship in scholarShips
                            orderby scholarship.StartYear, scholarship.StartTerm
                            select MapScholarshipDTO(scholarship, scholarship.ReserveBudgets, scholarship.Usages))
                           .ToList();

            return response;
        }

        public IEnumerable<StudentScholarshipDTO> GetByStudentId(Guid studentId)
        {
            var scholarShips = _dbContext.StudentScholarships.AsNoTracking()
                                                             .Include(x => x.ReserveBudgets)
                                                             .Include(x => x.Usages)
                                                             .Where(x => x.StudentId == studentId)
                                                             .ToList();

            if (!scholarShips.Any())
            {
                return Enumerable.Empty<StudentScholarshipDTO>();
            }

            var response = (from scholarship in scholarShips
                            orderby scholarship.StartYear, scholarship.StartTerm
                            select MapScholarshipDTO(scholarship, scholarship.ReserveBudgets, scholarship.Usages))
                           .ToList();

            return response;
        }

        public IEnumerable<StudentScholarshipReserveBudgetDTO> SearchBudgets(SearchCriteriaViewModel parameters)
        {
            var query = GenerateSearchQuery(parameters);

            var scholarShips = query.ToList();

            var dto = (from scholarship in scholarShips
                       select MapScholarshipDTO(scholarship, scholarship.ReserveBudgets, scholarship.Usages))
                      .ToList();

            var response = dto.SelectMany(x => x.ReserveBudgets)
                              .Where(x => x.Id.HasValue)
                              .OrderBy(x => x.Name)
                              .ToList();
                              
            return response;
        }
        
        public StudentScholarshipReserveBudgetDTO GetBudgetById(Guid id)
        {
            var budget = _dbContext.StudentScholarshipReserveBudgets.Include(x => x.StudentScholarship)
                                                                    .AsNoTracking()
                                                                    .SingleOrDefault(x => x.Id == id);

            if (budget is null)
            {
                throw new StudentScholarshipException.BudgetNotFound(id);
            }

            var usages = _dbContext.StudentScholarshipUsages.AsNoTracking()
                                                            .Where(x => x.ReserveBudgetId.HasValue
                                                                        && x.ReserveBudgetId == id)
                                                            .ToList();

            var response = MapBudgetModelToDTO(budget.StudentScholarship.ScholarShipId, budget, usages);

            return response;
        }

        public PagedViewModel<StudentScholarshipUsageDTO> SearchUsages(Guid studentId, SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var query = _dbContext.StudentScholarshipUsages.Include(x => x.StudentScholarship)
                                                           .AsNoTracking()
                                                           .Where(x => x.StudentScholarship.StudentId == studentId);
            
            if (parameters is not null)
            {
                if (parameters.ScholarshipId.HasValue)
                {
                    query = query.Where(x => x.StudentScholarship.ScholarShipId == parameters.ScholarshipId.Value);
                }
            }

            query = query.OrderBy(x => x.CreatedAt);

            var pagedUsage = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<StudentScholarshipUsageDTO>
            {
                Page = pagedUsage.Page,
                TotalPage = pagedUsage.TotalPage,
                TotalItem = pagedUsage.TotalItem,
                Items = (from usage in pagedUsage.Items
                         select MapUsageModelToDTO(usage))
                        .ToList()
            };

            return response;
        }

        public IEnumerable<StudentScholarshipDTO> GetByScholarshipId(Guid scholarshipId)
        {
            var scholarShips = _dbContext.StudentScholarships.AsNoTracking()
                                                             .Include(x => x.ReserveBudgets)
                                                             .Include(x => x.Usages)
                                                             .Where(x => x.ScholarShipId == scholarshipId)
                                                             .ToList();

            if (!scholarShips.Any())
            {
                return Enumerable.Empty<StudentScholarshipDTO>();
            }

            var response = (from scholarship in scholarShips
                            orderby scholarship.StartYear, scholarship.StartTerm
                            select MapScholarshipDTO(scholarship, scholarship.ReserveBudgets, scholarship.Usages))
                           .ToList();

            return response;
        }

        public StudentScholarshipDTO Update(StudentScholarshipDTO request, string requester)
        {
            var scholarship = _dbContext.StudentScholarships.Include(x => x.ReserveBudgets)
                                                            .Include(x => x.Usages)
                                                            .SingleOrDefault(x => x.Id == request.Id);

            if (scholarship is null)
            {
                throw new StudentScholarshipException.NotFound(request.Id);
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                scholarship.StartTerm = request.StartTerm;
                scholarship.StartYear = request.EndYear;
                scholarship.EndTerm = request.EndTerm;
                scholarship.EndYear = request.EndYear;
                scholarship.IsSendContract = request.IsSendContract;
                scholarship.IsActive = request.IsActive;
                scholarship.Remark = request.Remark;
                scholarship.UpdatedAt = DateTime.UtcNow;
                scholarship.UpdatedBy = requester;

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapScholarshipDTO(scholarship, scholarship.ReserveBudgets, scholarship.Usages);

            return response;
        }

        public void Acitve(Guid id, bool isActive, string requester)
        {
            var studentScholarship = _dbContext.StudentScholarships.SingleOrDefault(x => x.Id == id);

            if (studentScholarship is null)
            {
                throw new StudentScholarshipException.NotFound(id);
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                studentScholarship.IsActive = isActive;
                studentScholarship.UpdatedAt = DateTime.UtcNow;
                studentScholarship.UpdatedBy = requester;

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        public void Approve(Guid id, DateTime approvedAt, Guid approvedBy, string? remark, string requester)
        {
            var studentScholarship = _dbContext.StudentScholarships.SingleOrDefault(x => x.Id == id);

            if (studentScholarship is null)
            {
                throw new StudentScholarshipException.NotFound(id);
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                studentScholarship.ApprovedAt = approvedAt.ToUniversalTime();
                studentScholarship.ApprovedBy = approvedBy;
                studentScholarship.ApprovalRemark = remark;
                studentScholarship.UpdatedAt = DateTime.UtcNow;
                studentScholarship.UpdatedBy = requester;

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var scholarship = _dbContext.StudentScholarships.SingleOrDefault(x => x.Id == id);

            if (scholarship is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.StudentScholarships.Remove(scholarship);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        public void CreateReservedBudget(Guid studentScholarshipId, CreateStudentScholarshipReserveBudgetDTO request, string requester)
        {
            var scholarship = _dbContext.StudentScholarships.AsNoTracking()
                                                            .Include(x => x.Usages)
                                                            .SingleOrDefault(x => x.Id == studentScholarshipId);

            if (scholarship is null)
            {
                throw new StudentScholarshipException.NotFound(studentScholarshipId);
            }

            var model = new StudentScholarshipReserveBudget
            {
                StudentScholarshipId = scholarship.Id,
                Name = request.Name,
                Amount = request.Amount,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = requester
            };


            var deductUsage = new StudentScholarshipUsage
            {
                Amount = request.Amount * -1,
                Action = ScholarshipUsageAction.RESERVE,
                StudentScholarshipId = studentScholarshipId,
                Remark = "Deduct for Reserve balance",
                Year = request.Year,
                Term = request.Term,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester
            };

            var reserveUsage = new StudentScholarshipUsage
            {
                Amount = request.Amount,
                DocumentNumber = request.Name,
                Action = ScholarshipUsageAction.RESERVE,
                StudentScholarshipId = studentScholarshipId,
                ReserveBudget = model,
                Remark = "Reserve balance",
                Year = request.Year,
                Term = request.Term,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester
            };

            var usages = new List<StudentScholarshipUsage> { deductUsage, reserveUsage };

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.StudentScholarshipReserveBudgets.Add(model);

                _dbContext.StudentScholarshipUsages.AddRange(usages);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        public void UpdateReservedBudgetBalance(UpdateScholarshipReservedBudgetDTO request, string requester)
        {
            var scholarship = _dbContext.StudentScholarships.Include(x => x.ReserveBudgets)
                                                            .SingleOrDefault(x => x.Id == request.StudentScholarshipId);

            if (scholarship is null)
            {
                throw new StudentScholarshipException.NotFound(request.StudentScholarshipId);
            }

            var existingBudget = scholarship.ReserveBudgets.SingleOrDefault(x => x.Id == request.ReservedBudgetId);

            if (existingBudget is null)
            {
                throw new StudentScholarshipException.ReservedBudgetNotFound(request.ReservedBudgetId);
            }

            var balanceDiff = request.Amount - existingBudget.Amount;

            var generalPoolUsage = new StudentScholarshipUsage
            {
                Amount = balanceDiff * -1,
                Action = ScholarshipUsageAction.RESERVE,
                StudentScholarshipId = scholarship.Id,
                Remark = "Update Reserve balance",
                Year = request.Year,
                Term = request.Term,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester
            };

            var reserveUsage = new StudentScholarshipUsage
            {
                Amount = balanceDiff,
                DocumentNumber = existingBudget.Name,
                Action = ScholarshipUsageAction.RESERVE,
                StudentScholarshipId = scholarship.Id,
                ReserveBudgetId = existingBudget.Id,
                Remark = "Update Reserve balance",
                Year = request.Year,
                Term = request.Term,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester
            };

            var usages = new[] { generalPoolUsage, reserveUsage };

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                existingBudget.Amount = request.Amount;
                existingBudget.UpdatedAt = DateTime.UtcNow;
                existingBudget.UpdatedBy = requester;

                _dbContext.StudentScholarshipUsages.AddRange(usages);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        public void AdjustScholarshipBudget(AdjustScholarshipBudgetDTO request, string requester)
        {
            var scholarship = _dbContext.StudentScholarships.Include(x => x.ReserveBudgets)
                                                            .SingleOrDefault(x => x.Id == request.StudentScholarshipId);

            if (scholarship is null)
            {
                throw new StudentScholarshipException.NotFound(request.StudentScholarshipId);
            }

            if (request.ReservedBudgetId.HasValue
                && !scholarship.ReserveBudgets.Any(x => x.Id == request.ReservedBudgetId.Value))
            {
                throw new StudentScholarshipException.ReservedBudgetNotFound(request.ReservedBudgetId.Value);
            }

            var adjustUsage = new StudentScholarshipUsage
            {
                Amount = request.Amount * -1,
                Action = ScholarshipUsageAction.ADJUSTMENT,
                StudentScholarshipId = scholarship.Id,
                Remark = request.Remark,
                Term = request.Term,
                Year = request.Year,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester
            };

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                scholarship.StartedLimitBalance += adjustUsage.Amount;

                _dbContext.StudentScholarshipUsages.Add(adjustUsage);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        public void UseScholarshipBudget(UseScholarshipBudgetDTO request, string requester)
        {
            var budget = _dbContext.StudentScholarshipReserveBudgets.AsNoTracking()
                                                                    .SingleOrDefault(x => x.Id == request.ReservedBudgetId);
            
            if (budget is null)
            {
                throw new StudentScholarshipException.ReservedBudgetNotFound(request.ReservedBudgetId);
            }

            var usedUsage = new StudentScholarshipUsage
            {
                DocumentNumber = budget.Name,
                Amount = request.Amount * -1,
                Action = ScholarshipUsageAction.USED,
                StudentScholarshipId = request.StudentScholarshipId,
                ReserveBudgetId = request.ReservedBudgetId,
                Remark = request.Remark,
                Term = request.Term,
                Year = request.Year,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester
            };

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.StudentScholarshipUsages.Add(usedUsage);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        private static (StudentScholarship, IEnumerable<StudentScholarshipReserveBudget>, List<StudentScholarshipUsage>) MapDTOToModel(CreateStudentScholarshipDTO request, string requester)
        {
            var model = new StudentScholarship
            {
                StudentId = request.StudentId,
                ScholarShipId = request.ScholarshipId,
                StartedLimitBalance = request.StartedLimitBalance,
                StartTerm = request.StartTerm,
                StartYear = request.StartYear,
                EndTerm = request.EndTerm,
                EndYear = request.EndYear,
                Remark = request.Remark,
                IsSendContract = request.IsSendContract,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = requester
            };

            var reservedBudgets = request.ReserveBudgets is null ? Enumerable.Empty<StudentScholarshipReserveBudget>()
                                                                 : (from budget in request.ReserveBudgets
                                                                    select new StudentScholarshipReserveBudget
                                                                    {
                                                                        Name = budget.Name,
                                                                        Amount = budget.Amount,
                                                                        StudentScholarship = model,
                                                                        CreatedAt = DateTime.UtcNow,
                                                                        CreatedBy = requester,
                                                                        UpdatedAt = DateTime.UtcNow,
                                                                        UpdatedBy = requester
                                                                    })
                                                                   .ToList();
            var initialUsage = new StudentScholarshipUsage
            {
                Amount = request.StartedLimitBalance,
                Year = request.StartYear,
                Term = request.StartTerm,
                Action = ScholarshipUsageAction.INITIAL,
                StudentScholarship = model,
                Remark = "Inital limit balance",
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester
            };

            var usages = new List<StudentScholarshipUsage> { initialUsage };

            foreach (var budget in reservedBudgets)
            {
                var deductUsage = new StudentScholarshipUsage
                {
                    Amount = budget.Amount * -1,
                    Year = request.StartYear,
                    Term = request.StartTerm,
                    Action = ScholarshipUsageAction.RESERVE,
                    StudentScholarship = model,
                    Remark = "Deduct for Reserve balance",
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = requester
                };

                var reserveUsage = new StudentScholarshipUsage
                {
                    DocumentNumber = budget.Name,
                    Amount = budget.Amount,
                    Year = request.StartYear,
                    Term = request.StartTerm,
                    Action = ScholarshipUsageAction.RESERVE,
                    StudentScholarship = model,
                    ReserveBudget = budget,
                    Remark = "Reserve balance",
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = requester
                };

                usages.Add(deductUsage);
                usages.Add(reserveUsage);
            }

            return (model, reservedBudgets, usages);
        }

        private static StudentScholarshipDTO MapScholarshipDTO(
            StudentScholarship model,
            IEnumerable<StudentScholarshipReserveBudget> reserveBudgets,
            IEnumerable<StudentScholarshipUsage> usages)
        {
            if (usages is null)
            {
                usages = Enumerable.Empty<StudentScholarshipUsage>();
            }

            if (reserveBudgets is null)
            {
                reserveBudgets = Enumerable.Empty<StudentScholarshipReserveBudget>();
            }

            var totalBudget = usages.Where(x => !x.ReserveBudgetId.HasValue
                                                && (x.Action == ScholarshipUsageAction.INITIAL
                                                   || x.Action == ScholarshipUsageAction.ADJUSTMENT))
                                    .Sum(x => x.Amount);

            var budgetPool = new List<StudentScholarshipReserveBudgetDTO>();

            var mainPoolBudget = new StudentScholarshipReserveBudgetDTO
            {
                Id = null,
                Name = "General Pool Budget",
                Amount = totalBudget - reserveBudgets!.Sum(x => x.Amount),
                RemainingAmount = usages.Where(x => !x.ReserveBudgetId.HasValue)
                                        .Sum(x => x.Amount)
            };

            budgetPool.Add(mainPoolBudget);

            if (reserveBudgets.Any())
            {
                var budgets = (from budget in reserveBudgets
                               let budgetUsages = usages.Where(x => x.ReserveBudgetId.HasValue
                                                                    && x.ReserveBudgetId.Value == budget.Id)
                                                        .ToList()
                               orderby budget.Name
                               select MapBudgetModelToDTO(model.ScholarShipId, budget, budgetUsages))
                               .ToList();

                budgetPool.AddRange(budgets);
            }

            var response = new StudentScholarshipDTO
            {
                Id = model.Id,
                StudentId = model.StudentId,
                ScholarshipId = model.ScholarShipId,
                StartTerm = model.StartTerm,
                StartYear = model.StartYear,
                EndTerm = model.EndTerm,
                EndYear = model.EndYear,
                StartedLimitBalance = model.StartedLimitBalance,
                Remark = model.Remark,
                IsSendContract = model.IsSendContract,
                IsActive = model.IsActive,
                ReserveBudgets = budgetPool,
                ApprovedAt = model.ApprovedAt,
                ApprovedBy = model.ApprovedBy,
                ApprovalRemark = model.ApprovalRemark,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt
            };

            return response;
        }

        private static StudentScholarshipReserveBudgetDTO MapBudgetModelToDTO(Guid scholarshipId, StudentScholarshipReserveBudget model, IEnumerable<StudentScholarshipUsage> usages)
        {
            var response = new StudentScholarshipReserveBudgetDTO
            {
                Id = model.Id,
                ScholarshipId = scholarshipId,
                Name = model.Name,
                Amount = model.Amount,
                RemainingAmount = usages.Sum(x => x.Amount)
            };

            return response;
        }

        private static StudentScholarshipUsageDTO MapUsageModelToDTO(StudentScholarshipUsage model)
        {
            var response = new StudentScholarshipUsageDTO
            {
                Id = model.Id,
                ScholarshipId = model.StudentScholarship.ScholarShipId,
                Year = model.Year,
                Term = model.Term,
                DocumentNumber = model.DocumentNumber,
                Amount = model.Amount,
                Remark = model.Remark,
                CreatedAt = model.CreatedAt
            };

            return response;
        }

        private IQueryable<StudentScholarship> GenerateSearchQuery(SearchCriteriaViewModel? parameters = null)
        {
            var query = _dbContext.StudentScholarships.Include(x => x.ReserveBudgets)
                                                      .Include(x => x.Usages)
                                                      .Include(x => x.Student)
                                                      .AsNoTracking();
                                                                    
            if (parameters is not null)
            {
                if (parameters.ScholarshipId.HasValue)
                {
                    query = query.Where(x => x.ScholarShipId == parameters.ScholarshipId.Value);
                }

                if (!string.IsNullOrEmpty(parameters.Code))
                {
                    query = query.Where(x => x.Student.Code.Contains(parameters.Code));
                }

                if (parameters.MinimumGPA.HasValue)
                {
                    query = query.Where(x => x.Student.GPA < parameters.MinimumGPA.Value);
                }

                if (parameters.StudentId.HasValue)
                {
                    query = query.Where(x => x.StudentId == parameters.StudentId.Value);
                }

                if (parameters.IsActive.HasValue)
                {
                    query = query.Where(x => x.IsActive == parameters.IsActive.Value);
                }
            }

            query = query.OrderBy(x => x.StartYear)
                         .ThenBy(x => x.StartTerm)
                         .ThenBy(x => x.Student.Code);

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

