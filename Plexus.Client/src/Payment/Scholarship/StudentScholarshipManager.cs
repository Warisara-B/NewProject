using System;
using Plexus.Client.ViewModel.DropDown;
using Plexus.Client.ViewModel.Payment.Scholarship;
using Plexus.Database.Model.Payment.Scholarship;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Payment.Scholarship;
using Plexus.Entity.Exception;
using Plexus.Entity.Provider;
using Plexus.Utility.ViewModel;

namespace Plexus.Client.src.Payment.Scholarship
{
    public class StudentScholarshipManager : IStudentScholarshipManager
    {
        private readonly IStudentProvider _studentProvider;
        private readonly IScholarshipProvider _scholarshipProvider;
        private readonly IStudentScholarshipProvider _studentScholarshipProvider;

        public StudentScholarshipManager(IStudentProvider studentProvider,
                                         IScholarshipProvider scholarshipProvider,
                                         IStudentScholarshipProvider studentScholarshipProvider)
        {
            _studentProvider = studentProvider;
            _scholarshipProvider = scholarshipProvider;
            _studentScholarshipProvider = studentScholarshipProvider;
        }

        public StudentScholarshipViewModel Create(Guid studentId, CreateStudentScholarshipViewModel request, Guid userId)
        {
            var student = _studentProvider.GetById(studentId);

            var scholarship = _scholarshipProvider.GetById(request.ScholarshipId);

            var studentScholarships = _studentScholarshipProvider.GetByStudentId(studentId)
                                                                 .ToList();
            
            if (studentScholarships.Any(x => x.ScholarshipId == request.ScholarshipId))
            {
                throw new StudentScholarshipException.Duplicate(student.Code);
            }

            if (request.BudgetPools is null)
            {
                request.BudgetPools = Enumerable.Empty<CreateStudentReservedBudgetViewModel>();
            }

            if (request.StartLimitBalance < decimal.Zero
                || request.BudgetPools.Any(x => x.Amount < decimal.Zero))
            {
                throw new StudentScholarshipException.NotAllowSetBudgetLessThanZero();
            }

            var reservedBudgetAmount = request.BudgetPools.Sum(x => x.Amount);

            if (reservedBudgetAmount > request.StartLimitBalance)
            {
                throw new StudentScholarshipException.ExceedLimitBalance();
            }

            var dto = new CreateStudentScholarshipDTO
            {
                StudentId = student.Id,
                ScholarshipId = scholarship.Id,
                StartedLimitBalance = request.StartLimitBalance,
                StartTerm = request.StartTerm,
                StartYear = request.StartYear,
                EndTerm = request.EndTerm,
                Remark = request.Remark,
                EndYear = request.EndYear,
                IsSendContract = request.IsSendContract,
                IsActive = request.IsActive,
                ReserveBudgets = (from budget in request.BudgetPools
                                  select new CreateStudentScholarshipReserveBudgetDTO
                                  {
                                      Name = budget.Name,
                                      Amount = budget.Amount
                                  })
                                 .ToList()
            };

            var studentScholarship = _studentScholarshipProvider.Create(dto, userId.ToString());

            var response = MapDTOToViewModel(studentScholarship, scholarship, student);

            return response;
        }

        public IEnumerable<StudentScholarshipViewModel> Create(Guid scholarshipId, CreateMultipleScholarshipViewModel request, Guid userId)
        {
            var students = _studentProvider.GetById(request.StudentIds)
                                           .ToList();
            
            foreach (var studentId in request.StudentIds)
            {
                var matchedStudent = students.SingleOrDefault(x => x.Id == studentId);

                if (matchedStudent is null)
                {
                    throw new StudentException.NotFound(studentId);
                }
            }

            var scholarship = _scholarshipProvider.GetById(scholarshipId);

            var existingStudentScholarships = _studentScholarshipProvider.GetByScholarshipId(scholarshipId)
                                                                         .ToList();
            
            var duplicatedStudentScholarships = existingStudentScholarships.Select(x => x.StudentId)
                                                                           .Intersect(request.StudentIds)
                                                                           .ToList();

            if (duplicatedStudentScholarships.Any())
            {
                var codes = students.Where(x => duplicatedStudentScholarships.Contains(x.Id))
                                    .Select(x => x.Code)
                                    .ToList();

                throw new StudentScholarshipException.Duplicate(codes);
            }

            var dto = new List<CreateStudentScholarshipDTO>();

            foreach (var student in students)
            {
                var scholarshipStudent = new CreateStudentScholarshipDTO
                {
                    StudentId = student.Id,
                    ScholarshipId = scholarshipId,
                    StartedLimitBalance = scholarship.LimitBalance,
                    StartTerm = request.StartTerm,
                    StartYear = request.StartYear,
                    EndTerm = request.StartTerm,
                    EndYear = request.StartYear + scholarship.YearDuration,
                    IsActive = true,
                    ReserveBudgets = (from budget in scholarship.Budgets
                                      select new CreateStudentScholarshipReserveBudgetDTO
                                      {
                                          Name = budget.Name,
                                          Amount = budget.Amount
                                      })
                                     .ToList()
                };

                dto.Add(scholarshipStudent);
            }

            var studentScholarships = _studentScholarshipProvider.Create(dto, userId.ToString());

            var response = (from studentScholarship in studentScholarships
                            let student = students.SingleOrDefault(x => x.Id == studentScholarship.StudentId)
                            select MapDTOToViewModel(studentScholarship, scholarship, student))
                           .ToList();
            
            return response;
        }

