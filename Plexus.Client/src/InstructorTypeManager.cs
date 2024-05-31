using Plexus.Client.ViewModel;
using Plexus.Client.ViewModel.DropDown;
using Plexus.Entity.DTO;
using Plexus.Entity.Provider;
using Plexus.Utility.ViewModel;

namespace Plexus.Client.src
{
    public class InstructorTypeManager : IInstructorTypeManager
    {
        private readonly IInstructorTypeProvider _instructorTypeProvider;

        public InstructorTypeManager(IInstructorTypeProvider instructorTypeProvider)
        {
            _instructorTypeProvider = instructorTypeProvider;
        }

        public InstructorTypeViewModel Create(CreateInstructorTypeViewModel request, Guid userId)
        {
            var dto = new CreateInstructorTypeDTO
            {
                Name = request.Name,
                IsActive = request.IsActive,
                Localizations = MapLocalizationViewModelToDTO(request.Localizations)
            };

            var instructorType = _instructorTypeProvider.Create(dto, userId.ToString());

            var response = MapDTOToViewModel(instructorType);

            return response;
        }

        public PagedViewModel<InstructorTypeViewModel> Search(SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var pagedInstructorType = _instructorTypeProvider.Search(parameters, page, pageSize);

            var response = new PagedViewModel<InstructorTypeViewModel>
            {
                Page = pagedInstructorType.Page,
                TotalPage = pagedInstructorType.TotalPage,
                TotalItem = pagedInstructorType.TotalItem,
                Items = (from instructorType in pagedInstructorType.Items
                         select MapDTOToViewModel(instructorType))
                        .ToList()
            };

            return response;
        }

        public IEnumerable<BaseDropDownViewModel> GetDropDownList(SearchCriteriaViewModel parameters)
        {
            var instructorTypes = _instructorTypeProvider.Search(parameters);

            var response = (from instructorType in instructorTypes
                            select MapDTOToDropDown(instructorType))
                           .ToList();
            
            return response;
        }

        public InstructorTypeViewModel GetById(Guid id)
        {
            var instructorType = _instructorTypeProvider.GetById(id);

            var response = MapDTOToViewModel(instructorType);

            return response;
        }

        public InstructorTypeViewModel Update(InstructorTypeViewModel request, Guid userId)
        {
            var instructorType = _instructorTypeProvider.GetById(request.Id);

            instructorType.Name = request.Name;
            instructorType.IsActive = request.IsActive;
            instructorType.Localizations = MapLocalizationViewModelToDTO(request.Localizations);

            var updatedInstructorType = _instructorTypeProvider.Update(instructorType, userId.ToString());

            var response = MapDTOToViewModel(updatedInstructorType);

            return response;
        }

        public void Delete(Guid id)
        {
            _instructorTypeProvider.Delete(id);
        }

        private static InstructorTypeViewModel MapDTOToViewModel(InstructorTypeDTO dto)
        {
            var response = new InstructorTypeViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                IsActive = dto.IsActive,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt,
                Localizations = (from localize in dto.Localizations
                                 orderby localize.Language
                                 select new InstructorTypeLocalizationViewModel
                                 {
                                     Language = localize.Language,
                                     Name = localize.Name
                                 })
                                .ToList()
            };

            return response;
        }

        private static IEnumerable<InstructorTypeLocalizationDTO> MapLocalizationViewModelToDTO(IEnumerable<InstructorTypeLocalizationViewModel>? localizations)
        {
            if (localizations is null)
            {
                return Enumerable.Empty<InstructorTypeLocalizationDTO>();
            }

            var response = (from localize in localizations
                            select new InstructorTypeLocalizationDTO
                            {
                                Language = localize.Language,
                                Name = localize.Name
                            })
                           .ToList();
            
            return response;
        }

        private BaseDropDownViewModel MapDTOToDropDown(InstructorTypeDTO dto)
        {
            var response = new BaseDropDownViewModel
            {
                Id = dto.Id.ToString(),
                Name = dto.Name
            };

            return response;
        }
    }
}