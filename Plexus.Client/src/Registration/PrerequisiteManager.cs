using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Plexus.Client.ViewModel.Academic;
using Plexus.Client.ViewModel.Registration;
using Plexus.Database;
using Plexus.Database.Enum.Registration;
using Plexus.Database.Model;
using Plexus.Database.Model.Academic;
using Plexus.Database.Model.Academic.Curriculum;
using Plexus.Database.Model.Registration;
using Plexus.Entity.Exception;
using Plexus.Entity.Provider;
using ServiceStack;

namespace Plexus.Client.src.Registration
{
    public class PrerequisiteManager : IPrerequisiteManager
    {
        private readonly IPrerequisiteProvider _prerequisiteProvider;
        private readonly ICourseProvider _courseProvider;
        private readonly IGradeProvider _gradeProvider;
        private readonly IFacultyProvider _facultyProvider;
        private readonly IDepartmentProvider _departmentProvider;
        private readonly ICurriculumVersionProvider _curriculumVersionProvider;
        private readonly IStudentProvider _studentProvider;
        private readonly IRegistrationManager _registrationManager;
        private readonly DatabaseContext _dbContext;

        public PrerequisiteManager(IPrerequisiteProvider prerequisiteProvider,
                                   ICourseProvider courseProvider,
                                   IGradeProvider gradeProvider,
                                   IFacultyProvider facultyProvider,
                                   IDepartmentProvider departmentProvider,
                                   ICurriculumVersionProvider curriculumVersionProvider,
                                   IStudentProvider studentProvider,
                                      IRegistrationManager registrationManager,
                                   DatabaseContext dbContext)
        {
            _prerequisiteProvider = prerequisiteProvider;
            _courseProvider = courseProvider;
            _gradeProvider = gradeProvider;
            _facultyProvider = facultyProvider;
            _departmentProvider = departmentProvider;
            _curriculumVersionProvider = curriculumVersionProvider;
            _studentProvider = studentProvider;
            // _registrationManager = registrationManager;
            _dbContext = dbContext;
        }

        public void VerifyPrerequisite(RegistrationViewModel request)
        {
            if (request.Sections is null || !request.Sections.Any())
            {
                return;
            }

            foreach (var studentId in request.StudentIds)
            {
                var student = _dbContext.Students.AsNoTracking()
                                                 .Include(x => x.Localizations)
                                                 .FirstOrDefault(x => x.Id == studentId);

                if (student is null)
                {
                    throw new StudentException.NotFound(studentId);
                }

                var studyCourses = _registrationManager.GetByStudent(studentId, null, false)
                                                       .Where(x => x.TermId != request.TermId)
                                                       .ToList();

                var courseIds = request.Sections.Select(x => x.CourseId)
                                                .Distinct()
                                                .ToList();

                var courses = _dbContext.Courses.AsNoTracking()
                                                .Include(x => x.Localizations)
                                                .Where(x => courseIds.Contains(x.Id))
                                                .ToList();

                var coursePrerequisites = _dbContext.CoursePrerequisites.AsNoTracking()
                                                                        .Where(x => courseIds.Contains(x.CourseId)
                                                                               && !x.DeactivatedAt.HasValue)
                                                                        .ToList();

                var grades = _dbContext.Grades.AsNoTracking()
                                              .ToList();

                foreach (var course in courses)
                {
                    IEnumerable<CreatePrerequisiteConditionViewModel>? conditions = null;

                    var matchedCoursePrerequisites = coursePrerequisites.SingleOrDefault(x => x.CourseId == course.Id);

                    if (matchedCoursePrerequisites is null)
                    {
                        continue;
                    }

                    conditions = (from condition in matchedCoursePrerequisites.Conditions
                                  select JsonConvert.DeserializeObject<CreatePrerequisiteConditionViewModel>(condition))
                                 .ToList();

                    var isPassedConditions = from condition in conditions
                                             select VerifyPrerequisiteConditions(student, grades, null, condition);

                    foreach (var isPassed in isPassedConditions)
                    {
                        if (!isPassed)
                        {
                            throw new PrerequisiteException.PrerequisiteConditionFail(course.Code, course.Name);
                        }
                    }
                }
            }
        }

