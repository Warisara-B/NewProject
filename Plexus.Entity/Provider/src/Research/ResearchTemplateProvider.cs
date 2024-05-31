using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Plexus.Database;
using Plexus.Database.Model.Localization.Research;
using Plexus.Database.Model.Research;
using Plexus.Entity.DTO.Research;
using Plexus.Entity.DTO.SearchFilter;
using Plexus.Entity.Exception;
using Plexus.Utility.Extensions;
using Plexus.Utility.ViewModel;
using ServiceStack;

namespace Plexus.Entity.Provider.src.Research
{
    public class ResearchTemplateProvider : IResearchTemplateProvider
    {
        private readonly DatabaseContext _dbContext;

        public ResearchTemplateProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ResearchTemplateDTO Create(CreateResearchTemplateDTO request, string requester)
        {
            // Serialize the request object to JSON
            string requestJson = JsonConvert.SerializeObject(request);

            // Print the JSON to console
            Console.WriteLine("Request JSON:");
            Console.WriteLine(requestJson);
            var template = new ResearchTemplate
            {
                Name = request.Name,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = requester,
            };

            var localizations = MapLocalizationDTOToModel(request.Localizations!, template).ToList();

            var sequences = MapSequenceDTOToModel(request.Sequences!, template).ToList();

            var sequenceLocalizations = sequences.SelectMany(seq => MapSequenceLocalizationDTOToModel(request.Sequences!.SelectMany(seq => seq.Localizations), seq)).ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.ResearchTemplates.Add(template);

                _dbContext.ResearchTemplateLocalizations.AddRange(localizations);

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

            var response = MapResearchTemplateModelToDTO(template, sequences);

            return response;
        }

        public PagedViewModel<ResearchTemplateDTO> Search(SearchResearchTemplateCriteriaDTO? parameters, int page = 1, int pageSize = 25)
        {
            var query = GenerateSearchQuery(parameters);

            var pagedResearchTemplate = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<ResearchTemplateDTO>
            {
                Page = pagedResearchTemplate.Page,
                TotalPage = pagedResearchTemplate.TotalPage,
                TotalItem = pagedResearchTemplate.TotalItem,
                Items = (from researchTemplate in pagedResearchTemplate.Items
                         select MapResearchTemplateModelToDTO(researchTemplate, researchTemplate.Sequences!))
                        .ToList()
            };

            return response;
        }

        public ResearchTemplateDTO GetById(Guid id)
        {
            var researchTemplate = _dbContext.ResearchTemplates
                                                .Include(x => x.Sequences)
                                                .AsNoTracking()
                                                .SingleOrDefault(x => x.Id == id);

            if (researchTemplate is null)
            {
                throw new ResearchTemplateException.NotFound(id);
            }

            var response = MapResearchTemplateModelToDTO(researchTemplate, researchTemplate.Sequences!);

            return response;
        }

