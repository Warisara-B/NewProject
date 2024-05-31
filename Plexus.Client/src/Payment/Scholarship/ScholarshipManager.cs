using Plexus.Client.ViewModel.DropDown;
using Plexus.Client.ViewModel.Payment.Scholarship;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Payment;
using Plexus.Entity.DTO.Payment.Scholarship;
using Plexus.Entity.Exception;
using Plexus.Entity.Provider;
using Plexus.Utility.ViewModel;

namespace Plexus.Client.src.Payment.Scholarship
{
    public class ScholarshipManager : IScholarshipManager
    {
        private readonly IScholarshipProvider _scholarshipProvider;
        private readonly IScholarshipTypeProvider _scholarshipTypeProvider;
        private readonly IFeeItemProvider _feeItemProvider;
        private readonly IStudentScholarshipProvider _studentScholarshipProvider;
        
        public ScholarshipManager(IScholarshipProvider scholarshipProvider,
                                  IScholarshipTypeProvider scholarshipTypeProvider,
                                  IFeeItemProvider feeTypeProvider,
                                  IStudentScholarshipProvider studentScholarshipProvider)
        {
            _scholarshipProvider = scholarshipProvider;
            _scholarshipTypeProvider = scholarshipTypeProvider;
            _feeItemProvider = feeTypeProvider;
            _studentScholarshipProvider = studentScholarshipProvider;
        }

        public ScholarshipViewModel Create(CreateScholarshipViewModel request, Guid userId)
        {
            var scholarshipType = _scholarshipTypeProvider.GetById(request.ScholarshipTypeId);

            var (scholarshipFeeItems, feeItems) = MapItemViewModelToDTO(request.FeeItems);

            var dto = new CreateScholarshipDTO
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
                Budgets = MapBudgetViewModelToDTO(request.Budgets),
                FeeItems = scholarshipFeeItems
            };

            var scholarship = _scholarshipProvider.Create(dto, userId.ToString());

            var response = MapDTOToViewModel(scholarship, scholarshipType, feeItems);

            return response;
        }

        public PagedViewModel<ScholarshipViewModel> Search(SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var pagedScholarship = _scholarshipProvider.Search(parameters, page, pageSize);

            var scholarshipTypeIds = pagedScholarship.Items.Select(x => x.ScholarshipTypeId)
                                                           .Distinct()
                                                           .ToList();
            
            var scholarshipTypes = _scholarshipTypeProvider.GetById(scholarshipTypeIds)
                                                           .ToList();

            var response = new PagedViewModel<ScholarshipViewModel>
            {
                Page = pagedScholarship.Page,
                TotalPage = pagedScholarship.TotalPage,
                TotalItem = pagedScholarship.TotalItem,
                Items = (from scholarship in pagedScholarship.Items
                         let scholarshipType = scholarshipTypes.SingleOrDefault(x => x.Id == scholarship.ScholarshipTypeId)
                         select MapDTOToViewModel(scholarship, scholarshipType))
                        .ToList()
            };

            return response;
        }

        public IEnumerable<ScholarshipDropDownViewModel> GetDropDownList(SearchCriteriaViewModel parameters)
        {
            var scholarshipList = _scholarshipProvider.Search(parameters)
                                                      .ToList();
            
            var response = (from scholarship in scholarshipList
                            select MapDTOToDropDown(scholarship))
                           .ToList();
            
            return response;
        }

        public ScholarshipViewModel GetById(Guid id)
        {
            var scholarship = _scholarshipProvider.GetById(id);
            
            var scholarshipType = _scholarshipTypeProvider.GetById(scholarship.ScholarshipTypeId);

            var feeTypeIds = scholarship.FeeItems is null || !scholarship.FeeItems.Any() ? Enumerable.Empty<Guid>()
                                                                                         : scholarship.FeeItems.Select(x => x.FeeItemId)
                                                                                                               .ToList();
                                                                                                            
            var feeItems = _feeItemProvider.GetById(feeTypeIds)
                                           .ToList();
            
            var studentScholarships = _studentScholarshipProvider.GetByScholarshipId(id)
                                                                 .ToList();

            var response = MapDTOToViewModel(scholarship, scholarshipType, feeItems, studentScholarships);

            return response;
        }

