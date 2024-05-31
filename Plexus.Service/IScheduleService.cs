using Plexus.Database.Enum;
using Plexus.Service.ViewModel;
using Plexus.Service.ViewModel.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plexus.Service
{
    public interface IScheduleService
    {
        /// <summary>
        /// Get Class Schedule By Date
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="language"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        List<ClassScheduleViewModel> GetClassScheduleByDate(Guid studentId, LanguageCode language, DateTime? startDate, DateTime? endDate);

        /// <summary>
        /// Get Class Schedule Detail By Class Id
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="language"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        ClassScheduleDetailViewModel GetClassScheduleDetailById(Guid studentId, LanguageCode language, Guid classId);

        /// <summary>
        /// Get Class Schedule By Term
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="language"></param>
        /// <param name="termId"></param>
        /// <returns></returns>
        List<ClassScheduleTermViewModel> GetClassScheduleByTerm(Guid studentId, LanguageCode language, Guid termId);

        /// <summary>
        /// Get Class Schedule Detail By Term and Class Id
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="language"></param>
        /// <param name="termId"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        ClassScheduleTermDetailViewModel GetClassScheduleDetailByTermAndClassId(Guid studentId, LanguageCode language, Guid termId, Guid classId);

        /// <summary>
        /// Get Examination Schedule By Date and Term
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="language"></param>
        /// <param name="termId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        List<ExaminationScheduleViewModel> GetExamScheduleByDate(Guid studentId, LanguageCode language, Guid termId, DateTime? startDate, DateTime? endDate);
    }
}
