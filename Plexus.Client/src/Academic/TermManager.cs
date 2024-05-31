using Microsoft.EntityFrameworkCore;
using Plexus.Client.ViewModel;
using Plexus.Client.ViewModel.Academic;
using Plexus.Client.ViewModel.DropDown;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Database;
using Plexus.Database.Enum.Academic;
using Plexus.Database.Model.Academic;
using Plexus.Entity.DTO.Academic;
using Plexus.Entity.Exception;
using Plexus.Entity.Provider;
using Plexus.Utility.Extensions;
using Plexus.Utility.ViewModel;
using ServiceStack;

namespace Plexus.Client.src.Academic
{
    public class TermManager : ITermManager
    {
        private readonly ITermProvider _termProvider;
        private readonly IAcademicLevelProvider _academicLevelProvider;
        private readonly DatabaseContext _dbContext;

        public TermManager(ITermProvider termProvider,
                           IAcademicLevelProvider academicLevelProvider,
                           DatabaseContext dbContext)
        {
            _termProvider = termProvider;
            _academicLevelProvider = academicLevelProvider;
            _dbContext = dbContext;
        }

        public TermViewModel Create(CreateTermViewModel request, Guid userId)
        {
            var academicLevel = _academicLevelProvider.GetById(request.AcademicLevelId);

            var terms = _termProvider.GetByAcademiceLevel(request.AcademicLevelId)
                                     .ToList();

            var duplicateTerms = terms.Where(x => x.Year == request.Year
                                                  && x.Number == request.Number
                                                  && x.AcademicLevelId == request.AcademicLevelId)
                                      .ToList();

            if (duplicateTerms.Any())
            {
                throw new TermException.Duplicate();
            }

            var validDate = request.StartedAt < request.EndedAt;

            if (!validDate)
            {
                throw new TermException.InvalidDateRange();
            }

            var dto = new CreateTermDTO
            {
                Year = request.Year,
                Number = request.Number,
                Type = request.Type,
                CollegeCalendarType = request.CollegeCalendarType,
                IsCurrent = request.IsCurrent,
                IsRegistration = request.IsRegistration,
                IsAdvising = request.IsAdvising,
                IsSurveyed = request.IsSurveyed,
                AcademicLevelId = request.AcademicLevelId,
                StartedAt = request.StartedAt,
                EndedAt = request.EndedAt,
                TotalWeeks = request.TotalWeeks
            };

            var term = _termProvider.Create(dto, userId.ToString());

            var response = MapDTOToViewModel(term, academicLevel);

            return response;
        }

        public IEnumerable<BaseDropDownViewModel> GetDropDownList(SearchTermCriteriaViewModel parameters)
        {
            var terms = Search(parameters);

            var response = (from term in terms
                            orderby term.Year descending, term.Number descending
                            select MapViewModelToDropDown(term))
                           .ToList();

            return response;
        }

        public IEnumerable<TermViewModel> Search(SearchTermCriteriaViewModel parameters)
        {
            var query = GenerateSearchQuery(parameters);

            var terms = query.ToList();

            var response = (from term in terms
                            orderby term.IsCurrent descending, term.Year descending, term.Number descending
                            select MapTermViewModel(term))
                           .ToList();

            return response;
        }

        public PagedViewModel<TermViewModel> Search(SearchTermCriteriaViewModel criteria, int page, int pageSize)
        {
            var query = GenerateSearchQuery(criteria);

            var pagedTerm = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<TermViewModel>
            {
                Page = pagedTerm.Page,
                TotalPage = pagedTerm.TotalPage,
                TotalItem = pagedTerm.TotalItem,
                Items = (from term in pagedTerm.Items
                         select MapTermViewModel(term))
                        .ToList()
            };

            return response;
        }

        public TermViewModel GetById(Guid id)
        {
            var term = _termProvider.GetById(id);

            var academicLevel = _academicLevelProvider.GetById(term.AcademicLevelId);

            var response = MapDTOToViewModel(term, academicLevel);

            return response;
        }

        public TermViewModel Update(Guid id, CreateTermViewModel request, Guid userId)
        {
            var term = _termProvider.GetById(id);

            if (term is null)
            {
                throw new TermException.NotFound(id);
            }

            var academicLevel = _academicLevelProvider.GetById(request.AcademicLevelId);

            var terms = _termProvider.GetByAcademiceLevel(request.AcademicLevelId)
                                     .ToList();

            var duplicateTerms = terms.Where(x => x.Id != id
                                                  && x.Year == request.Year
                                                  && x.Number == request.Number
                                                  && x.AcademicLevelId == request.AcademicLevelId)
                                      .ToList();

            if (duplicateTerms.Any())
            {
                throw new TermException.Duplicate();
            }

            var validDate = request.StartedAt < request.EndedAt;

            if (!validDate)
            {
                throw new TermException.InvalidDateRange();
            }

            term.Year = request.Year;
            term.Number = request.Number;
            term.Type = request.Type;
            term.CollegeCalendarType = request.CollegeCalendarType;
            term.IsCurrent = request.IsCurrent;
            term.IsRegistration = request.IsRegistration;
            term.IsAdvising = request.IsAdvising;
            term.IsSurveyed = request.IsSurveyed;
            term.AcademicLevelId = request.AcademicLevelId;
            term.StartedAt = request.StartedAt;
            term.EndedAt = request.EndedAt;
            term.TotalWeeks = request.TotalWeeks;

            var updatedTerm = _termProvider.Update(term, userId.ToString());

            var response = MapDTOToViewModel(updatedTerm, academicLevel);

            return response;
        }

