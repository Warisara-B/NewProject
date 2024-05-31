using Plexus.Client.ViewModel.DropDown;
using Plexus.Client.ViewModel.Payment;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Payment;
using Plexus.Entity.Exception;
using Plexus.Entity.Provider;
using Plexus.Utility.ViewModel;

namespace Plexus.Client.src.Payment
{
    public class StudentFeeTypeManager : IStudentFeeTypeManager
    {
        private readonly IStudentFeeTypeProvider _studentFeeTypeProvider;

        public StudentFeeTypeManager(IStudentFeeTypeProvider studentFeeTypeProvider)
        {
            _studentFeeTypeProvider = studentFeeTypeProvider;
        }

        public StudentFeeTypeViewModel Create(CreateStudentFeeTypeViewModel request, Guid userId)
        {
            var dto = new CreateStudentFeeTypeDTO
            {
                Name = request.Name,
                IsActive = request.IsActive,
                Localizations = MapLocalizationViewModelToDTO(request.Localizations)
            };

            var studentFeeType = _studentFeeTypeProvider.Create(dto, userId.ToString());

            var response = MapDTOToViewModel(studentFeeType);

            return response;
        }

        public PagedViewModel<StudentFeeTypeViewModel> Search(SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var pagedStudentFeeType = _studentFeeTypeProvider.Search(parameters, page, pageSize);

            var response = new PagedViewModel<StudentFeeTypeViewModel>
            {
                Page = pagedStudentFeeType.Page,
                TotalPage = pagedStudentFeeType.TotalPage,
                TotalItem = pagedStudentFeeType.TotalItem,
                Items = (from studentFeeType in pagedStudentFeeType.Items
                         select MapDTOToViewModel(studentFeeType))
                        .ToList()
            };

            return response;
        }

        public IEnumerable<BaseDropDownViewModel> GetDropDownList(SearchCriteriaViewModel parameters)
        {
            var studentFeeTypes = _studentFeeTypeProvider.Search(parameters)
                                                         .ToList();
            
            var response = (from studentFeeType in studentFeeTypes
                            select new BaseDropDownViewModel
                            {
                                Id = studentFeeType.Id.ToString(),
                                Name = studentFeeType.Name
                            })
                           .ToList();
            
            return response;
        }

        public StudentFeeTypeViewModel GetById(Guid id)
        {
            var studentFeeType = _studentFeeTypeProvider.GetById(id);

            var response = MapDTOToViewModel(studentFeeType);

            return response;
        }

        public StudentFeeTypeViewModel Update(StudentFeeTypeViewModel request, Guid userId)
        {
            var studentFeeType = _studentFeeTypeProvider.GetById(request.Id);

            studentFeeType.Name = request.Name;
            studentFeeType.IsActive = request.IsActive;
            studentFeeType.Localizations = MapLocalizationViewModelToDTO(request.Localizations);

            var updatedStudentFeeType = _studentFeeTypeProvider.Update(studentFeeType, userId.ToString());

            var response = MapDTOToViewModel(updatedStudentFeeType);

            return response;
        }

        public void Delete(Guid id)
        {
            _studentFeeTypeProvider.Delete(id);
        }

        private static StudentFeeTypeViewModel MapDTOToViewModel(StudentFeeTypeDTO dto)
        {
            var response = new StudentFeeTypeViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                IsActive = dto.IsActive,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt,
                Localizations = (from localize in dto.Localizations
                                 orderby localize.Language
                                 select new StudentFeeTypeLocalizationViewModel
                                 {
                                     Language = localize.Language,
                                     Name = localize.Name
                                 })
                                .ToList()
            };

            return response;
        }

        private static IEnumerable<StudentFeeTypeLocalizationDTO> MapLocalizationViewModelToDTO(IEnumerable<StudentFeeTypeLocalizationViewModel>? localizations)
        {
            if (localizations is null)
            {
                return Enumerable.Empty<StudentFeeTypeLocalizationDTO>();
            }

            var response = (from localize in localizations
                            select new StudentFeeTypeLocalizationDTO
                            {
                                Language = localize.Language,
                                Name = localize.Name
                            })
                           .ToList();
            
            return response;
        }
    }
}