using Plexus.Client.ViewModel.Academic.Curriculum;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface ICurriculumInstructorManager
    {
        /// <summary>
        /// Create curriculum instructor by curriculum version id.
        /// </summary>
        /// <param name="curriculumVersionId" ></param>
        /// <param name="request"></param>
        /// <returns></returns>
        CurriculumInstructorViewModel Create(Guid curriculumVersionId, CreateCurriculumInstructorViewModel request);

        /// <summary>
        /// Get curriculum instructor list as paged.
        /// </summary>
        /// <returns></returns>
        PagedViewModel<CurriculumInstructorViewModel> GetList(Guid curriculumVersionId, int page, int pageSize);

        /// <summary>
        /// Update curriculum instructor by id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        CurriculumInstructorViewModel Update(Guid id, CreateCurriculumInstructorViewModel request);

        /// <summary>
        /// Delete curriculum instructor by given id.
        /// </summary>
        /// <param name="id"></param>
        void Delete(Guid id);
    }
}