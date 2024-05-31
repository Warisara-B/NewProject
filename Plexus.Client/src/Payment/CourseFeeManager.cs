using Plexus.Client.ViewModel.Payment;
using Plexus.Entity.DTO.Academic;
using Plexus.Entity.DTO.Academic.Curriculum;
using Plexus.Entity.DTO.Payment;
using Plexus.Entity.Provider;

namespace Plexus.Client.src.Payment
{
    public class CourseFeeManager : ICourseFeeManager
    {
        private readonly ICourseFeeProvider _courseFeeProvider;
        private readonly IAcademicLevelProvider _academicLevelProvider;
        private readonly IFacultyProvider _facultyProvider;
        private readonly IDepartmentProvider _departmentProvider;
        private readonly ICurriculumProvider _curriculumProvider;
        private readonly ICurriculumVersionProvider _curriculumVersionProvider;
        private readonly ICourseProvider _courseProvider;
        private readonly IFeeItemProvider _feeItemProvider;
        private readonly IRateTypeProvider _rateTypeProvider;
        private readonly IStudentFeeTypeProvider _studentFeeTypeProvider;

        public CourseFeeManager(ICourseFeeProvider courseFeeProvider,
                                IAcademicLevelProvider academicLevelProvider,
                                IFacultyProvider facultyProvider,
                                IDepartmentProvider departmentProvider,
                                ICurriculumProvider curriculumProvider,
                                ICurriculumVersionProvider curriculumVersionProvider,
                                IFeeItemProvider feeItemProvider,
                                ICourseProvider courseProvider,
                                IRateTypeProvider rateTypeProvider,
                                IStudentFeeTypeProvider studentFeeTypeProvider)
        {
            _courseFeeProvider = courseFeeProvider;
            _academicLevelProvider = academicLevelProvider;
            _facultyProvider = facultyProvider;
            _departmentProvider = departmentProvider;
            _curriculumProvider = curriculumProvider;
            _curriculumVersionProvider = curriculumVersionProvider;
            _feeItemProvider = feeItemProvider;
            _courseProvider = courseProvider;
            _rateTypeProvider = rateTypeProvider;
            _studentFeeTypeProvider = studentFeeTypeProvider;
        }

        public CourseFeeViewModel Create(Guid id, CreateCourseFeeViewModel request, Guid userId)
        {
            var course = _courseProvider.GetById(id);

            var feeItem = _feeItemProvider.GetById(request.FeeItemId);

            var rateType = _rateTypeProvider.GetById(request.RateTypeId);

            var condition = request.Condition is null ? null
                                                      : new CourseFeeConditionDTO
                                                      {
                                                          SectionNumber = request.Condition.SectionNumber,
                                                          FromBatch = request.Condition.FromBatch,
                                                          ToBatch = request.Condition.ToBatch,
                                                          AcademicLevelId = request.Condition.AcademicLevelId,
                                                          FacultyId = request.Condition.FacultyId,
                                                          DepartmentId = request.Condition.DepartmentId,
                                                          CurriculumId = request.Condition.CurriculumId,
                                                          CurriculumVersionId = request.Condition.CurriculumVersionId,
                                                          StudentFeeTypeId = request.Condition.StudentFeeTypeId
                                                      };

            var academicLevel = condition is not null && condition.AcademicLevelId.HasValue ? _academicLevelProvider.GetById(condition.AcademicLevelId.Value)
                                                                                            : null;

            var faculty = condition is not null && condition.FacultyId.HasValue ? _facultyProvider.GetById(condition.FacultyId.Value)
                                                                                : null;

            var department = condition is not null && condition.DepartmentId.HasValue ? _departmentProvider.GetById(condition.DepartmentId.Value)
                                                                                      : null;

            var curriculum = condition is not null && condition.CurriculumId.HasValue ? _curriculumProvider.GetById(condition.CurriculumId.Value)
                                                                                      : null;

            var curriculumVersion = condition is not null && condition.CurriculumVersionId.HasValue ? _curriculumVersionProvider.GetById(condition.CurriculumVersionId.Value)
                                                                                                    : null;

            var studentFeeType = condition is not null && condition.StudentFeeTypeId.HasValue ? _studentFeeTypeProvider.GetById(condition.StudentFeeTypeId.Value)
                                                                                              : null;

            var dto = new CreateCourseFeeDTO
            {
                CourseId = id,
                FeeItemId = request.FeeItemId,
                CalculationType = request.CalculationType,
                Condition = condition,
                RateTypeId = request.RateTypeId,
                RateIndex = request.RateIndex
            };

            var courseFee = _courseFeeProvider.Create(dto, userId.ToString());

            var response = MapDTOToViewModel(courseFee, course, feeItem, rateType, academicLevel, faculty, department, curriculum, curriculumVersion, studentFeeType);

            return response;
        }

