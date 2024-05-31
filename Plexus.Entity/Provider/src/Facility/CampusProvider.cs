using Microsoft.EntityFrameworkCore;
using Plexus.Database;
using Plexus.Database.Model.Facility;
using Plexus.Database.Model.Localization.Facility;
using Plexus.Entity.DTO.Facility;
using Plexus.Entity.Exception;
using Plexus.Utility.Extensions;

namespace Plexus.Entity.Provider.src.Facility
{
    public class CampusProvider : ICampusProvider
    {
        private readonly DatabaseContext _dbContext;

        public CampusProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public CampusDTO Create(CreateCampusDTO request, string requester)
        {
            var model = new Campus
            {
                Name = request.Name,
                Code = request.Code,
                Address1 = request.Address1,
                Address2 = request.Address2,
                ContactNumber = request.ContactNumber,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = requester
            };

            var localizes = MapLocalizationDTOToModel(request.Localizations, model).ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Campuses.Add(model);

                if (localizes.Any())
                {
                    _dbContext.CampusLocalizations.AddRange(localizes);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(model, localizes);

            return response;
        }

        public IEnumerable<CampusDTO> GetAll()
        {
            var campuses = _dbContext.Campuses.AsNoTracking()
                                              .Include(x => x.Localizations)
                                              .ToList();

            var response = (from campus in campuses
                            orderby campus.Code
                            select MapModelToDTO(campus, campus.Localizations))
                           .ToList();

            return response;
        }

        public CampusDTO GetById(Guid id)
        {
            var campus = _dbContext.Campuses.AsNoTracking()
                                            .Include(x => x.Localizations)
                                            .SingleOrDefault(x => x.Id == id);

            if (campus is null)
            {
                throw new CampusException.NotFound(id);
            }

            var response = MapModelToDTO(campus, campus.Localizations);

            return response;
        }

        public CampusDTO Update(CampusDTO request, string requester)
        {
            var campus = _dbContext.Campuses.Include(x => x.Localizations)
                                            .SingleOrDefault(x => x.Id == request.Id);

            if (campus is null)
            {
                throw new CampusException.NotFound(request.Id);
            }

            var localizes = MapLocalizationDTOToModel(request.Localizations, campus).ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                campus.Name = request.Name;
                campus.Code = request.Code;
                campus.Address1 = request.Address1;
                campus.Address2 = request.Address2;
                campus.ContactNumber = request.ContactNumber;
                campus.IsActive = request.IsActive;
                campus.UpdatedAt = DateTime.UtcNow;
                campus.UpdatedBy = requester;

                _dbContext.CampusLocalizations.RemoveRange(campus.Localizations);

                if (localizes.Any())
                {
                    _dbContext.CampusLocalizations.AddRange(localizes);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(campus, localizes);

            return response;
        }

        public void Delete(Guid id)
        {
            var campus = _dbContext.Campuses.SingleOrDefault(x => x.Id == id);

            if (campus is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Campuses.Remove(campus);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }
        private static CampusDTO MapModelToDTO(Campus model, IEnumerable<CampusLocalization> localizations)
        {
            return new CampusDTO
            {
                Id = model.Id,
                Code = model.Code,
                Name = model.Name,
                Address1 = model.Address1,
                Address2 = model.Address2,
                ContactNumber = model.ContactNumber,
                IsActive = model.IsActive,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                Localizations = localizations is null ? Enumerable.Empty<CampusLocalizationDTO>()
                                                      : (from localize in localizations
                                                         orderby localize.Language
                                                         select new CampusLocalizationDTO
                                                         {
                                                             Language = localize.Language,
                                                             Name = localize.Name,
                                                             Address1 = localize.Address1,
                                                             Address2 = localize.Address2
                                                         })
                                                        .ToList()
            };
        }

        private static IEnumerable<CampusLocalization> MapLocalizationDTOToModel(
            IEnumerable<CampusLocalizationDTO>? localizations,
            Campus model)
        {
            if (localizations is null)
            {
                return Enumerable.Empty<CampusLocalization>();
            }

            var response = (from locale in localizations
                            select new CampusLocalization
                            {
                                Campus = model,
                                Language = locale.Language,
                                Name = locale.Name,
                                Address1 = locale.Address1,
                                Address2 = locale.Address2
                            })
                            .ToList();

            return response;
        }
    }
}

