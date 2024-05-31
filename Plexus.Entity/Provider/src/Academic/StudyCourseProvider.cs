using System;
using Microsoft.EntityFrameworkCore;
using Plexus.Database;
using Plexus.Database.Enum;
using Plexus.Database.Enum.Academic;
using Plexus.Database.Enum.Academic.Section;
using Plexus.Database.Model;
using Plexus.Database.Model.Academic;
using Plexus.Database.Model.Academic.Section;
using Plexus.Database.Model.Registration;
using Plexus.Entity.DTO.Academic;
using Plexus.Entity.DTO.Registration;
using Plexus.Entity.Exception;

namespace Plexus.Entity.Provider.src.Academic
{
    public class StudyCourseProvider : IStudyCourseProvider
    {
        private readonly DatabaseContext _dbContext;
        private readonly ISectionSeatProvider _sectionSeatProvider;

        public static IEnumerable<StudyCourseStatus> activeStudyCourseStatus =
            new[] { StudyCourseStatus.REGISTERED,
                    StudyCourseStatus.ACTIVE, StudyCourseStatus.WITHDRAWN,
                    StudyCourseStatus.TRANSFERED };

        public StudyCourseProvider(DatabaseContext dbContext,
                                   ISectionSeatProvider sectionSeatProvider)
        {
            _dbContext = dbContext;
            _sectionSeatProvider = sectionSeatProvider;
        }

        public IEnumerable<StudyCourseDTO> Create(IEnumerable<CreateStudyCourseDTO> request, string requester)
        {
            if (request is null || !request.Any())
            {
                return Enumerable.Empty<StudyCourseDTO>();
            }

            var (studentId, termId, channel) = request.Select(x => (x.StudentId, x.TermId, x.RegistrationChannel))
                                                      .First();

            var log = new RegistrationLog
            {
                StudentId = studentId,
                TermId = termId,
                RegistrationChannel = channel,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester
            };

            var models = (from data in request
                          select new StudyCourse
                          {
                              StudentId = data.StudentId,
                              TermId = data.TermId,
                              CourseId = data.CourseId,
                              SectionId = data.SectionId,
                              Status = data.Status,
                              RegistrationChannel = data.RegistrationChannel,
                              Credit = data.Credit,
                              RegistrationCredit = data.RegistrationCredit,
                              GradeId = data.GradeId,
                              GradeWeight = data.GradeWeight,
                              GradePublishedAt = data.GradePublishedAt,
                              CreatedAt = DateTime.UtcNow,
                              CreatedBy = requester,
                              UpdatedAt = DateTime.UtcNow,
                              UpdatedBy = requester
                          })
                         .ToList();

            var logCourses = (from studyCourse in models
                              select new RegistrationLogCourse
                              {
                                  Log = log,
                                  StudyCourse = studyCourse,
                                  Action = RegistrationLogAction.ADDED
                              })
                             .ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.StudyCourses.AddRange(models);

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = (from model in models
                            select MapModelToDTO(model))
                           .ToList();

            return response;
        }

        public IEnumerable<StudyCourseDTO> CreateTransferCourses(IEnumerable<CreateStudyCourseDTO> request, string requester)
        {
            var models = (from data in request
                          select new StudyCourse
                          {
                              StudentId = data.StudentId,
                              TermId = data.TermId,
                              CourseId = data.CourseId,
                              SectionId = data.SectionId,
                              Status = data.Status,
                              RegistrationChannel = data.RegistrationChannel,
                              Credit = data.Credit,
                              RegistrationCredit = data.RegistrationCredit,
                              GradeId = data.GradeId,
                              GradeWeight = data.GradeWeight,
                              GradePublishedAt = data.GradePublishedAt,
                              Remark = data.Remark,
                              CreatedAt = DateTime.UtcNow,
                              CreatedBy = requester,
                              UpdatedAt = DateTime.UtcNow,
                              UpdatedBy = requester
                          })
                         .ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.StudyCourses.AddRange(models);

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = (from model in models
                            select MapModelToDTO(model))
                           .ToList();

            return response;
        }

        public StudyCourseDTO GetById(Guid id)
        {
            var studyCourse = _dbContext.StudyCourses.AsNoTracking()
                                                     .SingleOrDefault(x => x.Id == id);

            if (studyCourse is null)
            {
                throw new StudyCourseException.NotFound(id);
            }

            var response = MapModelToDTO(studyCourse);

            return response;
        }

