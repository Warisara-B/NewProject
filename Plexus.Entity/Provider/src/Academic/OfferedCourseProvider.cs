// using Microsoft.EntityFrameworkCore;
// using Newtonsoft.Json;
// using Plexus.Database;
// using Plexus.Database.Enum.Academic;
// using Plexus.Database.Enum.Academic.Section;
// using Plexus.Database.Model.Academic;
// using Plexus.Database.Model.Academic.Section;
// using Plexus.Entity.DTO;
// using Plexus.Entity.DTO.Academic;
// using Plexus.Entity.DTO.Academic.Section;
// using Plexus.Entity.Exception;
// using Plexus.Entity.Provider.src.Academic.Section;
// using Plexus.Utility.Extensions;
// using Plexus.Utility.ViewModel;
// using SectionModel = Plexus.Database.Model.Academic.Section.Section;

// namespace Plexus.Entity.Provider.src.Academic
// {
//     public class OfferedCourseProvider : IOfferedCourseProvider
//     {
//         private readonly DatabaseContext _dbContext;
//         private readonly ISectionSeatProvider _sectionSeatProvider;
//         private readonly ISectionProvider _sectionProvider;

//         private static IEnumerable<StudyCourseStatus> activeStudyCourseStatus =
//             new[] { StudyCourseStatus.REGISTERED,
//                     StudyCourseStatus.ACTIVE, StudyCourseStatus.WITHDRAWN,
//                     StudyCourseStatus.TRANSFERED };

//         public OfferedCourseProvider(DatabaseContext dbContext,
//                                      ISectionSeatProvider sectionSeatProvider,
//                                      ISectionProvider sectionProvider)
//         {
//             _dbContext = dbContext;
//             _sectionSeatProvider = sectionSeatProvider;
//             _sectionProvider = sectionProvider;
//         }

//         public OfferedCourseDTO Create(CreateOfferedCourseDTO request, string requester)
//         {
//             // SECTION
//             var (section, seat, usage) = SectionProvider.MapDTOToModel(request, requester);

//             // SECTION SEATS
//             var (createdResponses, updatedResponses) = _sectionSeatProvider.Upsert(request.Seats, section.SeatLimit);

//             var defaultSeats = new List<SectionSeat> { seat };

//             var (seats, usages) = _sectionSeatProvider.MapCreateDTOToModel(section, createdResponses, defaultSeats, requester);

//             // SECTION DETAILS
//             var details = request.Details is null || !request.Details.Any() ? Enumerable.Empty<SectionDetail>()
//                                                                             : SectionProvider.MapDetailDTOToModel(section, request.Details, requester);

//             // SECTION EXAMINATIONS
//             var examinations = request.Examinations is null || !request.Examinations.Any() ? Enumerable.Empty<SectionExamination>()
//                                                                                            : SectionProvider.MapExaminationDTOToModel(section, request.Examinations, requester);

//             using (var transaction = _dbContext.Database.BeginTransaction())
//             {
//                 // SECTION
//                 _dbContext.Sections.Add(section);
//                 _dbContext.SectionSeats.Add(seat);
//                 _dbContext.SectionSeatUsages.Add(usage);

//                 // SECTION SEATS
//                 if (seats.Any())
//                 {
//                     _dbContext.SectionSeats.AddRange(seats);
//                 }

//                 if (usages.Any())
//                 {
//                     _dbContext.SectionSeatUsages.AddRange(usages);
//                 }

//                 // SECTION DETAILS
//                 if (details.Any())
//                 {
//                     _dbContext.SectionDetails.AddRange(details);
//                 }

//                 // SECTION EXAMINATION
//                 if (examinations.Any())
//                 {
//                     _dbContext.SectionExaminations.AddRange(examinations);
//                 }

//                 transaction.Commit();
//             }

//             _dbContext.SaveChanges();

//             var response = MapModelToDTO(section, seats, null, details, examinations);

//             return response;
//         }

//         public PagedViewModel<OfferedCourseDTO> Search(SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25)
//         {
//             var query = GenerateSeachQuery(parameters);

//             var pagedSection = query.GetPagedViewModel(page, pageSize);

//             var sectionIds = pagedSection.Items.Select(x => x.Id)
//                                                .ToList();

//             var sectionSeats = _dbContext.SectionSeats.AsNoTracking()
//                                                       .Where(x => sectionIds.Contains(x.SectionId))
//                                                       .ToList();

//             var sectionDetails = _dbContext.SectionDetails.AsNoTracking()
//                                                           .Where(x => sectionIds.Contains(x.SectionId))
//                                                           .ToList();

