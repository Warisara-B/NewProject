using Microsoft.EntityFrameworkCore;
using Plexus.Database;
using Plexus.Database.Enum.Registration;
using Plexus.Database.Model;
using Plexus.Database.Model.Registration;
using Plexus.Entity.DTO.Registration;
using Plexus.Entity.DTO.SearchFilter;
using Plexus.Entity.Exception;
using Plexus.Utility.Extensions;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider.src.Registration
{
    public class PeriodAndSlotProvider : IPeriodAndSlotProvider
    {
        private readonly DatabaseContext _dbContext;

        public PeriodAndSlotProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region period

        public PeriodDTO CreatePeriod(CreatePeriodDTO request, string requester)
        {
            var period = new Period
            {
                Name = request.Name,
                FromDate = request.FromDate,
                ToDate = request.ToDate,
                Type = request.Type,
                TermId = request.TermId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = requester
            };

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Periods.Add(period);

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapPeriodDTO(period);

            return response;
        }

        public IEnumerable<PeriodDTO> GetAllPeriods()
        {
            var periods = _dbContext.Periods.AsNoTracking()
                                            .Include(x => x.Slots)
                                            //.ThenInclude(x => x.SlotConditions)
                                            .Include(x => x.Term)
                                            .ToList();

            var response = (from period in periods
                            orderby period.Term.Year descending, period.Term.Number descending, period.Name
                            select MapPeriodDTO(period, period.Slots))
                           .ToList();

            return response;
        }

        public PeriodDTO GetPeriodById(Guid periodId)
        {
            var period = _dbContext.Periods.AsNoTracking()
                                           .Include(x => x.Slots)
                                           //.ThenInclude(x => x.SlotConditions)
                                           .SingleOrDefault(x => x.Id == periodId);

            if (period is null)
            {
                throw new PeriodException.NotFound(periodId);
            }

            var response = MapPeriodDTO(period, period.Slots);

            return response;
        }

        public IEnumerable<PeriodDTO> GetPeriodsByTermId(Guid termId)
        {
            var periods = _dbContext.Periods.Include(x => x.Slots)
                                            //.ThenInclude(x => x.SlotConditions)
                                            .Include(x => x.Term)
                                            .Where(x => x.TermId == termId)
                                            .ToList();

            var response = (from period in periods
                            orderby period.FromDate, period.Name
                            select MapPeriodDTO(period, period.Slots))
                           .ToList();

            return response;
        }

        public PeriodDTO UpdatePeriod(PeriodDTO request, string requester)
        {
            var period = _dbContext.Periods.Include(x => x.Slots)
                                           //.ThenInclude(x => x.SlotConditions)
                                           .SingleOrDefault(x => x.Id == request.Id);

            if (period is null)
            {
                throw new PeriodException.NotFound(request.Id);
            }

            period.Name = request.Name;
            period.FromDate = request.FromDate;
            period.ToDate = request.ToDate;
            period.TermId = request.TermId;
            period.Type = request.Type;
            period.UpdatedAt = DateTime.UtcNow;
            period.UpdatedBy = requester;

            _dbContext.SaveChanges();

            var response = MapPeriodDTO(period, period.Slots);

            return response;
        }

        public void DeletePeriod(Guid periodId)
        {
            var period = _dbContext.Periods.SingleOrDefault(x => x.Id == periodId);

            if (period is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Periods.Remove(period);
                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        public bool IsStudentHasAvailablePeriod(Guid studentId, Guid termId, PeriodType periodType)
        {
            return false;

            //var student = _dbContext.Students.AsNoTracking()
            //                                 .SingleOrDefault(x => x.Id == studentId);

            //if (student is null)
            //{
            //    throw new StudentException.NotFound(studentId);
            //}

            //var now = DateTime.UtcNow;

            //var periods = _dbContext.Periods.AsNoTracking()
            //                                .Include(x => x.Slots)
            //                                .ThenInclude(x => x.SlotConditions)
            //                                .Where(x => x.FromDate <= now
            //                                            && now <= x.ToDate
            //                                            && x.TermId == termId
            //                                            && x.Type == periodType)
            //                                .ToList();

            //if (!periods.Any())
            //{
            //    return false;
            //}

            //var conditionIds = periods.SelectMany(x => x.Slots)
            //                          .SelectMany(x => x.SlotConditions)
            //                          .Select(x => x.ConditionId)
            //                          .ToList();

            //var conditions = !conditionIds.Any() ? Enumerable.Empty<Condition>()
            //                                     : _dbContext.Conditions.AsNoTracking()
            //                                                            .Where(x => conditionIds.Contains(x.Id))
            //                                                            .ToList();

            //var response = periods.Any(x => IsPeriodAvailableForStudent(student, x, conditions));

            //return response;
        }

        #endregion

        #region slot

        public SlotDTO CreateSlot(CreateSlotDTO request, string requester)
        {
            var period = _dbContext.Periods.AsNoTracking()
                                           .SingleOrDefault(x => x.Id == request.PeriodId);

            if (period is null)
            {
                throw new PeriodException.NotFound(request.PeriodId);
            }

            var slot = new Slot
            {
                Name = request.Name,
                Description = request.Description,
                StartedAt = request.StartedAt,
                EndedAt = request.EndedAt,
                PeriodId = period.Id,
                IsActive = request.IsActive,
                IsSpecialSlot = request.IsSpecialSlot,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = requester,
            };

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Slots.Add(slot);

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapSlotDTO(slot);

            return response;
        }

        public SlotDTO GetSlotById(Guid slotId)
        {
            var slot = _dbContext.Slots.AsNoTracking()
                                       .SingleOrDefault(x => x.Id == slotId);

            if (slot is null)
            {
                throw new SlotException.NotFound(slotId);
            }

            var response = MapSlotDTO(slot);

            return response;
        }

        public IEnumerable<SlotDTO> GetSlotById(IEnumerable<Guid> slotIds)
        {
            var slots = _dbContext.Slots.AsNoTracking()
                                        .Where(x => slotIds.Contains(x.Id))
                                        .ToList();

            var response = (from slot in slots
                            orderby slot.Id
                            select MapSlotDTO(slot))
                           .ToList();

            return response;
        }

        public IEnumerable<SlotDTO> GetSlotByPeriodId(Guid periodId)
        {
            var slots = _dbContext.Slots.AsNoTracking()
                                        .Where(x => x.PeriodId == periodId)
                                        .ToList();

            var response = (from slot in slots
                            orderby slot.StartedAt
                            select MapSlotDTO(slot))
                           .ToList();

            return response;
        }

        public PagedViewModel<SlotDTO> GetPagedSlotByPeriodId(Guid periodId, int page = 1, int pageSize = 25)
        {
            var query = _dbContext.Slots.Where(x => x.PeriodId == periodId)
                                        .AsNoTracking();

            var pagedSlot = query.GetPagedViewModel(page, pageSize);


            var response = new PagedViewModel<SlotDTO>
            {
                TotalPage = pagedSlot.TotalPage,
                Page = pagedSlot.Page,
                TotalItem = pagedSlot.TotalItem,
                Items = (from slot in pagedSlot.Items
                         select MapSlotDTO(slot))
                        .ToList()
            };

            return response;
        }

        public SlotDTO UpdateSlot(SlotDTO request, string requester)
        {
            var slot = _dbContext.Slots.SingleOrDefault(x => x.Id == request.Id);

            if (slot is null)
            {
                throw new SlotException.NotFound(request.Id);
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                slot.Name = request.Name;
                slot.Description = request.Description;
                slot.StartedAt = request.StartedAt;
                slot.EndedAt = request.EndedAt;
                slot.UpdatedAt = DateTime.UtcNow;
                slot.UpdatedBy = requester;
                slot.IsActive = request.IsActive;
                slot.IsSpecialSlot = request.IsSpecialSlot;

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapSlotDTO(slot);

            return response;
        }

        public void DeleteSlot(Guid slotId)
        {
            var slot = _dbContext.Slots.SingleOrDefault(x => x.Id == slotId);

            if (slot is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Slots.Remove(slot);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        #endregion

        private static PeriodDTO MapPeriodDTO(Period model, IEnumerable<Slot> slots = null)
        {
            if (model is null)
            {
                return null;
            }

            var response = new PeriodDTO
            {
                Id = model.Id,
                Name = model.Name,
                FromDate = model.FromDate,
                ToDate = model.ToDate,
                Type = model.Type,
                TermId = model.TermId,
                Slots = slots is null ? new List<SlotDTO>()
                                      : (from periodSlot in model.Slots
                                         orderby periodSlot.StartedAt
                                         select MapSlotDTO(periodSlot))
                                        .ToList(),
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt
            };

            return response;
        }

        private static SlotDTO MapSlotDTO(Slot slot)
        {
            if (slot is null)
            {
                return null;
            }

            var response = new SlotDTO
            {
                Id = slot.Id,
                PeriodId = slot.PeriodId,
                Name = slot.Name,
                Description = slot.Description,
                StartedAt = slot.StartedAt,
                EndedAt = slot.EndedAt,
                CreatedAt = slot.CreatedAt,
                UpdatedAt = slot.UpdatedAt,
                IsActive = slot.IsActive,
                IsSpecialSlot = slot.IsSpecialSlot
            };

            return response;
        }

        private static bool IsPeriodAvailableForStudent(Student student, Period period, IEnumerable<Condition> conditions)
        {
            var now = DateTime.UtcNow;

            if (period.FromDate > now || now > period.ToDate)
            {
                return false;
            }

            var matchingSlots = period.Slots == null ? Enumerable.Empty<Slot>()
                                                     : (from slot in period.Slots
                                                        where slot.StartedAt <= now && now <= slot.EndedAt
                                                        select slot)
                                                        .ToList();

            if (!matchingSlots.Any())
            {
                // REGISTRATION MUST HAVE SLOT
                return period.Type != PeriodType.REGISTRATION;
            }

            //if (matchingSlots.Any(x => x.SlotConditions is null || !x.SlotConditions.Any()))
            //{
            //    return true;
            //}

            //var slotConditions = matchingSlots.Where(x => x.SlotConditions is not null)
            //                                  .SelectMany(x => x.SlotConditions)
            //                                  .ToList();

            // CHECK IF ONLY ONE CONDITION IS MATCH, MEAN AVAILABLE FOR PERIOD
            //var hasMatchingConditions = slotConditions.Any(x =>
            //{
            //    var condition = conditions.SingleOrDefault(y => x.ConditionId == y.Id);

            //    if (condition is null)
            //    {
            //        return false;
            //    }

            //    return IsConditionMatchStudent(condition, student);
            //});

            return true;
        }

        private static bool IsConditionMatchStudent(ConditionDTO condition, Student student)
        {
            // if (!string.IsNullOrEmpty(condition.AllowStudentCodes)
            //     && condition.AllowStudentCodes.Contains($"\"{student.Code}\""))
            // {
            //     return true;
            // }

            if (condition.FacultyId.HasValue
                && student.FacultyId != condition.FacultyId.Value)
            {
                return false;
            }

            if (condition.DepartmentId.HasValue)
            {
                if (!student.DepartmentId.HasValue
                    || student.DepartmentId.Value != condition.DepartmentId.Value)
                {
                    return false;
                }
            }

            // if (!string.IsNullOrEmpty(condition.FromCode)
            //     && string.Compare(student.Code, condition.FromCode) < 0)
            // {
            //     return false;
            // }

            // if (!string.IsNullOrEmpty(condition.ToCode)
            //     && string.Compare(student.Code, condition.ToCode) > 0)
            // {
            //     return false;
            // }

            // if (condition.FromBatch.HasValue
            //     && student.BatchCode < condition.FromBatch.Value)
            // {
            //     return false;
            // }

            // if (condition.ToBatch.HasValue
            //     && student.BatchCode > condition.ToBatch.Value)
            // {
            //     return false;
            // }

            // if (condition.AdmissionType.HasValue
            //     && !condition.AdmissionType.Value.HasFlag(student.AdmissionType))
            // {
            //     return false;
            // }

            // if (condition.AcademicStatus.HasValue
            //     && !condition.AcademicStatus.Value.HasFlag(student.AcademicStatus))
            // {
            //     return false;
            // }

            return true;
        }
    }
}