        public IEnumerable<StudyCourseDTO> GetById(IEnumerable<Guid> ids)
        {
            var studyCourses = _dbContext.StudyCourses.AsNoTracking()
                                                      .Where(x => ids.Contains(x.Id));

            var response = (from studyCourse in studyCourses
                            select MapModelToDTO(studyCourse))
                           .ToList();

            return response;
        }

        public IEnumerable<StudyCourseDTO> GetByStudent(Guid studentId, Guid? termId, IEnumerable<StudyCourseStatus>? statuses)
        {
            var query = _dbContext.StudyCourses.AsNoTracking()
                                               .Where(x => x.StudentId == studentId);

            if (termId.HasValue)
            {
                query = query.Where(x => x.TermId == termId.Value);
            }

            if (statuses is not null && statuses.Any())
            {
                query = query.Where(x => statuses.Contains(x.Status));
            }

            var studyCourses = query.ToList();

            var response = (from studyCourse in studyCourses
                            orderby studyCourse.TermId, studyCourse.CreatedAt
                            select MapModelToDTO(studyCourse))
                           .ToList();

            return response;
        }

        public IEnumerable<StudyCourseDTO> GetBySectionId(Guid sectionId, IEnumerable<StudyCourseStatus>? statuses)
        {
            var query = _dbContext.StudyCourses.AsNoTracking()
                                               .Where(x => x.SectionId.HasValue
                                                           && x.SectionId == sectionId);

            if (statuses is not null && statuses.Any())
            {
                query = query.Where(x => statuses.Contains(x.Status));
            }

            var studyCourses = query.ToList();

            var response = (from studyCourse in studyCourses
                            orderby studyCourse.TermId, studyCourse.CreatedAt
                            select MapModelToDTO(studyCourse))
                           .ToList();

            return response;
        }

        public IEnumerable<StudyCourseDTO> GetBySectionIdAndStudentId(
                IEnumerable<Guid> sectionIds, IEnumerable<Guid> studentIds,
                IEnumerable<StudyCourseStatus>? statuses)
        {
            var query = _dbContext.StudyCourses.AsNoTracking()
                                               .Where(x => x.SectionId.HasValue
                                                           && sectionIds.Contains(x.SectionId.Value)
                                                           && studentIds.Contains(x.StudentId));

            if (statuses is not null && statuses.Any())
            {
                query = query.Where(x => statuses.Contains(x.Status));
            }

            var studyCourses = query.ToList();

            var response = (from studyCourse in studyCourses
                            orderby studyCourse.TermId, studyCourse.CreatedAt
                            select MapModelToDTO(studyCourse))
                           .ToList();

            return response;
        }

