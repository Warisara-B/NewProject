using Microsoft.EntityFrameworkCore;
using Plexus.Database;
using Plexus.Database.Model.Academic;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Academic;
using Plexus.Utility.ViewModel;
using Plexus.Utility.Extensions;
using Plexus.Entity.Exception;
using Plexus.Database.Model.Localization.Academic;

namespace Plexus.Entity.Provider.src.Academic
{
    public class CourseTrackProvider : ICourseTrackProvider
    {
        private readonly DatabaseContext _dbContext;

        public CourseTrackProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public CourseTrackDTO Create(CreateCourseTrackDTO request, string requester)
        {
            var model = new CourseTrack
            {
                Code = request.Code,
                Name = request.Name,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = requester
            };

            var localizes = MapLocalizationDTOToModel(request.Localizations, model).ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.CourseTracks.Add(model);

                if (localizes.Any())
                {
                    _dbContext.CourseTrackLocalizations.AddRange(localizes);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(model, localizes);

            return response;
        }

        public PagedViewModel<CourseTrackDTO> Search(SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var query = GenerateSearchQuery(parameters);

            var pagedCourseTrack = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<CourseTrackDTO>
            {
                Page = pagedCourseTrack.Page,
                TotalPage = pagedCourseTrack.TotalPage,
                TotalItem = pagedCourseTrack.TotalItem,
                Items = (from courseTrack in pagedCourseTrack.Items
                         select MapModelToDTO(courseTrack, courseTrack.Localizations))
                        .ToList()
            };

            return response;
        }

        public IEnumerable<CourseTrackDTO> Search(SearchCriteriaViewModel? parameters = null)
        {
            var query = GenerateSearchQuery(parameters);

            var courseTracks = query.ToList();

            var response = (from courseTrack in courseTracks
                            select MapModelToDTO(courseTrack, courseTrack.Localizations))
                           .ToList();
            
            return response;
        }

        public IEnumerable<CourseTrackDTO> GetById(IEnumerable<Guid> ids)
        {
            var courseTracks = _dbContext.CourseTracks.Include(x => x.Localizations)
                                                      .AsNoTracking()
                                                      .Where(x => ids.Contains(x.Id))
                                                      .ToList();
            
            var response = (from courseTrack in courseTracks
                            select MapModelToDTO(courseTrack, courseTrack.Localizations))
                           .ToList();
            
            return response;
        }

        public IEnumerable<CourseTrackDTO> GetByStudentId(Guid studentId)
        {
            var studentCourseTracks = _dbContext.StudentCourseTracks.Include(x => x.CourseTrack)
                                                                    .ThenInclude(x => x.Localizations)
                                                                    .AsNoTracking()
                                                                    .Where(x => x.StudentId == studentId)
                                                                    .ToList();
            
            var response = (from studentCourseTrack in studentCourseTracks
                            select MapModelToDTO(studentCourseTrack.CourseTrack, studentCourseTrack.CourseTrack.Localizations))
                           .ToList();
            
            return response;
        }

        public CourseTrackDTO GetById(Guid id)
        {
            var courseTrack = _dbContext.CourseTracks.Include(x => x.Localizations)
                                                     .AsNoTracking()
                                                     .SingleOrDefault(x => x.Id == id);

            if (courseTrack is null)
            {
                throw new CourseTrackException.NotFound(id);
            }

            var response = MapModelToDTO(courseTrack, courseTrack.Localizations);

            return response;
        }

        public CourseTrackDTO Update(CourseTrackDTO request, string requester)
        {
            var courseTrack = _dbContext.CourseTracks.Include(x => x.Localizations)
                                                     .SingleOrDefault(x => x.Id == request.Id);

            if (courseTrack is null)
            {
                throw new CourseTrackException.NotFound(request.Id);
            }

            var localizes = request.Localizations is null ? Enumerable.Empty<CourseTrackLocalization>()
                                                          : (from data in request.Localizations
                                                             select new CourseTrackLocalization
                                                             {
                                                                 Language = data.Language,
                                                                 CourseTrackId = courseTrack.Id,
                                                                 Name = data.Name
                                                             })
                                                            .ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                courseTrack.Code = request.Code;
                courseTrack.Name = request.Name;
                courseTrack.IsActive = request.IsActive;
                courseTrack.UpdatedAt = DateTime.UtcNow;
                courseTrack.UpdatedBy = requester;

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(courseTrack, localizes);

            return response;
        }

        public void Delete(Guid id)
        {
            var courseTrack = _dbContext.CourseTracks.SingleOrDefault(x => x.Id == id);

            if (courseTrack is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.CourseTracks.Remove(courseTrack);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        public IEnumerable<UpdateCourseTrackDetailDTO> GetDetailByCourseTrackId(Guid id)
        {
            var details = _dbContext.CourseTrackDetails.AsNoTracking()
                                                       .Where(x => x.CourseTrackId == id)
                                                       .ToList();
            
            var response = (from detail in details
                            orderby detail.Step, detail.IsRequired descending
                            select MapDetailModelToDTO(detail))
                           .ToList();
            
            return response;
        }

        public IEnumerable<UpdateCourseTrackDetailDTO> GetDetailByCourseTrackId(IEnumerable<Guid> ids)
        {
            var details = _dbContext.CourseTrackDetails.AsNoTracking()
                                                       .Where(x => ids.Contains(x.CourseTrackId))
                                                       .ToList();
            
            var response = (from detail in details
                            orderby detail.Step, detail.IsRequired descending
                            select MapDetailModelToDTO(detail))
                           .ToList();
            
            return response;
        }

        public void UpdateDetails(Guid id, IEnumerable<UpdateCourseTrackDetailDTO> details)
        {
            var existingDetails = _dbContext.CourseTrackDetails.Where(x => x.CourseTrackId == id)
                                                               .ToList();
            
            var newDetails = details is null ? Enumerable.Empty<CourseTrackDetail>()
                                             : (from detail in details
                                                select new CourseTrackDetail
                                                {
                                                    CourseTrackId = id,
                                                    CourseId = detail.CourseId,
                                                    Step = detail.Step,
                                                    IsRequired = detail.IsRequired
                                                })
                                               .ToList();
            
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                if (existingDetails.Any())
                {
                    _dbContext.CourseTrackDetails.RemoveRange(existingDetails);
                }

                if (newDetails.Any())
                {
                    _dbContext.CourseTrackDetails.AddRange(newDetails);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        private static CourseTrackDTO MapModelToDTO(CourseTrack model, IEnumerable<CourseTrackLocalization> localizations)
        {
            var response = new CourseTrackDTO
            {
                Id = model.Id,
                Code = model.Code,
                Name = model.Name,
                IsActive = model.IsActive,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                Localizations = localizations is null ? Enumerable.Empty<CourseTrackLocalizationDTO>()
                                                      : (from localize in localizations
                                                         orderby localize.Language
                                                         select new CourseTrackLocalizationDTO
                                                         {
                                                             Language = localize.Language,
                                                             Name = localize.Name
                                                         })
                                                        .ToList()
            };

            return response;
        }

        private IQueryable<CourseTrack> GenerateSearchQuery(SearchCriteriaViewModel? parameters)
        {
            var query = _dbContext.CourseTracks.Include(x => x.Localizations)
                                               .AsNoTracking();

            if (parameters is not null)
            {
                if (!string.IsNullOrEmpty(parameters.Keyword))
                {
                    query = query.Where(x => x.Code.Contains(parameters.Keyword)
                                             || x.Name.Contains(parameters.Keyword));
                }

                if (parameters.IsActive.HasValue)
                {
                    query = query.Where(x => x.IsActive == parameters.IsActive);
                }
            }

            query = query.OrderBy(x => x.CreatedAt);

            if (parameters is not null)
            {
                if (!string.IsNullOrEmpty(parameters.SortBy))
                {
                    try
                    {
                        query = query.OrderBy(parameters.SortBy, parameters.OrderBy);
                    }
                    catch (System.Exception) 
                    {
                        // invalid property name
                    }
                    
                }
            }

            return query;
        }

        private static UpdateCourseTrackDetailDTO MapDetailModelToDTO(CourseTrackDetail model)
        {
            var response = new UpdateCourseTrackDetailDTO
            {
                CourseTrackId = model.CourseTrackId,
                CourseId = model.CourseId,
                Step = model.Step,
                IsRequired = model.IsRequired
            };

            return response;
        }

        private static IEnumerable<CourseTrackLocalization> MapLocalizationDTOToModel(
                IEnumerable<CourseTrackLocalizationDTO> localizations,
                CourseTrack model)
        {
            if(localizations is null)
            {
                return Enumerable.Empty<CourseTrackLocalization>();
            }

            var response = (from locale in localizations
                            select new CourseTrackLocalization
                            {
                                CourseTrack = model,
                                Language = locale.Language,
                                Name = locale.Name
                            })
                           .ToList();

            return response;
        }
    }
}