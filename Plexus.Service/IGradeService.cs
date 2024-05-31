using Plexus.Database.Enum;
using Plexus.Service.ViewModel;

namespace Plexus.Service
{
    public interface IGradeService
    {
        /// <summary>
        /// Get student grade by term.
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        StudentGradeViewModel GetGradeByTerm(Guid studentId, LanguageCode language);

        /// <summary>
        /// Get student grade by curriculum.
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        IEnumerable<StudentCurriculumViewModel> GetGradeByCurriculum(Guid studentId, LanguageCode language);
    }
}