        public StudentScholarshipViewModel GetById(Guid id)
        {
            var studentScholarship = _studentScholarshipProvider.GetById(id);

            var student = _studentProvider.GetById(studentScholarship.StudentId);

            var scholarship = _scholarshipProvider.GetById(studentScholarship.ScholarshipId);

            var response = MapDTOToViewModel(studentScholarship, scholarship, student);

            return response;
        }

        public IEnumerable<StudentScholarshipViewModel> GetByStudentId(Guid studentId)
        {
            var studentScholarships = _studentScholarshipProvider.GetByStudentId(studentId)
                                                                 .ToList();

            if (!studentScholarships.Any())
            {
                return Enumerable.Empty<StudentScholarshipViewModel>();
            }

            var student = _studentProvider.GetById(studentId);

            var scholarshipIds = studentScholarships.Select(x => x.ScholarshipId)
                                                    .Distinct()
                                                    .ToList();

            var scholarships = _scholarshipProvider.GetById(scholarshipIds)
                                                   .ToList();

            var response = (from studentScholarship in studentScholarships
                            let scholarship = scholarships.SingleOrDefault(x => x.Id == studentScholarship.ScholarshipId)
                            orderby studentScholarship.StartYear, studentScholarship.StartTerm
                            select MapDTOToViewModel(studentScholarship, scholarship, student))
                           .ToList();

            return response;
        }

        public PagedViewModel<StudentScholarshipViewModel> Search(SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var studentScholarships = _studentScholarshipProvider.Search(parameters, page, pageSize);
            
            var studentIds = studentScholarships.Items.Select(x => x.StudentId)
                                                      .Distinct()
                                                      .ToList();

            var students = _studentProvider.GetById(studentIds)
                                           .ToList();

            var scholarshipIds = studentScholarships.Items.Select(x => x.ScholarshipId)
                                                          .Distinct()
                                                          .ToList();
            
            var scholarships = _scholarshipProvider.GetById(scholarshipIds)
                                                   .ToList();

            var response = new PagedViewModel<StudentScholarshipViewModel>
            {
                Page = studentScholarships.Page,
                TotalPage = studentScholarships.TotalPage,
                TotalItem = studentScholarships.TotalItem,
                Items = (from studentScholarship in studentScholarships.Items
                         let student = students.SingleOrDefault(x => x.Id == studentScholarship.StudentId)
                         let scholarship = scholarships.SingleOrDefault(x => x.Id == studentScholarship.ScholarshipId)
                         select MapDTOToViewModel(studentScholarship, scholarship, student))
                        .ToList()
            };

            return response;
        }

        public IEnumerable<StudentScholarshipBudgetDropDownViewModel> GetBudgetDropDownList(SearchCriteriaViewModel parameters)
        {
            var budgets = _studentScholarshipProvider.SearchBudgets(parameters)
                                                     .ToList();
            
            var response = (from budget in budgets
                            select new StudentScholarshipBudgetDropDownViewModel
                            {
                                Id = budget.Id!.Value.ToString(),
                                Name = budget.Name,
                                RemainingAmount = budget.RemainingAmount
                            })
                           .ToList();
            
            return response;
        }

        public StudentReservedBudgetViewModel GetBudgetById(Guid id)
        {
            var budget = _studentScholarshipProvider.GetBudgetById(id);

            var response = MapBudgetDTOToViewModel(budget);

            return response;
        }

