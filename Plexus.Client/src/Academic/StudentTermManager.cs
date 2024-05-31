using Plexus.Client.ViewModel.Academic;
using Plexus.Entity.DTO.Academic;
using Plexus.Entity.Exception;
using Plexus.Entity.Provider;

namespace Plexus.Client.src.Academic
{
    public class StudentTermManager : IStudentTermManager
    {
        private readonly ITermProvider _termProvider;
        private readonly IStudentProvider _studentProvider;
        private readonly IStudentTermProvider _studentTermProvider;

        public StudentTermManager(ITermProvider termProvider,
                                  IStudentProvider studentProvider,
                                  IStudentTermProvider studentTermProvider)
        {
            _termProvider = termProvider;
            _studentProvider = studentProvider;
            _studentTermProvider = studentTermProvider;
        }

        public StudentTermViewModel Create(Guid studentId, UpdateStudentTermViewModel request, Guid userId)
        {
            var student = _studentProvider.GetById(studentId);

            var term = _termProvider.GetById(request.TermId);

            var studentTerms = _studentTermProvider.GetByStudentId(studentId)
                                                   .ToList();

            if (studentTerms.Any(x => x.TermId == request.TermId))
            {
                throw new StudentException.DuplicateStudentTerm(request.TermId);
            }

            var dto = new UpdateStudentTermDTO
            {
                TermId = request.TermId,
                Status = request.Status,
                MinCredit = request.MinCredit,
                MaxCredit = request.MaxCredit
            };

            var studentTerm = _studentTermProvider.Create(studentId, dto, userId.ToString());

            var response = MapDTOToViewModel(studentTerm, term);

            return response;
        }

        public IEnumerable<StudentTermViewModel> GetByStudentId(Guid id)
        {
            var student = _studentProvider.GetById(id);

            var studentTerms = _studentTermProvider.GetByStudentId(id)
                                                   .ToList();

            var termIds = studentTerms is null ? Enumerable.Empty<Guid>()
                                               : studentTerms.Select(x => x.TermId)
                                                             .Distinct()
                                                             .ToList();

            var terms = _termProvider.GetById(termIds);

            var response = (from studentTerm in studentTerms
                            let term = terms.SingleOrDefault(x => x.Id == studentTerm.TermId)
                            select MapDTOToViewModel(studentTerm, term))
                           .ToList();

            return response;
        }

        public StudentTermViewModel GetByStudentIdAndTermId(Guid studentId, Guid termId)
        {
            var student = _studentProvider.GetById(studentId);

            var term = _termProvider.GetById(termId);

            var studentTerm = _studentTermProvider.GetByStudentIdAndTermId(studentId, termId);

            var response = MapDTOToViewModel(studentTerm, term);

            return response;
        }

        public StudentTermViewModel Update(Guid studentId, UpdateStudentTermViewModel request, Guid userId)
        {
            var student = _studentProvider.GetById(studentId);

            var term = _termProvider.GetById(request.TermId);

            var studentTerm = _studentTermProvider.GetByStudentIdAndTermId(studentId, request.TermId);

            studentTerm.Status = request.Status;
            studentTerm.MinCredit = request.MinCredit;
            studentTerm.MaxCredit = request.MaxCredit;

            var updatedStudentTerm = _studentTermProvider.Update(studentId, studentTerm, userId.ToString());

            var response = MapDTOToViewModel(updatedStudentTerm, term);

            return response;
        }

        private static StudentTermViewModel MapDTOToViewModel(StudentTermDTO dto, TermDTO term)
        {
            var response = new StudentTermViewModel
            {
                TermId = dto.TermId,
                Status = dto.Status,
                MinCredit = dto.MinCredit,
                MaxCredit = dto.MaxCredit,
                TotalCredit = dto.TotalCredit,
                TotalRegistrationCredit = dto.TotalRegistrationCredit,
                GPAX = dto.GPAX,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt
            };

            return response;
        }
    }
}