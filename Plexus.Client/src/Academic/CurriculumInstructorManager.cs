using Microsoft.EntityFrameworkCore;
using Plexus.Client.ViewModel.Academic.Curriculum;
using Plexus.Database;
using Plexus.Database.Model;
using Plexus.Database.Model.Academic;
using Plexus.Database.Model.Academic.Curriculum;
using Plexus.Entity.Exception;
using Plexus.Utility.Extensions;
using Plexus.Utility.ViewModel;
using ServiceStack;

namespace Plexus.Client.src.Academic
{
    public class CurriculumInstructorManager : ICurriculumInstructorManager
    {
        private readonly IUnitOfWork _uow;
        private readonly IAsyncRepository<AcademicPosition> _academicPositionRepository;
        private readonly IAsyncRepository<CareerPosition> _careerPositionRepository;
        private readonly IAsyncRepository<CurriculumVersion> _curriculumVersionRepository;
        private readonly IAsyncRepository<CurriculumInstructor> _curriculumInstructorRepository;
        private readonly IAsyncRepository<Employee> _employeesRepository;
        private readonly IAsyncRepository<InstructorRole> _instructorRoleRepository;

        public CurriculumInstructorManager(IUnitOfWork uow,
                                           IAsyncRepository<AcademicPosition> academicPositionRepository,
                                           IAsyncRepository<CareerPosition> careerPositionRepository,
                                           IAsyncRepository<CurriculumVersion> curriculumVersionRepository,
                                           IAsyncRepository<CurriculumInstructor> curriculumInstructorRepository,
                                           IAsyncRepository<Employee> employeeRepository,
                                           IAsyncRepository<InstructorRole> instructorRoleRepository)
        {
            _uow = uow;
            _academicPositionRepository = academicPositionRepository;
            _careerPositionRepository = careerPositionRepository;
            _curriculumVersionRepository = curriculumVersionRepository;
            _curriculumInstructorRepository = curriculumInstructorRepository;
            _employeesRepository = employeeRepository;
            _instructorRoleRepository = instructorRoleRepository;
        }

        public CurriculumInstructorViewModel Create(Guid curriculumVersionId, CreateCurriculumInstructorViewModel request)
        {
            var instructor = _employeesRepository.Query()
                                                 .Include(x => x.Localizations)
                                                 .FirstOrDefault(x => x.Id == request.InstructorId);

            if (instructor is null)
            {
                throw new EmployeeException.NotFound(request.InstructorId);
            }

            AcademicPosition? academicPosition = null;
            CareerPosition? careerPosition = null;

            if (instructor.AcademicPositionId.HasValue)
            {
                academicPosition = _academicPositionRepository.Query()
                                                              .Include(x => x.Localizations)
                                                              .FirstOrDefault(x => x.Id == instructor.AcademicPositionId);
            }

            if (instructor.CareerPositionId.HasValue)
            {
                careerPosition = _careerPositionRepository.Query()
                                                          .Include(x => x.Localizations)
                                                          .FirstOrDefault(x => x.Id == instructor.CareerPositionId);
            }

            var instructorRole = _instructorRoleRepository.Query()
                                                          .Include(x => x.Localizations)
                                                          .FirstOrDefault(x => x.Id == request.InstructorRoleId);

            if (instructorRole is null)
            {
                throw new InstructorRoleException.NotFound(request.InstructorRoleId);
            }

            var model = new CurriculumInstructor
            {
                CurriculumVersionId = curriculumVersionId,
                InstructorId = request.InstructorId,
                InstructorRoleId = request.InstructorRoleId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "", // TODO : Add requester.
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = ""
            };

            _uow.BeginTran();
            _curriculumInstructorRepository.Add(model);
            _uow.Complete();
            _uow.CommitTran();

            var response = MapModelToViewModel(model, instructorRole, instructor, academicPosition, careerPosition);

            return response;
        }