        public PagedViewModel<StudentScholarshipUsageViewModel> SearchUsages(Guid studentId, SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var usages = _studentScholarshipProvider.SearchUsages(studentId, parameters, page, pageSize);

            var scholarshipIds = usages.Items.Select(x => x.ScholarshipId)
                                             .Distinct()
                                             .ToList();
            
            var scholarships = _scholarshipProvider.GetById(scholarshipIds)
                                                   .ToList();
            
            var response = new PagedViewModel<StudentScholarshipUsageViewModel>
            {
                Page = usages.Page,
                TotalPage = usages.TotalPage,
                TotalItem = usages.TotalItem,
                Items = (from usage in usages.Items
                         let scholarship = scholarships.SingleOrDefault(x => x.Id == usage.ScholarshipId)
                         select MapUsageDTOToVieWModel(usage, scholarship))
                        .ToList()
            };

            return response;
        }

        public StudentScholarshipViewModel Update(UpdateStudentScholarShipViewModel request, Guid userId)
        {
            var studentScholarship = _studentScholarshipProvider.GetById(request.Id);

            studentScholarship.StartTerm = request.StartTerm;
            studentScholarship.StartYear = request.StartYear;
            studentScholarship.EndTerm = request.EndTerm;
            studentScholarship.EndYear = request.EndYear;
            studentScholarship.Remark = request.Remark;
            studentScholarship.IsSendContract = request.IsSendContract;
            studentScholarship.IsActive = request.IsActive;

            var updateStudentScholarship = _studentScholarshipProvider.Update(studentScholarship, userId.ToString());

            var student = _studentProvider.GetById(updateStudentScholarship.StudentId);
            
            var scholarship = _scholarshipProvider.GetById(updateStudentScholarship.ScholarshipId);

            var response = MapDTOToViewModel(updateStudentScholarship, scholarship, student);

            return response;
        }

        public void Active(Guid id, bool isActive, Guid userId)
        {
            var studentScholarship = _studentScholarshipProvider.GetById(id);

            studentScholarship.IsActive = isActive;

            _studentScholarshipProvider.Acitve(id, isActive, userId.ToString());
        }

        public void Approve(Guid id, ApproveStudentScholarshipViewModel request, Guid userId)
        {
            var studentScholarship = _studentScholarshipProvider.GetById(id);

            if (studentScholarship.ApprovedAt.HasValue)
            {
                return;
            }

            _studentScholarshipProvider.Approve(id, request.ApprovedAt, userId, request.Remark, userId.ToString());
        }

        public void Delete(Guid id)
        {
            _studentScholarshipProvider.Delete(id);
        }

        public void CreateNewReserveBudget(Guid id, CreateStudentReservedBudgetViewModel request, Guid userId)
        {
            var studentScholarship = _studentScholarshipProvider.GetById(id);

            var remainingAmount = studentScholarship.ReserveBudgets.Single(x => !x.Id.HasValue).Amount;

            if (request.Amount > remainingAmount)
            {
                throw new StudentScholarshipException.ExceedLimitBalance();
            }

            var dto = new CreateStudentScholarshipReserveBudgetDTO
            {
                Name = request.Name,
                Amount = request.Amount,
                Term = request.Term,
                Year = request.Year
            };

            _studentScholarshipProvider.CreateReservedBudget(id, dto, userId.ToString());
        }

        public void UpdateReserveBudgetBalance(Guid id, UpdateStudentReservedBudgetViewModel request, Guid userId)
        {
            if (request.Amount < decimal.Zero)
            {
                throw new StudentScholarshipException.NotAllowSetBudgetLessThanZero();
            }

            var studentScholarship = _studentScholarshipProvider.GetById(id);

            var matchingReserveBudget = studentScholarship.ReserveBudgets.SingleOrDefault(x => x.Id == request.Id);

            if (matchingReserveBudget is null)
            {
                throw new StudentScholarshipException.ReservedBudgetNotFound(request.Id);
            }

            var amountDifferent = request.Amount - matchingReserveBudget.Amount;

            var generalPoolRemaining = studentScholarship.ReserveBudgets.Single(x => !x.Id.HasValue).Amount;

            if (amountDifferent == decimal.Zero)
            {
                return;
            }

            if (amountDifferent < decimal.Zero
                && Math.Abs(amountDifferent) > matchingReserveBudget.RemainingAmount)
            {
                throw new StudentScholarshipException.NotAllowUpdateReserveBalance();
            }
            else
            {
                if (amountDifferent > generalPoolRemaining)
                {
                    throw new StudentScholarshipException.ExceedLimitBalance();
                }
            }

            var dto = new UpdateScholarshipReservedBudgetDTO
            {
                StudentScholarshipId = id,
                ReservedBudgetId = request.Id,
                Amount = request.Amount,
                Year = request.Year,
                Term = request.Term
            };

            _studentScholarshipProvider.UpdateReservedBudgetBalance(dto, userId.ToString());
        }

