using Plexus.Database.Enum;
using Plexus.Service.ViewModel;
using Plexus.Service.ViewModel.Notification;
using Plexus.Utility.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plexus.Service
{
    public interface INotificationService
    {
        /// <summary>
        /// Get list of notifications by studentId.
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="language"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<NotificationViewModel> GetNotificationByStudentId(Guid studentId, LanguageCode language, int page, int pageSize);

        /// <summary>
        /// Get count of unread notifications by studentId.
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        CountNotificationViewModel GetCountUnreadNotification(Guid studentId);

        /// <summary>
        /// Get notification detail by studentId and notification ID.
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="language"></param>
        /// <param name="notificationID"></param>
        /// <returns></returns>
        NotificaitonDetailViewModel GetNotificationDetailById(Guid studentId, LanguageCode language, Guid notificationID);

        /// <summary>
        /// Update read flag notification by studentId and notification ID.
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="notificationID"></param>
        /// <returns></returns>
        void UpdateReadNotificationById(Guid studentId, Guid notificationID);

        /// <summary>
        /// Update read flag all notification by studentId.
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        void UpdateReadNotificationAll(Guid studentId);
    }
}
