using Plexus.Database.Enum.Academic.Section;
using Plexus.Database.Model.Academic;
using Plexus.Database.Model.Academic.Section;
using Plexus.Entity.DTO.Academic;
using SectionModel = Plexus.Database.Model.Academic.Section.Section;
using Plexus.Entity.DTO.Academic.Section;
using Plexus.Utility.ViewModel;
using Plexus.Entity.DTO;

namespace Plexus.Entity.Provider
{
    public interface ISectionProvider
    {
        /// <summary>
        /// Get sections by course ids
        /// </summary>
        /// <param name="termId"></param>
        /// <param name="courseIds"></param>
        /// <returns></returns>
        IEnumerable<SectionDTO> GetByTermIdAndCourseId(Guid termId, IEnumerable<Guid> courseIds);

        /// <summary>
        /// Search section by given parameters as paged.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<SectionDTO> Search(SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25);

        /// <summary>
        /// Search section by given parameters.
        /// </summary>
        /// <param name="parameters">search parameter, termId, courseId, roomId, number, isActive</param>
        /// <returns></returns>
        IEnumerable<SectionDTO> Search(SearchCriteriaViewModel parameters);

        /// <summary>
        /// Get section by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        SectionDTO GetById(Guid id);

        /// <summary>
        /// Get section by ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        IEnumerable<SectionDTO> GetById(IEnumerable<Guid> ids);
        
        /// <summary>
        /// Get section details by section ids
        /// </summary>
        /// <param name="sectionIds"></param>
        /// <returns></returns>
        IEnumerable<SectionDetailDTO> GetDetailBySectionId(IEnumerable<Guid> sectionIds);

        /// <summary>
        /// Open section by given id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="requester"></param>
        void OpenSection(Guid id, string requester);

        /// <summary>
        /// Close section by given id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="requester"></param>
        void CloseSection(Guid id, string requester);

        /// <summary>
        /// Update section status by given id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="requester"></param>
        void UpdateStatus(Guid id, SectionStatus status, string requester);

        /// <summary>
        /// Update main instructor by given id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="instructorId"></param>
        /// <param name="requester"></param>
        void UpdateMainInstructor(Guid id, Guid? instructorId, string requester);

        /// <summary>
        /// Update section details and section examinations.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="details"></param>
        /// <param name="examinations"></param>
        /// <param name="requester"></param>
        void Update(Guid id, IEnumerable<UpdateSectionDetailDTO> details, IEnumerable<UpdateSectionExaminationDTO> examinations, string requester);
        
        /// <summary>
        /// Map dto to model with usages.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="defaultSeats"></param>
        /// <param name="requester"></param>
        (SectionModel, SectionSeatUsage?) MapDTOToModel(SectionDTO request, IEnumerable<SectionSeat> defaultSeats, string requester);
    }
}