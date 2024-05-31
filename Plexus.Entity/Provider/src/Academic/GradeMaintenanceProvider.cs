using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Plexus.Database;
using Plexus.Database.Model;
using Plexus.Database.Model.Academic;
using Plexus.Database.Model.Academic.Advising;
using Plexus.Database.Model.Academic.Curriculum;
using Plexus.Database.Model.Facility;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Academic;
using Plexus.Entity.Exception;
using Plexus.Utility.Extensions;
using Plexus.Utility.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plexus.Entity.Provider.src.Academic
{
    public class GradeMaintenanceProvider : IGradeMaintenanceProvider
    {
        private readonly DatabaseContext _dbContext;

        public GradeMaintenanceProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public GradeMaintenanceDTO GetById(Guid gradeId)
        {
            var gradeMaintenance = _dbContext.GradeMaintenance.AsNoTracking()
                                         .SingleOrDefault(x => x.Id == gradeId);

            if (gradeMaintenance is null)
            {
                throw new GradeException.NotFound(gradeId);
            }

            var response = MapModelToDTO(gradeMaintenance);

            return response;
        }

        public IEnumerable<GradeMaintenanceDTO> GetById(IEnumerable<Guid> gradeIds)
        {
            if (gradeIds is null || !gradeIds.Any())
            {
                return Enumerable.Empty<GradeMaintenanceDTO>();
            }

            var gradesMaintenance = _dbContext.GradeMaintenance.AsNoTracking()
                                          .Where(x => gradeIds.Contains(x.Id))
                                          .ToList();


            var response = (from grade in gradesMaintenance
                            orderby grade.Grade descending, grade.Grade
                            select MapModelToDTO(grade))
                           .ToList();

            return response;
        }
        public GradeMaintenanceDTO Create(CreateGradeMaintenanceDTO request, string requester)
        {
            var gradeMaintenance = new GradeMaintenance
            {
                StudentCode = request.StudentCode,
                CoursesCode = request.CoursesCode,
                Grade = request.Grade.ToUpper(),
                Remark = request.Remark,
                PathFile = request.PathFile,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = requester
            };

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.GradeMaintenance.Add(gradeMaintenance);
                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(gradeMaintenance);

            return response;
        }
        public PagedViewModel<GradeMaintenanceDTO> Search(SearchGradeMaintenanceViewModel parameters, int page, int pageSize)
        {
            var query = GenerateSearchQuery(parameters);
            var pagedGrades = query.GetPagedViewModel(page, pageSize);
            var response = new PagedViewModel<GradeMaintenanceDTO>
            {
                Page = pagedGrades.Page,
                TotalPage = pagedGrades.TotalPage,
                TotalItem = pagedGrades.TotalItem,
                Items = (from grade in pagedGrades.Items
                         select MapModelToDTO(grade))
                         .ToList()
            };

            return response;
        }
        public PagedViewModel<CourseGradeMaintenanceDTO> SearchCouse(SearchGradeMaintenanceViewModel parameters, int page, int pageSize)
        {
            var query = GenerateSearchCouseQuery(parameters);

            var pagedGrades = query.GetPagedViewModel(page, pageSize);
            var response = new PagedViewModel<CourseGradeMaintenanceDTO>
            {
                Page = pagedGrades.Page,
                TotalPage = pagedGrades.TotalPage,
                TotalItem = pagedGrades.TotalItem,
                Items = pagedGrades.Items.ToList()
            };
            return response;

        }
        public void Delete(Guid gradeId)
        {
            var grade = _dbContext.GradeMaintenance.SingleOrDefault(x => x.Id == gradeId);

            if (grade is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.GradeMaintenance.Remove(grade);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        private IQueryable<CourseGradeMaintenanceDTO> GenerateSearchCouseQuery(SearchGradeMaintenanceViewModel? parameters)
        {
            var queryProfile = from section in _dbContext.Sections
                               join course in _dbContext.Courses on section.CourseId equals course.Id into courseGroup
                               from course in courseGroup.DefaultIfEmpty()
                               join term in _dbContext.Terms on section.TermId equals term.Id into termGroup
                               from term in termGroup.DefaultIfEmpty()
                               join student in _dbContext.Students on section.AcademicLevelId equals student.AcademicLevelId into studentGroup
                               from student in studentGroup.DefaultIfEmpty()
                               join studyCourse in _dbContext.StudyCourses on student.Id equals studyCourse.StudentId into studyCourseGroup
                               from studyCourse in studyCourseGroup.DefaultIfEmpty()
                               join academicLevel in _dbContext.AcademicLevels on section.AcademicLevelId equals academicLevel.Id into academicLevelGroup
                               from academicLevel in academicLevelGroup.DefaultIfEmpty()
                               join grade in _dbContext.Grades on studyCourse.GradeId equals grade.Id into gradeGroup
                               from grade in gradeGroup.DefaultIfEmpty()
                               join department in _dbContext.Departments on course.FacultyId equals department.FacultyId into departmentGroup
                               from department in departmentGroup.DefaultIfEmpty()
                               join curriculum in _dbContext.Curriculums on course.AcademicLevelId equals curriculum.AcademicLevelId into curriculumGroup
                               from curriculum in curriculumGroup.DefaultIfEmpty()
                               join curriculumVersion in _dbContext.CurriculumVersions on curriculum.Id equals curriculumVersion.CurriculumId into curriculumVersionGroup
                               from curriculumVersion in curriculumVersionGroup.DefaultIfEmpty()
                               join studyPlan in _dbContext.StudyPlans on curriculumVersion.Id equals studyPlan.CurriculumVersionId into studyPlanGroup
                               from studyPlan in studyPlanGroup.DefaultIfEmpty()
                               select new 
                               {
                                   StudentCode = student.Code,
                                   Term = term.Number + "/" + term.Year,
                                   StudentName = student.Title + " " + student.FirstName + " " + student.LastName,
                                   AcademicLevelFormalName = academicLevel.FormalName,
                                   DepartmentName = department.FormalName,
                                   CurriculumVersionName = curriculumVersion.Name,
                                   GPAX = (decimal?)student.GPA,
                                   StudyPlans = studyPlan.Name
                               };
            if (parameters is not null)
            {
                if (!string.IsNullOrEmpty(parameters.StudentCode))
                {
                    queryProfile = queryProfile.Where(x => x.StudentCode.Contains(parameters.StudentCode));
                }
            }

            if (parameters is not null)
            {
                if (!string.IsNullOrEmpty(parameters.Term))
                {
                    queryProfile = queryProfile.Where(x => x.Term.Contains(parameters.Term));
                }
            }

            var profeile = queryProfile.FirstOrDefault();
            var profeileDefault = new  ProfileGradeMaintenanceDTO();
            if (profeile == null)
            {
                profeileDefault = new ProfileGradeMaintenanceDTO
                {
                    StudentName = (string)null,
                    AcademicLevelFormalName = (string)null,
                    DepartmentName = (string)null,
                    CurriculumVersionName = (string)null,
                    GPAX = (decimal?)0.00,
                    StudyPlans = (string)null
                };               
            }
            
         

            var query = (from section in _dbContext.Sections.AsNoTracking()
                         join course in _dbContext.Courses.AsNoTracking() on section.CourseId equals course.Id
                         join term in _dbContext.Terms.AsNoTracking() on section.TermId equals term.Id
                         join campus in _dbContext.Campuses.AsNoTracking() on section.CampusId equals campus.Id
                         join student in _dbContext.Students.AsNoTracking() on section.AcademicLevelId equals student.AcademicLevelId into studentsGroup
                         from student in studentsGroup.DefaultIfEmpty()
                         join academicLevel in _dbContext.AcademicLevels.AsNoTracking() on section.AcademicLevelId equals academicLevel.Id into academicLevelsGroup
                         from academicLevel in academicLevelsGroup.DefaultIfEmpty()
                         join studyCourse in _dbContext.StudyCourses.AsNoTracking() on student.Id equals studyCourse.StudentId into studyCoursesGroup
                         from studyCourse in studyCoursesGroup.DefaultIfEmpty()
                         join grade in _dbContext.Grades.AsNoTracking() on studyCourse.GradeId equals grade.Id into gradesGroup
                         from grade in gradesGroup.DefaultIfEmpty()

                         select new CourseGradeMaintenanceDTO
                         {
                             StudentCode = student.Code,
                             Term = term.Number + "/" + term.Year,
                             StudentName  = profeile != null ? profeile.StudentName : profeileDefault.StudentName,
                             AcademicLevelFormalName = profeile != null ? profeile.AcademicLevelFormalName : profeileDefault.AcademicLevelFormalName ,
                             DepartmentName = profeile != null ? profeile.DepartmentName : profeileDefault.DepartmentName ,
                             CurriculumVersionName = profeile != null ? profeile.CurriculumVersionName : profeileDefault.CurriculumVersionName,
                             GPAX = profeile != null ? profeile.GPAX : profeileDefault.GPAX,
                             StudyPlans = profeile != null ? profeile.StudyPlans : profeileDefault.StudyPlans,
                             Course = course.Code,
                             Section = section.SectionNo,
                             Instructor = student.Title + " " + student.FirstName + " " + student.LastName,
                             Grade = academicLevel.Name,
                             Status = "",
                         });

            if (parameters is not null)
            {
                if (!string.IsNullOrEmpty(parameters.StudentCode))
                {
                    query = query.Where(x => x.StudentCode.Contains(parameters.StudentCode));
                }
            }

            if (parameters is not null)
            {
                if (!string.IsNullOrEmpty(parameters.Term))
                {
                    query = query.Where(x => x.Term.Contains(parameters.Term));
                }
            }

            query = query.OrderByDescending(x => x.StudentCode)
                         .ThenBy(x => x.StudentCode);

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

        private IQueryable<GradeMaintenance> GenerateSearchQuery(SearchGradeMaintenanceViewModel? parameters)
        {
            var query = _dbContext.GradeMaintenance.AsNoTracking();

            if (parameters is not null)
            {
                if (!string.IsNullOrEmpty(parameters.StudentCode))
                {
                    query = query.Where(x => x.StudentCode.Contains(parameters.StudentCode));
                }
            }

            query = query.OrderByDescending(x => x.StudentCode)
                         .ThenBy(x => x.StudentCode);

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

        public IEnumerable<GradeMaintenanceDTO> GetByName(string gradeName)
        {
            var gradeseMaintenance = _dbContext.GradeMaintenance.AsNoTracking()
                                          .Where(x => x.Grade == gradeName)
                                          .ToList();
            var response = (from gradeMainten in gradeseMaintenance
                            orderby gradeMainten.Grade descending, gradeMainten.Grade
                            select MapModelToDTO(gradeMainten))
                           .ToList();
            return response;
        }

        public GradeMaintenanceDTO Update(GradeMaintenanceDTO request, string requester)
        {
            var grade = _dbContext.GradeMaintenance.SingleOrDefault(x => x.Id == request.Id);

            if (grade is null)
            {
                throw new GradeException.NotFound(request.Id);
            }
            grade.Grade = request.Grade;
            grade.Remark = request.Remark;
            grade.PathFile = request.PathFile;
            grade.UpdatedAt = DateTime.UtcNow;
            grade.UpdatedBy = requester;
            _dbContext.SaveChanges();
            var response = MapModelToDTO(grade);
            return response;
        }

        private static GradeMaintenanceDTO MapModelToDTO(GradeMaintenance model)
        {
            return new GradeMaintenanceDTO
            {
                Id = model.Id,
                StudentCode = model.StudentCode,
                CoursesCode = model.CoursesCode,
                Grade = model.Grade,
                Remark = model.Remark,
                PathFile = model.PathFile,
                IsActive = model.IsActive,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt
            };
        }

    }
}