        public ResearchTemplateDTO Update(ResearchTemplateDTO request, string requester)
        {
            var researchTemplate = _dbContext.ResearchTemplates
                                             .Include(x => x.Localizations)
                                             .Include(x => x.Sequences!)
                                                .ThenInclude(x => x.Localizations)
                                             .SingleOrDefault(x => x.Id == request.Id);

            if (researchTemplate is null)
            {
                throw new ResearchTemplateException.NotFound(request.Id);
            }

            var localization = new ResearchTemplateLocalization
            {
                ResearchTemplate = researchTemplate,
                Language = researchTemplate.Language,
                Name = request.Name
            };

            var sequences = MapSequenceDTOToModel(request.Sequences!, researchTemplate).ToList();

            var sequenceLocalizations = sequences.Select(x => new ResearchTemplateSequenceLocalization
            {
                ResearchTemplateSequence = x,
                Language = researchTemplate.Language,
                Name = x.Name
            }).ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                researchTemplate.Name = request.Name;
                researchTemplate.IsActive = request.IsActive;
                researchTemplate.UpdatedAt = DateTime.UtcNow;
                researchTemplate.UpdatedBy = requester;

                _dbContext.ResearchTemplateSequences.RemoveRange(researchTemplate.Sequences!);

                _dbContext.ResearchTemplateLocalizations.RemoveRange(researchTemplate.Localizations!);

                _dbContext.ResearchTemplateSequenceLocalizations.RemoveRange(researchTemplate.Sequences!.SelectMany(x => x.Localizations));

                if (sequences.Any())
                {
                    _dbContext.ResearchTemplateSequences.AddRange(sequences);

                    if (sequenceLocalizations.Any())
                    {
                        _dbContext.ResearchTemplateSequenceLocalizations.AddRange(sequenceLocalizations);
                    }
                }

                if (localization != null)
                {
                    _dbContext.ResearchTemplateLocalizations.AddRange(localization);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapResearchTemplateModelToDTO(researchTemplate, sequences);

            return response;
        }

        public void Delete(Guid id)
        {
            var researchTemplate = _dbContext.ResearchTemplates.SingleOrDefault(x => x.Id == id);

            if (researchTemplate is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.ResearchTemplates.Remove(researchTemplate);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        public void DeleteSequence(Guid sequenceId)
        {
            var sequence = _dbContext.ResearchTemplateSequences.SingleOrDefault(x => x.Id == sequenceId);

            if (sequence is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.ResearchTemplateSequences.Remove(sequence);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        private IQueryable<ResearchTemplate> GenerateSearchQuery(SearchResearchTemplateCriteriaDTO? parameters = null)
        {
            var query = _dbContext.ResearchTemplates
                                                .Include(x => x.Sequences)
                                                .AsNoTracking();

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

            query = query.OrderBy(x => x.UpdatedAt);

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

        private static ResearchTemplateDTO MapResearchTemplateModelToDTO(
                       ResearchTemplate model,
                       IEnumerable<ResearchTemplateSequence> sequences)
        {
            return new ResearchTemplateDTO
            {
                Id = model.Id,
                Name = model.Name,
                IsActive = model.IsActive,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                Sequences = (from sequence in sequences
                             select MapSequenceModelTODTO(sequence))
            };
        }

        private static ResearchTemplateSequenceDTO MapSequenceModelTODTO(
            ResearchTemplateSequence model)
        {
            return new ResearchTemplateSequenceDTO
            {
                Id = model.Id,
                Name = model.Name,
                FilePrefix = model.FilePrefix,
                Type = model.Type
            };
        }

        private static IEnumerable<ResearchTemplateLocalization> MapLocalizationDTOToModel(
                IEnumerable<ResearchTemplateLocalizationDTO> localizations,
                ResearchTemplate model
        )
        {
            if (localizations is null)
            {
                return Enumerable.Empty<ResearchTemplateLocalization>();
            }

            var response = (from locale in localizations
                            select new ResearchTemplateLocalization
                            {
                                ResearchTemplate = model,
                                Language = locale.Language,
                                Name = locale.Name
                            })
                            .ToList();

            return response;
        }

        private static IEnumerable<ResearchTemplateSequence> MapSequenceDTOToModel(
                IEnumerable<CreateResearchTemplateSequenceDTO> sequences,
                ResearchTemplate model
        )
        {
            if (sequences is null)
            {
                return Enumerable.Empty<ResearchTemplateSequence>();
            }

            var response = (from sequence in sequences
                            select new ResearchTemplateSequence
                            {
                                ResearchTemplate = model,
                                Name = sequence.Name,
                                FilePrefix = sequence.FilePrefix
                            })
                            .ToList();

            return response;
        }

        private static IEnumerable<ResearchTemplateSequenceLocalization> MapSequenceLocalizationDTOToModel(
                IEnumerable<ResearchTemplateSequenceLocalizationDTO> localizations,
                ResearchTemplateSequence model
        )
        {
            if (localizations is null)
            {
                return Enumerable.Empty<ResearchTemplateSequenceLocalization>();
            }

            var response = (from locale in localizations


                            select new ResearchTemplateSequenceLocalization
                            {
                                ResearchTemplateSequence = model,
                                Language = locale.Language,
                                Name = locale.Name
                            })
                            .ToList();

            return response;
        }
    }
}