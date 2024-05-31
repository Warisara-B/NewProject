using System;
using Microsoft.EntityFrameworkCore;
using Plexus.Database;
using Plexus.Database.Enum;
using Plexus.Database.Model.Registration;
using Plexus.Entity.DTO.Registration;
using Plexus.Entity.Exception;

namespace Plexus.Entity.Provider.src.Registration
{
    public class PlanProvider
    {
        private readonly DatabaseContext _dbContext;

        public PlanProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region plan

        //public PlanDTO CreatePlan(Guid studentId, Guid termId, CreatePlanDTO request)
        //{
        //    var plans = _dbContext.Plans.AsNoTracking()
        //                                .Include(x => x.PlanCourses)
        //                                .Where(x => x.StudentId == studentId
        //                                            && x.TermId == termId
        //                                            && x.Type == request.Type)
        //                                .ToList();

        //    var requestCourses = request.CourseIds.OrderBy(x => x)
        //                                          .ToList();

        //    if (plans.Any(x => IsCourseMatching(x.PlanCourses, requestCourses)))
        //    {
        //        throw new PlanException.PlanWithSameCourseAlreadyExists(request.Type, requestCourses);
        //    }

        //    var newPlan = AddPlanToDB(studentId, termId, request.Type, requestCourses);

        //    var response = MapPlanDTO(newPlan, null);

        //    response.CourseIds = requestCourses;

        //    return response;
        //}

        //public PlanDTO GetById(Guid planId)
        //{
        //    var plan = _dbContext.Plans.AsNoTracking()
        //                               .Include(x => x.PlanCourses)
        //                               .SingleOrDefault(x => x.Id == planId);

        //    if (plan is null)
        //    {
        //        throw new PlanException.NotFound(planId);
        //    }

        //    var response = MapPlanDTO(plan, plan.PlanCourses);

        //    return response;
        //}

        //public IEnumerable<PlanDTO> GetByStudentAndTermId(Guid studentId, Guid termId)
        //{
        //    var plans = _dbContext.Plans.AsNoTracking()
        //                                .Include(x => x.PlanCourses)
        //                                .Where(x => x.StudentId == studentId
        //                                            && x.TermId == termId)
        //                                .ToList();

        //    var response = (from plan in plans
        //                    select MapPlanDTO(plan, plan.PlanCourses))
        //                   .ToList();

        //    return response;
        //}

        //public void Delete(Guid planId)
        //{
        //    var plan = _dbContext.Plans.Include(x => x.PlanCourses)
        //                               .Include(x => x.PlanSchedules)
        //                               .ThenInclude(x => x.PlanSections)
        //                               .SingleOrDefault(x => x.Id == planId);

        //    if (plan is null)
        //    {
        //        throw new PlanException.NotFound(planId);
        //    }

        //    using (var transaction = _dbContext.Database.BeginTransaction())
        //    {
        //        foreach (var schedule in plan.PlanSchedules)
        //        {
        //            _dbContext.PlanScheduleSections.RemoveRange(schedule.PlanSections);
        //            _dbContext.PlanSchedules.Remove(schedule);
        //        }

        //        _dbContext.PlanCourses.RemoveRange(plan.PlanCourses);
        //        _dbContext.Plans.Remove(plan);

        //        transaction.Commit();
        //    }

        //    _dbContext.SaveChanges();
        //}

        #endregion

        #region Schedule

        //public PlanScheduleDTO CreateSchedule(Guid studentId, Guid termId, CreatePlanScheduleDTO request)
        //{
        //    var plans = _dbContext.Plans.AsNoTracking()
        //                                .Where(x => x.StudentId == studentId
        //                                            && x.TermId == termId
        //                                            && x.Type == PlanType.PLAN)
        //                                .ToList();

        //    var sections = _dbContext.Sections.AsNoTracking()
        //                                      .Where(x => request.SectionIds.Contains(x.Id))
        //                                      .ToList();

        //    var requestCourses = sections.Select(x => x.CourseId)
        //                                 .Distinct()
        //                                 .OrderBy(x => x)
        //                                 .ToList();

        //    Guid? matchingPlanId = null;

        //    foreach(var plan in plans)
        //    {
        //        if (IsCourseMatching(plan.PlanCourses, requestCourses))
        //        {
        //            matchingPlanId = plan.Id;
        //            break;
        //        }
        //    }

        //    // NO MATCHING COURE PLAN - ADD PLAN
        //    if(!matchingPlanId.HasValue)
        //    {
        //        var newPlan = AddPlanToDB(studentId, termId, PlanType.PLAN, requestCourses);

        //        matchingPlanId = newPlan.Id;
        //    }

        //    // MAP SCHEDULE SECTIONS
        //    var schedules = _dbContext.PlanSchedules.AsNoTracking()
        //                                            .Include(x => x.PlanSections)
        //                                            .Where(x => x.PlanId == matchingPlanId.Value)
        //                                            .ToList();

        //    var requestSections = request.SectionIds.OrderBy(x => x)
        //                                            .ToList();

        //    foreach(var schedule in schedules)
        //    {
        //        if(schedule.PlanSections.Count() != request.SectionIds.Count())
        //        {
        //            continue;
        //        }

        //        var plannedSections = schedule.PlanSections.OrderBy(x => x.SectionId)
        //                                                   .Select(x => x.SectionId)
        //                                                   .ToList();

