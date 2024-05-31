using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Client.ViewModel.Registration;
using Plexus.Entity.DTO.Academic;
using Plexus.Entity.DTO.Registration;
using Plexus.Entity.DTO.SearchFilter;
using Plexus.Entity.Exception;
using Plexus.Entity.Provider;
using Plexus.Utility.ViewModel;
using ServiceStack;
using Plexus.Database;
using Microsoft.EntityFrameworkCore;
using Plexus.Utility.Extensions;
using Plexus.Database.Model.Registration;
using Plexus.Database.Model.Academic;
using Plexus.Client.ViewModel.Academic;

namespace Plexus.Client.src.Registration
{
    public class PeriodAndSlotManager : IPeriodAndSlotManager
    {
        private readonly IPeriodAndSlotProvider _periodAndSlotProvider;
        private readonly ITermProvider _termProvider;
        private readonly IAcademicLevelProvider _academicLevelProvider;
        private readonly DatabaseContext _dbContext;

        public PeriodAndSlotManager(IPeriodAndSlotProvider periodAndSlotProvider,
                                    ITermProvider termProvider,
                                    IAcademicLevelProvider academicLevelProvider,
                                    DatabaseContext dbContext)
        {
            _periodAndSlotProvider = periodAndSlotProvider;
            _termProvider = termProvider;
            _academicLevelProvider = academicLevelProvider;
            _dbContext = dbContext;
        }

        public PeriodViewModel CreatePeriod(CreatePeriodViewModel request, Guid userId)
        {
            var term = _termProvider.GetById(request.TermId);

            var academicLevel = _academicLevelProvider.GetById(term.AcademicLevelId);

            if (IsPeriodOverlapped(request))
            {
                throw new PeriodException.TimeRangeOverlap(request.Type);
            }

            var dto = new PeriodDTO
            {
                Name = request.Name,
                TermId = request.TermId,
                FromDate = request.FromDate,
                ToDate = request.ToDate,
                Type = request.Type,
                Slots = Enumerable.Empty<SlotDTO>()
            };

            var period = _periodAndSlotProvider.CreatePeriod(dto, userId.ToString());

            var response = MapPeriodDTOToViewModel(period, term, academicLevel);

            return response;
        }

        public PagedViewModel<PeriodViewModel> GetPagedPeriod(int page, int pageSize)
        {
            var query = _dbContext.Periods.Include(x => x.Slots)
                                          .Include(x => x.Term)
                                          .AsNoTracking();

            query = query.OrderByDescending(x => x.Term.Year)
                         .ThenByDescending(x => x.Term.Number)
                         .ThenBy(x => x.FromDate);

            var pagedPeriod = query.GetPagedViewModel(page, pageSize);

            var termIds = pagedPeriod.Items.Select(x => x.TermId)
                                           .Distinct()
                                           .ToList();

            var terms = _dbContext.Terms.AsNoTracking()
                                        .Where(x => termIds.Contains(x.Id))
                                        .ToList();

            var academicLevelIds = terms.Select(x => x.AcademicLevelId)
                                        .ToList();

            var academicLevels = _dbContext.AcademicLevels.AsNoTracking()
                                                          .Include(x => x.Localizations)
                                                          .Where(x => academicLevelIds.Contains(x.Id))
                                                          .ToList();

            var response = new PagedViewModel<PeriodViewModel>
            {
                Page = pagedPeriod.Page,
                TotalItem = pagedPeriod.TotalItem,
                TotalPage = pagedPeriod.TotalPage,
                Items = (from period in pagedPeriod.Items
                         let term = terms.SingleOrDefault(x => x.Id == period.TermId)
                         let academicLevel = term is null ? null : academicLevels.SingleOrDefault(x => x.Id == term.AcademicLevelId)
                         select MapPeriodToViewModel(period, term, academicLevel))
                        .ToList()
            };

            return response;
        }

        public PeriodViewModel GetPeriodById(Guid id)
        {
            var period = _periodAndSlotProvider.GetPeriodById(id);

            var term = _termProvider.GetById(period.TermId);

            var academicLevel = _academicLevelProvider.GetById(term.AcademicLevelId);

            var response = MapPeriodDTOToViewModel(period, term, academicLevel);

            return response;
        }

        public PeriodViewModel UpdatePeriod(Guid periodId, CreatePeriodViewModel request, Guid userId)
        {
            var period = _periodAndSlotProvider.GetPeriodById(periodId);

            var term = _termProvider.GetById(request.TermId);

            var academicLevel = _academicLevelProvider.GetById(term.AcademicLevelId);

            if (IsPeriodOverlapped(request, periodId))
            {
                throw new PeriodException.TimeRangeOverlap(request.Type);
            }

            period.Name = request.Name;
            period.FromDate = request.FromDate;
            period.ToDate = request.ToDate;
            period.Type = request.Type;
            period.TermId = request.TermId;

            var updatedPeriod = _periodAndSlotProvider.UpdatePeriod(period, userId.ToString());

            var response = MapPeriodDTOToViewModel(updatedPeriod, term, academicLevel);

            return response;
        }

        public void DeletePeriod(Guid periodId)
        {
            _periodAndSlotProvider.DeletePeriod(periodId);
        }

        public SlotViewModel CreateSlot(Guid periodId, CreateSlotViewModel request, Guid userId)
        {
            var dto = new CreateSlotDTO
            {
                Name = request.Name,
                Description = request.Description,
                PeriodId = periodId,
                StartedAt = request.StartedAt,
                EndedAt = request.EndedAt,
                IsActive = request.IsActive,
                IsSpecialSlot = request.IsSpecialSlot
            };

            var slot = _periodAndSlotProvider.CreateSlot(dto, userId.ToString());

            var response = MapSlotDTOToViewModel(slot);

            return response;
        }


