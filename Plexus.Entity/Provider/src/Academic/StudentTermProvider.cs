using Microsoft.EntityFrameworkCore;
using Plexus.Database;
using Plexus.Database.Model.Academic;
using Plexus.Entity.DTO.Academic;
using Plexus.Entity.Exception;

namespace Plexus.Entity.Provider.src.Academic
{
    public class StudentTermProvider : IStudentTermProvider
    {
        private readonly DatabaseContext _dbContext;

        public StudentTermProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public StudentTermDTO Create(Guid studentId, UpdateStudentTermDTO request, string requester)
        {
            var model = new StudentTerm
            {
                StudentId = studentId,
                TermId = request.TermId,
                TotalCredit = 0,
                TotalRegistrationCredit = 0,
                GPAX = null,
                Status = request.Status,
                MinCredit = request.MinCredit,
                MaxCredit = request.MaxCredit,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = requester
            };

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.StudentTerms.Add(model);

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(model);

            return response;
        }

        public IEnumerable<StudentTermDTO> GetByStudentId(Guid studentId)
        {
            var terms = _dbContext.StudentTerms.AsNoTracking()
                                               .Where(x => x.StudentId == studentId)
                                               .ToList();

            var response = (from term in terms
                            select MapModelToDTO(term))
                           .ToList();

            return response;
        }

        public StudentTermDTO GetByStudentIdAndTermId(Guid studentId, Guid termId)
        {
            var term = _dbContext.StudentTerms.AsNoTracking()
                                              .SingleOrDefault(x => x.StudentId == studentId
                                                                    && x.TermId == termId);

            if (term is null)
            {
                throw new StudentException.StudentTermNotFound(termId);
            }

            var response = MapModelToDTO(term);

            return response;
        }

        public StudentTermDTO Update(Guid studentId, StudentTermDTO request, string requester)
        {
            var term = _dbContext.StudentTerms.SingleOrDefault(x => x.StudentId == studentId
                                                                    && x.TermId == request.TermId);

            if (term is null)
            {
                throw new StudentException.StudentTermNotFound(request.TermId);
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                term.Status = request.Status;
                term.MinCredit = request.MinCredit;
                term.MaxCredit = request.MaxCredit;
                term.UpdatedAt = DateTime.UtcNow;
                term.UpdatedBy = requester;

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(term);

            return response;
        }

        public StudentTermDTO UpdateGPAXAndCredit(Guid studentId, StudentTermDTO request, string requester)
        {
            var term = _dbContext.StudentTerms.SingleOrDefault(x => x.StudentId == studentId
                                                                    && x.TermId == request.TermId);

            if (term is null)
            {
                throw new StudentException.StudentTermNotFound(request.TermId);
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                term.TotalCredit = request.TotalCredit;
                term.TotalRegistrationCredit = request.TotalRegistrationCredit;
                term.GPAX = request.GPAX;
                term.UpdatedAt = DateTime.UtcNow;
                term.UpdatedBy = requester;

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(term);

            return response;
        }

        private static StudentTermDTO MapModelToDTO(StudentTerm term)
        {
            return new StudentTermDTO
            {
                TermId = term.TermId,
                TotalCredit = term.TotalCredit,
                TotalRegistrationCredit = term.TotalRegistrationCredit,
                GPAX = term.GPAX,
                Status = term.Status,
                MinCredit = term.MinCredit,
                MaxCredit = term.MaxCredit,
                CreatedAt = term.CreatedAt,
                UpdatedAt = term.UpdatedAt
            };
        }
    }
}