        public ScholarshipViewModel Update(ScholarshipViewModel request, Guid userId)
        {
            var scholarship = _scholarshipProvider.GetById(request.Id);

            var scholarshipType = _scholarshipTypeProvider.GetById(request.ScholarshipTypeId);

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

            scholarship.Budgets = MapBudgetViewModelToDTO(request.Budgets);

            var (scholarshipFeeItems, feeItems) = MapItemViewModelToDTO(request.FeeItems);

            scholarship.FeeItems = scholarshipFeeItems;
            
            var updatedScholarship = _scholarshipProvider.Update(scholarship, userId.ToString());

            var response = MapDTOToViewModel(updatedScholarship, scholarshipType, feeItems);

            return response;
        }

        public void Delete(Guid id)
        {
            _scholarshipProvider.Delete(id);
        }

        public IEnumerable<ScholarshipReserveBudgetViewModel> GetReserveBudgetByScholarshipId(Guid id)
        {
            var scholarship = _scholarshipProvider.GetById(id);

            var budgets = _scholarshipProvider.GetReserveBudgetByScholarshipId(id)
                                              .ToList();
            
            var response = (from budget in budgets
                            select MapBudgetDTOToViewModel(budget))
                           .ToList();
            
            return response;
        }

        [Obsolete("logic is include in updated, waiting to be deleted")]
        public IEnumerable<ScholarshipReserveBudgetViewModel> UpdateReserveBudgets(Guid id, IEnumerable<ScholarshipReserveBudgetViewModel> budgets)
        {
            var scholarship = _scholarshipProvider.GetById(id);

            var reserveBudgets = budgets is null ? Enumerable.Empty<ScholarshipReserveBudgetDTO>()
                                                 : (from budget in budgets
                                                    select new ScholarshipReserveBudgetDTO
                                                    {
                                                        Name = budget.Name,
                                                        Amount = budget.Amount
                                                    })
                                                   .ToList();

            var duplicateBudgets = reserveBudgets.GroupBy(x => x.Name)
                                                 .Where(x => x.Count() > 1)
                                                 .ToList();
            
            if (duplicateBudgets.Any())
            {
                throw new ScholarshipException.DuplicateBudgets();
            }

            foreach (var budget in reserveBudgets)
            {
                if (budget.Amount < decimal.Zero)
                {
                    throw new ScholarshipException.BudgetInvalidException(budget.Name);
                }
            }

            _scholarshipProvider.UpdateReserveBudgets(id, reserveBudgets);

            var response = (from budget in reserveBudgets
                            select MapBudgetDTOToViewModel(budget))
                           .ToList();
            
            return response;
        }

        public IEnumerable<ScholarshipFeeItemViewModel> GetFeeItemByScholarshipId(Guid id)
        {
            var scholarship = _scholarshipProvider.GetById(id);

            var scholarshipFeeItems = _scholarshipProvider.GetFeeItemByScholarshipId(id);

            var feeItemIds = scholarshipFeeItems.Select(x => x.FeeItemId)
                                     .ToList();
            
            var feeItems = _feeItemProvider.GetById(feeItemIds)
                                           .ToList();
                
            var response = (from scholarshipFeeItem in scholarshipFeeItems
                            let feeItem = feeItems.SingleOrDefault(x => x.Id == scholarshipFeeItem.FeeItemId)
                            select MapItemDTOToModel(scholarshipFeeItem, feeItem))
                           .ToList();

            return response;
        }

