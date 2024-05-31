using System;
using Microsoft.EntityFrameworkCore;
using Plexus.Database;
using Plexus.Database.Model.Academic;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Academic;
using Plexus.Entity.Exception;
using Plexus.Utility.Extensions;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider.src.Academic
{
    public class GradeTemplateProvider : IGradeTemplateProvider
    {
        private readonly DatabaseContext _dbContext;

        public GradeTemplateProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public GradeTemplateDTO Create(CreateGradeTemplateDTO request, string requester)
        {
            var gradeTemplate = new GradeTemplate
            {
                Name = request.Name.ToUpper(),
                Description = request.Description,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = requester
            };

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.GradeTemplates.Add(gradeTemplate);
                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(gradeTemplate);

            return response;
        }

        public IEnumerable<GradeTemplateDTO> GetAll()
        {
            var grades = _dbContext.GradeTemplates.AsNoTracking()
                                          .ToList();

            var response = (from grade in grades
                            orderby grade.Name descending, grade.Name
                            select MapModelToDTO(grade))
                           .ToList();
            return response;
        }

        public PagedViewModel<GradeTemplateDTO> Search(SearchGradeTemplateViewModel parameters, int page, int pageSize)
        {
            var query = GenerateSearchQuery(parameters);

            var pagedGrades = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<GradeTemplateDTO>
            {
                Page = pagedGrades.Page,
                TotalPage = pagedGrades.TotalPage,
                TotalItem = pagedGrades.TotalItem,
                Items = (from grade in pagedGrades.Items
                         select MapModelToDTO(grade))
                        .ToList()
            };

            return response;
        }

        public GradeTemplateDTO GetById(Guid gradeId)
        {
            var gradeTemp = _dbContext.GradeTemplates.AsNoTracking()
                                         .SingleOrDefault(x => x.Id == gradeId);

            if (gradeTemp is null)
            {
                throw new GradeException.NotFound(gradeId);
            }

            var response = MapModelToDTO(gradeTemp);

            return response;
        }

        public IEnumerable<GradeTemplateDTO> GetById(IEnumerable<Guid> gradeIds)
        {
            if (gradeIds is null || !gradeIds.Any())
            {
                return Enumerable.Empty<GradeTemplateDTO>();
            }

            var gradesTemplate = _dbContext.GradeTemplates.AsNoTracking()
                                          .Where(x => gradeIds.Contains(x.Id))
                                          .ToList();


            var response = (from grade in gradesTemplate
                            orderby grade.Name descending, grade.Name
                            select MapModelToDTO(grade))
                           .ToList();

            return response;
        }

        public IEnumerable<GradeTemplateDTO> GetByName(string gradeName)
        {
            var gradesTemplate = _dbContext.GradeTemplates.AsNoTracking()
                                          .Where(x => x.Name == gradeName)
                                          .ToList();

            var response = (from grade in gradesTemplate
                            orderby grade.Name descending, grade.Name
                            select MapModelToDTO(grade))
                           .ToList();

            return response;
        }

        public GradeTemplateDTO Update(GradeTemplateDTO request, string requester)
        {
            var grade = _dbContext.GradeTemplates.SingleOrDefault(x => x.Id == request.Id);

            if (grade is null)
            {
                throw new GradeException.NotFound(request.Id);
            }

            grade.Name = request.Name;
            grade.Description = request.Description;
            grade.UpdatedAt = DateTime.UtcNow;
            grade.UpdatedBy = requester;
            _dbContext.SaveChanges();
            var response = MapModelToDTO(grade);
            return response;
        }

        private static GradeTemplateDTO MapModelToDTO(GradeTemplate model)
        {
            return new GradeTemplateDTO
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                IsActive = model.IsActive,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt
            };
        }

        private IQueryable<GradeTemplate> GenerateSearchQuery(SearchGradeTemplateViewModel? parameters)
        {
            var query = _dbContext.GradeTemplates.AsNoTracking();

            if (parameters is not null)
            {
                if (!string.IsNullOrEmpty(parameters.Name))
                {
                    query = query.Where(x => x.Name.Contains(parameters.Name));
                }
            }

            query = query.OrderByDescending(x => x.Name)
                         .ThenBy(x => x.Name);

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

        public void Delete(Guid gradeId)
        {
            var grade = _dbContext.GradeTemplates.SingleOrDefault(x => x.Id == gradeId);

            if (grade is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.GradeTemplates.Remove(grade);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }
    }

}
