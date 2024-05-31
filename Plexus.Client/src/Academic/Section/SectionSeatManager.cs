using Newtonsoft.Json;
using Plexus.Client.ViewModel.Academic.Section;
using Plexus.Database;
using Plexus.Database.Model.Academic;
using Plexus.Entity.DTO.Academic;
using Plexus.Entity.DTO.Academic.Curriculum;
using Plexus.Entity.DTO.Academic.Section;
using Plexus.Utility.Extensions;

namespace Plexus.Client.src.Academic.Section
{
    public class SectionSeatManager : ISectionSeatManager
    {
        private readonly IAsyncRepository<SectionSeat> _sectionSeatRepository;

        public SectionSeatManager(IAsyncRepository<SectionSeat> sectionSeatRepository)
        {
            _sectionSeatRepository = sectionSeatRepository;
        }

        public IEnumerable<SectionSeatViewModel> GetBySectionId(IEnumerable<Guid> sectionIds)
        {
            var sectionSeats = _sectionSeatRepository.Query()
                                                     .Where(x => sectionIds.Contains(x.SectionId))
                                                     .ToList();

            var response = (from seat in sectionSeats
                            orderby seat.Name
                            select MapModelToViewModel(seat))
                           .ToList();

            return response;
        }

        public static SectionSeatViewModel MapModelToViewModel(SectionSeat model)
        {
            var response = new SectionSeatViewModel
            {
                Id = model.Id,
                SectionId = model.SectionId,
                Name = model.Name,
                Type = model.Type,
                MasterSeatId = model.MasterSeatId,
                Remark = model.Remark,
                Conditions = string.IsNullOrEmpty(model.Conditions) ? Enumerable.Empty<SectionConditionViewModel>()
                                                                    : JsonConvert.DeserializeObject<IEnumerable<SectionConditionViewModel>>(model.Conditions),
                TotalSeat = model.TotalSeat,
                SeatUsed = model.SeatUsed,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt
            };

            return response;
        }

        public static UpsertSectionSeatDTO MapViewModelToDTO(UpsertSectionSeatViewModel request)
        {
            var dto = new UpsertSectionSeatDTO
            {
                Id = request.Id,
                Name = request.Name,
                TotalSeat = request.TotalSeat,
                Type = request.Type,
                Remark = request.Remark,
                Conditions = (from condition in request.Conditions
                              select new SectionConditionDTO
                              {
                                  CurriculumId = condition.CurriculumId,
                                  CurriculumVersionId = condition.CurriculumVersionId,
                                  FacultyId = condition.FacultyId,
                                  DepartmentId = condition.DepartmentId,
                                  Batches = condition.Batches,
                                  Codes = condition.Codes.SplitWithCommaSeparator()
                              })
                             .ToList()
            };

            return dto;
        }

        public static SectionSeatViewModel MapDTOToViewModel(
            SectionSeatDTO dto,
            IEnumerable<CurriculumDTO> curriculums,
            IEnumerable<CurriculumVersionDTO> versions,
            IEnumerable<FacultyDTO> faculties,
            IEnumerable<DepartmentDTO> departments)
        {
            var response = new SectionSeatViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Type = dto.Type,
                Conditions = (
                                from condition in dto.Conditions
                                let curriculum = !condition.CurriculumId.HasValue ? null
                                                                                  : curriculums.SingleOrDefault(x => x.Id == condition.CurriculumId.Value)
                                let version = !condition.CurriculumVersionId.HasValue ? null
                                                                                      : versions.SingleOrDefault(x => x.Id == condition.CurriculumVersionId.Value)
                                let faculty = !condition.FacultyId.HasValue ? null
                                                                            : faculties.SingleOrDefault(x => x.Id == condition.FacultyId.Value)
                                let department = !condition.DepartmentId.HasValue ? null
                                                                                  : departments.SingleOrDefault(x => x.Id == condition.DepartmentId.Value)
                                select new SectionConditionViewModel
                                {
                                    CurriculumId = condition.CurriculumId,
                                    CurriculumName = curriculum?.Name,
                                    CurriculumVersionId = condition.CurriculumVersionId,
                                    CurriculumVersionName = version?.Name,
                                    FacultyId = condition.FacultyId,
                                    FacultyName = faculty?.Name,
                                    DepartmentId = condition.DepartmentId,
                                    DepartmentName = department?.Name,
                                    Batches = condition.Batches,
                                    Codes = condition.Codes.ToStringWithCommaSeparator()
                                })
                                .ToList(),
                TotalSeat = dto.TotalSeat,
                Remark = dto.Remark,
                UpdatedAt = dto.UpdatedAt
            };

            return response;
        }
    }
}