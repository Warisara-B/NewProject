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
    public class GradeProvider : IGradeProvider
    {
        private readonly DatabaseContext _dbContext;

        public GradeProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public GradeDTO Create(CreateGradeDTO request, string requester)
        {
            var grade = new Grade
            {
                Letter = request.Letter.ToUpper(),
                Weight = request.Weight,
                IsCalculateGPA = request.IsCalculateGPA,
                IsCalculateAccumulateCredit = request.IsCalculateAccumulateCredit,
                IsCalculateRegistrationCredit = request.IsCalculateRegistrationCredit,
                IsShowTranscript = request.IsShowTranscript,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = requester
            };

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Grades.Add(grade);

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(grade);

            return response;
        }

        public IEnumerable<GradeDTO> GetAll()
        {
            var grades = _dbContext.Grades.AsNoTracking()
                                          .ToList();

            var response = (from grade in grades
                            orderby grade.Weight descending, grade.Letter
                            select MapModelToDTO(grade))
                           .ToList();

            return response;
        }

        public PagedViewModel<GradeDTO> Search(SearchCriteriaViewModel parameters, int page, int pageSize)
        {
            var query = GenerateSearchQuery(parameters);

            var pagedGrades = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<GradeDTO>
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

        public GradeDTO GetById(Guid gradeId)
        {
            var grade = _dbContext.Grades.AsNoTracking()
                                         .SingleOrDefault(x => x.Id == gradeId);

            if (grade is null)
            {
                throw new GradeException.NotFound(gradeId);
            }

            var response = MapModelToDTO(grade);

            return response;
        }

        public IEnumerable<GradeDTO> GetById(IEnumerable<Guid> gradeIds)
        {
            if (gradeIds is null || !gradeIds.Any())
            {
                return Enumerable.Empty<GradeDTO>();
            }

            var grades = _dbContext.Grades.AsNoTracking()
                                          .Where(x => gradeIds.Contains(x.Id))
                                          .ToList();


            var response = (from grade in grades
                            orderby grade.Weight descending, grade.Letter
                            select MapModelToDTO(grade))
                           .ToList();

            return response;
        }

        public IEnumerable<GradeDTO> GetByLetter(string gradeLetter)
        {
            var grades = _dbContext.Grades.AsNoTracking()
                                          .Where(x => x.Letter == gradeLetter)
                                          .ToList();

            var response = (from grade in grades
                            orderby grade.Weight descending, grade.Letter
                            select MapModelToDTO(grade))
                           .ToList();

            return response;
        }

        public GradeDTO Update(GradeDTO request, string requester)
        {
            var grade = _dbContext.Grades.SingleOrDefault(x => x.Id == request.Id);

            if (grade is null)
            {
                throw new GradeException.NotFound(request.Id);
            }

            grade.Letter = request.Letter.ToUpper();
            grade.Weight = request.Weight;
            grade.IsCalculateGPA = request.IsCalculateGPA;
            grade.IsCalculateAccumulateCredit = request.IsCalculateAccumulateCredit;
            grade.IsCalculateRegistrationCredit = request.IsCalculateRegistrationCredit;
            grade.IsShowTranscript = request.IsShowTranscript;
            grade.UpdatedAt = DateTime.UtcNow;
            grade.UpdatedBy = requester;

            _dbContext.SaveChanges();

            var response = MapModelToDTO(grade);

            return response;
        }

        public void Delete(Guid gradeId)
        {
            var grade = _dbContext.Grades.SingleOrDefault(x => x.Id == gradeId);

            if (grade is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Grades.Remove(grade);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        private static GradeDTO MapModelToDTO(Grade model)
        {
            return new GradeDTO
            {
                Id = model.Id,
                Letter = model.Letter,
                Weight = model.Weight,
                IsCalculateGPA = model.IsCalculateGPA,
                IsCalculateAccumulateCredit = model.IsCalculateAccumulateCredit,
                IsCalculateRegistrationCredit = model.IsCalculateRegistrationCredit,
                IsShowTranscript = model.IsShowTranscript,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt
            };
        }

        private IQueryable<Grade> GenerateSearchQuery(SearchCriteriaViewModel? parameters)
        {
            var query = _dbContext.Grades.AsNoTracking();

            if (parameters is not null)
            {
                if (!string.IsNullOrEmpty(parameters.Keyword))
                {
                    query = query.Where(x => x.Letter.Contains(parameters.Keyword));
                }
            }

            query = query.OrderByDescending(x => x.Weight)
                         .ThenBy(x => x.Letter);

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
    }
}