        public void AddAdjustmentBalance(Guid id, CreateStudentReservedBudgetViewModel request, Guid userId)
        {
            if (request.Amount < decimal.Zero)
            {
                throw new StudentScholarshipException.NotAllowAdjustPositionAmount();
            }

            var studentScholarship = _studentScholarshipProvider.GetById(id);

            var diffAmount = studentScholarship.StartedLimitBalance - request.Amount;

            if (diffAmount == decimal.Zero)
            {
                return;
            }

            var matchingBudget = studentScholarship.ReserveBudgets.Single(x => !x.Id.HasValue);

            if (matchingBudget.RemainingAmount < diffAmount)
            {
                throw new StudentScholarshipException.NotAllowAdjustOverRemaining();
            }

            var dto = new AdjustScholarshipBudgetDTO
            {
                StudentScholarshipId = id,
                Amount = diffAmount,
                Remark = request.Remark,
                Term = request.Term,
                Year = request.Year
            };

            _studentScholarshipProvider.AdjustScholarshipBudget(dto, userId.ToString());
        }

        public void UseReserveBudget(Guid id, UpdateStudentReservedBudgetViewModel request, Guid userId)
        {
            var studentScholarship = _studentScholarshipProvider.GetById(id);

            if (request.Amount < decimal.Zero)
            {
                throw new StudentScholarshipException.NotAllowAdjustPositionAmount();
            }

            var budget = studentScholarship.ReserveBudgets.SingleOrDefault(x => x.Id == request.Id);

            if (budget is null)
            {
                throw new StudentScholarshipException.ReservedBudgetNotFound(request.Id);
            }

            if (budget.RemainingAmount < request.Amount)
            {
                throw new StudentScholarshipException.NotAllowAdjustOverRemaining();
            }

            var dto = new UseScholarshipBudgetDTO
            {
                StudentScholarshipId = id,
                ReservedBudgetId = request.Id,
                Amount = request.Amount,
                Remark = request.Remark,
                Term = request.Term,
                Year = request.Year
            };

            _studentScholarshipProvider.UseScholarshipBudget(dto, userId.ToString());
        }

        private static StudentScholarshipViewModel MapDTOToViewModel(StudentScholarshipDTO dto, ScholarshipDTO scholarship, StudentDTO student)
        {
            return new StudentScholarshipViewModel
            {
                Id = dto.Id,
                ScholarshipId = dto.ScholarshipId,
                ScholarshipName = scholarship?.Name,
                StudentId = dto.StudentId,
                StudentCode = student?.Code,
                FirstName = student?.FirstName,
                MiddleName = student?.MiddleName,
                LastName = student?.LastName,
                GPA = student?.GPA,
                StartTerm = dto.StartTerm,
                StartYear = dto.StartYear,
                EndTerm = dto.EndTerm,
                EndYear = dto.EndYear,
                StartLimitBalance = dto.StartedLimitBalance,
                Remark = dto.Remark,
                IsSendContract = dto.IsSendContract,
                IsActive = dto.IsActive,
                ApprovedAt = dto.ApprovedAt,
                ApprovedBy = dto.ApprovedBy,
                ApprovalRemark = dto.ApprovalRemark,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt,
                BudgetPools = (from budget in dto.ReserveBudgets
                               select MapBudgetDTOToViewModel(budget))
                              .ToList()
            };
        }
        
        private static StudentReservedBudgetViewModel MapBudgetDTOToViewModel(StudentScholarshipReserveBudgetDTO dto)
        {
            var response = new StudentReservedBudgetViewModel
            {
                Id = dto.Id,
                ScholarshipId = dto.ScholarshipId,
                Name = dto.Name,
                Amount = dto.Amount,
                UsedAmount = dto.Amount - dto.RemainingAmount,
                RemainingAmount = dto.RemainingAmount
            };

            return response;
        }

        private static StudentScholarshipUsageViewModel MapUsageDTOToVieWModel(StudentScholarshipUsageDTO dto, ScholarshipDTO scholarship)
        {
            var response = new StudentScholarshipUsageViewModel
            {
                Id = dto.Id,
                ScholarshipId = dto.ScholarshipId,
                ScholarshipName = scholarship.Name,
                Year = dto.Year,
                Term = dto.Term,
                DocumentNumber = dto.DocumentNumber,
                Amount = dto.Amount,
                Remark = dto.Remark,
                CreatedAt = dto.CreatedAt
            };

            return response;
        }
        
    }
}