//             var sectionExaminations = _dbContext.SectionExaminations.AsNoTracking()
//                                                                     .Where(x => sectionIds.Contains(x.SectionId))
//                                                                     .ToList();

//             var response = new PagedViewModel<OfferedCourseDTO>
//             {
//                 Page = pagedSection.Page,
//                 TotalPage = pagedSection.TotalPage,
//                 TotalItem = pagedSection.TotalItem,
//                 Items = (from section in pagedSection.Items
//                          let seats = sectionSeats.Where(x => x.SectionId == section.Id)
//                                                  .ToList()
//                          let details = sectionDetails.Where(x => x.SectionId == section.Id)
//                                                      .ToList()
//                          let examinations = sectionExaminations.Where(x => x.SectionId == section.Id)
//                                                                .ToList()
//                          select MapModelToDTO(section, seats, section.JointSections, details, examinations))
//                         .ToList()
//             };

//             return response;
//         }

//         public PagedViewModel<StudentDTO> SearchStudents(Guid id, SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25)
//         {
//             var sectionQuery = _dbContext.Sections.AsNoTracking()
//                                                   .Where(x => x.Id == id
//                                                               || x.ParentSectionId == id);

//             if (parameters is not null
//                 && parameters.IsMasterSection.HasValue)
//             {
//                 if (parameters.IsMasterSection.Value)
//                 {
//                     sectionQuery = sectionQuery.Where(x => x.Id == id);
//                 }
//                 else
//                 {
//                     sectionQuery = sectionQuery.Where(x => x.Id != id);
//                 }
//             }  

//             var sections = sectionQuery.ToList();

//             var sectionIds = sections.Select(x => x.Id)
//                                      .ToList();

//             var query = _dbContext.StudyCourses.Include(x => x.Student)
//                                                    .ThenInclude(x => x.Localizations)
//                                                .Where(x => activeStudyCourseStatus.Contains(x.Status)
//                                                            && x.SectionId.HasValue
//                                                            && sectionIds.Contains(x.SectionId!.Value))
//                                                .OrderBy(x => x.Student.Code)
//                                                .AsNoTracking();

//             var pagedStudyCourse = query.GetPagedViewModel(page, pageSize);

//             var response = new PagedViewModel<StudentDTO>
//             {
//                 Page = pagedStudyCourse.Page,
//                 TotalPage = pagedStudyCourse.TotalPage,
//                 TotalItem = pagedStudyCourse.TotalItem,
//                 Items = (from studyCourse in pagedStudyCourse.Items
//                          select StudentProvider.MapModelToDTO(studyCourse.Student, studyCourse.Student.Localizations))
//                         .ToList()
//             };

//             return response;
//         }

//         public OfferedCourseDTO GetById(Guid id)
//         {
//             var section = _dbContext.Sections.AsNoTracking()
//                                              .SingleOrDefault(x => x.Id == id);

//             if (section is null)
//             {
//                 throw new SectionException.NotFound(id);
//             }

//             var jointSections = _dbContext.Sections.AsNoTracking()
//                                                    .Where(x => x.ParentSectionId == id)
//                                                    .ToList();

//             var sectionSeats = _dbContext.SectionSeats.AsNoTracking()
//                                                       .Where(x => x.SectionId == id)
//                                                       .ToList();

//             var sectionDetails = _dbContext.SectionDetails.AsNoTracking()
//                                                           .Where(x => x.SectionId == id)
//                                                           .ToList();

//             var sectionExaminations = _dbContext.SectionExaminations.AsNoTracking()
//                                                                     .Where(x => x.SectionId == id)
//                                                                     .ToList();

//             var response = MapModelToDTO(section, sectionSeats, jointSections, sectionDetails, sectionExaminations);

//             return response;
//         }

//         public OfferedCourseDTO Update(SectionDTO request, IEnumerable<UpsertSectionSeatDTO> seats, IEnumerable<UpdateSectionDetailDTO> details, IEnumerable<UpdateSectionExaminationDTO> examinations, string requester)
//         {
//             var sectionIds = request.ParentSectionId.HasValue ? new[] { request.Id, request.ParentSectionId.Value }
//                                                               : new[] { request.Id };

//             var defaultSeats = _dbContext.SectionSeats.Include(x => x.Section)
//                                                       .Where(x => sectionIds.Contains(x.SectionId)
//                                                                   && x.Type == SeatType.DEFAULT)
//                                                       .ToList();

//             // SECTION
//             var (section, usage) = _sectionProvider.MapDTOToModel(request, defaultSeats, requester);