        public CoursePrerequisiteViewModel UpdateCoursePrerequisite(Guid courseId, CreatePrerequisiteViewModel request, Guid userId)
        {
            var existingPrerequisite = _dbContext.CoursePrerequisites.SingleOrDefault(x => x.CourseId == courseId
                                                                                      && !x.DeactivatedAt.HasValue);

            var model = new CoursePrerequisite
            {
                CourseId = courseId,
                Conditions = (from condition in request.PrerequisiteConditions
                              select JsonConvert.SerializeObject(request.PrerequisiteConditions)),
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "" // TODO : Add requester
            };

            var curriculumVersions = MapCurriculumVersionViewModel(request.CurriculumVersionsIds, model);
            CurriculumCoursePrerequisite? existingCurriculumPrerequisite;

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                if (existingPrerequisite is not null)
                {
                    existingPrerequisite.DeactivatedAt = DateTime.UtcNow;
                    existingCurriculumPrerequisite = _dbContext.CurriculumCoursePrerequisites.SingleOrDefault(x => x.CoursePrerequisiteId == existingPrerequisite.Id);

                    if (existingCurriculumPrerequisite is not null)
                    {
                        _dbContext.CurriculumCoursePrerequisites.Remove(existingCurriculumPrerequisite);
                    }
                }

                _dbContext.CoursePrerequisites.Add(model);

                if (curriculumVersions.Any())
                {
                    _dbContext.CurriculumCoursePrerequisites.AddRange(curriculumVersions);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapCourseModelToViewModel(model);

            return response;
        }

        public IEnumerable<CoursePrerequisiteViewModel> GetCurriculumVersionPrerequisites(Guid curriculumVersionId)
        {
            var curriculumVersions = _dbContext.CurriculumVersions.AsNoTracking()
                                                                 .Include(x => x.Localizations)
                                                                 .Where(x => x.Id == curriculumVersionId)
                                                                 .ToList();
            if (curriculumVersions is null)
            {
                throw new CurriculumException.VersionNotFound(curriculumVersionId);
            }

            var prerequisiteIds = _dbContext.CurriculumCoursePrerequisites.Where(x => x.Id == curriculumVersionId)
                                                                          .Select(x => x.CoursePrerequisiteId);

            var coursePrerequisites = _dbContext.CoursePrerequisites.Where(x => prerequisiteIds.Contains(x.Id))
                                                                    .ToList();

            if (!coursePrerequisites.Any())
            {
                return Enumerable.Empty<CoursePrerequisiteViewModel>();
            }

            var courseIds = coursePrerequisites.Select(x => x.CourseId)
                                               .Distinct()
                                               .ToList();

            var courses = _dbContext.Courses.AsNoTracking()
                                            .Include(x => x.Localizations)
                                            .Where(x => courseIds.Contains(x.Id))
                                            .ToList();

            var response = (from data in coursePrerequisites
                            join course in courses on data.CourseId equals course.Id
                            select MapCoursePrerequisiteModelToViewModel(data, course, curriculumVersions))
                            .ToList();

            return response;
        }

        private CoursePrerequisiteViewModel MapCoursePrerequisiteModelToViewModel(CoursePrerequisite model, Course course, IEnumerable<CurriculumVersion> versions)
        {
            var response = new CoursePrerequisiteViewModel
            {
                Id = model.Id,
                CourseId = model.CourseId,
                CourseCode = course.Code,
                CourseName = course.Name,
                Condition = (from condition in model.Conditions
                             select MapPrerequisiteConditionToViewModel(JsonConvert.DeserializeObject<CreatePrerequisiteConditionViewModel>(condition)!)),
                CurriculumVersions = (from curriculumVersion in versions
                                      select new CurriculumCoursePrerequisiteViewModel
                                      {
                                          CurriculumVersionId = curriculumVersion.Id,
                                          CurriculumVersionName = curriculumVersion.Name
                                      })
                                      .ToList(),
                CreatedAt = model.CreatedAt,
                DeactivatedAt = model.DeactivatedAt
            };

            return response;
        }

        private PrerequisiteViewModel MapPrerequisiteToViewModel(CreatePrerequisiteViewModel condition)
        {
            var curriculumVersions = _dbContext.CurriculumVersions.AsNoTracking()
                                                                  .Where(x => condition.CurriculumVersionsIds.Contains(x.Id))
                                                                  .Include(x => x.Localizations)
                                                                  .ToList();
            var response = new PrerequisiteViewModel
            {
                CurriculumVersionNames = (from curriculumVersion in curriculumVersions
                                          select curriculumVersion.Name),
                PrerequisiteConditions = (from prerequisite in condition.PrerequisiteConditions
                                          select MapPrerequisiteConditionToViewModel(prerequisite))
            };

            return response;
        }

        private PrerequisiteConditionViewModel MapPrerequisiteConditionToViewModel(CreatePrerequisiteConditionViewModel condition)
        {
            var response = new PrerequisiteConditionViewModel
            {
                ConditionType = condition.ConditionType,
                Conditions = (from con in condition.Conditions
                              select MapDetailToViewModel(con))
                             .ToList()
            };

            return response;
        }

        private PrerequisiteConditionDetailViewModel MapDetailToViewModel(CreatePrerequisiteConditionDetailViewModel detail)
        {
            var course = detail.CourseId.HasValue ? _courseProvider.GetById(detail.CourseId.Value) : null;
            var grade = detail.GradeId.HasValue ? _gradeProvider.GetById(detail.GradeId.Value) : null;

            var response = new PrerequisiteConditionDetailViewModel
            {
                PrerequisiteType = detail.PrerequisiteType,
                CourseId = detail.CourseId,
                CourseCode = course?.Code,
                CourseName = course?.Name,
                GradeId = detail.GradeId,
                GradeLetter = grade?.Letter,
                GPA = detail.GPA,
                Credit = detail.Credit,
                Term = detail.Term,
                FromBatch = detail.FromBatch,
                ToBatch = detail.ToBatch,
                PrerequisiteConditions = (from prerequisite in detail.PrerequisiteConditions
                                          select MapPrerequisiteConditionToViewModel(prerequisite))
                                         .ToList()
            };

            return response;
        }

        // private CreatePrerequisiteConditionViewModel ValidateCondition(CreatePrerequisiteConditionViewModel condition)
        // {
        //     CreatePrerequisiteConditionViewModel viewModel = new CreatePrerequisiteConditionViewModel
        //     {
        //         Condition = condition.Condition,
        //         Type = condition.Type
        //     };

        //     switch (condition.Type)
        //     {
        //         case PrerequisiteConditionType.GRADE:
        //             if (!condition.CourseId.HasValue || !condition.GradeId.HasValue)
        //             {
        //                 throw new PrerequisiteException.InvalidGradeCondition();
        //             }

        //             var course = _courseProvider.GetById(condition.CourseId.Value);

        //             var grade = _gradeProvider.GetById(condition.GradeId.Value);

        //             viewModel.CourseId = condition.CourseId;
        //             dto.GradeId = condition.GradeId;

        //             return dto;
        //         case PrerequisiteConditionType.GPA:
        //             if (!condition.GPA.HasValue || condition.GPA.Value < decimal.Zero)
        //             {
        //                 throw new PrerequisiteException.InvalidGPACondition();
        //             }

        //             dto.GPA = condition.GPA;

        //             return dto;
        //         case PrerequisiteConditionType.CREDIT:
        //             if (!condition.Credit.HasValue || condition.Credit.Value < decimal.Zero)
        //             {
        //                 throw new PrerequisiteException.InvalidCreditCondition();
        //             }

        //             dto.Credit = condition.Credit;

        //             return dto;
        //         case PrerequisiteConditionType.TERM:
        //             if (!condition.Term.HasValue || condition.Term.Value < 0)
        //             {
        //                 throw new PrerequisiteException.InvalidTermCountCondition();
        //             }

        //             dto.TermCount = condition.Term;

        //             return dto;

        //         default:
        //             dto.Conditions = condition.Conditions is null || !condition.Conditions.Any() ? null
        //                                                                                          : (from prerequisite in condition.Conditions
        //                                                                                             select ValidateCondition(prerequisite))
        //                                                                                          .ToList();

        //             return dto;
        //     }
        // }

        private bool VerifyPrerequisiteConditions(Student student, IEnumerable<Grade> grades, IEnumerable<StudyCourseViewModel> studyCourses, CreatePrerequisiteConditionViewModel condition)
        {
            foreach (var con in condition.Conditions)
            {
                switch (con.PrerequisiteType)
                {
                    case PrerequisiteConditionType.GRADE:
                        var grade = grades.SingleOrDefault(x => x.Id == con.GradeId!.Value);

                        var matchedCourses = studyCourses.Where(x => x.CourseId == con.CourseId!.Value);

                        if (grade!.IsCalculateGPA)
                        {
                            matchedCourses = matchedCourses.Where(x => x.GradeWeight.HasValue
                                                                       && x.GradeWeight.Value >= grade.Weight);
                        }
                        else
                        {
                            matchedCourses = matchedCourses.Where(x => x.GradeId.HasValue
                                                                       && x.GradeId.Value == con.GradeId!.Value);
                        }

                        if (!matchedCourses.Any())
                        {
                            return false;
                        }

                        break;
                    case PrerequisiteConditionType.GPA:
                        if (student.GPA < con.GPA!.Value)
                        {
                            return false;
                        }

                        break;
                    case PrerequisiteConditionType.CREDIT:
                        // TODO : Verify how to get student total credit

                        var totalCredit = studyCourses.Sum(x => x.Credit);

                        if (totalCredit < con.Credit!.Value)
                        {
                            return false;
                        }

                        break;
                    case PrerequisiteConditionType.TERM:
                        var termCount = studyCourses.GroupBy(x => x.TermId)
                                                    .Count();

                        if (termCount < con.Term!.Value)
                        {
                            return false;
                        }

                        break;

                    default:
                        break;
                }

                switch (condition.ConditionType)
                {
                    case PrerequisiteCondition.AND:
                        if (condition.Conditions is null || !condition.Conditions.Any())
                        {
                            break;
                        }

                        foreach (var andCondition in con.PrerequisiteConditions!)
                        {
                            var isPassed = VerifyPrerequisiteConditions(student, grades, studyCourses, andCondition);

                            if (!isPassed)
                            {
                                return false;
                            }
                        }

                        break;
                    case PrerequisiteCondition.OR:
                        if (condition.Conditions is null || !condition.Conditions.Any())
                        {
                            break;
                        }

                        foreach (var orCondition in con.PrerequisiteConditions!)
                        {
                            var isPassed = VerifyPrerequisiteConditions(student, grades, studyCourses, orCondition);

                            if (isPassed)
                            {
                                return true;
                            }
                        }

                        return false;
                    default:
                        break;
                }

            }

            return true;
        }

        private static CoursePrerequisiteViewModel MapCourseModelToViewModel(CoursePrerequisite model)
        {
            var response = new CoursePrerequisiteViewModel
            {
                Id = model.Id,
                CourseId = model.CourseId,
                CurriculumVersions = (from curriculumVersion in model.Curriculums
                                      select new CurriculumCoursePrerequisiteViewModel
                                      {
                                          CurriculumVersionId = curriculumVersion.Id,
                                          CurriculumVersionName = curriculumVersion.CurriculumVersion.Name,
                                      })
                                      .ToList(),
                Condition = (from condition in model.Conditions
                             select JsonConvert.DeserializeObject<PrerequisiteConditionViewModel>(condition)),
                CreatedAt = model.CreatedAt,
                DeactivatedAt = model.DeactivatedAt
            };

            return response;
        }

        private IEnumerable<CurriculumCoursePrerequisite> MapCurriculumVersionViewModel(
                 IEnumerable<Guid> curriculumVersionIds,
                 CoursePrerequisite model
        )
        {
            var curriculumVersions = _dbContext.CurriculumVersions.Include(x => x.Localizations)
                                                                  .Where(x => curriculumVersionIds.Contains(x.Id))
                                                                  .ToList();
            if (curriculumVersionIds is null)
            {
                return Enumerable.Empty<CurriculumCoursePrerequisite>();
            }

            var response = (from curriculum in curriculumVersions
                            select new CurriculumCoursePrerequisite
                            {
                                CoursePrerequisite = model,
                                CurriculumVersion = curriculum,
                            })
                            .ToList();

            return response;

        }
    }
}