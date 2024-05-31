using Microsoft.EntityFrameworkCore;
using Plexus.Client.ViewModel.DropDown;
using Plexus.Client.ViewModel.Facility;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Database;
using Plexus.Database.Model.Facility;
using Plexus.Database.Model.Localization.Facility;
using Plexus.Entity.DTO.Facility;
using Plexus.Entity.Exception;
using Plexus.Entity.Provider;
using Plexus.Utility.Extensions;
using Plexus.Utility.ViewModel;

namespace Plexus.Client.src.Facility
{
    public class CampusManager : ICampusManager
    {
        private readonly ICampusProvider _campusProvider;
        private readonly DatabaseContext _dbContext;

        public CampusManager(ICampusProvider campusProvider, DatabaseContext dbContext)
        {
            _campusProvider = campusProvider;
            _dbContext = dbContext;
        }

        public CampusViewModel Create(CreateCampusViewModel request, Guid userId)
        {
            var campuses = _campusProvider.GetAll()
                                          .ToList();

            var duplicateCampuses = campuses.Where(x => x.Code == request.Code)
                                            .ToList();

            if (duplicateCampuses.Any())
            {
                throw new CampusException.Duplicate(request.Code);
            }

            var dto = new CreateCampusDTO
            {
                Name = request.Name,
                Code = request.Code,
                Address1 = request.Address1,
                Address2 = request.Address2,
                ContactNumber = request.ContactNumber,
                IsActive = request.IsActive,
                Localizations = MapLocalizationViewModelToDTO(request.Localizations).ToList()
            };

            var campus = _campusProvider.Create(dto, userId.ToString());

            var response = MapDTOToViewModel(campus);

            return response;
        }

        public CampusViewModel GetById(Guid id)
        {
            var campus = _campusProvider.GetById(id);

            var response = MapDTOToViewModel(campus);

            return response;
        }

        public IEnumerable<CampusViewModel> Search(SearchCampusCriteriaViewModel? parameters = null)
        {
            var query = GenerateSearchQuery(parameters);

            var campuses = query.ToList();

            var response = (from campus in campuses
                            select MapModelToViewModel(campus, campus.Localizations))
                            .ToList();

            return response;
        }

        public PagedViewModel<CampusViewModel> Search(SearchCampusCriteriaViewModel parameters, int page, int pageSize)
        {
            var query = GenerateSearchQuery(parameters);

            var pagedCampus = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<CampusViewModel>
            {
                Page = pagedCampus.Page,
                TotalPage = pagedCampus.TotalPage,
                TotalItem = pagedCampus.TotalItem,
                Items = (from campus in pagedCampus.Items
                         select MapModelToViewModel(campus, campus.Localizations))
                        .ToList()
            };

            return response;
        }

        public CampusViewModel Update(Guid id, CreateCampusViewModel request, Guid userId)
        {
            var campuses = _campusProvider.GetAll()
                                          .ToList();

            var campus = campuses.SingleOrDefault(x => x.Id == id);

            if (campus is null)
            {
                throw new CampusException.NotFound(id);
            }

            var duplicateCampuses = campuses.Where(x => x.Id != id
                                                        && x.Code == request.Code)
                                            .ToList();

            if (duplicateCampuses.Any())
            {
                throw new CampusException.Duplicate(request.Code);
            }

            campus.Name = request.Name;
            campus.Code = request.Code;
            campus.Address1 = request.Address1;
            campus.Address2 = request.Address2;
            campus.ContactNumber = request.ContactNumber;
            campus.IsActive = request.IsActive;
            campus.Localizations = MapLocalizationViewModelToDTO(request.Localizations).ToList();

            var updatedCampus = _campusProvider.Update(campus, userId.ToString());

            var response = MapDTOToViewModel(updatedCampus);

            return response;
        }

        public void Delete(Guid id)
        {
            _campusProvider.Delete(id);
        }

        public IEnumerable<BaseDropDownViewModel> GetDropdownList(SearchCampusCriteriaViewModel parameters)
        {
            var campuses = Search().ToList();

            var response = (from campus in campuses
                            orderby campus.Code
                            select new BaseDropDownViewModel
                            {
                                Id = campus.Id.ToString(),
                                Name = campus.Name
                            })
                            .ToList();

            return response;
        }

        private static CampusViewModel MapModelToViewModel(Campus model, IEnumerable<CampusLocalization> localizations)
        {
            return new CampusViewModel
            {
                Id = model.Id,
                Code = model.Code,
                ContactNumber = model.ContactNumber,
                IsActive = model.IsActive,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                Localizations = localizations is null ? Enumerable.Empty<CampusLocalizationViewModel>()
                                          : (from localize in localizations
                                             orderby localize.Language
                                             select new CampusLocalizationViewModel
                                             {
                                                 Language = localize.Language,
                                                 Name = localize.Name,
                                                 Address1 = localize.Address1,
                                                 Address2 = localize.Address2
                                             })
                                            .ToList()
            };
        }

        private static CampusViewModel MapDTOToViewModel(CampusDTO dto)
        {
            var response = new CampusViewModel
            {
                Id = dto.Id,
                Code = dto.Code,
                ContactNumber = dto.ContactNumber,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt,
                IsActive = dto.IsActive,
                Localizations = (from localize in dto.Localizations
                                 orderby localize.Language
                                 select new CampusLocalizationViewModel
                                 {
                                     Language = localize.Language,
                                     Name = localize.Name,
                                     Address1 = localize.Address1,
                                     Address2 = localize.Address2
                                 })
                                .ToList()
            };

            return response;
        }

        private static IEnumerable<CampusLocalizationDTO> MapLocalizationViewModelToDTO(IEnumerable<CampusLocalizationViewModel>? localizations)
        {
            if (localizations is null)
            {
                return Enumerable.Empty<CampusLocalizationDTO>();
            }

            var response = (from locale in localizations
                            select new CampusLocalizationDTO
                            {
                                Language = locale.Language,
                                Name = locale.Name,
                                Address1 = locale.Address1,
                                Address2 = locale.Address2
                            })
                            .ToList();

            return response;
        }

        private IQueryable<Campus> GenerateSearchQuery(SearchCampusCriteriaViewModel? parameters)
        {
            var query = _dbContext.Campuses.Include(x => x.Localizations)
                                           .AsNoTracking();

            if (parameters is not null)
            {
                if (!string.IsNullOrEmpty(parameters.Code))
                {
                    query = query.Where(x => x.Code.Contains(parameters.Code));
                }

                if (!string.IsNullOrEmpty(parameters.Name))
                {
                    query = query.Where(x => x.Name.Contains(parameters.Name));
                }

                if (parameters.IsActive.HasValue)
                {
                    query = query.Where(x => x.IsActive == parameters.IsActive);
                }
            }

            query = query.OrderBy(x => x.Code);

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