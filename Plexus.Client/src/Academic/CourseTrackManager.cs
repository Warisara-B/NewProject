using Plexus.Client.ViewModel.Academic;
using Plexus.Client.ViewModel.DropDown;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Academic;
using Plexus.Entity.Exception;
using Plexus.Entity.Provider;
using Plexus.Utility.ViewModel;

namespace Plexus.Client.src.Academic
{
    public class CourseTrackManager : ICourseTrackManager
    {
        private ICourseTrackProvider _courseTrackProvider;
        private ICourseProvider _courseProvider;

        public CourseTrackManager(ICourseTrackProvider courseTrackProvider,
                                  ICourseProvider courseProvider)
        {
            _courseTrackProvider = courseTrackProvider;
            _courseProvider = courseProvider;
        }
        
        public CourseTrackViewModel Create(CreateCourseTrackViewModel request, Guid userId)
        {
            var courseTracks = _courseTrackProvider.Search()
                                                   .ToList();

            var duplicateCourseTracks = courseTracks.Where(x => x.Code == request.Code)
                                                    .ToList();

            if (duplicateCourseTracks.Any())
            {
                throw new CourseTrackException.Duplicate(request.Code);
            }

            var dto = new CreateCourseTrackDTO
            {
                Code = request.Code,
                Name = request.Name,
                IsActive = request.IsActive,
                Localizations = MapLocalizationViewModelToDTO(request.Localizations).ToList()
            };

            var courseTrack = _courseTrackProvider.Create(dto, userId.ToString());

            var response = MapDTOToViewModel(courseTrack);

            return response;
        }

        public PagedViewModel<CourseTrackViewModel> Search(SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var pagedCourseTrack = _courseTrackProvider.Search(parameters, page, pageSize);

            var courseTrackIds = pagedCourseTrack.Items.Select(x => x.Id)
                                                       .ToList();
            
            var courseTrackDetails = _courseTrackProvider.GetDetailByCourseTrackId(courseTrackIds)
                                                         .ToList();
                                                         
            var courseIds = courseTrackDetails.Select(x => x.CourseId)
                                              .Distinct()
                                              .ToList();

            var courses = _courseProvider.GetById(courseIds)
                                         .ToList();

            var response = new PagedViewModel<CourseTrackViewModel>
            {
                Page = pagedCourseTrack.Page,
                TotalPage = pagedCourseTrack.TotalPage,
                TotalItem = pagedCourseTrack.TotalItem,
                Items = (from courseTrack in pagedCourseTrack.Items
                         let details = courseTrackDetails.Where(x => x.CourseTrackId == courseTrack.Id)
                                                         .ToList()
                         select MapDTOToViewModel(courseTrack, details, courses))
                        .ToList()
            };

            return response;
        }

        public IEnumerable<BaseDropDownViewModel> GetDropDownList(SearchCriteriaViewModel parameters)
        {
            var courseTracks = _courseTrackProvider.Search(parameters);

            var response = (from courseTrack in courseTracks
                            select MapDTOToDropDown(courseTrack))
                           .ToList();

            return response;
        }

        public CourseTrackViewModel GetById(Guid id)
        {
            var courseTrack = _courseTrackProvider.GetById(id);

            var courseTrackDetails = _courseTrackProvider.GetDetailByCourseTrackId(id);

            var courseIds = courseTrackDetails.Select(x => x.CourseId)
                                              .ToList();
            
            var courses = _courseProvider.GetById(courseIds)
                                         .ToList();

            var response = MapDTOToViewModel(courseTrack, courseTrackDetails, courses);

            return response;
        }

        public CourseTrackViewModel Update(CourseTrackViewModel request, Guid userId)
        {
            var courseTracks = _courseTrackProvider.Search()
                                                   .ToList();

            var courseTrack = courseTracks.SingleOrDefault(x => x.Id == request.Id);

            if (courseTrack is null)
            {
                throw new CourseTrackException.NotFound(request.Id);
            }

            var duplicateCourseTracks = courseTracks.Where(x => x.Id != request.Id
                                                                && x.Code == request.Code)
                                                    .ToList();

            if (duplicateCourseTracks.Any())
            {
                throw new AcademicLevelException.Duplicate(request.Code);
            }

            courseTrack.Code = request.Code;
            courseTrack.Name = request.Name;
            courseTrack.IsActive = request.IsActive;
            courseTrack.Localizations = request.Localizations is null ? Enumerable.Empty<CourseTrackLocalizationDTO>()
                                                                      : (from localize in request.Localizations
                                                                         select new CourseTrackLocalizationDTO
                                                                         {
                                                                             Language = localize.Language,
                                                                             Name = localize.Name
                                                                         })
                                                                        .ToList();

            var updatedCourseTrack = _courseTrackProvider.Update(courseTrack, userId.ToString());

            var courseTrackDetails = _courseTrackProvider.GetDetailByCourseTrackId(request.Id);

            var courseIds = courseTrackDetails.Select(x => x.CourseId)
                                              .ToList();
            
            var courses = _courseProvider.GetById(courseIds)
                                         .ToList();

            var response = MapDTOToViewModel(updatedCourseTrack, courseTrackDetails, courses);

            return response;
        }