        [Obsolete("logic is include in created and updated, waiting to be deleted")]
        public IEnumerable<ScholarshipFeeItemViewModel> UpdateFeeItems(Guid id, IEnumerable<UpdateScholarshipFeeItemViewModel> items, Guid userId)
        {
            var scholarship = _scholarshipProvider.GetById(id);
            
            var scholarshipFeeItems = items is null ? Enumerable.Empty<ScholarshipFeeItemDTO>()
                                                    : (from item in items
                                                       select new ScholarshipFeeItemDTO
                                                       {
                                                           FeeItemId = item.FeeItemId,
                                                           Percentage = item.Percentage,
                                                           Amount = item.Amount
                                                       })
                                                      .ToList();

            var duplicateFeeItems = scholarshipFeeItems.GroupBy(x => x.FeeItemId)
                                                       .Where(x => x.Count() > 1)
                                                       .ToList();
                                            
            if (duplicateFeeItems.Any())
            {
                throw new ScholarshipException.DuplicateFeeItems();
            }
            
            var feeItemIds = scholarshipFeeItems.Select(x => x.FeeItemId)
                                                .Distinct()
                                                .ToList();
            
            var feeItems = _feeItemProvider.GetById(feeItemIds)
                                           .ToList();

            foreach (var item in scholarshipFeeItems)
            {
                var feeItem = feeItems.SingleOrDefault(x => x.Id == item.FeeItemId);
                if (feeItem is null)
                {
                    throw new FeeItemException.NotFound(item.FeeItemId);
                }

                if ((!item.Percentage.HasValue && !item.Amount.HasValue)
                    || (item.Percentage.HasValue && item.Percentage < decimal.Zero)
                    || (item.Amount.HasValue && item.Amount.Value < decimal.Zero))
                {
                    throw new ScholarshipException.FeeItemInvalidException(item.FeeItemId);
                }
            }

            _scholarshipProvider.UpdateFeeItems(id, scholarshipFeeItems, userId.ToString());

            var response = (from item in scholarshipFeeItems
                            let feeItem = feeItems.SingleOrDefault(x => x.Id == item.FeeItemId)
                            select MapItemDTOToModel(item, feeItem))
                           .ToList();
            
            return response;
        }

        private static ScholarshipViewModel MapDTOToViewModel(ScholarshipDTO dto,
                                                              ScholarshipTypeDTO scholarshipType,
                                                              IEnumerable<FeeItemDTO>? feeItems = null,
                                                              IEnumerable<StudentScholarshipDTO>? studentScholarships = null)
        {
            var response = new ScholarshipViewModel
            {
                Id = dto.Id,
                ScholarshipTypeId = dto.ScholarshipTypeId,
                Sponsor = dto.Sponsor,
                Name = dto.Name,
                TotalBudget = dto.TotalBudget,
                LimitBalance = dto.LimitBalance,
                YearDuration = dto.YearDuration,
                MinGPA = dto.MinGPA,
                MaxGPA = dto.MaxGPA,
                IsRepeatCourseApplied = dto.IsRepeatCourseApplied,
                IsAllCoverage = dto.IsAllCoverage,
                IsActive = dto.IsActive,
                Remark = dto.Remark,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt,
                ScholarshipTypeName = scholarshipType.Name,
                TotalStudents = studentScholarships is null || !studentScholarships.Any() ? 0
                                                                                          : studentScholarships.Count(),
                Budgets = dto.Budgets is null || !dto.Budgets.Any() ? null
                                                                    : (from budget in dto.Budgets
                                                                       select MapBudgetDTOToViewModel(budget))
                                                                      .ToList(),
                FeeItems = dto.FeeItems is null || !dto.FeeItems.Any() ? null
                                                                       : (from scholarshipFeeItem in dto.FeeItems
                                                                          let feeItem = feeItems?.SingleOrDefault(x => x.Id == scholarshipFeeItem.FeeItemId)
                                                                          select MapItemDTOToModel(scholarshipFeeItem, feeItem))
                                                                         .ToList()
            };

            return response;
        }

