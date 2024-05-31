using System;
using Plexus.Client.ViewModel.Academic;
using Plexus.Database.Enum.Academic;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Academic;
using Plexus.Entity.Exception;
using Plexus.Entity.Provider;
using Plexus.Entity.Provider.src;
using Plexus.Entity.Provider.src.Academic;
using Plexus.Utility.ViewModel;

namespace Plexus.Client.src.Academic
{
    public class WithdrawManager : IWithdrawalManager
    {
        private readonly IStudentProvider _studentProvider;
        private readonly ICourseProvider _courseProvider;
        // private readonly ISectionProvider _sectionProvider;
        private readonly IStudyCourseProvider _studyCourseProvider;
        private readonly IWithdrawalRequestProvider _withdrawalRequestProvider;
        private readonly IStudentManager _studentManager;

        public WithdrawManager(IStudentProvider studentProvider,
                               ICourseProvider courseProvider,
                               //    ISectionProvider sectionProvider,
                               IStudyCourseProvider studyCourseProvider,
                               IWithdrawalRequestProvider withdrawalRequestProvider,
                               IStudentManager studentManager)
        {
            _studentProvider = studentProvider;
            _courseProvider = courseProvider;
            // _sectionProvider = sectionProvider;
            _studyCourseProvider = studyCourseProvider;
            _withdrawalRequestProvider = withdrawalRequestProvider;
            _studentManager = studentManager;
        }

        public void Create(CreateWithdrawalRequestViewModel request, Guid userId)
        {
            // var sections = _sectionProvider.GetById(request.SectionIds)
            //                                .ToList();

            // VALIDATE SECTION
            // foreach (var id in request.SectionIds)
            // {
            //     var matchedSection = sections.SingleOrDefault(x => x.Id == id);

            //     if (matchedSection is null)
            //     {
            //         throw new SectionException.NotFound(id);
            //     }
            // }

            var students = _studentProvider.GetById(request.StudentIds)
                                           .ToList();

            // VALIDATE STUDENTS
            foreach (var id in request.StudentIds)
            {
                var matchedStudent = students.SingleOrDefault(x => x.Id == id);

                if (matchedStudent is null)
                {
                    throw new SectionException.NotFound(id);
                }
            }

            // VALIDATE STUDY COURSES
            var studyCourses = _studyCourseProvider.GetBySectionIdAndStudentId(request.SectionIds,
                                                                               request.StudentIds,
                                                                               new[] { StudyCourseStatus.REGISTERED, StudyCourseStatus.ACTIVE })
                                                   .ToList();

            foreach (var sectionId in request.SectionIds)
            {
                foreach (var studentId in request.StudentIds)
                {
                    var matchedStudyCourse = studyCourses.SingleOrDefault(x => x.SectionId == sectionId
                                                                               && x.StudentId == studentId);

                    if (matchedStudyCourse is null)
                    {
                        throw new StudyCourseException.NotFound(sectionId, studentId);
                    }

                    if (matchedStudyCourse.GradeId.HasValue)
                        throw new WithdrawalException.NotAllowRequestWithdrawal(matchedStudyCourse.Id);
                }
            }

            // GET CURRENT PENDING REQUESTS
            var pendingRequests = _withdrawalRequestProvider.GetPendingRequestByStudyCourseId(studyCourses.Select(x => x.Id))
                                                            .ToList();

            var pendingRequestStudyCourseIds = pendingRequests.Select(x => x.StudyCourseId)
                                                              .Distinct()
                                                              .ToList();

            var studyCourseIds = studyCourses.Select(x => x.Id)
                                             .ToList();

            // TODO : CONFIRM LOGIC IS A - B OR JUST THROW ERROR
            var requestWithdrawStudyCourseIds = studyCourseIds.Except(pendingRequestStudyCourseIds)
                                                             .ToList();

            if (!requestWithdrawStudyCourseIds.Any())
            {
                return;
            }

            // CREATE WITHDRAWAL REQUESTS
            _withdrawalRequestProvider.Create(requestWithdrawStudyCourseIds, request.Remark, userId.ToString());
        }