//             var seatLimit = defaultSeats.Min(x => x.TotalSeat - x.SeatUsed);

//             // SECTION SEATS
//             var (createdResponses, updatedResponses) = _sectionSeatProvider.Upsert(seats, seatLimit);

//             // UPDATE SEATS
//             var (updatedSeats, updatedUsages) = _sectionSeatProvider.MapUpdateDTOToModel(request.Id, updatedResponses, defaultSeats, requester);

//             // CREATE SEATS
//             var (createdSeats, createdUsages) = _sectionSeatProvider.MapCreateDTOToModel(section, createdResponses, defaultSeats, requester);

//             // SECTION DETAILS
//             var sectionDetails = _dbContext.SectionDetails.Where(x => x.SectionId == request.Id)
//                                                           .ToList();

//             var newDetails = SectionProvider.MapDetailDTOToModel(section, details, requester);

//             // SECTION EXAMINATIONS
//             var sectionExaminations = _dbContext.SectionExaminations.Where(x => x.SectionId == request.Id)
//                                                                     .ToList();

//             var newExaminations = SectionProvider.MapExaminationDTOToModel(section, examinations, requester);

//             using (var transaction = _dbContext.Database.BeginTransaction())
//             {
//                 // ADJUST DEFAULT SEAT
//                 if (usage is not null)
//                 {
//                     _dbContext.SectionSeatUsages.Add(usage);
//                 }

//                 // SECTION SEATS
//                 if (updatedUsages is not null && updatedUsages.Any())
//                 {
//                     _dbContext.SectionSeatUsages.AddRange(updatedUsages);
//                 }

//                 if (createdSeats is not null && createdSeats.Any())
//                 {
//                     _dbContext.SectionSeats.AddRange(createdSeats);
//                 }

//                 if (createdUsages is not null && createdUsages.Any())
//                 {
//                     _dbContext.SectionSeatUsages.AddRange(createdUsages);
//                 }

//                 // SECTION DETAILS
//                 _dbContext.SectionDetails.RemoveRange(sectionDetails);

//                 if (newDetails.Any())
//                 {
//                     _dbContext.SectionDetails.AddRange(newDetails);
//                 }

//                 // SECTION EXAMINATION
//                 _dbContext.SectionExaminations.RemoveRange(sectionExaminations);

//                 if (newExaminations.Any())
//                 {
//                     _dbContext.SectionExaminations.AddRange(newExaminations);
//                 }

//                 transaction.Commit();
//             }

//             _dbContext.SaveChanges();

//             var sectionSeats = createdSeats is null || !createdSeats.Any() ? updatedSeats 
//                                                                            : updatedSeats.Concat(createdSeats);

//             var response = MapModelToDTO(section, sectionSeats, section.JointSections, newDetails, newExaminations);

//             return response;
//         }

//         public IEnumerable<SectionSeatDTO> UpdateSeats(Guid sectionId, IEnumerable<UpsertSectionSeatDTO> requests, string requester)
//         {
//             var section = _dbContext.Sections.SingleOrDefault(x => x.Id == sectionId);

//             if (section is null)
//             {
//                 throw new SectionException.NotFound(sectionId);
//             }

//             var sectionIds = section.ParentSectionId.HasValue ? new[] { section.Id, section.ParentSectionId.Value }
//                                                               : new[] { section.Id };

//             var defaultSeats = _dbContext.SectionSeats.Include(x => x.Section)
//                                                       .Where(x => sectionIds.Contains(x.SectionId)
//                                                                   && x.Type == SeatType.DEFAULT)
//                                                       .ToList();

//             var seatLimit = defaultSeats.Min(x => x.TotalSeat - x.SeatUsed);

//             // SECTION SEATS
//             var (createdResponses, updatedResponses) = _sectionSeatProvider.Upsert(requests, seatLimit);

//             // UPDATE SEATS
//             var (updatedSeats, updatedUsages) = _sectionSeatProvider.MapUpdateDTOToModel(sectionId, updatedResponses, defaultSeats, requester);

//             // CREATE SEATS
//             var (createdSeats, createdUsages) = _sectionSeatProvider.MapCreateDTOToModel(section, createdResponses, defaultSeats, requester);

//             using (var transaction = _dbContext.Database.BeginTransaction())
//             {
//                 // SECTION SEATS
//                 if (updatedUsages is not null && updatedUsages.Any())
//                 {
//                     _dbContext.SectionSeatUsages.AddRange(updatedUsages);
//                 }

//                 if (createdSeats is not null && createdSeats.Any())
//                 {
//                     _dbContext.SectionSeats.AddRange(createdSeats);
//                 }

