using Plexus.Client.ViewModel.Academic.Curriculum;

namespace Plexus.Client
{
    public interface IStudyPlanManager
    {
        /// <summary>
        /// Create study plan.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        StudyPlanViewModel CreateStudyPlan(CreateStudyPlanViewModel request);

        /// <summary>
        /// Add study plan course to existing study plan by id.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        StudyPlanViewModel AddStudyPlanDetail(Guid id, CreateStudyPlanDetailViewModel request);

        /// <summary>
        /// Get all study plans.
        /// </summary>
        /// <returns></returns>
        IEnumerable<StudyPlanViewModel> GetStudyPlans();

        /// <summary>
        /// Update study plan name.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        StudyPlanViewModel UpdateStudyPlan(Guid id, UpdateStudyPlanViewModel request);

        /// <summary>
        /// Update study plan's detail by studyPlanCourseId.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        StudyPlanViewModel UpdateStudyPlanDetail(Guid id, int year, string term, CreateStudyPlanDetailViewModel request);

        /// <summary>
        /// Delete study plan by id.
        /// </summary>
        /// <param name="id"></param>
        void Delete(Guid id);

        /// <summary>
        /// Delete study plan by year.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Year"></param>
        void DeleteByYear(Guid id, int year);

        /// <summary>
        /// Delete study plan by term.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Year"></param>
        /// <param name="Term"></param>
        void DeleteByTerm(Guid id, int year, string term);
    }
}