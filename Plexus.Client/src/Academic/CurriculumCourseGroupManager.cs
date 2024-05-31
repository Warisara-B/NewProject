using System;
using Plexus.Client.ViewModel.Academic.Curriculum;
using Plexus.Database.Enum.Academic.Curriculum;
using Plexus.Entity.DTO.Academic;
using Plexus.Entity.DTO.Academic.Curriculum;
using Plexus.Entity.Exception;
using Plexus.Entity.Provider;
using Plexus.Utility.ViewModel;

namespace Plexus.Client.src.Academic
{
    public class CurriculumCourseGroupManager : ICurriculumCourseGroupManager
    {
        private readonly ICurriculumVersionProvider _curriculumVersionProvider;
        private readonly ICurriculumCourseGroupProvider _curriculumCourseGroupProvider;
        private readonly ICourseProvider _courseProvider;
        private readonly IGradeProvider _gradeProvider;

        public CurriculumCourseGroupManager(ICurriculumVersionProvider curriculumVersionProvider,
                                            ICurriculumCourseGroupProvider curriculumCourseGroupProvider,
                                            ICourseProvider courseProvider,
                                            IGradeProvider gradeProvider)
        {
            _curriculumVersionProvider = curriculumVersionProvider;
            _curriculumCourseGroupProvider = curriculumCourseGroupProvider;
            _courseProvider = courseProvider;
            _gradeProvider = gradeProvider;
        }

        public CurriculumCourseGroupViewModel Create(CreateCurriculumCourseGroupViewModel request, Guid userId)
        {
            if (request.ParentCourseGroupId.HasValue)
            {
                var parentGroup = _curriculumCourseGroupProvider.GetById(request.ParentCourseGroupId.Value);

                // FREE ELECTIVE GROUP CAN'T HAVE SUBGROUP
                if (request.Type == CourseGroupType.FREE_ELECTIVE
                   || parentGroup.Type == CourseGroupType.FREE_ELECTIVE)
                {
                    throw new CurriculumException.FreeElectiveSubGroupException();
                }
            }

            var dto = new CreateCurriculumCourseGroupDTO
            {
                ParentCourseGroupId = request.ParentCourseGroupId,
                CurriculumVersionId = request.CurriculumVersionId,
                Name = request.Name,
                Description = request.Description,
                Type = request.Type,
                RequiredCredit = request.RequiredCredit,
                Sequence = request.Sequence,
                Remark = request.Remark,
                Localizations = MapLocalizationViewModelToDTO(request.Localizations).ToList()
            };

            var courseGroup = _curriculumCourseGroupProvider.Create(dto, userId.ToString());

            var response = MapCourseGroupDTOToViewModel(courseGroup);

            return response;
        }

        public CurriculumCourseGroupViewModel GetById(Guid id)
        {
            var courseGroup = _curriculumCourseGroupProvider.GetById(id);

            var currVersionCourseGroups = _curriculumCourseGroupProvider.GetByCurriculumVersionId(courseGroup.CurriculumVersionId)
                                                                        .ToList();

            var courseIds = currVersionCourseGroups.SelectMany(x => x.Courses)
                                                   .Select(x => x.CourseId)
                                                   .Distinct()
                                                   .ToList();

            var ignoreCourseIds = currVersionCourseGroups.SelectMany(x => x.IgnoreCourses)
                                                         .Select(x => x.CourseId)
                                                         .Distinct()
                                                         .ToList();

            courseIds = courseIds.Concat(ignoreCourseIds).ToList();

            var courses = !courseIds.Any() ? Enumerable.Empty<CourseDTO>()
                                           : _courseProvider.GetById(courseIds)
                                                            .ToList();

            var grades = !courses.Any() ? Enumerable.Empty<GradeDTO>()
                                        : _gradeProvider.GetAll()
                                                        .ToList();

            var response = MapCourseGroupDTOToViewModel(courseGroup, currVersionCourseGroups, courses, grades);

            return response;
        }