        //        if (plannedSections.SequenceEqual(requestSections))
        //        {
        //            throw new PlanException.PlanWithSameSectionAlreadyExists(requestSections);
        //        }
        //    }

        //    var newSchedule = new PlanSchedule
        //    {
        //        PlanId = matchingPlanId.Value,
        //        CreatedAt = DateTime.UtcNow
        //    };

        //    var scheduleSections = (from sectionId in request.SectionIds
        //                           select new PlanScheduleSection
        //                           {
        //                               PlanSchedule = newSchedule,
        //                               SectionId = sectionId
        //                           })
        //                           .ToList();

        //    using(var transaction = _dbContext.Database.BeginTransaction())
        //    {
        //        _dbContext.PlanSchedules.Add(newSchedule);
        //        _dbContext.PlanScheduleSections.AddRange(scheduleSections);

        //        transaction.Commit();
        //    }

        //    _dbContext.SaveChanges();

        //    var response = MapPlanScheduleDTO(newSchedule, scheduleSections);

        //    return response;
        //}

        //public IEnumerable<PlanScheduleDTO> GetScheduleByPlanId(Guid planId)
        //{
        //    var schedules = _dbContext.PlanSchedules.AsNoTracking()
        //                                            .Include(x => x.PlanSections)
        //                                            .Where(x => x.PlanId == planId)
        //                                            .ToList();

        //    var response = (from schedule in schedules
        //                    select MapPlanScheduleDTO(schedule, schedule.PlanSections)
        //                   ).ToList();

        //    return response;
        //}

        //public void DeleteSchedule(Guid scheduleId)
        //{
        //    var schedule = _dbContext.PlanSchedules.Include(x => x.PlanSections)
        //                                           .SingleOrDefault(x => x.Id == scheduleId);

        //    if(schedule is null)
        //    {
        //        return;
        //    }

        //    var plan = _dbContext.Plans.Include(x => x.PlanSchedules)
        //                               .Single(x => x.Id == schedule.PlanId);

        //    var otherSchedulesCount = plan.PlanSchedules.Where(x => x.Id != scheduleId).Count();

        //    using(var transaction = _dbContext.Database.BeginTransaction())
        //    {
        //        _dbContext.PlanScheduleSections.RemoveRange(schedule.PlanSections);
        //        _dbContext.PlanSchedules.Remove(schedule);

        //        if(otherSchedulesCount == 0)
        //        {
        //            _dbContext.Plans.Remove(plan);
        //        }

        //        transaction.Commit();
        //    }

        //    _dbContext.SaveChanges();
        //}

        #endregion

        //private Plan AddPlanToDB(Guid studentId, Guid termId, PlanType type, List<Guid> requestCourses)
        //{
        //    var newPlan = new Plan
        //    {
        //        StudentId = studentId,
        //        TermId = termId,
        //        Type = type,
        //        CreatedAt = DateTime.UtcNow,
        //        UpdatedAt = DateTime.UtcNow
        //    };

        //    var planCourses = (from courseId in requestCourses
        //                       select new PlanCourse
        //                       {
        //                           Plan = newPlan,
        //                           CourseId = courseId
        //                       })
        //                      .ToList();

        //    using (var transaction = _dbContext.Database.BeginTransaction())
        //    {
        //        _dbContext.Plans.Add(newPlan);
        //        _dbContext.PlanCourses.AddRange(planCourses);

        //        transaction.Commit();
        //    }

        //    _dbContext.SaveChanges();

        //    return newPlan;
        //}

        private static bool IsCourseMatching(IEnumerable<PlanCourse> courses, IEnumerable<Guid> sortedCourseIds)
        {
            if (courses is null || sortedCourseIds is null)
            {
                return false;
            }

            if (courses.Count() != sortedCourseIds.Count())
            {
                return false;
            }

            var plannedCourses = courses.OrderBy(x => x.CourseId)
                                        .Select(x => x.CourseId)
                                        .ToList();

            return plannedCourses.SequenceEqual(sortedCourseIds);
        }

        private static bool IsSectionMatching(IEnumerable<PlanScheduleSection> sections, IEnumerable<Guid> sortedSectionIds)
        {
            if(sections is null || sortedSectionIds is null)
            {
                return false;
            }

            if (sections.Count() != sortedSectionIds.Count())
            {
                return false;
            }

            var plannedSections = sections.OrderBy(x => x.SectionId)
                                          .Select(x => x.SectionId)
                                          .ToList();

            return plannedSections.SequenceEqual(sortedSectionIds);
        }

        private static PlanDTO MapPlanDTO(Plan model, IEnumerable<PlanCourse> courses)
        {
            return new PlanDTO
            {
                Id = model.Id,
                Type = model.Type,
                CourseIds = courses is null ? Enumerable.Empty<Guid>()
                                            : courses.OrderBy(x => x.CourseId)
                                                     .Select(x => x.CourseId)
                                                     .ToList()
            };
        }

        private static PlanScheduleDTO MapPlanScheduleDTO(PlanSchedule model, IEnumerable<PlanScheduleSection> sections)
        {
            return new PlanScheduleDTO
            {
                Id = model.Id,
                SectionIds = sections is null ? Enumerable.Empty<Guid>()
                                              : (from planSection in sections
                                                 orderby planSection.SectionId
                                                 select planSection.SectionId)
                                                .ToList()
            };
        }
    }
}