        public IEnumerable<CourseFeeViewModel> GetByCourseId(Guid courseId)
        {
            var course = _courseProvider.GetById(courseId);

            var courseFees = _courseFeeProvider.GetByCourseId(courseId);

            var feeTypeIds = courseFees.Select(x => x.FeeItemId)
                                       .Distinct()
                                       .ToList();

            var feeItems = _feeItemProvider.GetById(feeTypeIds)
                                           .ToList();

            var rateTypeIds = courseFees.Select(x => x.RateTypeId)
                                        .Distinct()
                                        .ToList();

            var rateTypes = _rateTypeProvider.GetById(rateTypeIds)
                                             .ToList();

            var conditions = courseFees.Where(x => x.Condition is not null)
                                       .Select(x => x.Condition)
                                       .ToList();

            var academicLevelIds = conditions.Where(x => x!.AcademicLevelId.HasValue)
                                             .Select(x => x!.AcademicLevelId!.Value)
                                             .Distinct()
                                             .ToList();

            var academicLevels = _academicLevelProvider.GetById(academicLevelIds)
                                                       .ToList();

            var facultyIds = conditions.Where(x => x!.FacultyId.HasValue)
                                       .Select(x => x!.FacultyId!.Value)
                                       .Distinct()
                                       .ToList();

            var faculties = _facultyProvider.GetById(facultyIds)
                                            .ToList();

            var departmentIds = conditions.Where(x => x!.DepartmentId.HasValue)
                                          .Select(x => x!.DepartmentId!.Value)
                                          .Distinct()
                                          .ToList();

            var departments = _departmentProvider.GetById(departmentIds)
                                                 .ToList();

            var curriculumIds = conditions.Where(x => x!.CurriculumId.HasValue)
                                          .Select(x => x!.CurriculumId!.Value)
                                          .Distinct()
                                          .ToList();

            var curriculums = _curriculumProvider.GetById(curriculumIds)
                                                 .ToList();

            var curriculumVersionIds = conditions.Where(x => x!.CurriculumVersionId.HasValue)
                                                 .Select(x => x!.CurriculumVersionId!.Value)
                                                 .Distinct()
                                                 .ToList();

            var curriculumVersions = _curriculumVersionProvider.GetById(curriculumVersionIds)
                                                               .ToList();

            var studentFeeTypeIds = conditions.Where(x => x!.StudentFeeTypeId.HasValue)
                                              .Select(x => x!.StudentFeeTypeId!.Value)
                                              .Distinct()
                                              .ToList();

            var studentFeeTypes = _studentFeeTypeProvider.GetById(studentFeeTypeIds)
                                                         .ToList();

            var response = (from courseFee in courseFees
                            let condition = courseFee.Condition
                            let feeItem = feeItems.Single(x => x.Id == courseFee.FeeItemId)
                            let rateType = rateTypes.Single(x => x.Id == courseFee.RateTypeId)
                            let academicLevel = condition is not null && condition.AcademicLevelId.HasValue ? academicLevels.SingleOrDefault(x => x.Id == condition.AcademicLevelId!.Value)
                                                                                                            : null
                            let faculty = condition is not null && condition.FacultyId.HasValue ? faculties.SingleOrDefault(x => x.Id == condition.FacultyId!.Value)
                                                                                                : null
                            let department = condition is not null && condition.DepartmentId.HasValue ? departments.SingleOrDefault(x => x.Id == condition.DepartmentId!.Value)
                                                                                                      : null
                            let curriculum = condition is not null && condition.CurriculumId.HasValue ? curriculums.SingleOrDefault(x => x.Id == condition.CurriculumId!.Value)
                                                                                                      : null
                            let curriculumVersion = condition is not null && condition.CurriculumVersionId.HasValue ? curriculumVersions.SingleOrDefault(x => x.Id == condition.CurriculumVersionId!.Value)
                                                                                                                    : null
                            let studentFeeType = condition is not null && condition.StudentFeeTypeId.HasValue ? studentFeeTypes.SingleOrDefault(x => x.Id == condition.StudentFeeTypeId!.Value)
                                                                                                              : null
                            select MapDTOToViewModel(courseFee, course, feeItem, rateType, academicLevel, faculty, department, curriculum, curriculumVersion, studentFeeType))
                           .ToList();

            return response;
        }

