using System;
using Microsoft.EntityFrameworkCore;
using Plexus.Database;
using Plexus.Database.Model.Academic.Curriculum;
using Plexus.Database.Model.Localization.Academic.Curriculum;
using Plexus.Entity.DTO.Academic.Curriculum;
using Plexus.Entity.Exception;
using Plexus.Utility.Extensions;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider.src.Academic.Curriculum
{
    public class CurriculumCourseGroupProvider : ICurriculumCourseGroupProvider
    {
        private readonly DatabaseContext _dbContext;

        public CurriculumCourseGroupProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public CurriculumCourseGroupDTO Create(CreateCurriculumCourseGroupDTO request, string requester)
        {
            var model = new CurriculumCourseGroup
            {
                Name = request.Name,
                Description = request.Description,
                Type = request.Type,
                RequiredCredit = request.RequiredCredit,
                Sequence = request.Sequence,
                Remark = request.Remark,
                CurriculumVersionId = request.CurriculumVersionId,
                ParentCourseGroupId = request.ParentCourseGroupId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = requester
            };

            var localizes = MapLocalizationDTOToModel(request.Localizations, model).ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.CurriculumCourseGroups.Add(model);

                if (localizes.Any())
                {
                    _dbContext.CurriculumCourseGroupLocalizations.AddRange(localizes);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(model, localizes);

            return response;
        }

        public CurriculumCourseGroupDTO GetById(Guid id)
        {
            var courseGroup = _dbContext.CurriculumCourseGroups.AsNoTracking()
                                                               .Include(x => x.Localizations)
                                                               .Include(x => x.CurriculumCourses)
                                                               .Include(x => x.CourseGroupIgnoreCourses)
                                                               .SingleOrDefault(x => x.Id == id);

            if (courseGroup is null)
            {
                throw new CurriculumException.CourseGroupNotFound(id);
            }

            var response = MapModelToDTO(courseGroup, courseGroup.Localizations, courseGroup.CurriculumCourses, courseGroup.CourseGroupIgnoreCourses);

            return response;
        }

        public IEnumerable<CurriculumCourseGroupDTO> GetByCurriculumVersionId(Guid curriculumVersionId)
        {
            var courseGroups = _dbContext.CurriculumCourseGroups.AsNoTracking()
                                                                .Include(x => x.Localizations)
                                                                .Include(x => x.CurriculumCourses)
                                                                .Include(x => x.CourseGroupIgnoreCourses)
                                                                .Where(x => x.CurriculumVersionId == curriculumVersionId)
                                                                .ToList();

            var response = (from courseGroup in courseGroups
                            orderby courseGroup.Sequence, courseGroup.Name
                            select MapModelToDTO(courseGroup, courseGroup.Localizations, courseGroup.CurriculumCourses, courseGroup.CourseGroupIgnoreCourses))
                           .ToList();

            return response;
        }

        public CurriculumCourseGroupDTO Update(CurriculumCourseGroupDTO request, string requester)
        {
            var courseGroup = _dbContext.CurriculumCourseGroups.Include(x => x.CurriculumCourses)
                                                               .Include(x => x.CourseGroupIgnoreCourses)
                                                               .Include(x => x.Localizations)
                                                               .SingleOrDefault(x => x.Id == request.Id);

            if (courseGroup is null)
            {
                throw new CurriculumException.NotFound(request.Id);
            }

            var localizes = MapLocalizationDTOToModel(request.Localizations, courseGroup).ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                courseGroup.Name = request.Name;
                courseGroup.Description = request.Description;
                courseGroup.Type = request.Type;
                courseGroup.RequiredCredit = request.RequiredCredit;
                courseGroup.Sequence = request.Sequence;
                courseGroup.Remark = request.Remark;
                courseGroup.CurriculumVersionId = request.CurriculumVersionId;
                courseGroup.ParentCourseGroupId = request.ParentCourseGroupId;
                courseGroup.UpdatedAt = DateTime.UtcNow;
                courseGroup.UpdatedBy = requester;

                _dbContext.CurriculumCourseGroupLocalizations.RemoveRange(courseGroup.Localizations);

                if (localizes.Any())
                {
                    _dbContext.CurriculumCourseGroupLocalizations.AddRange(localizes);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(courseGroup, localizes, courseGroup.CurriculumCourses, courseGroup.CourseGroupIgnoreCourses);

            return response;
        }

        public void Delete(Guid id)
        {
            var courseGroup = _dbContext.CurriculumCourseGroups.AsNoTracking()
                                                               .SingleOrDefault(x => x.Id == id);

            if (courseGroup is null)
            {
                return;
            }

            var currCourseGroups = _dbContext.CurriculumCourseGroups.Where(x => x.CurriculumVersionId == courseGroup.CurriculumVersionId)
                                                                    .ToList();

            var deleteCourseGroupIds = new List<Guid> { id };

            var subGroupIds = FindSubGroupId(deleteCourseGroupIds, currCourseGroups).ToList();

            deleteCourseGroupIds.AddRange(subGroupIds);

            do
            {
                subGroupIds = FindSubGroupId(subGroupIds, currCourseGroups).ToList();

                if (subGroupIds.Any())
                {
                    deleteCourseGroupIds.AddRange(subGroupIds);
                }

            } while (subGroupIds.Any());

            var deleteCourseGroups = currCourseGroups.Where(x => deleteCourseGroupIds.Contains(x.Id))
                                                     .ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.CurriculumCourseGroups.RemoveRange(deleteCourseGroups);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        public IEnumerable<CurriculumCourseDTO> GetCourses(Guid courseGroupid)
        {
            var courses = _dbContext.CurriculumCourses.AsNoTracking()
                                                      .Where(x => x.CourseGroupId == courseGroupid);

            var response = (from course in courses
                            select MapCourseModelToDTO(course))
                            .ToList();

            return response;
        }

        public void UpdateCourses(Guid courseGroupId, IEnumerable<CurriculumCourseDTO> request)
        {
            var existingCourses = _dbContext.CurriculumCourses.Where(x => x.CourseGroupId == courseGroupId)
                                                              .ToList();

            var newCourses = (from course in request
                              select new CurriculumCourse
                              {
                                  CourseId = course.CourseId,
                                  CourseGroupId = courseGroupId,
                                  RequiredGradeId = course.RequiredGradeId
                              })
                             .ToList();

            using (var transcation = _dbContext.Database.BeginTransaction())
            {
                if (existingCourses.Any())
                {
                    _dbContext.CurriculumCourses.RemoveRange(existingCourses);
                }

                if (newCourses.Any())
                {
                    _dbContext.CurriculumCourses.AddRange(newCourses);
                }

                transcation.Commit();
            }

            _dbContext.SaveChanges();
        }

        public IEnumerable<CurriculumCourseGroupIgnoreCourseDTO> GetIgnoreCourses(Guid courseGroupId)
        {
            var ignoreCourses = _dbContext.CurriculumCourseGroupIgnoreCourses.AsNoTracking()
                                                                             .Where(x => x.CourseGroupId == courseGroupId)
                                                                             .ToList();

            var response = (from course in ignoreCourses
                            select MapIgnoreCourseModelToDTO(course))
                           .ToList();

            return response;
        }

        public void UpdateIgnoreCourses(Guid courseGroupId, IEnumerable<Guid> courseIds)
        {
            var existingIgnoreCourses = _dbContext.CurriculumCourseGroupIgnoreCourses.Where(x => x.CourseGroupId == courseGroupId)
                                                                                     .ToList();

            var newIgnoreCourses = courseIds is null ? Enumerable.Empty<CurriculumCourseGroupIgnoreCourse>()
                                                     : (from courseId in courseIds
                                                        select new CurriculumCourseGroupIgnoreCourse
                                                        {
                                                            CourseGroupId = courseGroupId,
                                                            CourseId = courseId
                                                        })
                                                       .ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.CurriculumCourseGroupIgnoreCourses.RemoveRange(existingIgnoreCourses);

                if (newIgnoreCourses.Any())
                {
                    _dbContext.CurriculumCourseGroupIgnoreCourses.AddRange(newIgnoreCourses);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        private static IEnumerable<Guid> FindSubGroupId(IEnumerable<Guid> baseGroupIds, IEnumerable<CurriculumCourseGroup> curriculumCourseGroups)
        {
            if (curriculumCourseGroups is null || !curriculumCourseGroups.Any())
            {
                return Enumerable.Empty<Guid>();
            }

            var subGroup = curriculumCourseGroups.Where(x => x.ParentCourseGroupId.HasValue && baseGroupIds.Contains(x.ParentCourseGroupId.Value))
                                                 .Select(x => x.Id)
                                                 .Distinct()
                                                 .ToList();

            return subGroup;
        }

        private static CurriculumCourseGroupDTO MapModelToDTO(CurriculumCourseGroup model,
                                                              IEnumerable<CurriculumCourseGroupLocalization> localizations,
                                                              IEnumerable<CurriculumCourse> courses = null,
                                                              IEnumerable<CurriculumCourseGroupIgnoreCourse> ignoreCourses = null)
        {
            var response = new CurriculumCourseGroupDTO
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Type = model.Type,
                RequiredCredit = model.RequiredCredit,
                Sequence = model.Sequence,
                Remark = model.Remark,
                CurriculumVersionId = model.CurriculumVersionId,
                ParentCourseGroupId = model.ParentCourseGroupId,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                Courses = courses is null ? Enumerable.Empty<CurriculumCourseDTO>()
                                          : (from course in courses
                                             select MapCourseModelToDTO(course))
                                            .ToList(),
                IgnoreCourses = ignoreCourses is null ? Enumerable.Empty<CurriculumCourseGroupIgnoreCourseDTO>()
                                                      : (from course in ignoreCourses
                                                         select MapIgnoreCourseModelToDTO(course))
                                                        .ToList(),
                Localizations = localizations is null ? Enumerable.Empty<CurriculumCourseGroupLocalizationDTO>()
                                                      : (from locale in localizations
                                                         orderby locale.Language
                                                         select new CurriculumCourseGroupLocalizationDTO
                                                         {
                                                             Language = locale.Language,
                                                             Name = locale.Name,
                                                             Description = locale.Description
                                                         })
                                                        .ToList()
            };

            return response;
        }

        private static CurriculumCourseDTO MapCourseModelToDTO(CurriculumCourse model)
        {
            return new CurriculumCourseDTO
            {
                CourseGroupId = model.CourseGroupId,
                CourseId = model.CourseId,
                RequiredGradeId = model.RequiredGradeId,
            };
        }

        private static CurriculumCourseGroupIgnoreCourseDTO MapIgnoreCourseModelToDTO(CurriculumCourseGroupIgnoreCourse model)
        {
            var response = new CurriculumCourseGroupIgnoreCourseDTO
            {
                CourseGroupId = model.CourseGroupId,
                CourseId = model.CourseId
            };

            return response;
        }

        private static IEnumerable<CurriculumCourseGroupLocalization> MapLocalizationDTOToModel(
                    IEnumerable<CurriculumCourseGroupLocalizationDTO> localizations,
                    CurriculumCourseGroup model)
        {
            if (localizations is null)
            {
                return Enumerable.Empty<CurriculumCourseGroupLocalization>();
            }

            var response = (from locale in localizations
                            select new CurriculumCourseGroupLocalization
                            {
                                CurriculumCourseGroup = model,
                                Language = locale.Language,
                                Name = locale.Name,
                                Description = locale.Description
                            })
                           .ToList();

            return response;
        }
    }
}

