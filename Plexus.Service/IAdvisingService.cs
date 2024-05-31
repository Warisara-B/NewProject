using Plexus.Database.Enum;
using Plexus.Service.ViewModel.Advising;

namespace Plexus.Service
{
    public interface IAdvisingService
    {
        /// <summary>
        /// Get advisor profile.
        /// </summary>
        /// <returns></returns>
        AdvisorProfileViewModel GetAdvisorProfile(Guid studentId, LanguageCode language);

        /// <summary>
        /// Get upcoming advising appointment slots.
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        IEnumerable<AdvisingAppointmentViewModel> GetUpcomingAppointmentSlots(Guid studentId, Guid advisorId);

        /// <summary>
        /// Get advising appointment history.
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        IEnumerable<AdvisingAppointmentViewModel> GetAppointmentSlotHistory(Guid studentId, Guid advisorId);

        /// <summary>
        /// Request Booked Advising slot.
        /// </summary>
        /// <param name="slotId"></param>
        /// <param name="studentId"></param>
        void BookAdvisingSlot(Guid slotId, Guid studentId);

        /// <summary>
        /// Unbooked advising slots.
        /// </summary>
        /// <param name="slotId"></param>
        /// <param name="studentId"></param>
        void UnbookAdvisingSlot(Guid slotId, Guid studentId);

        /// <summary>
        /// Get advising information slots.
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        AdvisingViewModel GetAdvisingInformation(Guid studentId, LanguageCode language);
    }
}