        private static ScholarshipReserveBudgetViewModel MapBudgetDTOToViewModel(ScholarshipReserveBudgetDTO dto)
        {
            var response = new ScholarshipReserveBudgetViewModel
            {
                Name = dto.Name,
                Amount = dto.Amount
            };

            return response;
        }

        private static ScholarshipFeeItemViewModel MapItemDTOToModel(ScholarshipFeeItemDTO dto, 
                                                                     FeeItemDTO? feeItem)
        {
            var response = new ScholarshipFeeItemViewModel
            {
                FeeItemId = dto.FeeItemId,
                Percentage = dto.Percentage,
                Amount = dto.Amount,
                FeeItemName = feeItem?.Name
            };

            return response;
        }

        private ScholarshipDropDownViewModel MapDTOToDropDown(ScholarshipDTO dto)
        {
            var response = new ScholarshipDropDownViewModel
            {
                Id = dto.Id.ToString(),
                Name = dto.Name,
                ScholarshipTypeId = dto.ScholarshipTypeId
            };

            return response;
        }
        
        private static IEnumerable<ScholarshipReserveBudgetDTO> MapBudgetViewModelToDTO(IEnumerable<ScholarshipReserveBudgetViewModel>? requests)
        {
            var reserveBudgets = requests is null ? Enumerable.Empty<ScholarshipReserveBudgetDTO>()
                                                  : (from budget in requests
                                                     select new ScholarshipReserveBudgetDTO
                                                     {
                                                         Name = budget.Name,
                                                         Amount = budget.Amount
                                                     })
                                                    .ToList();
                                                                                   
            var duplicateBudgets = reserveBudgets.GroupBy(x => x.Name)
                                                 .Where(x => x.Count() > 1)
                                                 .ToList();
            
            if (duplicateBudgets.Any())
            {
                throw new ScholarshipException.DuplicateBudgets();
            }

            foreach (var budget in reserveBudgets)
            {
                if (budget.Amount < decimal.Zero)
                {
                    throw new ScholarshipException.BudgetInvalidException(budget.Name);
                }
            }

            return reserveBudgets;
        }

        private (IEnumerable<ScholarshipFeeItemDTO>, IEnumerable<FeeItemDTO>) MapItemViewModelToDTO(IEnumerable<UpdateScholarshipFeeItemViewModel>? requests)
        {
            var scholarshipFeeItems = requests is null ? Enumerable.Empty<ScholarshipFeeItemDTO>()
                                                       : (from item in requests
                                                          select new ScholarshipFeeItemDTO
                                                          {
                                                              FeeItemId = item.FeeItemId,
                                                              Percentage = item.Percentage,
                                                              Amount = item.Amount
                                                          })
                                                         .ToList();

            var duplicateFeeItems = scholarshipFeeItems.GroupBy(x => x.FeeItemId)
                                                       .Where(x => x.Count() > 1)
                                                       .ToList();
                                            
            if (duplicateFeeItems.Any())
            {
                throw new ScholarshipException.DuplicateFeeItems();
            }
            
            var feeItemIds = scholarshipFeeItems.Select(x => x.FeeItemId)
                                                .Distinct()
                                                .ToList();
            
            var feeItems = _feeItemProvider.GetById(feeItemIds)
                                           .ToList();

            foreach (var item in scholarshipFeeItems)
            {
                var feeItem = feeItems.SingleOrDefault(x => x.Id == item.FeeItemId);
                if (feeItem is null)
                {
                    throw new FeeItemException.NotFound(item.FeeItemId);
                }

                if ((!item.Percentage.HasValue && !item.Amount.HasValue)
                    || (item.Percentage.HasValue && item.Percentage < decimal.Zero)
                    || (item.Amount.HasValue && item.Amount.Value < decimal.Zero))
                {
                    throw new ScholarshipException.FeeItemInvalidException(item.FeeItemId);
                }
            }

            return (scholarshipFeeItems, feeItems);
        }
    }
}