        public void Delete(Guid id)
        {
            _courseTrackProvider.Delete(id);
        }

        public IEnumerable<CourseTrackDetailViewModel> GetDetailByCourseTrackId(Guid id)
        {
            var details = _courseTrackProvider.GetDetailByCourseTrackId(id);

            var courseIds = details.Select(x => x.CourseId)
                                   .ToList();
            
            var courses = _courseProvider.GetById(courseIds)
                                         .ToList();

            var response = (from detail in details
                            let course = courses.SingleOrDefault(x => x.Id == detail.CourseId)
                            select MapDetailDTOToViewModel(detail, course))
                           .ToList();
            
            return response;
        }

        public IEnumerable<CourseTrackDetailViewModel> UpdateDetails(Guid id, IEnumerable<UpdateCourseTrackDetailViewModel> details)
        {
            var courseTrack = _courseTrackProvider.GetById(id);

            var courseTrackDetails = details is null ? Enumerable.Empty<UpdateCourseTrackDetailDTO>()
                                                     : (from detail in details
                                                        select new UpdateCourseTrackDetailDTO
                                                        {
                                                            CourseId = detail.CourseId,
                                                            Step = detail.Step,
                                                            IsRequired = detail.IsRequired
                                                        })
                                                       .ToList();
            
            var duplicateCourses = courseTrackDetails.GroupBy(x => x.CourseId)
                                                     .Where(x => x.Count() > 1)
                                                     .ToList();
            
            if (duplicateCourses.Any())
            {
                throw new CourseTrackException.DuplicateCourses();
            }

            var courseIds = courseTrackDetails.Select(x => x.CourseId)
                                              .ToList();

            var courses = _courseProvider.GetById(courseIds)
                                         .ToList();

            foreach (var detail in courseTrackDetails)
            {
                var course = courses.SingleOrDefault(x => x.Id == detail.CourseId);
                
                if (course is null)
                {
                    throw new CourseException.NotFound(detail.CourseId);
                }
            }

            _courseTrackProvider.UpdateDetails(id, courseTrackDetails);

            var response = (from detail in courseTrackDetails
                            let course = courses.SingleOrDefault(x => x.Id == detail.CourseId)
                            orderby detail.Step, detail.IsRequired descending
                            select MapDetailDTOToViewModel(detail, course))
                           .ToList();
            
            return response;
        }
        
        public static CourseTrackViewModel MapDTOToViewModel(CourseTrackDTO dto, IEnumerable<UpdateCourseTrackDetailDTO>? details = null, IEnumerable<CourseDTO>? courses = null)
        {
            var response = new CourseTrackViewModel
            {
                Id = dto.Id,
                Code = dto.Code,
                Name = dto.Name,
                IsActive = dto.IsActive,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt,
                Localizations = (from data in dto.Localizations
                                 orderby data.Language
                                 select new CourseTrackLocalizationViewModel
                                 {
                                     Language = data.Language,
                                     Name = data.Name
                                 })
                                .ToList(),
                Details = details is not null && details.Any() ? (from detail in details
                                                                  let course = courses?.SingleOrDefault(x => x.Id == detail.CourseId)
                                                                  select MapDetailDTOToViewModel(detail, course))
                                                                 .ToList()
                                                               : null
            };

            return response;
        }

        private BaseDropDownViewModel MapDTOToDropDown(CourseTrackDTO dto)
        {
            var response = new BaseDropDownViewModel
            {
                Id = dto.Id.ToString(),
                Name = dto.Name
            };

            return response;
        }

        private static CourseTrackDetailViewModel MapDetailDTOToViewModel(UpdateCourseTrackDetailDTO dto, CourseDTO? course)
        {
            var response = new CourseTrackDetailViewModel
            {
                CourseTrackId = dto.CourseTrackId,
                CourseId = dto.CourseId,
                CourseName = course?.Name,
                Step = dto.Step,
                IsRequired = dto.IsRequired
            };

            return response;
        }

        private static IEnumerable<CourseTrackLocalizationDTO> MapLocalizationViewModelToDTO(
            IEnumerable<CourseTrackLocalizationViewModel>? localizations)
        {
            if(localizations is null)
            {
                return Enumerable.Empty<CourseTrackLocalizationDTO>();
            }

            var response = (from locale in localizations
                            select new CourseTrackLocalizationDTO
                            {
                                Language = locale.Language,
                                Name = locale.Name
                            })
                           .ToList();

            return response;
        }
    }
}