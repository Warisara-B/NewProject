using Microsoft.EntityFrameworkCore;
using Plexus.Database;
using Plexus.Database.Enum.Academic.Section;
using Plexus.Database.Model.Academic;
using Plexus.Database.Model.Academic.Section;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Academic;
using Plexus.Entity.DTO.Academic.Section;
using Plexus.Entity.Exception;
using Plexus.Utility.ViewModel;
using SectionModel = Plexus.Database.Model.Academic.Section.Section;
using Plexus.Utility.Extensions;

namespace Plexus.Entity.Provider.src.Academic
{
    public class SectionProvider : ISectionProvider
    {
        private readonly DatabaseContext _dbContext;

        public SectionProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<SectionDTO> GetByTermIdAndCourseId(Guid termId, IEnumerable<Guid> courseIds)
        {
            var sections = _dbContext.Sections.AsNoTracking()
                                              .Where(x => x.TermId == termId
                                                          && courseIds.Contains(x.CourseId))
                                              .ToList();

            var response = (from section in sections
                            select MapModelToDTO(section))
                           .ToList();

            return response;
        }

        public PagedViewModel<SectionDTO> Search(SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var query = GenerateSeachQuery(parameters);

            var pagedSection = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<SectionDTO>
            {
                Page = pagedSection.Page,
                TotalPage = pagedSection.TotalPage,
                TotalItem = pagedSection.TotalItem,
                Items = (from section in pagedSection.Items
                         select MapModelToDTO(section))
                        .ToList()
            };

            return response;
        }

        public IEnumerable<SectionDTO> Search(SearchCriteriaViewModel parameters)
        {
            var query = GenerateSeachQuery(parameters);

            var sections = query.ToList();

            var response = (from section in sections
                            select MapModelToDTO(section))
                           .ToList();

            return response;
        }

        public SectionDTO GetById(Guid id)
        {
            var section = _dbContext.Sections.AsNoTracking()
                                             .SingleOrDefault(x => x.Id == id);

            if (section is null)
            {
                throw new SectionException.NotFound(id);
            }

            var response = MapModelToDTO(section);

            return response;
        }

        public IEnumerable<SectionDTO> GetById(IEnumerable<Guid> ids)
        {
            var sections = _dbContext.Sections.AsNoTracking()
                                              .Where(x => ids.Contains(x.Id))
                                              .ToList();

            var response = (from section in sections
                            select MapModelToDTO(section))
                           .ToList();

            return response;
        }

        public IEnumerable<SectionDetailDTO> GetDetailBySectionId(IEnumerable<Guid> sectionIds)
        {
            var details = _dbContext.SectionDetails.AsNoTracking()
                                                   .Where(x => sectionIds.Contains(x.SectionId))
                                                   .ToList();

            var response = (from detail in details
                            select MapSectionDetailModelToDTO(detail))
                           .ToList();

            return response;
        }

