using Microsoft.EntityFrameworkCore;
using Plexus.Client.ViewModel;
using Plexus.Client.ViewModel.DropDown;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Database;
using Plexus.Database.Model;
using Plexus.Database.Model.Localization;
using Plexus.Entity.Exception;
using Plexus.Utility.Extensions;
using Plexus.Utility.ViewModel;
using ServiceStack;

namespace Plexus.Client.src
{
    public class InstructorRoleManager : IInstructorRoleManager
    {
        private readonly IUnitOfWork _uow;
        private readonly IAsyncRepository<InstructorRole> _instructorRoleRepo;
        private readonly IAsyncRepository<InstructorRoleLocalization> _instructorRoleLocalizationRepo;

        public InstructorRoleManager(IUnitOfWork uow,
                                     IAsyncRepository<InstructorRole> instructorRoleRepo,
                                     IAsyncRepository<InstructorRoleLocalization> instructoRoleLocalizationRepo)
        {
            _uow = uow;
            _instructorRoleRepo = instructorRoleRepo;
            _instructorRoleLocalizationRepo = instructoRoleLocalizationRepo;
        }

        public InstructorRoleViewModel Create(CreateInstructorRoleViewModel request, Guid userId)
        {
            var roles = _instructorRoleRepo.Query().ToList();
            int latestSequence = roles.Any() ? roles.Max(role => role.Sequence) : 0;

            var model = new InstructorRole
            {
                Sequence = latestSequence + 1,
                Name = request.Name,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "", // TODO : Add requester
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = "", // TODO : Add requester
            };

            var localizations = request.Localizations is null ? Enumerable.Empty<InstructorRoleLocalization>()
                                                              : (from localize in request.Localizations
                                                                 select new InstructorRoleLocalization
                                                                 {
                                                                     InstructorRole = model,
                                                                     Language = localize.Language,
                                                                     Name = localize.Name
                                                                 })
                                                                 .ToList();

            _uow.BeginTran();
            _instructorRoleRepo.Add(model);

            if (localizations.Any())
            {
                _instructorRoleLocalizationRepo.AddRange(localizations);
            }

            _uow.Complete();
            _uow.CommitTran();

            var response = MapModelToViewModel(model, localizations);

            return response;
        }

        public IEnumerable<BaseDropDownViewModel> GetDropDownList(SearchInstructorRoleCriteriaViewModel parameters)
        {
            var instructorRoles = Search(parameters).ToList();

            var response = (from role in instructorRoles
                            select MapViewModelToDropDown(role))
                           .ToList();

            return response;
        }

        public IEnumerable<InstructorRoleViewModel> Search(SearchInstructorRoleCriteriaViewModel? parameters)
        {
            var query = GenerateSearchQuery(parameters);

            var instructorRoles = query.ToList();

            var response = (from role in instructorRoles
                            select MapModelToViewModel(role, role.Localizations))
                           .ToList();

            return response;
        }

        public PagedViewModel<InstructorRoleViewModel> Search(SearchInstructorRoleCriteriaViewModel parameters, int page = 1, int pageSize = 5)
        {
            var query = GenerateSearchQuery(parameters);

            var pagedInstructorRole = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<InstructorRoleViewModel>
            {
                Page = pagedInstructorRole.Page,
                TotalPage = pagedInstructorRole.TotalPage,
                TotalItem = pagedInstructorRole.TotalItem,
                Items = (from instructorRole in pagedInstructorRole.Items
                         orderby instructorRole.Sequence
                         select MapModelToViewModel(instructorRole, instructorRole.Localizations))
                        .ToList()
            };

            return response;
        }

        public InstructorRoleViewModel GetById(Guid id)
        {
            var instructorRole = _instructorRoleRepo.Query()
                                                    .Include(x => x.Localizations)
                                                    .FirstOrDefault(x => x.Id == id);

            if (instructorRole is null)
            {
                throw new InstructorRoleException.NotFound(id);
            }

            var response = MapModelToViewModel(instructorRole, instructorRole.Localizations);

            return response;
        }

        public InstructorRoleViewModel Update(Guid id, CreateInstructorRoleViewModel request, Guid userId)
        {
            var role = _instructorRoleRepo.Query()
                                          .Include(x => x.Localizations)
                                          .FirstOrDefault(x => x.Id == id);

            if (role is null)
            {
                throw new InstructorRoleException.NotFound(id);
            }

            var localizations = request.Localizations is null ? Enumerable.Empty<InstructorRoleLocalization>()
                                                              : (from localize in request.Localizations
                                                                 select new InstructorRoleLocalization
                                                                 {
                                                                     InstructorRoleId = role.Id,
                                                                     Language = localize.Language,
                                                                     Name = localize.Name
                                                                 })
                                                                .ToList();

            role.Name = request.Name;
            role.IsActive = request.IsActive;
            role.UpdatedAt = DateTime.UtcNow;
            role.UpdatedBy = ""; // TODO : Add requester

            _uow.BeginTran();
            _instructorRoleRepo.Update(role);
            _instructorRoleLocalizationRepo.DeleteRange(role.Localizations.ToList());

            if (localizations.Any())
            {
                _instructorRoleLocalizationRepo.AddRange(localizations);
            }

            _uow.Complete();
            _uow.CommitTran();

            var response = MapModelToViewModel(role, localizations);

            return response;
        }

        public void Delete(Guid id)
        {
            var instructorRole = _instructorRoleRepo.Query()
                                                    .FirstOrDefault(x => x.Id == id);

            if (instructorRole is null)
            {
                return;
            }

            var rolesToUpdate = _instructorRoleRepo.Query()
                                                   .Where(x => x.Sequence > instructorRole.Sequence)
                                                   .OrderBy(x => x.Sequence)
                                                   .ToList();

            _uow.BeginTran();

            foreach (var role in rolesToUpdate)
            {
                role.Sequence--;
                _instructorRoleRepo.Update(role);
            }

            _instructorRoleRepo.Delete(instructorRole);
            _uow.Complete();
            _uow.CommitTran();
        }

        private static BaseDropDownViewModel MapViewModelToDropDown(InstructorRoleViewModel viewModel)
        {
            var response = new BaseDropDownViewModel
            {
                Id = viewModel.Id.ToString(),
                Name = viewModel.Name
            };

            return response;
        }

        private static InstructorRoleViewModel MapModelToViewModel(InstructorRole model, IEnumerable<InstructorRoleLocalization> localizations)
        {
            var response = new InstructorRoleViewModel
            {
                Id = model.Id,
                Sequence = model.Sequence,
                IsActive = model.IsActive,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                Localizations = localizations is null ? Enumerable.Empty<InstructorRoleLocalizationViewModel>()
                                                      : (from localize in localizations
                                                         orderby localize.Language
                                                         select new InstructorRoleLocalizationViewModel
                                                         {
                                                             Language = localize.Language,
                                                             Name = localize.Name
                                                         })
                                                        .ToList()
            };

            return response;
        }

        private IQueryable<InstructorRole> GenerateSearchQuery(SearchInstructorRoleCriteriaViewModel? parameters = null)
        {
            var query = _instructorRoleRepo.Query()
                                           .Include(x => x.Localizations);

            if (parameters is not null)
            {
                if (!string.IsNullOrEmpty(parameters.Name))
                {
                    query.Where(x => x.Name.Contains(parameters.Name));
                }

                if (!string.IsNullOrEmpty(parameters.SortBy))
                {
                    query.OrderBy(parameters.SortBy, parameters.OrderBy);
                }
            }

            query.OrderBy(x => x.Sequence);

            return query;
        }
    }
}