        public IEnumerable<StudyCourseDTO> Update(IEnumerable<UpdateStudyCourseDTO> request, string requester)
        {
            if (request is null || !request.Any())
            {
                return Enumerable.Empty<StudyCourseDTO>();
            }

            var ids = request.Select(x => x.Id)
                             .Distinct()
                             .ToList();

            var studyCourses = _dbContext.StudyCourses.Where(x => ids.Contains(x.Id))
                                                      .ToList();

            if (!studyCourses.Any())
            {
                return Enumerable.Empty<StudyCourseDTO>();
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                foreach (var data in studyCourses)
                {
                    var matchingRequest = request.First(x => x.Id == data.Id);

                    data.Status = matchingRequest.Status;

                    if (matchingRequest.Status == StudyCourseStatus.ACTIVE && !data.PaidAt.HasValue)
                    {
                        data.PaidAt = DateTime.UtcNow;
                    }

                    data.GradeId = matchingRequest.GradeId;
                    data.GradeWeight = matchingRequest.GradeWeight;
                    data.GradePublishedAt = matchingRequest.GradePublishedAt;
                    data.UpdatedAt = DateTime.UtcNow;
                    data.UpdatedBy = requester;
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = (from studyCourse in studyCourses
                            select MapModelToDTO(studyCourse))
                           .ToList();

            return response;
        }

        public IEnumerable<StudyCourseDTO> Update(RegistrationDTO request, string requester)
        {
            if (request.Sections is null || !request.Sections.Any())
            {
                return Enumerable.Empty<StudyCourseDTO>();
            }

            var log = new RegistrationLog
            {
                StudentId = request.StudentId,
                TermId = request.TermId,
                RegistrationChannel = request.RegistrationChannel,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester
            };

            var existingCourses = _dbContext.StudyCourses.Where(x => x.StudentId == request.StudentId
                                                                     && x.TermId == request.TermId
                                                                     && activeStudyCourseStatus.Contains(x.Status))
                                                         .ToList();

            var newStudyCourses = new List<StudyCourse>();

            var logCourses = new List<RegistrationLogCourse>();

            var usages = new List<SectionSeatUsage>();

            var sectionIds = request.Sections.Where(x => x.SectionId.HasValue)
                                             .SelectMany(x => x.ParentSectionId.HasValue ? new[] { x.SectionId!.Value, x.ParentSectionId.Value }
                                                                                         : new[] { x.SectionId!.Value })
                                             .ToList();

            var defaultSeats = _dbContext.SectionSeats.Where(x => sectionIds.Contains(x.SectionId)
                                                                  && x.Type == SeatType.DEFAULT)
                                                      .ToList();

            foreach (var section in request.Sections)
            {
                if (existingCourses.Any(x => x.CourseId == section.CourseId))
                {
                    continue;
                }

                var ids = section.SectionId.HasValue ? section.ParentSectionId.HasValue ? new[] { section.SectionId.Value, section.ParentSectionId.Value }
                                                                                        : new[] { section.SectionId.Value }
                                                     : Enumerable.Empty<Guid>();

                var sectionDefaultSeats = defaultSeats.Where(x => ids.Contains(x.SectionId));

                var studyCourse = new StudyCourse
                {
                    StudentId = request.StudentId,
                    TermId = request.TermId,
                    CourseId = section.CourseId,
                    SectionId = section.SectionId,
                    Status = StudyCourseStatus.REGISTERED,
                    RegistrationChannel = request.RegistrationChannel,
                    Credit = section.Credit,
                    RegistrationCredit = section.RegistrationCredit,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = requester,
                    UpdatedAt = DateTime.UtcNow,
                    UpdatedBy = requester
                };

                var used = _sectionSeatProvider.CutSeat(studyCourse, sectionDefaultSeats, section.SectionSeatId, 1, request.RegistrationChannel, requester);

                usages.AddRange(used);

                newStudyCourses.Add(studyCourse);

                var addedLog = new RegistrationLogCourse
                {
                    Log = log,
                    StudyCourse = studyCourse,
                    Action = RegistrationLogAction.ADDED
                };

                logCourses.Add(addedLog);
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                foreach (var studyCourse in existingCourses)
                {
                    if (request.Sections.Any(x => x.CourseId == studyCourse.CourseId))
                    {
                        var retainedLog = new RegistrationLogCourse
                        {
                            Log = log,
                            StudyCourse = studyCourse,
                            Action = RegistrationLogAction.RETAINED
                        };

                        logCourses.Add(retainedLog);
                    }
                    else
                    {
                        studyCourse.Status = StudyCourseStatus.DROP;
                        studyCourse.UpdatedAt = DateTime.UtcNow;
                        studyCourse.UpdatedBy = requester;

                        var droppedLog = new RegistrationLogCourse
                        {
                            Log = log,
                            StudyCourse = studyCourse,
                            Action = RegistrationLogAction.DROPPED
                        };

                        logCourses.Add(droppedLog);
                    }
                }

                _dbContext.RegistrationLogs.Add(log);

                if (newStudyCourses.Any())
                {
                    _dbContext.StudyCourses.AddRange(newStudyCourses);
                }

                if (logCourses.Any())
                {
                    _dbContext.RegistrationLogCourses.AddRange(logCourses);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var studyCourses = existingCourses.Where(x => x.Status != StudyCourseStatus.DROP)
                                              .Concat(newStudyCourses);

            var response = (from studyCourse in studyCourses
                            select MapModelToDTO(studyCourse))
                           .ToList();

            return response;
        }

        private static StudyCourseDTO MapModelToDTO(StudyCourse model)
        {
            var response = new StudyCourseDTO
            {
                Id = model.Id,
                StudentId = model.StudentId,
                TermId = model.TermId,
                CourseId = model.CourseId,
                SectionId = model.SectionId,
                RegistrationChannel = model.RegistrationChannel,
                Credit = model.Credit,
                RegistrationCredit = model.RegistrationCredit,
                GradeId = model.GradeId,
                GradeWeight = model.GradeWeight,
                GradePublishedAt = model.GradePublishedAt,
                Remark = model.Remark,
                Status = model.Status,
                PaidAt = model.PaidAt,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt
            };

            return response;
        }
    }
}