        public IEnumerable<CurriculumCourseGroupViewModel> GetByCurriculumVersionId(Guid curriculumVersionId)
        {
            var courseGroups = _curriculumCourseGroupProvider.GetByCurriculumVersionId(curriculumVersionId)
                                                             .ToList();

            if (!courseGroups.Any())
            {
                return Enumerable.Empty<CurriculumCourseGroupViewModel>();
            }

            var courseIds = courseGroups.SelectMany(x => x.Courses)
                                        .Select(x => x.CourseId)
                                        .Distinct()
                                        .ToList();

            var ignoreCourseIds = courseGroups.SelectMany(x => x.IgnoreCourses)
                                              .Select(x => x.CourseId)
                                              .Distinct()
                                              .ToList();

            courseIds = courseIds.Concat(ignoreCourseIds).ToList();

            var courses = !courseIds.Any() ? Enumerable.Empty<CourseDTO>()
                                           : _courseProvider.GetById(courseIds)
                                                            .ToList();

            var grades = !courses.Any() ? Enumerable.Empty<GradeDTO>()
                                        : _gradeProvider.GetAll()
                                                        .ToList();

            var response = (from courseGroup in courseGroups
                            where !courseGroup.ParentCourseGroupId.HasValue
                            orderby courseGroup.Sequence, courseGroup.Name
                            select MapCourseGroupDTOToViewModel(courseGroup, courseGroups, courses, grades))
                           .ToList();

            return response;
        }

        public CurriculumCourseGroupViewModel Update(Guid id, CreateCurriculumCourseGroupViewModel request, Guid userId)
        {
            var courseGroup = _curriculumCourseGroupProvider.GetById(id);

            var currVersionCourseGroups = _curriculumCourseGroupProvider.GetByCurriculumVersionId(request.CurriculumVersionId)
                                                                        .ToList();

            if (request.ParentCourseGroupId.HasValue)
            {
                var parentGroup = currVersionCourseGroups.SingleOrDefault(x => x.Id == request.ParentCourseGroupId.Value);

                if (parentGroup is null)
                {
                    throw new CurriculumException.CourseGroupNotFound(request.ParentCourseGroupId.Value);
                }

                // FREE ELECTIVE GROUP CAN'T HAVE SUBGROUP
                if (request.Type == CourseGroupType.FREE_ELECTIVE
                   || parentGroup.Type == CourseGroupType.FREE_ELECTIVE
                   || currVersionCourseGroups.Any(x => x.ParentCourseGroupId.HasValue
                                                       && x.ParentCourseGroupId.Value == id))
                {
                    throw new CurriculumException.FreeElectiveSubGroupException();
                }
            }

            var shouldClearCourse = request.Type == CourseGroupType.FREE_ELECTIVE
                                    && courseGroup.Type != request.Type;

            courseGroup.Name = request.Name;
            courseGroup.Sequence = request.Sequence;
            courseGroup.Description = request.Description;
            courseGroup.Remark = request.Remark;
            courseGroup.ParentCourseGroupId = request.ParentCourseGroupId;
            courseGroup.Localizations = MapLocalizationViewModelToDTO(request.Localizations).ToList();

            // ENSURE NO COURSE UNDER FREE ELECTIVE GROUP TYPE
            if (shouldClearCourse)
            {
                _curriculumCourseGroupProvider.UpdateCourses(courseGroup.Id, Enumerable.Empty<CurriculumCourseDTO>());
            }

            var updatedGroup = _curriculumCourseGroupProvider.Update(courseGroup, userId.ToString());

            var courseIds = currVersionCourseGroups.SelectMany(x => x.Courses)
                                                  .Select(x => x.CourseId)
                                                  .Distinct()
                                                  .ToList();

            var courses = !courseIds.Any() ? Enumerable.Empty<CourseDTO>()
                                           : _courseProvider.GetById(courseIds)
                                                            .ToList();

            var grades = !courses.Any() ? Enumerable.Empty<GradeDTO>()
                                        : _gradeProvider.GetAll()
                                                        .ToList();

            var response = MapCourseGroupDTOToViewModel(updatedGroup, currVersionCourseGroups, courses, grades);

            return response;
        }