//                 if (createdUsages is not null && createdUsages.Any())
//                 {
//                     _dbContext.SectionSeatUsages.AddRange(createdUsages);
//                 }

//                 transaction.Commit();
//             }

//             _dbContext.SaveChanges();

//             var sectionSeats = createdSeats is null || !createdSeats.Any() ? updatedSeats 
//                                                                            : updatedSeats.Concat(createdSeats);

//             var response = (from seat in sectionSeats
//                             select SectionSeatProvider.MapModelToDTO(seat))
//                            .ToList();

//             return response;
//         }

//         public void Delete(Guid id)
//         {
//             throw new NotImplementedException();
//         }

//         private static OfferedCourseDTO MapModelToDTO(SectionModel section,
//             IEnumerable<SectionSeat>? seats = null,
//             IEnumerable<SectionModel>? jointSections = null,
//             IEnumerable<SectionDetail>? details = null,
//             IEnumerable<SectionExamination>? examinations = null)
//         {
//             var response = new OfferedCourseDTO
//             {
//                 Id = section.Id,
//                 CourseId = section.CourseId,
//                 TermId = section.TermId,
//                 Number = section.Number,
//                 PlanningSeat = section.PlanningSeat,
//                 MinimumSeat = section.MinimumSeat,
//                 SeatLimit = section.SeatLimit,
//                 AvailableSeat = section.AvailableSeat,
//                 MainInstructorId = section.MainInstructorId,
//                 Status = section.Status,
//                 IsWithdrawable = section.IsWithdrawable,
//                 IsGhostSection = section.IsGhostSection,
//                 IsOutboundSection = section.IsOutboundSection,
//                 Remark = section.Remark,
//                 StartedDate = section.StartedDate,
//                 TotalWeeks = section.TotalWeeks,
//                 ParentSectionId = section.ParentSectionId,
//                 IsClosed = section.IsClosed,
//                 CreatedAt = section.CreatedAt,
//                 UpdatedAt = section.UpdatedAt,
//                 Seats = seats is null || !seats.Any() ? null
//                                                       : (from seat in seats
//                                                          orderby seat.Name
//                                                          select SectionSeatProvider.MapModelToDTO(seat))
//                                                         .ToList(),
//                 JointSections = jointSections is null || !jointSections.Any() ? null
//                                                                               : (from jointSection in jointSections
//                                                                                  orderby jointSection.Number, jointSection.StartedDate
//                                                                                  select new JointSectionDTO
//                                                                                  {
//                                                                                      Id = jointSection.Id,
//                                                                                      CourseId = jointSection.CourseId,
//                                                                                      Number = jointSection.Number,
//                                                                                      SeatLimit = jointSection.SeatLimit,
//                                                                                      Remark = jointSection.Remark
//                                                                                  })
//                                                                                 .ToList(),
//                 Details = details is null || !details.Any() ? Enumerable.Empty<SectionDetailDTO>()
//                                                             : (from detail in details
//                                                                orderby detail.Day, detail.StartTime
//                                                                select SectionProvider.MapSectionDetailModelToDTO(detail))
//                                                               .ToList(),
//                 Examinations = examinations is null || !examinations.Any() ? Enumerable.Empty<SectionExaminationDTO>()
//                                                                            : (from examination in examinations
//                                                                               select SectionProvider.MapExaminationModelToDTO(examination))
//                                                                              .ToList()
//             };

//             return response;
//         }

//         private IQueryable<SectionModel> GenerateSeachQuery(SearchCriteriaViewModel? parameters)
//         {
//             var query = _dbContext.Sections.Include(x => x.Course)
//                                            .Include(x => x.JointSections)
//                                            .ThenInclude(x => x.Course)
//                                            .Where(x => !x.ParentSectionId.HasValue)
//                                            .AsNoTracking();

//             if (parameters is not null)
//             {
//                 if (!string.IsNullOrEmpty(parameters.Keyword))
//                 {
//                     query = query.Where(x => x.Course.Code.Contains(parameters.Keyword)
//                                              || x.Course.Name.Contains(parameters.Keyword)
//                                              || (x.JointSections != null && x.JointSections.Any(x => x.Course.Code.Contains(parameters.Keyword)
//                                                                                                      || x.Course.Name.Contains(parameters.Keyword))));
//                 }
//             }

//             query = query.OrderBy(x => x.Course.Name)
//                          .ThenBy(x => x.Number);