        public SlotViewModel GetSlotById(Guid slotId)
        {
            var slot = _periodAndSlotProvider.GetSlotById(slotId);

            var response = MapSlotDTOToViewModel(slot);

            return response;
        }

        public IEnumerable<SlotViewModel> GetSlotByPeriodId(Guid periodId)
        {
            var slots = _periodAndSlotProvider.GetSlotByPeriodId(periodId)
                                              .ToList();

            if (!slots.Any())
            {
                return Enumerable.Empty<SlotViewModel>();
            }

            var response = (from slot in slots
                            orderby slot.StartedAt
                            select MapSlotDTOToViewModel(slot))
                            .ToList();

            return response;
        }

        public PagedViewModel<SlotViewModel> GetPagedSlotByPeriodId(Guid periodId, int page, int pageSize)
        {
            var pagedSlot = _periodAndSlotProvider.GetPagedSlotByPeriodId(periodId, page, pageSize);

            var response = new PagedViewModel<SlotViewModel>
            {
                Page = pagedSlot.Page,
                TotalItem = pagedSlot.TotalItem,
                TotalPage = pagedSlot.TotalPage,
                Items = (from slot in pagedSlot.Items
                         select MapSlotDTOToViewModel(slot))
            .ToList()
            };

            return response;
        }

        public SlotViewModel UpdateSlot(Guid periodId, Guid slotId, CreateSlotViewModel request, Guid userId)
        {
            var slots = _periodAndSlotProvider.GetSlotByPeriodId(periodId);

            var slot = slots.SingleOrDefault(x => x.Id == slotId) ?? throw new SlotException.NotFound(slotId);

            slot.Name = request.Name;
            slot.Description = request.Description;
            slot.StartedAt = request.StartedAt;
            slot.EndedAt = request.EndedAt;
            slot.IsActive = request.IsActive;
            slot.IsSpecialSlot = request.IsSpecialSlot;

            var updatedSlot = _periodAndSlotProvider.UpdateSlot(slot, userId.ToString());

            var response = MapSlotDTOToViewModel(updatedSlot);

            return response;
        }

        public void DeleteSlot(Guid periodId, Guid slotId)
        {
            var slots = _periodAndSlotProvider.GetSlotByPeriodId(periodId);

            var slot = slots.SingleOrDefault(x => x.Id == slotId) ?? throw new SlotException.NotFound(slotId);

            _periodAndSlotProvider.DeleteSlot(slotId);
        }

        private static PeriodViewModel MapPeriodDTOToViewModel(PeriodDTO dto, TermDTO term, AcademicLevelDTO academicLevel)
        {
            return new PeriodViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                FromDate = dto.FromDate,
                ToDate = dto.ToDate,
                Type = dto.Type,
                TermId = dto.TermId,
                AcademicLevelName = (from data in academicLevel.Localizations
                                     orderby data.Language
                                     select new AcademicLevelLocalizationDTO
                                     {
                                         Language = data.Language,
                                         Name = data.Name
                                     }).FirstOrDefault()?.Name,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt
            };
        }

        private static PeriodViewModel MapPeriodToViewModel(Period period, Term term, AcademicLevel academicLevel, IEnumerable<Slot> slots = null)
        {
            if (period is null)
            {
                return null;
            }

            var response = new PeriodViewModel
            {
                Id = period.Id,
                Name = period.Name,
                FromDate = period.FromDate,
                ToDate = period.ToDate,
                Type = period.Type,
                TermId = period.TermId,
                Slots = slots is null ? Enumerable.Empty<SlotViewModel>()
                                      : (from periodSlot in period.Slots
                                         orderby periodSlot.StartedAt
                                         select MapSlotViewModel(periodSlot))
                                        .ToList(),
                AcademicLevelName = (from data in academicLevel.Localizations
                                     orderby data.Language
                                     select new AcademicLevelLocalizationViewModel
                                     {
                                         Language = data.Language,
                                         Name = data.Name
                                     }).FirstOrDefault()?.Name,
                CreatedAt = period.CreatedAt,
                UpdatedAt = period.UpdatedAt
            };

            return response;
        }

        private static SlotViewModel MapSlotViewModel(Slot slot)
        {
            if (slot is null)
            {
                return null;
            }

            var response = new SlotViewModel
            {
                Id = slot.Id,
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

        private static SlotViewModel MapSlotDTOToViewModel(SlotDTO dto)
        {
            var response = new SlotViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                StartedAt = dto.StartedAt,
                EndedAt = dto.EndedAt,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt,
                IsActive = dto.IsActive,
                IsSpecialSlot = dto.IsSpecialSlot
            };

            return response;
        }

        private bool IsPeriodOverlapped(CreatePeriodViewModel request, Guid? periodId = null)
        {
            var termPeriods = _periodAndSlotProvider.GetPeriodsByTermId(request.TermId)
                                                    .ToList();

            var overlappedPeriod = (from termPeriod in termPeriods
                                    where (!periodId.HasValue || termPeriod.Id != periodId.Value)
                                          && termPeriod.Type == request.Type
                                          && request.FromDate < termPeriod.ToDate
                                          && termPeriod.FromDate < request.ToDate
                                    select termPeriod.Id)
                                   .ToList();

            return overlappedPeriod.Any();
        }
    }
}