        public PagedViewModel<CurriculumInstructorViewModel> GetList(Guid curriculumVersionId, int page, int pageSize)
        {
            var query = _curriculumInstructorRepository.Query()
                                                       .Where(x => x.CurriculumVersionId == curriculumVersionId);

            var pagedCurriculumInstructors = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<CurriculumInstructorViewModel>
            {
                Page = pagedCurriculumInstructors.Page,
                PageSize = pagedCurriculumInstructors.PageSize,
                TotalPage = pagedCurriculumInstructors.TotalPage,
                TotalItem = pagedCurriculumInstructors.TotalItem,
                Items = (from curriculumInstructor in pagedCurriculumInstructors.Items
                         let role = _instructorRoleRepository.Query()
                                                             .Include(x => x.Localizations)
                                                             .FirstOrDefault(x => x.Id == curriculumInstructor.InstructorRoleId)
                         let instructor = _employeesRepository.Query()
                                                              .Include(x => x.Localizations)
                                                              .FirstOrDefault(x => x.Id == curriculumInstructor.InstructorId)
                         let academicPosition = _academicPositionRepository.Query()
                                                                           .Include(x => x.Localizations)
                                                                           .FirstOrDefault(x => x.Id == instructor.AcademicPositionId)
                         let careerPosition = _careerPositionRepository.Query()
                                                                       .Include(x => x.Localizations)
                                                                       .FirstOrDefault(x => x.Id == instructor.CareerPositionId)
                         select MapModelToViewModel(curriculumInstructor, role, instructor, academicPosition, careerPosition))
                        .ToList()
            };

            return response;
        }

        public CurriculumInstructorViewModel Update(Guid id, CreateCurriculumInstructorViewModel request)
        {
            var curriculumInstructor = _curriculumInstructorRepository.Query()
                                                                      .FirstOrDefault(x => x.Id == id);

            if (curriculumInstructor is null)
            {
                throw new CurriculumVersionException.InstructorNotFound(id);
            }

            var instructor = _employeesRepository.Query()
                                                 .Include(x => x.Localizations)
                                                 .FirstOrDefault(x => x.Id == request.InstructorId);

            if (instructor is null)
            {
                throw new EmployeeException.NotFound(request.InstructorId);
            }

            var role = _instructorRoleRepository.Query()
                                                .Include(x => x.Localizations)
                                                .FirstOrDefault(x => x.Id == request.InstructorRoleId);

            if (role is null)
            {
                throw new InstructorRoleException.NotFound(request.InstructorRoleId);
            }

            var academicPosition = _academicPositionRepository.Query()
                                                              .Include(x => x.Localizations)
                                                              .FirstOrDefault(x => x.Id == instructor.AcademicPositionId);

            var careerPosition = _careerPositionRepository.Query()
                                                          .Include(x => x.Localizations)
                                                          .FirstOrDefault(x => x.Id == instructor.CareerPositionId);

            curriculumInstructor.InstructorId = request.InstructorId;
            curriculumInstructor.InstructorRoleId = request.InstructorRoleId;

            _uow.BeginTran();
            _curriculumInstructorRepository.Update(curriculumInstructor);
            _uow.Complete();
            _uow.CommitTran();

            var response = MapModelToViewModel(curriculumInstructor, role, instructor, academicPosition, careerPosition);

            return response;
        }

        public void Delete(Guid id)
        {
            var curriculumInstructor = _curriculumInstructorRepository.Query()
                                                                      .FirstOrDefault(x => x.Id == id);

            if (curriculumInstructor is null)
            {
                return;
            }

            _uow.BeginTran();
            _curriculumInstructorRepository.Delete(curriculumInstructor);
            _uow.Complete();
            _uow.CommitTran();
        }

        private CurriculumInstructorViewModel MapModelToViewModel(CurriculumInstructor curriculumInstructor,
                                                                  InstructorRole role, Employee instructor,
                                                                  AcademicPosition? academicPosition,
                                                                  CareerPosition? careerPosition)
        {
            var response = new CurriculumInstructorViewModel
            {
                Id = curriculumInstructor.Id,
                InstructorRoleId = curriculumInstructor.InstructorRoleId,
                InstructorRoleName = role.Name,
                InstructorId = curriculumInstructor.InstructorId,
                InstructorFirstName = instructor.FirstName,
                InstructorMiddleName = instructor.MiddleName,
                InstructorLastName = instructor.LastName,
                AcademicPositionName = academicPosition is null ? null
                                                                : academicPosition.Name,
                CareerPositionName = careerPosition is null ? null
                                                            : careerPosition.Name,
                CreatedAt = curriculumInstructor.CreatedAt,
                UpdatedAt = curriculumInstructor.UpdatedAt
            };

            return response;
        }
    }
}