//             if (parameters is not null)
//             {
//                 if (!string.IsNullOrEmpty(parameters.SortBy))
//                 {
//                     try
//                     {
//                         query = query.OrderBy(parameters.SortBy, parameters.OrderBy);
//                     }
//                     catch (System.Exception)
//                     {
//                         // invalid property name
//                     }
//                 }
//             }

//             return query;
//         }

//         private (List<SectionSeat>, List<SectionSeat>) UpdateSeats(SectionModel section, IEnumerable<SectionSeatDTO> requests, IEnumerable<SectionSeat> defaultSeats, List<SectionSeatUsage> usages, string requester)
//         {
//             throw new NotImplementedException();
//             // var newSeats = new List<SectionSeat>();

//             // var sectionSeats = _dbContext.SectionSeats.Where(x => x.SectionId == section.Id
//             //                                                       && x.Type != SeatType.DEFAULT)
//             //                                           .ToList();

//             // // VALIDATE && UPDATE EXISTING SEAT
//             // foreach (var sectionSeat in sectionSeats)
//             // {
//             //     var seatRequest = requests.SingleOrDefault(x => x.Id == sectionSeat.Id);

//             //     if (seatRequest is null)
//             //     {
//             //         continue;
//             //     }

//             //     if (seatRequest.TotalSeat < sectionSeat.SeatUsed)
//             //     {
//             //         throw new SectionException.NotAllowUpdateSeatLessThanAlreadyUsed(sectionSeat.SeatUsed);
//             //     }

//             //     // IF RESERVED TYPE NEED TO ADJUST USAGE
//             //     if (seatRequest.Type != SeatType.RESERVED)
//             //     {
//             //         continue;
//             //     }

//             //     var seatDiff = seatRequest.TotalSeat - sectionSeat.TotalSeat;

//             //     if (seatDiff != 0)
//             //     {
//             //         // CHECK DEFAUT SEAT REMAINING
//             //         if (seatDiff > 0)
//             //         {
//             //             foreach (var defaultSeat in defaultSeats)
//             //             {
//             //                 var defaultRemaingingSeat = defaultSeat is null ? 0
//             //                                                                 : defaultSeat.TotalSeat - defaultSeat.SeatUsed;

//             //                 if (defaultRemaingingSeat < seatDiff)
//             //                 {
//             //                     throw new SectionException.NotEnoughRemainingSeat(defaultRemaingingSeat);
//             //                 }
//             //             }
//             //         }

//             //         // ADD DEFAULT SEAT USAGE ADJUSTMENT
//             //         foreach (var defaultSeat in defaultSeats)
//             //         {
//             //             defaultSeat.SeatUsed += seatDiff;

//             //             var adjustUsage = new SectionSeatUsage
//             //             {
//             //                 SeatId = defaultSeat.Id,
//             //                 Amount = seatDiff * -1,
//             //                 SectionId = defaultSeat.SectionId,
//             //                 ReferenceSeatId = sectionSeat.Id,
//             //                 CreatedAt = DateTime.UtcNow,
//             //                 CreatedBy = requester
//             //             };

//             //             usages.Add(adjustUsage);
//             //         }

//             //         var reserveSeat = new SectionSeatUsage
//             //         {
//             //             SeatId = sectionSeat.Id,
//             //             SectionId = sectionSeat.SectionId,
//             //             Amount = seatDiff,
//             //             ReferenceSeatId = sectionSeat.Id,
//             //             CreatedAt = DateTime.UtcNow,
//             //             CreatedBy = requester
//             //         };

//             //         usages.Add(reserveSeat);
//             //     }

//             //     sectionSeat.Name = seatRequest.Name;
//             //     sectionSeat.TotalSeat = seatRequest.TotalSeat;
//             //     sectionSeat.Conditions = seatRequest.Conditions is null ? null
//             //                                                             : JsonConvert.SerializeObject(seatRequest.Conditions);
//             //     sectionSeat.Remark = seatRequest.Remark;
//             //     sectionSeat.UpdatedAt = DateTime.UtcNow;

//             // }

//             // // CREATE SEAT
//             // var existingSeatIds = sectionSeats.Select(x => x.Id)
//             //                                   .ToList();

//             // var createdSeats = requests.Where(x => !existingSeatIds.Contains(x.Id))
//             //                            .ToList();

//             // foreach (var createdSeat in createdSeats)
//             // {
//             //     var mappedSeat = SectionSeatProvider.MapDTOToModel(createdSeat, requester, section, defaultSeats, out List<SectionSeatUsage> createdUsages);

//             //     newSeats.Add(mappedSeat);

//             //     usages.AddRange(createdUsages);
//             // }

//             // return (sectionSeats, newSeats);
//         }
//     }
// }