        public void OpenSection(Guid id, string requester)
        {
            var section = _dbContext.Sections.SingleOrDefault(x => x.Id == id);

            if (section is null)
            {
                throw new SectionException.NotFound(id);
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                section.IsClosed = false;
                section.UpdatedAt = DateTime.UtcNow;
                section.UpdatedBy = requester;

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        public void CloseSection(Guid id, string requester)
        {
            var section = _dbContext.Sections.SingleOrDefault(x => x.Id == id);

            if (section is null)
            {
                throw new SectionException.NotFound(id);
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                section.IsClosed = true;
                section.UpdatedAt = DateTime.UtcNow;
                section.UpdatedBy = requester;

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        public void UpdateStatus(Guid id, SectionStatus status, string requester)
        {
            var section = _dbContext.Sections.SingleOrDefault(x => x.Id == id);

            if (section is null)
            {
                throw new SectionException.NotFound(id);
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                section.Status = status;
                section.UpdatedAt = DateTime.UtcNow;
                section.UpdatedBy = requester;

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        public void UpdateMainInstructor(Guid id, Guid? instructorId, string requester)
        {
            var section = _dbContext.Sections.SingleOrDefault(x => x.Id == id);

            if (section is null)
            {
                throw new SectionException.NotFound(id);
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                // section.MainInstructorId = instructorId;
                section.UpdatedAt = DateTime.UtcNow;
                section.UpdatedBy = requester;

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        public void Update(Guid id, IEnumerable<UpdateSectionDetailDTO> details, IEnumerable<UpdateSectionExaminationDTO> examinations, string requester)
        {
            var section = _dbContext.Sections.SingleOrDefault(x => x.Id == id);

            if (section is null)
            {
                throw new SectionException.NotFound(id);
            }

            // SECTION DETAILS
            var sectionDetails = _dbContext.SectionDetails.Where(x => x.SectionId == section.Id)
                                                          .ToList();

            var newDetails = SectionProvider.MapDetailDTOToModel(section, details, requester);

            // SECTION EXAMINATIONS
            var sectionExaminations = _dbContext.SectionExaminations.Where(x => x.SectionId == section.Id)
                                                                    .ToList();

            var newExaminations = SectionProvider.MapExaminationDTOToModel(section, examinations, requester);

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                // SECTION DETAILS
                _dbContext.SectionDetails.RemoveRange(sectionDetails);

                if (newDetails.Any())
                {
                    _dbContext.SectionDetails.AddRange(newDetails);
                }

                // SECTION EXAMINATION
                _dbContext.SectionExaminations.RemoveRange(sectionExaminations);

                if (newExaminations.Any())
                {
                    _dbContext.SectionExaminations.AddRange(newExaminations);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        public (SectionModel, SectionSeatUsage?) MapDTOToModel(SectionDTO request, IEnumerable<SectionSeat> defaultSeats, string requester)
        {
            var section = _dbContext.Sections.Include(x => x.JointSections)
                                             .SingleOrDefault(x => x.Id == request.Id);

            if (section is null)
            {
                throw new SectionException.NotFound(request.Id);
            }

            var seatUsed = defaultSeats.Min(x => x.SeatUsed);

            if (request.SeatLimit < seatUsed)
            {
                throw new SectionException.NotAllowUpdateSeatLessThanAlreadyUsed(seatUsed);
            }

            var seatDiff = request.SeatLimit - section.SeatLimit;

            // SECTION
            section.CourseId = request.CourseId;
            section.TermId = request.TermId;
            section.SectionNo = request.Number;
            section.SeatLimit = request.SeatLimit;
            section.AvailableSeat = request.AvailableSeat + seatDiff;
            // section.PlanningSeat = request.PlanningSeat;
            // section.MinimumSeat = request.MinimumSeat;
            // section.MainInstructorId = request.MainInstructorId;
            section.Status = request.Status;
            section.IsWithdrawable = request.IsWithdrawable;
            // section.IsGhostSection = request.IsGhostSection;
            // section.IsOutboundSection = request.IsOutboundSection;
            section.Remark = request.Remark;
            section.StartedAt = request.StartedDate;
            // section.TotalWeeks = request.TotalWeeks;
            section.ParentSectionId = request.ParentSectionId;
            section.IsClosed = request.IsClosed;
            section.UpdatedAt = DateTime.UtcNow;
            section.UpdatedBy = requester;

            if (seatDiff == 0)
            {
                return (section, null);
            }

            // UPDATE TOTAL SEAT
            var defaultSeat = defaultSeats.SingleOrDefault(x => x.SectionId == request.Id);

            if (defaultSeat is null)
            {
                throw new SectionException.DefaultSeatNotFound(request.Id);
            }

            defaultSeat.TotalSeat += seatDiff;

            // ADJUST USAGE
            var adjustUsage = new SectionSeatUsage
            {
                SeatId = defaultSeat.Id,
                Amount = seatDiff,
                SectionId = defaultSeat.SectionId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester
            };

            return (section, adjustUsage);
        }

        public static (SectionModel, SectionSeat, SectionSeatUsage) MapDTOToModel(CreateSectionDTO request, string requester)
        {
            var model = new SectionModel
            {
                CourseId = request.CourseId,
                TermId = request.TermId,
                SectionNo = request.Number,
                SeatLimit = request.SeatLimit,
                // PlanningSeat = request.PlanningSeat,
                // MinimumSeat = request.MinimumSeat,
                // MainInstructorId = request.MainInstructorId,
                Status = request.Status,
                IsWithdrawable = request.IsWithdrawable,
                // IsGhostSection = request.IsGhostSection,
                // IsOutboundSection = request.IsOutboundSection,
                Remark = request.Remark,
                AvailableSeat = request.AvailableSeat,
                StartedAt = request.StartedDate,
                // TotalWeeks = request.TotalWeeks,
                ParentSectionId = request.ParentSectionId,
                IsClosed = request.IsClosed,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = requester
            };

            var seat = new SectionSeat
            {
                Section = model,
                Name = "Default",
                Type = SeatType.DEFAULT,
                TotalSeat = request.SeatLimit,
                SeatUsed = 0,
                Conditions = null,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = requester
            };

            var usage = new SectionSeatUsage
            {
                Section = model,
                Seat = seat,
                Amount = request.SeatLimit,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester
            };

            return (model, seat, usage);
        }

        public static IEnumerable<SectionDetail> MapDetailDTOToModel(SectionModel section, IEnumerable<UpdateSectionDetailDTO> requests, string requester)
        {
            var response = (from detail in requests
                            select new SectionDetail
                            {
                                Section = section,
                                Day = detail.Day,
                                StartTime = detail.StartTime,
                                EndTime = detail.EndTime,
                                RoomId = detail.RoomId,
                                InstructorId = detail.InstructorId,
                                TeachingTypeId = detail.TeachingTypeId,
                                Remark = detail.Remark,
                                CreatedAt = DateTime.UtcNow,
                                CreatedBy = requester
                            })
                           .ToList();

            return response;
        }

        public static SectionDetailDTO MapSectionDetailModelToDTO(SectionDetail model)
        {
            return new SectionDetailDTO
            {
                Id = model.Id,
                SectionId = model.SectionId,
                Day = model.Day,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                RoomId = model.RoomId,
                InstructorId = model.InstructorId,
                TeachingTypeId = model.TeachingTypeId,
                Remark = model.Remark,
                CreatedAt = model.CreatedAt
            };
        }

        public static IEnumerable<SectionExamination> MapExaminationDTOToModel(SectionModel section, IEnumerable<UpdateSectionExaminationDTO> examinations, string requester)
        {
            var response = (from examination in examinations
                            select new SectionExamination
                            {
                                Section = section,
                                ExamType = examination.ExamType,
                                Date = examination.Date,
                                StartTime = examination.StartTime,
                                EndTime = examination.EndTime,
                                RoomId = examination.RoomId,
                                CreatedAt = DateTime.UtcNow,
                                CreatedBy = requester
                            })
                           .ToList();

            return response;
        }

        public static SectionExaminationDTO MapExaminationModelToDTO(SectionExamination examination)
        {
            return new SectionExaminationDTO
            {
                Id = examination.Id,
                SectionId = examination.SectionId,
                ExamType = examination.ExamType.Value,
                Date = examination.Date.Value,
                StartTime = examination.StartTime.Value,
                EndTime = examination.EndTime.Value,
                RoomId = examination.RoomId,
                CreatedAt = examination.CreatedAt
            };
        }

        private static SectionDTO MapModelToDTO(SectionModel section)
        {
            var response = new SectionDTO
            {
                Id = section.Id,
                CourseId = section.CourseId,
                TermId = section.TermId,
                Number = section.SectionNo,
                // PlanningSeat = section.PlanningSeat,
                // MinimumSeat = section.MinimumSeat,
                SeatLimit = section.SeatLimit,
                AvailableSeat = section.AvailableSeat.Value,
                // MainInstructorId = section.MainInstructorId,
                Status = section.Status,
                IsWithdrawable = section.IsWithdrawable,
                // IsGhostSection = section.IsGhostSection,
                // IsOutboundSection = section.IsOutboundSection,
                Remark = section.Remark,
                StartedDate = section.StartedAt.Value,
                // TotalWeeks = section.TotalWeeks,
                ParentSectionId = section.ParentSectionId,
                IsClosed = section.IsClosed,
                CreatedAt = section.CreatedAt,
                UpdatedAt = section.UpdatedAt
            };

            return response;
        }

        private IQueryable<SectionModel> GenerateSeachQuery(SearchCriteriaViewModel? parameters)
        {
            var query = _dbContext.Sections.Include(x => x.Course)
                                           .AsNoTracking();

            if (parameters is not null)
            {
                if (parameters.TermId.HasValue)
                {
                    query = query.Where(x => x.TermId == parameters.TermId.Value);
                }

                if (parameters.CourseId.HasValue)
                {
                    query = query.Where(x => x.CourseId == parameters.CourseId.Value);
                }

                if (!string.IsNullOrEmpty(parameters.Keyword))
                {
                    query = query.Where(x => x.Course.Code.Contains(parameters.Keyword)
                                             || x.Course.Name.Contains(parameters.Keyword));
                }

                if (!string.IsNullOrEmpty(parameters.Number))
                {
                    query = query.Where(x => x.SectionNo.Contains(parameters.Number));
                }

                if (parameters.IsClosed.HasValue)
                {
                    query = query.Where(x => x.IsClosed == parameters.IsClosed.Value);
                }
            }

            query = query.OrderBy(x => x.SectionNo)
                         .ThenBy(x => x.StartedAt);

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