        public CourseFeeViewModel GetById(Guid id)
        {
            var courseFee = _courseFeeProvider.GetById(id);

            var course = _courseProvider.GetById(courseFee.CourseId);

            var feeItem = _feeItemProvider.GetById(courseFee.FeeItemId);

            var rateType = _rateTypeProvider.GetById(courseFee.RateTypeId);

            var condition = courseFee.Condition;

            var academicLevel = condition is not null && condition.AcademicLevelId.HasValue ? _academicLevelProvider.GetById(condition.AcademicLevelId.Value)
                                                                                            : null;

            var faculty = condition is not null && condition.FacultyId.HasValue ? _facultyProvider.GetById(condition.FacultyId.Value)
                                                                                : null;

            var department = condition is not null && condition.DepartmentId.HasValue ? _departmentProvider.GetById(condition.DepartmentId.Value)
                                                                                      : null;

            var curriculum = condition is not null && condition.CurriculumId.HasValue ? _curriculumProvider.GetById(condition.CurriculumId.Value)
                                                                                      : null;

            var curriculumVersion = condition is not null && condition.CurriculumVersionId.HasValue ? _curriculumVersionProvider.GetById(condition.CurriculumVersionId.Value)
                                                                                                    : null;

            var studentFeeType = condition is not null && condition.StudentFeeTypeId.HasValue ? _studentFeeTypeProvider.GetById(condition.StudentFeeTypeId.Value)
                                                                                              : null;

            var response = MapDTOToViewModel(courseFee, course, feeItem, rateType, academicLevel, faculty, department, curriculum, curriculumVersion, studentFeeType);

            return response;
        }

