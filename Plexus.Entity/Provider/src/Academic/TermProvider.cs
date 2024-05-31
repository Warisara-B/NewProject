using Microsoft.EntityFrameworkCore;
using Plexus.Database;
using Plexus.Database.Model.Academic;
using Plexus.Entity.DTO.Academic;
using Plexus.Entity.Exception;

namespace Plexus.Entity.Provider.src.Academic
{
    public class TermProvider : ITermProvider
    {
        private readonly DatabaseContext _dbContext;

        public TermProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region term

        public TermDTO Create(CreateTermDTO request, string requester)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var term = new Term
            {
                Year = request.Year,
                Number = request.Number,
                Type = request.Type,
                CollegeCalendarType = request.CollegeCalendarType,
                StartedAt = request.StartedAt,
                EndedAt = request.EndedAt,
                IsCurrent = request.IsCurrent,
                IsRegistration = request.IsRegistration,
                IsAdvising = request.IsAdvising,
                IsSurveyed = request.IsSurveyed,
                AcademicLevelId = request.AcademicLevelId,
                TotalWeeks = request.TotalWeeks,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = requester
            };

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Terms.Add(term);

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapTermDTO(term);

            return response;
        }

        public TermDTO GetById(Guid termId)
        {
            var term = _dbContext.Terms.AsNoTracking()
                                       .SingleOrDefault(x => x.Id == termId);

            if (term is null)
            {
                throw new TermException.NotFound(termId);
            }

            var response = MapTermDTO(term);

            return response;
        }

        public IEnumerable<TermDTO> GetById(IEnumerable<Guid> termIds)
        {
            var terms = _dbContext.Terms.AsNoTracking()
                                        .Where(x => termIds.Contains(x.Id))
                                        .ToList();

            var response = (from term in terms
                            orderby term.Year descending, term.Number descending
                            select MapTermDTO(term))
                           .ToList();

            return response;
        }

        public IEnumerable<TermDTO> GetByAcademiceLevel(Guid academicLevelId)
        {
            var terms = _dbContext.Terms.AsNoTracking()
                                        .Where(x => x.AcademicLevelId == academicLevelId)
                                        .ToList();

            var response = (from term in terms
                            orderby term.Year descending, term.Number descending
                            select MapTermDTO(term))
                           .ToList();

            return response;
        }

        public TermDTO Update(TermDTO request, string requester)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var term = _dbContext.Terms.SingleOrDefault(x => x.Id == request.Id);

            if (term is null)
            {
                throw new TermException.NotFound(request.Id);
            }

            term.Year = request.Year;
            term.Number = request.Number;
            term.Type = request.Type;
            term.CollegeCalendarType = request.CollegeCalendarType;
            term.AcademicLevelId = request.AcademicLevelId;
            term.StartedAt = request.StartedAt;
            term.EndedAt = request.EndedAt;
            term.IsCurrent = request.IsCurrent;
            term.IsRegistration = request.IsRegistration;
            term.IsAdvising = request.IsAdvising;
            term.IsSurveyed = request.IsSurveyed;
            term.TotalWeeks = request.TotalWeeks;
            term.UpdatedAt = DateTime.UtcNow;
            term.UpdatedBy = requester;

            _dbContext.SaveChanges();

            var response = MapTermDTO(term);

            return response;
        }

        public void Delete(Guid id)
        {
            var term = _dbContext.Terms.AsNoTracking()
                                       .SingleOrDefault(x => x.Id == id);

            if (term is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Terms.Remove(term);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        public TermDTO? GetRegistrationTerm(Guid studentId)
        {
            return null;

            //var student = _dbContext.Students.AsNoTracking()
            //                                 .SingleOrDefault(x => x.Id == studentId);

            //if (student == null)
            //{
            //    throw new StudentException.NotFound(studentId);
            //}

            //// NOT ASSIGN AcademicLevel
            //if (!student.AcademicLevelId.HasValue)
            //{
            //    return null;
            //}

            //// NOT STUDYING STUDENT, RETURN NULL
            //if (student.AcademicStatus != AcademicStatus.STUDYING
            //    && student.AcademicStatus != AcademicStatus.GRADUATING)
            //{
            //    return null;
            //}

            //var terms = _dbContext.Terms.AsNoTracking()
            //                            .Where(x => x.IsRegister
            //                                        && x.AcademicLevelId == student.AcademicLevelId.Value)
            //                            .ToList();

            //var registrationTerm = (from term in terms
            //                        orderby term.Year descending, term.Number descending
            //                        select term).FirstOrDefault();

            //var response = registrationTerm is null ? null
            //                                        : MapTermDTO(registrationTerm);

            //return response;
        }

        #endregion

        private static TermDTO MapTermDTO(Term term)
        {
            if (term is null)
            {
                return null;
            }

            return new TermDTO
            {
                Id = term.Id,
                Year = term.Year,
                Number = term.Number,
                Type = term.Type,
                CollegeCalendarType = term.CollegeCalendarType,
                IsCurrent = term.IsCurrent,
                IsRegistration = term.IsRegistration,
                IsAdvising = term.IsAdvising,
                IsSurveyed = term.IsSurveyed,
                StartedAt = term.StartedAt,
                EndedAt = term.EndedAt,
                AcademicLevelId = term.AcademicLevelId,
                TotalWeeks = term.TotalWeeks,
                CreatedAt = term.CreatedAt,
                UpdatedAt = term.UpdatedAt
            };
        }
    }
}

