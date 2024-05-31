using Plexus.Database.Enum;
using Plexus.Database.Model.Academic;
using Plexus.Database.Model.Academic.Section;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Academic.Curriculum;
using Plexus.Entity.DTO.Academic.Section;
using SectionModel = Plexus.Database.Model.Academic.Section.Section;

namespace Plexus.Entity.Provider
{
    public interface ISectionSeatProvider
    {
        /// <summary>
        /// Get section seat by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        SectionSeatDTO GetById(Guid id);

        /// <summary>
        /// Get section seat by ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        IEnumerable<SectionSeatDTO> GetById(IEnumerable<Guid> ids);

        /// <summary>
        /// Get section seat by section ids.
        /// </summary>
        /// <param name="sectionIds"></param>
        /// <returns></returns>
        IEnumerable<SectionSeatDTO> GetBySectionId(IEnumerable<Guid> sectionIds);
        
        /// <summary>
        /// Map create section seats dto to model.
        /// </summary>
        /// <param name="section"></param>
        /// <param name="requests"></param>
        /// <param name="defaultSeats"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        (IEnumerable<SectionSeat>, List<SectionSeatUsage>) MapCreateDTOToModel(SectionModel section, IEnumerable<CreateSectionSeatDTO> requests, IEnumerable<SectionSeat> defaultSeats, string requester);

        /// <summary>
        /// Map upsert section seats dto to create dto and update dto.
        /// </summary>
        /// <param name="requests"></param>
        /// <param name="remaingingSeat"></param>
        /// <returns></returns>
        (List<CreateSectionSeatDTO>, List<SectionSeatDTO>) Upsert(IEnumerable<UpsertSectionSeatDTO>? requests, int remaingingSeat);

        /// <summary>
        /// Map update section seats dto to model.
        /// </summary>
        /// <param name="sectionId"></param>
        /// <param name="requests"></param>
        /// <param name="defaultSeats"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        (IEnumerable<SectionSeat>, List<SectionSeatUsage>) MapUpdateDTOToModel(Guid sectionId, IEnumerable<SectionSeatDTO> requests, IEnumerable<SectionSeat> defaultSeats, string requester);

        /// <summary>
        /// Map deduct seat usage new study course.
        /// </summary>
        /// <param name="studyCourse"></param>
        /// <param name="defaultSeats"></param>
        /// <param name="sectionSeatId"></param>
        /// <param name="amount"></param>
        /// <param name="channel"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        IEnumerable<SectionSeatUsage> CutSeat(StudyCourse studyCourse, IEnumerable<SectionSeat> defaultSeats, Guid sectionSeatId, int amount, RegistrationChannel channel, string requester);

        /// <summary>
        /// Map reverse usage from drop study course.
        /// </summary>
        /// <param name="studyCourseId"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        IEnumerable<SectionSeatUsage> ReturnSeats(Guid studyCourseId, string requester);
    }
}