        public void Delete(Guid id)
        {
            _curriculumCourseGroupProvider.Delete(id);
        }

        public IEnumerable<CurriculumCourseViewModel> GetCourses(Guid courseGroupId)
        {
            var currCourses = _curriculumCourseGroupProvider.GetCourses(courseGroupId)
                                                        .ToList();

            if (!currCourses.Any())
            {
                return Enumerable.Empty<CurriculumCourseViewModel>();
            }

            var courseIds = currCourses.Select(x => x.CourseId)
                                   .ToList();

            var courses = _courseProvider.GetById(courseIds)
                                         .ToList();

            var grades = _gradeProvider.GetAll()
                                       .ToList();

            var response = (from currCourse in currCourses
                            let course = courses.SingleOrDefault(x => x.Id == currCourse.CourseId)
                            let grade = !currCourse.RequiredGradeId.HasValue ? null
                                                                             : grades.SingleOrDefault(x => x.Id == currCourse.RequiredGradeId.Value)
                            where course is not null
                            select MapCurriculumCourseViewModel(currCourse, course, grade))
                           .ToList();

            return response;
        }

        public IEnumerable<CurriculumCourseViewModel> UpdateCourses(Guid courseGroupId, IEnumerable<CreateCurriculumCourseViewModel> requests)
        {
            var courseGroup = _curriculumCourseGroupProvider.GetById(courseGroupId);

            if (courseGroup.Type == CourseGroupType.FREE_ELECTIVE)
            {
                throw new CurriculumException.FreeElectiveCourseException();
            }

            var existingCourseIds = courseGroup.Courses.Select(x => x.CourseId)
                                                       .ToList();

            var equivalentCourses = _curriculumVersionProvider.GetEquivalentCourses(courseGroup.CurriculumVersionId)
                                                              .ToList();

            var coRequisiteCourses = _curriculumVersionProvider.GetCorequisites(courseGroup.CurriculumVersionId)
                                                               .ToList();

            var courseIds = requests.Select(x => x.CourseId)
                                    .ToList();

            var removeCourseIds = existingCourseIds.Except(courseIds)
                                                   .ToList();

            var courses = _courseProvider.GetById(courseIds)
                                         .ToList();

            var grades = _gradeProvider.GetAll()
                                       .ToList();

            var versionBlackListCourses = _curriculumVersionProvider.GetBlackListCourses(courseGroup.CurriculumVersionId)
                                                                    .ToList();

            foreach (var currCourse in requests)
            {
                var matchingCourse = courses.SingleOrDefault(x => x.Id == currCourse.CourseId);
                var matchingGrade = !currCourse.RequiredGradeId.HasValue ? null
                                                                         : grades.SingleOrDefault(x => x.Id == currCourse.RequiredGradeId.Value);

                if (matchingCourse is null)
                {
                    throw new CourseException.NotFound(currCourse.CourseId);
                }

                if (currCourse.RequiredGradeId.HasValue && matchingGrade is null)
                {
                    throw new GradeException.NotFound(currCourse.RequiredGradeId.Value);
                }

                var isNewCourses = !existingCourseIds.Contains(currCourse.CourseId);

                if (isNewCourses)
                {
                    var isBlackListCourse = versionBlackListCourses.Any(x => x.CourseId == currCourse.CourseId);

                    if (isBlackListCourse)
                    {
                        throw new CurriculumException.InBlackListCourseException(matchingCourse.Id);
                    }
                }
            }

            foreach (var courseId in removeCourseIds)
            {
                var isInCoRequisite = coRequisiteCourses.Any(x => x.CourseId == courseId || x.CorequisiteCourseId == courseId);

                if (isInCoRequisite)
                {
                    throw new CurriculumException.InCurriculumCoRequisiteException(courseId);
                }

                var isInEquivalentCourse = equivalentCourses.Any(x => x.CourseId == courseId);

                if (isInEquivalentCourse)
                {
                    throw new CurriculumException.InCurriculumCourseEquiException(courseId);
                }
            }

            var dtos = (from request in requests
                        select new CurriculumCourseDTO
                        {
                            CourseGroupId = courseGroupId,
                            CourseId = request.CourseId,
                            RequiredGradeId = request.RequiredGradeId
                        })
                       .ToList();

            _curriculumCourseGroupProvider.UpdateCourses(courseGroupId, dtos);

            var response = (from currCourse in dtos
                            let course = courses.Single(x => x.Id == currCourse.CourseId)
                            let grade = !currCourse.RequiredGradeId.HasValue ? null
                                                                             : grades.SingleOrDefault(x => x.Id == currCourse.RequiredGradeId.Value)
                            where course is not null
                            orderby course.Code
                            select MapCurriculumCourseViewModel(currCourse, course, grade))
                           .ToList();

            return response;
        }