        public void Delete(Guid id)
        {
            _termProvider.Delete(id);
        }

        public TermViewModel CheckStatus(TermStatusCheckViewModel criteria)
        {
            var terms = _termProvider.GetByAcademiceLevel(criteria.AcademicLevelId)
                        .Where(t => t.Id != criteria.TermId
                        && t.CollegeCalendarType == criteria.CollegeCalendarType)
                        .ToList();

            var academicLevel = _academicLevelProvider.GetById(criteria.AcademicLevelId);

            TermViewModel? response = null;

            foreach (var term in terms)
            {
                bool isConflicted = false;

                switch (criteria.Status)
                {
                    case TermStatus.CURRENT:
                        isConflicted = term.IsCurrent;
                        break;

                    case TermStatus.ADVISING:
                        isConflicted = term.IsAdvising;
                        break;

                    case TermStatus.REGISTRATION:
                        isConflicted = term.IsRegistration;
                        break;

                    case TermStatus.SURVEY:
                        isConflicted = term.IsSurveyed;
                        break;
                }

                if (isConflicted)
                {
                    response = MapDTOToViewModel(term, academicLevel);
                    break;
                }
            }

            return response;
        }

        public static TermViewModel MapDTOToViewModel(TermDTO dto, AcademicLevelDTO? academicLevel)
        {
            var response = new TermViewModel
            {
                Id = dto.Id,
                Year = dto.Year,
                Number = dto.Number,
                Type = dto.Type,
                CollegeCalendarType = dto.CollegeCalendarType,
                AcademicLevelName = academicLevel?.Name,
                StartedAt = dto.StartedAt,
                EndedAt = dto.EndedAt,
                IsCurrent = dto.IsCurrent,
                IsRegistration = dto.IsRegistration,
                IsAdvising = dto.IsAdvising,
                IsSurveyed = dto.IsSurveyed,
                AcademicLevelId = dto.AcademicLevelId,
                TotalWeeks = dto.TotalWeeks,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt
            };

            return response;
        }

        private static TermViewModel MapTermViewModel(Term term)
        {
            if (term is null)
            {
                return null;
            }

            return new TermViewModel
            {
                Id = term.Id,
                Year = term.Year,
                Number = term.Number,
                Type = term.Type,
                CollegeCalendarType = term.CollegeCalendarType,
                IsCurrent = term.IsCurrent,
                IsRegistration = term.IsRegistration,
                IsAdvising = term.IsAdvising,
                IsSurveyed = term.IsSurveyed,
                StartedAt = term.StartedAt,
                EndedAt = term.EndedAt,
                AcademicLevelId = term.AcademicLevelId,
                AcademicLevelName = term.AcademicLevel.Name,
                TotalWeeks = term.TotalWeeks,
                CreatedAt = term.CreatedAt,
                UpdatedAt = term.UpdatedAt
            };
        }

        private static BaseDropDownViewModel MapViewModelToDropDown(TermViewModel viewModel)
        {
            var response = new BaseDropDownViewModel
            {
                Id = viewModel.Id.ToString(),
                Name = $"{viewModel.Number}/{viewModel.Year}"
            };

            return response;
        }

        private IQueryable<Term> GenerateSearchQuery(SearchTermCriteriaViewModel? parameters)
        {
            var query = _dbContext.Terms.Include(x => x.AcademicLevel)
                                            .ThenInclude(x => x.Localizations)
                                        .AsNoTracking();

            if (parameters is not null)
            {
                if (parameters.AcademicLevelId.HasValue)
                {
                    query = query.Where(x => x.AcademicLevelId == parameters.AcademicLevelId.Value);
                }

                if (!string.IsNullOrEmpty(parameters.Term))
                {
                    query = query.Where(x => x.Number.Contains(parameters.Term));
                }

                if (parameters.Year.HasValue)
                {
                    query = query.Where(x => x.Year == parameters.Year.Value);
                }
            }

            query = query.OrderBy(x => x.AcademicLevel.Name)
                         .ThenBy(x => x.Year)
                         .ThenBy(x => x.Number);

            if (parameters is not null)
            {
                if (!string.IsNullOrEmpty(parameters.SortBy))
                {
                    if (string.Equals(parameters.SortBy, "term", StringComparison.InvariantCultureIgnoreCase))
                    {
                        parameters.SortBy = "number";
                    }

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

