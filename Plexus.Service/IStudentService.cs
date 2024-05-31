using Plexus.Database.Enum;
using Plexus.Service.ViewModel;

namespace Plexus.Service
{
    public interface IStudentService
    {
        /// <summary>
        /// Get student mini card by id.
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        StudentProfileCardViewModel GetStudentCardById(Guid studentId, LanguageCode language);

        /// <summary>
        /// Get student full profile by id.
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        StudentFullProfileViewModel GetStudentFullProfileById(Guid studentId, LanguageCode language);
    }
}