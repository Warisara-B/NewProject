using Microsoft.EntityFrameworkCore;
using Plexus.Client.ViewModel.Academic;
using Plexus.Client.ViewModel.DropDown;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Database;
using Plexus.Database.Model.Academic;
using Plexus.Database.Model.Localization.Academic;
using Plexus.Entity.DTO.Academic;
using Plexus.Entity.Provider;
using Plexus.Utility.Extensions;
using Plexus.Utility.ViewModel;

namespace Plexus.Client.src.Academic
{
    public class TeachingTypeManager : ITeachingTypeManager
    {
        private readonly ITeachingTypeProvider _teachingTypeProvider;
        private readonly DatabaseContext _dbContext;

        public TeachingTypeManager(ITeachingTypeProvider teachingTypeProvider, DatabaseContext dbContext)
        {
            _teachingTypeProvider = teachingTypeProvider;
            _dbContext = dbContext;
        }

        public TeachingTypeViewModel Create(CreateTeachingTypeViewModel request, Guid userId)
        {
            var dto = new CreateTeachingTypeDTO
            {
                Name = request.Name,
                Description = request.Description,
                IsActive = request.IsActive,
                Localizations = MapLocalizationViewModelToDTO(request.Localizations).ToList()
            };

            var academicProgram = _teachingTypeProvider.Create(dto, userId.ToString());

            var response = MapDTOToViewModel(academicProgram);

            return response;
        }

        public PagedViewModel<TeachingTypeViewModel> Search(SearchTeachingTypeCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var query = GenerateSearchQuery(parameters);

            var pagedTeachingType = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<TeachingTypeViewModel>
            {
                Page = pagedTeachingType.Page,
                TotalPage = pagedTeachingType.TotalPage,
                TotalItem = pagedTeachingType.TotalItem,
                Items = (from teachingType in pagedTeachingType.Items
                         select MapModelToViewModel(teachingType, teachingType.Localizations))
                        .ToList()
            };

            return response;
        }

        public IEnumerable<TeachingTypeViewModel> Search(SearchTeachingTypeCriteriaViewModel? parameters)
        {
            var query = GenerateSearchQuery(parameters);

            var teachingTypes = query.ToList();

            var response = (from teachingType in teachingTypes
                            select MapModelToViewModel(teachingType, teachingType.Localizations))
                           .ToList();

            return response;
        }

        public IEnumerable<BaseDropDownViewModel> GetDropDownList(SearchTeachingTypeCriteriaViewModel parameters)
        {
            var teachingTypes = Search(parameters).ToList();

            var response = (from teachingType in teachingTypes
                            select MapViewModelToDropDown(teachingType))
                           .ToList();

            return response;
        }

        public TeachingTypeViewModel GetById(Guid id)
        {
            var teachingType = _teachingTypeProvider.GetById(id);

            var response = MapDTOToViewModel(teachingType);

            return response;
        }

        public TeachingTypeViewModel Update(Guid id, CreateTeachingTypeViewModel request, Guid userId)
        {
            var teachingType = _teachingTypeProvider.GetById(id);

            teachingType.Name = request.Name;
            teachingType.Description = request.Description;
            teachingType.IsActive = request.IsActive;
            teachingType.Localizations = MapLocalizationViewModelToDTO(request.Localizations);

            var updatedTeachingType = _teachingTypeProvider.Update(teachingType, userId.ToString());

            var response = MapDTOToViewModel(updatedTeachingType);

            return response;
        }

        public void Delete(Guid id)
        {
            _teachingTypeProvider.Delete(id);
        }

        public static TeachingTypeViewModel MapDTOToViewModel(TeachingTypeDTO dto)
        {
            var response = new TeachingTypeViewModel
            {
                Id = dto.Id,
                IsActive = dto.IsActive,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt,
                Localizations = (from data in dto.Localizations
                                 orderby data.Language
                                 select new TeachingTypeLocalizationViewModel
                                 {
                                     Language = data.Language,
                                     Name = data.Name,
                                     Description = data.Description
                                 })
                                .ToList()
            };

            return response;
        }

        private static TeachingTypeViewModel MapModelToViewModel(TeachingType model, IEnumerable<TeachingTypeLocalization> localizations)
        {
            return new TeachingTypeViewModel
            {
                Id = model.Id,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                IsActive = model.IsActive,
                Localizations = localizations is null ? Enumerable.Empty<TeachingTypeLocalizationViewModel>()
                                          : (from localize in localizations
                                             orderby localize.Language
                                             select new TeachingTypeLocalizationViewModel
                                             {
                                                 Language = localize.Language,
                                                 Name = localize.Name,
                                                 Description = localize.Description
                                             })
                                            .ToList()
            };
        }

        private static IEnumerable<TeachingTypeLocalizationDTO> MapLocalizationViewModelToDTO(
                       IEnumerable<TeachingTypeLocalizationViewModel>? localizations)
        {
            if (localizations is null)
            {
                return Enumerable.Empty<TeachingTypeLocalizationDTO>();
            }

            var response = (from locale in localizations
                            select new TeachingTypeLocalizationDTO
                            {
                                Language = locale.Language,
                                Name = locale.Name,
                                Description = locale.Description
                            })
                           .ToList();

            return response;
        }

        private IQueryable<TeachingType> GenerateSearchQuery(SearchTeachingTypeCriteriaViewModel? parameters = null)
        {
            var query = _dbContext.TeachingTypes.Include(x => x.Localizations)
                                                .AsNoTracking();

            if (parameters is not null)
            {
                if (!string.IsNullOrEmpty(parameters.Name))
                {
                    query = query.Where(x => x.Name.Contains(parameters.Name));
                }

                if (parameters.IsActive.HasValue)
                {
                    query = query.Where(x => x.IsActive == parameters.IsActive.Value);
                }
            }

            query = query.OrderBy(x => x.Name);

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

        private static BaseDropDownViewModel MapViewModelToDropDown(TeachingTypeViewModel viewModel)
        {
            var response = new BaseDropDownViewModel
            {
                Id = viewModel.Id.ToString(),
                Name = viewModel.Name
            };

            return response;
        }
    }
}