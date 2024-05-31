using System;
using Plexus.Client.ViewModel.Academic;
using Plexus.Client.ViewModel.DropDown;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Academic;
using Plexus.Entity.Exception;
using Plexus.Entity.Provider;
using Plexus.Utility.ViewModel;

namespace Plexus.Client.src.Academic
{
    public class GradeManager : IGradeManager
    {
        private readonly IGradeProvider _gradeProvider;

        public GradeManager(IGradeProvider gradeProvider)
        {
            _gradeProvider = gradeProvider;
        }

        public GradeViewModel Create(CreateGradeViewModel request, Guid userId)
        {
            var duplicateGradeLetter = _gradeProvider.GetByLetter(request.Letter)
                                                     .ToList();

            if (duplicateGradeLetter.Any())
            {
                throw new GradeException.LetterDuplicate(request.Letter);
            }

            var dto = new CreateGradeDTO
            {
                Letter = request.Letter,
                Weight = request.Weight,
                IsCalculateGPA = request.IsCalculateGPA,
                IsCalculateAccumulateCredit = request.IsCalculateAccumulateCredit,
                IsCalculateRegistrationCredit = request.IsCalculateRegistrationCredit,
                IsShowTranscript = request.IsShowTranscript
            };

            var grade = _gradeProvider.Create(dto, userId.ToString());

            var response = MapDTOToViewModel(grade);

            return response;
        }

        public PagedViewModel<GradeViewModel> Search(SearchCriteriaViewModel parameters, int page, int pageSize)
        {
            var pagedGrade = _gradeProvider.Search(parameters, page, pageSize);

            var response = new PagedViewModel<GradeViewModel>
            {
                Page = pagedGrade.Page,
                TotalPage = pagedGrade.TotalPage,
                TotalItem = pagedGrade.TotalItem,
                Items = (from grade in pagedGrade.Items
                         select MapDTOToViewModel(grade))
                        .ToList()
            };

            return response;
        }

        public GradeViewModel GetById(Guid gradeId)
        {
            var grade = _gradeProvider.GetById(gradeId);

            var response = MapDTOToViewModel(grade);

            return response;
        }

        public GradeViewModel Update(GradeViewModel request, Guid userId)
        {
            var grade = _gradeProvider.GetById(request.Id);

            var duplicateGradeLetters = _gradeProvider.GetByLetter(request.Letter);

            if (duplicateGradeLetters.Any(x => x.Id != request.Id))
            {
                throw new GradeException.LetterDuplicate(request.Letter);
            }

            grade.Letter = request.Letter;
            grade.Weight = request.Weight;
            grade.IsCalculateGPA = request.IsCalculateGPA;
            grade.IsCalculateAccumulateCredit = request.IsCalculateAccumulateCredit;
            grade.IsCalculateRegistrationCredit = request.IsCalculateRegistrationCredit;
            grade.IsShowTranscript = request.IsShowTranscript;

            var updatedGrade = _gradeProvider.Update(grade, userId.ToString());

            var response = MapDTOToViewModel(updatedGrade);

            return response;
        }

        public void Delete(Guid id)
        {
            _gradeProvider.Delete(id);
        }

        public IEnumerable<BaseDropDownViewModel> GetDropdownList()
        {
            var grades = _gradeProvider.GetAll()
                                       .ToList();

            var response = (from grade in grades
                            orderby grade.Weight descending, grade.Letter
                            select new BaseDropDownViewModel
                            {
                                Id = grade.Id.ToString(),
                                Name = grade.Letter
                            })
                           .ToList();

            return response;
        }

        public static GradeViewModel MapDTOToViewModel(GradeDTO dto)
        {
            return new GradeViewModel
            {
                Id = dto.Id,
                Letter = dto.Letter,
                Weight = dto.Weight,
                IsCalculateAccumulateCredit = dto.IsCalculateAccumulateCredit,
                IsCalculateGPA = dto.IsCalculateGPA,
                IsCalculateRegistrationCredit = dto.IsCalculateRegistrationCredit,
                IsShowTranscript = dto.IsShowTranscript,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt
            };
        }
    }
}

