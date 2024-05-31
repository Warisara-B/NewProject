using Microsoft.EntityFrameworkCore;
using Plexus.Client.Exceptions;
using Plexus.Client.ViewModel.Research;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Database;
using Plexus.Database.Model.Localization.Research;
using Plexus.Database.Model.Research;
using Plexus.Utility.Extensions;
using Plexus.Utility.ViewModel;

namespace Plexus.Client.src.Research
{
    public class ResearchTemplateManager : IResearchTemplateManager
    {
        private readonly DatabaseContext _dbContext;

        public ResearchTemplateManager(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ResearchTemplateViewModel Create(UpsertResearchTemplateViewModel request, Guid userId)
        {
            var (template, templateLocalizations) = MapTemplateViewModelToModel(request, userId.ToString());
            var (sequences, sequenceLocalizations) = MapSequenceViewModelToModel(template, request.Sequences);

            using(var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.ResearchTemplates.Add(template);
                if (templateLocalizations.Any())
                {
                    _dbContext.ResearchTemplateLocalizations.AddRange(templateLocalizations);
                }

                if (sequences.Any())
                {
                    _dbContext.ResearchTemplateSequences.AddRange(sequences);
                    if (sequenceLocalizations.Any())
                    {
                        _dbContext.ResearchTemplateSequenceLocalizations.AddRange(sequenceLocalizations);
                    }
                }

                transaction.Commit();
            }
            _dbContext.SaveChanges();

            // MAP PREP DATA
            template.Localizations = templateLocalizations;
            foreach(var sequence in sequences)
            {
                sequence.Localizations = sequenceLocalizations.Where(x => x.SequenceId == sequence.Id);
            }

            var response = MapTemplateToViewModel(template, sequences);
            return response;
        }

        public PagedViewModel<ResearchTemplateListViewModel> Search(SearchResearchTemplateCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var query = GenerateSearchQuery(parameters);

            var pagedResearchTemplate = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<ResearchTemplateListViewModel>
            {
                Page = pagedResearchTemplate.Page,
                TotalPage = pagedResearchTemplate.TotalPage,
                TotalItem = pagedResearchTemplate.TotalItem,
                Items = (from researchTemplate in pagedResearchTemplate.Items
                         select new ResearchTemplateListViewModel
                         {
                             Id = researchTemplate.Id,
                             Name = researchTemplate.Name,
                             IsActive = researchTemplate.IsActive,
                             UpdatedAt = researchTemplate.UpdatedAt
                         })
                        .ToList()
            };

            return response;
        }

        public ResearchTemplateViewModel GetById(Guid id)
        {
            var template = _dbContext.ResearchTemplates.AsNoTracking()
                                                       .Include(x => x.Localizations)
                                                       .Include(x => x.Sequences)
                                                            .ThenInclude(x => x.Localizations)
                                                       .SingleOrDefault(x => x.Id == id);

            if(template is null)
            {
                throw new ResearchException.TemplateNotFound(id);
            }

            var response = MapTemplateToViewModel(template, template.Sequences);
            return response;
        }

        public ResearchTemplateViewModel Update(Guid id, UpsertResearchTemplateViewModel request, Guid userId)
        {
            var template = _dbContext.ResearchTemplates.Include(x => x.Localizations)
                                                       .Include(x => x.Sequences)
                                                       .SingleOrDefault(x => x.Id == id);
            if (template is null)
            {
                throw new ResearchException.TemplateNotFound(id);
            }

            template.Name = request.Name;
            template.IsActive = request.IsActive;
            template.UpdatedAt = DateTime.UtcNow;
            template.UpdatedBy = userId.ToString();

            var (tmp, templateLocalizations) = MapTemplateViewModelToModel(request, userId.ToString(), template.Id);
            var (newSequences, newSequenceLocalizations) = MapSequenceViewModelToModel(template, request.Sequences, template.Id);

            using(var transaction = _dbContext.Database.BeginTransaction())
            {
                if (template.Localizations.Any())
                {
                    _dbContext.ResearchTemplateLocalizations.RemoveRange(template.Localizations);
                }

                _dbContext.ResearchTemplateLocalizations.AddRange(templateLocalizations);

                if (template.Sequences.Any())
                {
                    _dbContext.ResearchTemplateSequences.RemoveRange(template.Sequences);
                }

                if (newSequences.Any())
                {
                    _dbContext.ResearchTemplateSequences.AddRange(newSequences);
                    if (newSequenceLocalizations.Any())
                    {
                        _dbContext.ResearchTemplateSequenceLocalizations.AddRange(newSequenceLocalizations);
                    }
                }

                transaction.Commit();
            }
            _dbContext.SaveChanges();

            // MAP PREP DATA
            template.Localizations = templateLocalizations;
            foreach (var sequence in newSequences)
            {
                sequence.Localizations = newSequenceLocalizations.Where(x => x.SequenceId == sequence.Id)
                                                                 .ToList();
            }

            var response = MapTemplateToViewModel(template, newSequences);
            return response;
        }

        public void Delete(Guid id)
        {
            var template = _dbContext.ResearchTemplates.AsNoTracking()
                                                       .Include(x => x.Sequences)
                                                       .SingleOrDefault(x => x.Id == id);
            if (template is null)
            {
                return;
            }

            using(var transaction = _dbContext.Database.BeginTransaction())
            {
                if (template.Sequences.Any())
                {
                    _dbContext.ResearchTemplateSequences.RemoveRange(template.Sequences);
                }
                _dbContext.ResearchTemplates.Remove(template);
                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        private static (ResearchTemplate, IEnumerable<ResearchTemplateLocalization>)
            MapTemplateViewModelToModel(UpsertResearchTemplateViewModel request, string requester, Guid? templateId = null)
        {
            var templateModel = new ResearchTemplate
            {
                Name = request.Name,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = requester
            };

            if (request.Localizations is null || !request.Localizations.Any())
            {
                return (templateModel, Enumerable.Empty<ResearchTemplateLocalization>());
            }

            var localization = (from locale in request.Localizations
                                select new ResearchTemplateLocalization
                                {
                                    Language = locale.Language,
                                    Name = locale.Name,
                                    ResearchTemplateId = templateId.HasValue ? templateId.Value : default,
                                    ResearchTemplate = templateId.HasValue ? null : templateModel
                                })
                               .ToList();

            return (templateModel, localization);
        }

        private static (IEnumerable<ResearchTemplateSequence>, IEnumerable<ResearchTemplateSequenceLocalization>)
            MapSequenceViewModelToModel(ResearchTemplate templateModel, IEnumerable<UpsertResearchTemplateSequenceViewModel >? sequences, Guid? templateId = null)
        {
            if(sequences is null || !sequences.Any())
            {
                return (Enumerable.Empty<ResearchTemplateSequence>(),
                        Enumerable.Empty<ResearchTemplateSequenceLocalization>());
            }

            var sequenceModels = new List<ResearchTemplateSequence>();
            var sequenceLocalizations = new List<ResearchTemplateSequenceLocalization>();
            var ordering = 0;

            foreach(var sequence in sequences)
            {
                var sequenceModel = new ResearchTemplateSequence
                {
                    Name = sequence.Name,
                    Ordering = ordering,
                    Type = sequence.Type,
                    FilePrefix = sequence.FilePrefix,
                    ResearchTemplateId = templateId.HasValue ? templateId.Value : default,
                    ResearchTemplate = templateId.HasValue ? null : templateModel
                };

                sequenceModels.Add(sequenceModel);
                ordering++;

                if (sequence.Localizations is null || !sequence.Localizations.Any())
                {
                    continue;
                }

                var localizations = (from locale in sequence.Localizations
                                     select new ResearchTemplateSequenceLocalization
                                     {
                                         Language = locale.Language,
                                         Name = locale.Name,
                                         ResearchTemplateSequence = sequenceModel
                                     })
                                    .ToList();

                sequenceLocalizations.AddRange(localizations);
            }

            return (sequenceModels, sequenceLocalizations);
        }

        private static ResearchTemplateViewModel MapTemplateToViewModel(
            ResearchTemplate template, IEnumerable<ResearchTemplateSequence>? sequences)
        {
            var templateViewModel = new ResearchTemplateViewModel
            {
                Id = template.Id,
                Name = template.Name,
                IsActive = template.IsActive,
                Localizations = Enumerable.Empty< ResearchTemplateLocalizationViewModel >(),
                Sequences = Enumerable.Empty<ResearchTemplateSequenceViewModel>(),
                CreatedAt = template.CreatedAt,
                UpdatedAt = template.UpdatedAt
            };

            if (template.Localizations is not null && template.Localizations.Any())
            {
                var localizations = (from locale in template.Localizations
                                     select new ResearchTemplateLocalizationViewModel
                                     {
                                         Language = locale.Language,
                                         Name = locale.Name
                                     })
                                    .ToList();

                templateViewModel.Localizations = localizations;
            }

            if (sequences is null || !sequences.Any())
            {
                return templateViewModel;
            }

            var sequenceViewModels = new List<ResearchTemplateSequenceViewModel>();

            foreach(var sequence in sequences.OrderBy(x => x.Ordering))
            {
                var sequenceViewModel = new ResearchTemplateSequenceViewModel
                {
                    Id = sequence.Id,
                    Name = sequence.Name,
                    FilePrefix = sequence.FilePrefix,
                    Type = sequence.Type,
                    Localizations = Enumerable.Empty<ResearchTemplateSequenceLocalizationViewModel>()
                };

                if (sequence.Localizations is null || !sequence.Localizations.Any())
                {
                    sequenceViewModels.Add(sequenceViewModel);
                    continue;
                }

                var localizations = (from locale in sequence.Localizations
                                     select new ResearchTemplateSequenceLocalizationViewModel
                                     {
                                         Language = locale.Language,
                                         Name = locale.Name
                                     })
                                    .ToList();

                sequenceViewModel.Localizations = localizations;
                sequenceViewModels.Add(sequenceViewModel);
            }

            templateViewModel.Sequences = sequenceViewModels;

            return templateViewModel;
        }

        private IQueryable<ResearchTemplate> GenerateSearchQuery(SearchResearchTemplateCriteriaViewModel? parameters = null)
        {
            var query = _dbContext.ResearchTemplates.AsNoTracking();

            if (parameters is not null)
            {
                if (!string.IsNullOrEmpty(parameters.Keyword))
                {
                    query = query.Where(x => !string.IsNullOrEmpty(x.Name)
                                        && x.Name.Contains(parameters.Keyword));
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
                    catch (Exception)
                    {
                        // invalid property name
                    }

                }
            }

            return query;
        }
    }
}