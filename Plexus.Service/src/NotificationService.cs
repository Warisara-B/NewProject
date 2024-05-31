using Plexus.Database.Model.Academic.Advising;
using Plexus.Database.Model.Academic;
using Plexus.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plexus.Database.Model.Notification;
using Plexus.Service.ViewModel.Notification;
using Plexus.Database.Enum;
using Microsoft.EntityFrameworkCore;
using Plexus.Utility.Extensions;
using Plexus.Service.ViewModel;
using Plexus.Utility.ViewModel;
using Plexus.Service.Exception;
using ServiceStack;
using System.Collections.Immutable;

namespace Plexus.Service.src
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _uow;
        private readonly IAsyncRepository<Notification> _notificationRepo;
        private readonly IAsyncRepository<NotificationStudent> _notificationStdRepo;
        private readonly IAsyncRepository<NotificationImage> _notificationImgRepo;

        public NotificationService(IUnitOfWork uow
            , IAsyncRepository<Notification> notificationRepo
            , IAsyncRepository<NotificationStudent> notificationStdRepo
            , IAsyncRepository<NotificationImage> notificationImgRepo)
        {
            _uow = uow;
            _notificationRepo = notificationRepo;
            _notificationStdRepo = notificationStdRepo;
            _notificationImgRepo = notificationImgRepo;
        }

        public CountNotificationViewModel GetCountUnreadNotification(Guid studentId)
        {
            var notiIdsByStd = _notificationStdRepo.Query().Where(x => x.StudentId == studentId && x.HasRead == false).Select(x => x.NotificationId).ToList();

            var countUnread = _notificationRepo.Query()
                .Include(x => x.NotificationStudents.Where(n => n.StudentId == studentId))
                .Where(x => notiIdsByStd.Contains(x.Id))
                .Count();

            return new CountNotificationViewModel
            {
                Count = countUnread
            };
        }

        public PagedViewModel<NotificationViewModel> GetNotificationByStudentId(Guid studentId, LanguageCode language, int page, int pageSize)
        {
            var notiIdsByStd = _notificationStdRepo.Query().Where(x => x.StudentId == studentId).Select(x => x.NotificationId).ToList();

            var notifications = _notificationRepo.Query()
                .Include(x => x.Images)
                .Include(x => x.NotificationStudents.Where(n => n.StudentId == studentId))
                .Where(x => notiIdsByStd.Contains(x.Id))
                .OrderByDescending(x => x.UpdatedAt)
                .GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<NotificationViewModel>
            {
                Page = page,
                PageSize = pageSize,
                TotalPage = notifications.TotalPage,
                TotalItem = notifications.TotalItem
            };

            if (!notifications.Items.Any())
            {
                return response;
            }

            response.Items = (from data in notifications.Items
                              select new NotificationViewModel
                              {
                                  Id = data.Id,
                                  Type = data.Type,
                                  Title = data.Title,
                                  Description = data.Description,
                                  IsRead = data.NotificationStudents.FirstOrDefault().HasRead,
                                  PublishAt = data.UpdatedAt,
                                  DeepLink = $"gc-mutm-stg://notification-detail/{data.Id}"
                              })
                             .ToList();

            return response;
        }

        public NotificaitonDetailViewModel GetNotificationDetailById(Guid studentId, LanguageCode language, Guid notificationID)
        {
            var isPermission = _notificationStdRepo.Query().Where(x => x.StudentId == studentId && x.NotificationId == notificationID).Count() > 0;

            if (!isPermission)
            {
                throw new NotificationException.NotFound();
            }

            var notification = _notificationRepo.Query()
                .Include(x => x.Images)
                .FirstOrDefault(x => x.Id == notificationID);


            if (notification is null)
            {
                throw new NotificationException.NotFound();
            }

            var response = new NotificaitonDetailViewModel
            {
                Id = notification.Id,
                Type = notification.Type,
                Title = notification.Title,
                Content = notification.Description,
                PublishAt = notification.UpdatedAt,
                UrlImgs = (from data in notification.Images
                           select data.UrlImage
                           ).ToList()
            };

            return response;
        }

        public void UpdateReadNotificationAll(Guid studentId)
        {
            var notiIdsByStd = _notificationStdRepo.Query().Where(x => x.StudentId == studentId).ToList();

            _uow.BeginTran();
            foreach (var item in notiIdsByStd)
            {
                item.HasRead = true;
                item.UpdatedAt = DateTime.UtcNow;
                _notificationStdRepo.Update(item);
            }
            _uow.Complete();
            _uow.CommitTran();
        }

        public void UpdateReadNotificationById(Guid studentId, Guid notificationID)
        {
            var notiIdsByStd = _notificationStdRepo.Query()
                .Where(x => x.StudentId == studentId
                        && x.NotificationId == notificationID
                        && x.HasRead == false)
                .ToList();

            _uow.BeginTran();
            foreach (var item in notiIdsByStd)
            {
                item.HasRead = true;
                item.UpdatedAt = DateTime.UtcNow;
                _notificationStdRepo.Update(item);
            }
            _uow.Complete();
            _uow.CommitTran();
        }
    }
}