        public IEnumerable<CurriculumCourseGroupIgnoreCourseViewModel> GetIgnoreCourses(Guid courseGroupId)
        {
            var ignoreCourses = _curriculumCourseGroupProvider.GetIgnoreCourses(courseGroupId)
                                                              .ToList();

            if (!ignoreCourses.Any())
            {
                return Enumerable.Empty<CurriculumCourseGroupIgnoreCourseViewModel>();
            }

            var courseIds = ignoreCourses.Select(x => x.CourseId)
                                         .Distinct()
                                         .ToList();

            var courses = _courseProvider.GetById(courseIds)
                                         .ToList();

            var response = (from ignoreCourse in ignoreCourses
                            join course in courses on ignoreCourse.CourseId equals course.Id
                            select MapCurriculumIgnoreCourseViewModel(course))
                           .ToList();

            return response;
        }

        public IEnumerable<CurriculumCourseGroupIgnoreCourseViewModel> UpdateIgnoreCourses(Guid courseGroupId, IEnumerable<Guid> courseIds)
        {
            var courseGroup = _curriculumCourseGroupProvider.GetById(courseGroupId);

            if (courseGroup.Type != CourseGroupType.FREE_ELECTIVE)
            {
                throw new CurriculumException.NonElectiveIgnoreCourseException();
            }

            var courses = _courseProvider.GetById(courseIds)
                                         .ToList();

            foreach (var courseId in courseIds)
            {
                var matchingCourses = courses.SingleOrDefault(x => x.Id == courseId);

                if (matchingCourses is null)
                {
                    throw new CourseException.NotFound(courseId);
                }
            }

            _curriculumCourseGroupProvider.UpdateIgnoreCourses(courseGroupId, courseIds);

            var response = (from courseId in courseIds
                            join course in courses on courseId equals course.Id
                            orderby course.Code
                            select MapCurriculumIgnoreCourseViewModel(course))
                           .ToList();

            return response;
        }

