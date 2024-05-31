using Microsoft.EntityFrameworkCore;
using Plexus.Client.ViewModel.Academic;
using Plexus.Client.ViewModel.DropDown;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Database;
using Plexus.Database.Model.Academic;
using Plexus.Database.Model.Localization.Academic;
using Plexus.Entity.Exception;
using Plexus.Utility.Extensions;
using Plexus.Utility.ViewModel;

namespace Plexus.Client.src.Academic
{
    public class AcademicProgramManager : IAcademicProgramManager
    {
        private readonly DatabaseContext _dbContext;

        public AcademicProgramManager(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public AcademicProgramViewModel Create(CreateAcademicProgramViewModel request, Guid userId)
        {
            var model = new AcademicProgram
            {
                Name = request.Name,
                FormalName = request.FormalName,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "", // TODO : Add requester
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = "" // TODO : Add requester
            };

            var localizes = MapLocalizationViewModelToModel(request.Localizations, model).ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.AcademicPrograms.Add(model);

                if (localizes.Any())
                {
                    _dbContext.AcademicProgramLocalizations.AddRange(localizes);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToViewModel(model, localizes);

            return response;
        }

        public IEnumerable<AcademicProgramViewModel> Search(SearchAcademicProgramCriteriaViewModel? parameters = null)
        {
            var query = GenerateSearchQuery(parameters);

            var academicPrograms = query.ToList();

            var response = (from academicProgram in academicPrograms
                            select MapModelToViewModel(academicProgram, academicProgram.Localizations))
                           .ToList();

            return response;
        }

        public PagedViewModel<AcademicProgramViewModel> Search(SearchAcademicProgramCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var query = GenerateSearchQuery(parameters);

            var pagedAcademicProgram = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<AcademicProgramViewModel>
            {
                Page = pagedAcademicProgram.Page,
                TotalPage = pagedAcademicProgram.TotalPage,
                TotalItem = pagedAcademicProgram.TotalItem,
                Items = (from academicProgram in pagedAcademicProgram.Items
                         select MapModelToViewModel(academicProgram, academicProgram.Localizations))
                        .ToList()
            };

            return response;
        }

        public IEnumerable<AcademicProgramDropDownViewModel> GetDropDownList(SearchAcademicProgramCriteriaViewModel parameters)
        {
            var academicPrograms = Search();

            var response = (from academicProgram in academicPrograms
                            select MapViewModelToDropDown(academicProgram))
                           .ToList();

            return response;
        }

        public AcademicProgramViewModel GetById(Guid id)
        {
            var academicProgram = _dbContext.AcademicPrograms.Include(x => x.Localizations)
                                                             .SingleOrDefault(x => x.Id == id);

            if (academicProgram is null)
            {
                throw new AcademicProgramException.NotFound(id);
            }

            var response = MapModelToViewModel(academicProgram, academicProgram.Localizations);

            return response;
        }

        public AcademicProgramViewModel Update(Guid id, CreateAcademicProgramViewModel request, Guid userId)
        {
            var academicProgram = _dbContext.AcademicPrograms.Include(x => x.Localizations)
                                                             .SingleOrDefault(x => x.Id == id);

            if (academicProgram is null)
            {
                throw new AcademicProgramException.NotFound(id);
            }

            var localizes = request.Localizations is null ? Enumerable.Empty<AcademicProgramLocalization>()
                                                          : (from data in request.Localizations
                                                             select new AcademicProgramLocalization
                                                             {
                                                                 Language = data.Language,
                                                                 AcademicProgramId = academicProgram.Id,
                                                                 Name = data.Name,
                                                                 FormalName = data.FormalName
                                                             })
                                                            .ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                academicProgram.Name = request.Name;
                academicProgram.FormalName = request.FormalName;
                academicProgram.IsActive = request.IsActive;
                academicProgram.UpdatedAt = DateTime.UtcNow;
                academicProgram.UpdatedBy = ""; // TODO : Add requester.

                _dbContext.AcademicProgramLocalizations.RemoveRange(academicProgram.Localizations);

                if (localizes.Any())
                {
                    _dbContext.AcademicProgramLocalizations.AddRange(localizes);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToViewModel(academicProgram, localizes);

            return response;
        }

        public void Delete(Guid id)
        {
            var academicProgram = _dbContext.AcademicPrograms.SingleOrDefault(x => x.Id == id);

            if (academicProgram is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.AcademicPrograms.Remove(academicProgram);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        private static AcademicProgramDropDownViewModel MapViewModelToDropDown(AcademicProgramViewModel viewModel)
        {
            var response = new AcademicProgramDropDownViewModel
            {
                Id = viewModel.Id.ToString(),
                Name = viewModel.Name
            };

            return response;
        }

        private static AcademicProgramViewModel MapModelToViewModel(AcademicProgram model, IEnumerable<AcademicProgramLocalization> localizations)
        {
            return new AcademicProgramViewModel
            {
                Id = model.Id,
                IsActive = model.IsActive,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                Localizations = localizations is null ? Enumerable.Empty<AcademicProgramLocalizationViewModel>()
                                                      : (from localize in localizations
                                                         orderby localize.Language
                                                         select new AcademicProgramLocalizationViewModel
                                                         {
                                                             Language = localize.Language,
                                                             Name = localize.Name,
                                                             FormalName = localize.FormalName
                                                         })
                                                        .ToList()
            };
        }

        private static IEnumerable<AcademicProgramLocalization> MapLocalizationViewModelToModel(
                       IEnumerable<AcademicProgramLocalizationViewModel>? localizations,
                       AcademicProgram model
        )
        {
            if (localizations is null)
            {
                return Enumerable.Empty<AcademicProgramLocalization>();
            }

            var response = (from locale in localizations
                            select new AcademicProgramLocalization
                            {
                                AcademicProgram = model,
                                Language = locale.Language,
                                Name = locale.Name,
                                FormalName = locale.FormalName
                            })
                            .ToList();

            return response;
        }

        private IQueryable<AcademicProgram> GenerateSearchQuery(SearchAcademicProgramCriteriaViewModel? parameters = null)
        {
            var query = _dbContext.AcademicPrograms.Include(x => x.Localizations)
                                                   .AsNoTracking();

            if (parameters is not null)
            {
                if (!string.IsNullOrEmpty(parameters.Name))
                {
                    query = query.Where(x => x.Name.Contains(parameters.Name) || x.FormalName.Contains(parameters.Name));
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
    }
}