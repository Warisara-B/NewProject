using Microsoft.EntityFrameworkCore;
using Plexus.Client.Exceptions;
using Plexus.Client.ViewModel.Academic.Curriculum;
using Plexus.Database;
using Plexus.Database.Model.Academic.Curriculum;
using ServiceStack;

namespace Plexus.Client.src.Academic
{
    public class StudyPlanManager : IStudyPlanManager
    {
        private readonly DatabaseContext _dbContext;

        public StudyPlanManager(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public StudyPlanViewModel CreateStudyPlan(CreateStudyPlanViewModel request)
        {
            var studyPlan = new StudyPlan
            {
                CurriculumVersionId = request.CurriculumVersionId,
                Name = request.Name
            };

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.StudyPlans.Add(studyPlan);
                transaction.Commit();
            }

            _dbContext.SaveChanges();

            return MapModelToViewModel(studyPlan, studyPlan.StudyPlanDetails);
        }

        public StudyPlanViewModel AddStudyPlanDetail(Guid id, CreateStudyPlanDetailViewModel request)
        {
            var existingSemesters = _dbContext.StudyPlanDetails.Where(x => x.StudyPlanId == id
                                                                    && x.Year == request.Year
                                                                    && x.Term == request.Term)
                                                               .ToList();

            if (existingSemesters.Any())
            {
                throw new StudyPlanException.SemesterExisted();
            }

            var existingCourses = _dbContext.StudyPlanDetails.Where(x => x.StudyPlanId == id
                                                                    && request.CourseIds.Contains(x.CourseId))
                                                             .ToList();

            if (existingCourses.Any())
            {
                throw new StudyPlanException.CourseExisted();
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                foreach (var courseId in request.CourseIds)
                {
                    var newStudyPlanDetail = new StudyPlanDetail
                    {
                        StudyPlanId = id,
                        CourseId = courseId,
                        Year = request.Year,
                        Term = request.Term
                    };

                    _dbContext.StudyPlanDetails.Add(newStudyPlanDetail);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var studyPlan = _dbContext.StudyPlans.AsNoTracking()
                                                 .Include(x => x.StudyPlanDetails)!
                                                    .ThenInclude(x => x.Course)
                                                        .ThenInclude(x => x.Localizations)
                                                 .FirstOrDefault(x => x.Id == id);

            if (studyPlan is null)
            {
                throw new StudyPlanException.NotFound(id);
            }

            return MapModelToViewModel(studyPlan, studyPlan.StudyPlanDetails);
        }

        public IEnumerable<StudyPlanViewModel> GetStudyPlans()
        {
            var studyPlans = _dbContext.StudyPlans.AsNoTracking()
                                                  .Include(x => x.StudyPlanDetails)!
                                                    .ThenInclude(x => x.Course)
                                                        .ThenInclude(x => x.Localizations)
                                                  .ToList();

            var response = (from data in studyPlans
                            select MapModelToViewModel(data, data.StudyPlanDetails));

            return response;
        }

        public StudyPlanViewModel UpdateStudyPlan(Guid id, UpdateStudyPlanViewModel request)
        {
            var studyPlan = _dbContext.StudyPlans.Include(x => x.StudyPlanDetails)!
                                                 .ThenInclude(x => x.Course)
                                                    .ThenInclude(x => x.Localizations)
                                                 .SingleOrDefault(x => x.Id == id);

            if (studyPlan is null)
            {
                throw new StudyPlanException.NotFound(id);
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                studyPlan.Name = request.Name;
                transaction.Commit();
            }

            _dbContext.SaveChanges();

            return MapModelToViewModel(studyPlan, studyPlan.StudyPlanDetails);
        }

        public StudyPlanViewModel UpdateStudyPlanDetail(Guid id, int year, string term, CreateStudyPlanDetailViewModel request)
        {
            var studyPlanDetails = _dbContext.StudyPlanDetails.Where(x => x.StudyPlanId == id
                                                                     && x.Year == year
                                                                     && x.Term.Contains(term))
                                                              .ToList();

            if (!studyPlanDetails.Any())
            {
                throw new StudyPlanException.NoCoursesFound();
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.StudyPlanDetails.RemoveRange(studyPlanDetails);

                foreach (var courseId in request.CourseIds)
                {
                    var updatedStudyPlanDetail = new StudyPlanDetail
                    {
                        StudyPlanId = id,
                        CourseId = courseId,
                        Year = request.Year,
                        Term = request.Term
                    };

                    _dbContext.StudyPlanDetails.Add(updatedStudyPlanDetail);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var studyPlan = _dbContext.StudyPlans.AsNoTracking()
                                                 .Include(x => x.StudyPlanDetails)!
                                                    .ThenInclude(x => x.Course)
                                                        .ThenInclude(x => x.Localizations)
                                                 .FirstOrDefault(x => x.Id == id);

            if (studyPlan is null)
            {
                throw new StudyPlanException.NotFound(id);
            }

            return MapModelToViewModel(studyPlan, studyPlan.StudyPlanDetails);
        }

        public void Delete(Guid id)
        {
            var studyPlan = _dbContext.StudyPlans.AsNoTracking()
                                                 .FirstOrDefault(x => x.Id == id);

            if (studyPlan is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.StudyPlans.Remove(studyPlan);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        public void DeleteByYear(Guid id, int year)
        {
            var studyPlanDetails = _dbContext.StudyPlanDetails.AsNoTracking()
                                                              .Where(x => x.StudyPlanId == id
                                                                     && x.Year == year)
                                                              .ToList();

            if (!studyPlanDetails.Any())
            {
                throw new StudyPlanException.NoCoursesFound();
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.StudyPlanDetails.RemoveRange(studyPlanDetails);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        public void DeleteByTerm(Guid id, int year, string term)
        {
            var studyPlanDetail = _dbContext.StudyPlanDetails.AsNoTracking()
                                                              .Where(x => x.StudyPlanId == id
                                                                     && x.Year == year
                                                                     && x.Term.Contains(term))
                                                              .ToList();

            if (!studyPlanDetail.Any())
            {
                throw new StudyPlanException.NoCoursesFound();
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.StudyPlanDetails.RemoveRange(studyPlanDetail);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        private static StudyPlanViewModel MapModelToViewModel(StudyPlan studyPlan, IEnumerable<StudyPlanDetail>? studyPlanDetails)
        {
            if (studyPlanDetails == null)
            {
                return new StudyPlanViewModel
                {
                    Id = studyPlan.Id,
                    CurriculumVersionId = studyPlan.CurriculumVersionId,
                    Name = studyPlan.Name,
                    Years = Enumerable.Empty<StudyPlanYearViewModel>()
                };
            }

            var yearGroups = studyPlanDetails.GroupBy(course => course.Year);

            var years = yearGroups.Select(yearGroup => new StudyPlanYearViewModel
            {
                Year = yearGroup.Key,
                Terms = yearGroup.GroupBy(course => course.Term)
                                 .OrderBy(group => group.Key)
                                 .Select(termGroup => new StudyPlanTermViewModel
                                 {
                                     Term = termGroup.Key,
                                     Courses = termGroup.Select(course => new StudyPlanCourseViewModel
                                     {
                                         CourseId = course.CourseId,
                                         Code = course.Course.Code,
                                         Credit = course.Course.Credit,
                                         RegistrationCredit = course.Course.RegistrationCredit,
                                         PaymentCredit = course.Course.PaymentCredit,
                                         Hour = course.Course.Hour,
                                         LectureCredit = course.Course.LectureCredit,
                                         LabCredit = course.Course.LabCredit,
                                         OtherCredit = course.Course.OtherCredit,
                                         Localizations = course.Course.Localizations.Select(locale => new StudyPlanCourseLocalizationViewModel
                                         {
                                             Language = locale.Language,
                                             Name = locale.Name
                                         })
                                         .ToList()
                                     })
                                     .ToList()
                                 })
                                 .ToList()
            })
            .OrderBy(year => year.Year)
            .ToList();

            return new StudyPlanViewModel
            {
                Id = studyPlan.Id,
                CurriculumVersionId = studyPlan.CurriculumVersionId,
                Name = studyPlan.Name,
                Years = years
            };
        }
    }
}