        private static CurriculumCourseGroupViewModel MapCourseGroupDTOToViewModel(CurriculumCourseGroupDTO baseGroup,
                                                                                   IEnumerable<CurriculumCourseGroupDTO> currVersionCourseGroups = null,
                                                                                   IEnumerable<CourseDTO> courses = null,
                                                                                   IEnumerable<GradeDTO> grades = null)
        {
            var response = new CurriculumCourseGroupViewModel
            {
                Id = baseGroup.Id,
                RequiredCredit = baseGroup.RequiredCredit,
                Type = baseGroup.Type,
                Sequence = baseGroup.Sequence,
                Remark = baseGroup.Remark,
                CurriculumVersionId = baseGroup.CurriculumVersionId,
                ParentCourseGroupId = baseGroup.ParentCourseGroupId,
                Localizations = (from locale in baseGroup.Localizations
                                 orderby locale.Language
                                 select new CurriculumCourseGroupLocalizationViewModel
                                 {
                                     Language = locale.Language,
                                     Name = locale.Name,
                                     Description = locale.Description
                                 })
                                .ToList(),
                SubGroups = currVersionCourseGroups is null
                                ? Enumerable.Empty<CurriculumCourseGroupViewModel>()
                                : (from courseGroup in currVersionCourseGroups
                                   where courseGroup.ParentCourseGroupId.HasValue
                                          && courseGroup.ParentCourseGroupId.Value == baseGroup.Id
                                   orderby courseGroup.Sequence, courseGroup.Name
                                   select MapCourseGroupDTOToViewModel(courseGroup, currVersionCourseGroups, courses, grades))
                                  .ToList(),
                Courses = baseGroup.Courses is null || courses is null
                                ? Enumerable.Empty<CurriculumCourseViewModel>()
                                : (from curriculumCourse in baseGroup.Courses
                                   let course = courses.SingleOrDefault(x => x.Id == curriculumCourse.CourseId)
                                   let grade = !curriculumCourse.RequiredGradeId.HasValue ? null
                                                                                          : grades.SingleOrDefault(x => x.Id == curriculumCourse.RequiredGradeId.Value)
                                   where course is not null && curriculumCourse.CourseGroupId == baseGroup.Id
                                   orderby course.Code
                                   select MapCurriculumCourseViewModel(curriculumCourse, course, grade))
                                  .ToList(),
                IgnoreCourses = baseGroup.IgnoreCourses is null || courses is null
                                ? Enumerable.Empty<CurriculumCourseGroupIgnoreCourseViewModel>()
                                : (from ignoreCourse in baseGroup.IgnoreCourses
                                   join course in courses on ignoreCourse.CourseId equals course.Id
                                   orderby course.Code
                                   select MapCurriculumIgnoreCourseViewModel(course))
                                  .ToList(),
                CreatedAt = baseGroup.CreatedAt,
                UpdatedAt = baseGroup.UpdatedAt,
            };

            return response;
        }

        private static CurriculumCourseGroupIgnoreCourseViewModel MapCurriculumIgnoreCourseViewModel(CourseDTO course)
        {
            var response = new CurriculumCourseGroupIgnoreCourseViewModel
            {
                CourseId = course.Id,
                CourseCode = course.Code,
                CourseName = (from data in course.Localizations
                              orderby data.Language
                              select new CourseLocalizationDTO
                              {
                                  Language = data.Language,
                                  Name = data.Name
                              }).SingleOrDefault()?.Name,
                Credit = course.Credit
            };

            return response;
        }

        private static CurriculumCourseViewModel MapCurriculumCourseViewModel(CurriculumCourseDTO dto, CourseDTO course, GradeDTO grade)
        {
            var response = new CurriculumCourseViewModel
            {
                CourseId = dto.CourseId,
                CourseCode = course.Code,
                CourseName = (from data in course.Localizations
                              orderby data.Language
                              select new CourseLocalizationDTO
                              {
                                  Language = data.Language,
                                  Name = data.Name
                              }).SingleOrDefault()?.Name,
                RequiredGradeId = dto.RequiredGradeId,
                RequiredGradeLetter = !dto.RequiredGradeId.HasValue ? null
                                                            : grade.Letter
            };

            return response;
        }

        private static IEnumerable<CurriculumCourseGroupLocalizationDTO> MapLocalizationViewModelToDTO(
                    IEnumerable<CurriculumCourseGroupLocalizationViewModel>? localizations)
        {
            if (localizations is null)
            {
                return Enumerable.Empty<CurriculumCourseGroupLocalizationDTO>();
            }

            var response = (from locale in localizations
                            select new CurriculumCourseGroupLocalizationDTO
                            {
                                Language = locale.Language,
                                Name = locale.Name,
                                Description = locale.Description
                            })
                           .ToList();

            return response;
        }
    }
}