        public CourseFeeViewModel Update(Guid id, Guid courseFeeId, CreateCourseFeeViewModel request, Guid userId)
        {
            var courseFee = _courseFeeProvider.GetById(courseFeeId);

            var course = _courseProvider.GetById(id);

            var feeItem = _feeItemProvider.GetById(request.FeeItemId);

            var rateType = _rateTypeProvider.GetById(request.RateTypeId);

            var condition = request.Condition is null ? null
                                                      : new CourseFeeConditionDTO
                                                      {
                                                          SectionNumber = request.Condition.SectionNumber,
                                                          FromBatch = request.Condition.FromBatch,
                                                          ToBatch = request.Condition.ToBatch,
                                                          AcademicLevelId = request.Condition.AcademicLevelId,
                                                          FacultyId = request.Condition.FacultyId,
                                                          DepartmentId = request.Condition.DepartmentId,
                                                          CurriculumId = request.Condition.CurriculumId,
                                                          CurriculumVersionId = request.Condition.CurriculumVersionId,
                                                          StudentFeeTypeId = request.Condition.StudentFeeTypeId
                                                      };

            var academicLevel = condition is not null && condition.AcademicLevelId.HasValue ? _academicLevelProvider.GetById(condition.AcademicLevelId.Value)
                                                                                            : null;

            var faculty = condition is not null && condition.FacultyId.HasValue ? _facultyProvider.GetById(condition.FacultyId.Value)
                                                                                : null;

            var department = condition is not null && condition.DepartmentId.HasValue ? _departmentProvider.GetById(condition.DepartmentId.Value)
                                                                                      : null;

            var curriculum = condition is not null && condition.CurriculumId.HasValue ? _curriculumProvider.GetById(condition.CurriculumId.Value)
                                                                                      : null;

            var curriculumVersion = condition is not null && condition.CurriculumVersionId.HasValue ? _curriculumVersionProvider.GetById(condition.CurriculumVersionId.Value)
                                                                                                    : null;

            var studentFeeType = condition is not null && condition.StudentFeeTypeId.HasValue ? _studentFeeTypeProvider.GetById(condition.StudentFeeTypeId.Value)
                                                                                              : null;

            courseFee.FeeItemId = request.FeeItemId;
            courseFee.CalculationType = request.CalculationType;
            courseFee.Condition = condition;
            courseFee.RateTypeId = request.RateTypeId;
            courseFee.RateIndex = request.RateIndex;

            var updatedCourseFee = _courseFeeProvider.Update(courseFee, userId.ToString());

            var response = MapDTOToViewModel(updatedCourseFee, course, feeItem, rateType, academicLevel, faculty, department, curriculum, curriculumVersion, studentFeeType);

            return response;
        }

        public void Delete(Guid id)
        {
            _courseFeeProvider.Delete(id);
        }

        private CourseFeeViewModel MapDTOToViewModel(CourseFeeDTO dto,
                                                     CourseDTO course,
                                                     FeeItemDTO feeItem,
                                                     RateTypeDTO rateType,
                                                     AcademicLevelDTO? academicLevel = null,
                                                     FacultyDTO? faculty = null,
                                                     DepartmentDTO? department = null,
                                                     CurriculumDTO? curriculum = null,
                                                     CurriculumVersionDTO? curriculumVersion = null,
                                                     StudentFeeTypeDTO? studentFeeType = null)
        {
            var condition = dto.Condition is null ? null
                                                  : new CourseFeeConditionViewModel
                                                  {
                                                      SectionNumber = dto.Condition.SectionNumber,
                                                      FromBatch = dto.Condition.FromBatch,
                                                      ToBatch = dto.Condition.ToBatch,
                                                      AcademicLevelId = dto.Condition.AcademicLevelId,
                                                      FacultyId = dto.Condition.FacultyId,
                                                      DepartmentId = dto.Condition.DepartmentId,
                                                      CurriculumId = dto.Condition.CurriculumId,
                                                      CurriculumVersionId = dto.Condition.CurriculumVersionId,
                                                      StudentFeeTypeId = dto.Condition.StudentFeeTypeId,
                                                      AcademicLevelName = academicLevel?.Name,
                                                      FacultyName = faculty?.Name,
                                                      DepartmentName = department?.Name,
                                                      CurriculumName = curriculum?.Name,
                                                      CurriculumVersionName = curriculumVersion?.Name,
                                                      StudentFeeTypeName = studentFeeType?.Name
                                                  };

            var response = new CourseFeeViewModel
            {
                Id = dto.Id,
                CourseId = dto.CourseId,
                FeeItemId = dto.FeeItemId,
                CalculationType = dto.CalculationType,
                Condition = condition,
                RateTypeId = dto.RateTypeId,
                RateTypeName = rateType.Name,
                RateIndex = dto.RateIndex,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt,
                CourseName = course.Name,
                FeeItemName = feeItem.Name
            };

            return response;
        }
    }
}