using System;
using Microsoft.EntityFrameworkCore;
using Plexus.Client.ViewModel.Academic;
using Plexus.Client.ViewModel.DropDown;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Academic;
using Plexus.Entity.Exception;
using Plexus.Entity.Provider;
using Plexus.Entity.Provider.src.Academic;
using Plexus.Utility.ViewModel;

namespace Plexus.Client.src.Academic
{
    public class GradeTemplateManager : IGradeTemplateManager
    {
        private readonly IGradeTemplateProvider _gradeTemplateProvider;

        public GradeTemplateManager(IGradeTemplateProvider gradeTemplateProvider)
        {
            _gradeTemplateProvider = gradeTemplateProvider;
        }

        public GradeTemplateViewModel GetById(Guid gradeId)
        {
            var grade = _gradeTemplateProvider.GetById(gradeId);

            var response = MapDTOToViewModel(grade);

            return response;
        }

        public GradeTemplateViewModel Create(CreateGradeTemplateViewModel request, Guid userId)
        {
            var duplicateGradeLetter = _gradeTemplateProvider.GetByName(request.Name)
                                                     .ToList();

            if (duplicateGradeLetter.Any())
            {
                throw new GradeException.LetterDuplicate(request.Name);
            }

            var dto = new CreateGradeTemplateDTO
            {
                Name = request.Name,
                Description = request.Description,
                IsActive = request.IsActive,
            };

            var grade = _gradeTemplateProvider.Create(dto, userId.ToString());

            var response = MapDTOToViewModel(grade);

            return response;
        }

        public PagedViewModel<GradeTemplateViewModel> Search(SearchGradeTemplateViewModel parameters, int page, int pageSize)
        {
            var pagedGrade = _gradeTemplateProvider.Search(parameters, page, pageSize);

            var response = new PagedViewModel<GradeTemplateViewModel>
            {
                Page = pagedGrade.Page,
                Items = (from grade in pagedGrade.Items
                         select MapDTOToViewModel(grade))
                        .ToList()
            };

            return response;
        }

        public GradeTemplateViewModel Update(GradeTemplateViewModel request, Guid userId)
        {
            var grade = _gradeTemplateProvider.GetById(request.Id);

            var duplicateGradeName = _gradeTemplateProvider.GetByName(request.Name);

            if (duplicateGradeName.Any(x => x.Id != request.Id))
            {
                throw new GradeException.LetterDuplicate(request.Name);
            }

            grade.Name = request.Name;
            grade.Description = request.Description;

            var updatedGrade = _gradeTemplateProvider.Update(grade, userId.ToString());

            var response = MapDTOToViewModel(updatedGrade);

            return response;
        }

        public void Delete(Guid id)
        {
            _gradeTemplateProvider.Delete(id);
        }

        public IEnumerable<BaseDropDownViewModel> GetDropdownList()
        {
            var grades = _gradeTemplateProvider.GetAll()
                                       .ToList();

            var response = (from grade in grades
                            orderby grade.Name descending, grade.Name
                            select new BaseDropDownViewModel
                            {
                                Id = grade.Id.ToString(),
                                Name = grade.Name
                            })
                           .ToList();

            return response;
        }

        public static GradeTemplateViewModel MapDTOToViewModel(GradeTemplateDTO dto)
        {
            return new GradeTemplateViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                IsActive = dto.IsActive,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt
            };
        }

    }
}
