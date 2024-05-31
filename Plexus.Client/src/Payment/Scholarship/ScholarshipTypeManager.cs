using Plexus.Client.ViewModel.DropDown;
using Plexus.Client.ViewModel.Payment.Scholarship;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Payment.Scholarship;
using Plexus.Entity.Provider;
using Plexus.Utility.ViewModel;

namespace Plexus.Client.src.Payment.Scholarship
{
    public class ScholarshipTypeManager : IScholarshipTypeManager
    {
        private readonly IScholarshipTypeProvider _scholarshipTypeProvider;

        public ScholarshipTypeManager(IScholarshipTypeProvider scholarshipTypeProvider)
        {
            _scholarshipTypeProvider = scholarshipTypeProvider;
        }

        public ScholarshipTypeViewModel Create(CreateScholarshipTypeViewModel request, Guid userId)
        {
            var dto = new CreateScholarshipTypeDTO
            {
                Name = request.Name,
                IsActive = request.IsActive
            };

            var scholarshipType = _scholarshipTypeProvider.Create(dto, userId.ToString());

            var response = MapDTOToViewModel(scholarshipType);

            return response;
        }

        public PagedViewModel<ScholarshipTypeViewModel> Search(SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var pagedScholarshipType = _scholarshipTypeProvider.Search(parameters, page, pageSize);

            var response = new PagedViewModel<ScholarshipTypeViewModel>
            {
                Page = pagedScholarshipType.Page,
                TotalPage = pagedScholarshipType.TotalPage,
                TotalItem = pagedScholarshipType.TotalItem,
                Items = (from scholarshipType in pagedScholarshipType.Items
                         select MapDTOToViewModel(scholarshipType))
                        .ToList()
            };

            return response;
        }

        public IEnumerable<BaseDropDownViewModel> GetDropDownList(SearchCriteriaViewModel parameters)
        {
            var scholarshipTypes = _scholarshipTypeProvider.Search(parameters);

            var response = (from scholarshipType in scholarshipTypes
                            select MapDTOToDropDown(scholarshipType))
                           .ToList();
            
            return response;
        }

        public ScholarshipTypeViewModel GetById(Guid id)
        {
            var scholarshipType = _scholarshipTypeProvider.GetById(id);

            var response = MapDTOToViewModel(scholarshipType);

            return response;
        }

        public ScholarshipTypeViewModel Update(ScholarshipTypeViewModel request, Guid userId)
        {
            var scholarshipType = _scholarshipTypeProvider.GetById(request.Id);

            scholarshipType.Name = request.Name;
            scholarshipType.IsActive = request.IsActive;

            var updatedScholarshipType = _scholarshipTypeProvider.Update(scholarshipType, userId.ToString());

            var response = MapDTOToViewModel(updatedScholarshipType);

            return response;
        }

        public void Delete(Guid id)
        {
            _scholarshipTypeProvider.Delete(id);
        }

        private static ScholarshipTypeViewModel MapDTOToViewModel(ScholarshipTypeDTO dto)
        {
            var response = new ScholarshipTypeViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                IsActive = dto.IsActive,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt
            };

            return response;
        }

        private BaseDropDownViewModel MapDTOToDropDown(ScholarshipTypeDTO dto)
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