        public WithdrawalRequestViewModel GetById(Guid requestId)
        {
            var withdrawalRequest = _withdrawalRequestProvider.GetById(requestId);

            var student = _studentProvider.GetById(withdrawalRequest.StudentId);

            var course = _courseProvider.GetById(withdrawalRequest.CourseId);

            // var section = !withdrawalRequest.SectionId.HasValue ? null
            //                                                     : _sectionProvider.GetById(withdrawalRequest.SectionId.Value);

            var response = MapDTOToViewModel(withdrawalRequest, student, course, null);

            return response;
        }

        public PagedViewModel<WithdrawalRequestViewModel> Search(SearchCriteriaViewModel parameter, int page, int pageSize)
        {
            var pagedWithdrawalRequest = _withdrawalRequestProvider.Search(parameter, page, pageSize);

            if (!pagedWithdrawalRequest.Items.Any())
            {
                return null;
            }

            var studentIds = pagedWithdrawalRequest.Items.Select(x => x.StudentId)
                                                         .Distinct()
                                                         .ToList();

            var courseIds = pagedWithdrawalRequest.Items.Select(x => x.CourseId)
                                                        .Distinct()
                                                        .ToList();

            var sectionIds = pagedWithdrawalRequest.Items.Where(x => x.SectionId.HasValue)
                                                         .Select(x => x.SectionId.Value)
                                                         .Distinct()
                                                         .ToList();

            var students = _studentProvider.GetById(studentIds)
                                          .ToList();

            var courses = _courseProvider.GetById(courseIds)
                                         .ToList();

            // var sections = _sectionProvider.GetById(sectionIds)
            //                                .ToList();

            var response = new PagedViewModel<WithdrawalRequestViewModel>
            {
                TotalPage = pagedWithdrawalRequest.TotalPage,
                Page = pagedWithdrawalRequest.Page,
                TotalItem = pagedWithdrawalRequest.TotalItem,
                Items = (from item in pagedWithdrawalRequest.Items
                         let student = students.Single(x => x.Id == item.StudentId)
                         let course = courses.Single(x => x.Id == item.CourseId)
                         //  let section = !item.SectionId.HasValue ? null
                         //                                         : sections.SingleOrDefault(x => x.Id == item.SectionId.Value)
                         select MapDTOToViewModel(item, student, course, null))
                        .ToList()
            };

            return response;
        }

        public void UpdateWithdrawalStatus(UpdateWithdrawalStatusViewModel request, Guid userId)
        {
            if (request is null || !request.Ids.Any())
            {
                return;
            }

            var withdrawalRequests = _withdrawalRequestProvider.GetById(request.Ids)
                                                               .ToList();

            // VALIDATE EXISTS AND NON-REVERSE STATUS
            foreach (var id in request.Ids)
            {
                var matchedRequest = withdrawalRequests.SingleOrDefault(x => x.Id == id);

                if (matchedRequest is null)
                    throw new WithdrawalException.NotFound(id);

                if (matchedRequest.Status >= request.Status)
                    throw new WithdrawalException.NotAllowReverseStatus();
            }

            _withdrawalRequestProvider.UpdateWithdrawalStatus(request.Ids, request.Status, request.Remark, userId.ToString());
        }

        private WithdrawalRequestViewModel MapDTOToViewModel(WithdrawalRequestDTO dto, StudentDTO student, CourseDTO course, SectionDTO? section)
        {
            return new WithdrawalRequestViewModel
            {
                Id = dto.Id,
                StudentId = student.Id,
                Student = _studentManager.MapDTOToViewModel(student),
                SectionId = dto.SectionId,
                SectionNumber = section?.Number,
                CourseCode = course.Code,
                CourseName = course.Name,
                Status = dto.Status,
                Remark = dto.Remark,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt,
                ApprovedAt = dto.ApprovedAt
            };
        }
    }
}

