using Microsoft.EntityFrameworkCore;
using Plexus.Client.ViewModel;
using Plexus.Database;
using Plexus.Database.Model;
using Plexus.Database.Model.Localization;
using Plexus.Entity.Exception;

namespace Plexus.Client.src
{
    public class StudentGuardianManager : IStudentGuardianManager
    {
        private readonly DatabaseContext _dbContext;

        public StudentGuardianManager(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public StudentGuardianViewModel Create(Guid studentId, CreateStudentGuardianViewModel request, Guid userId)
        {
            var student = _dbContext.Students.SingleOrDefault(x => x.Id == studentId);

            if (student is null)
            {
                throw new StudentException.NotFound(studentId);
            }

            var model = new StudentGuardian
            {
                Student = student,
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                LastName = request.LastName,
                Relationship = request.Relationship,
                CitizenNo = request.CitizenNo,
                PhoneNumber = request.PhoneNumber,
                EmailAddress = request.EmailAddress,
                IsMainContact = request.IsMainContact,
                IsEmergencyContact = request.IsEmergencyContact,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "", // TODO : Add requester
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = "" // TODO : Add requester
            };

            var localizations = MapGuardianLocalizationsModel(request.Localizations, model);

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.StudentGuardians.Add(model);

                if (localizations.Any())
                {
                    _dbContext.StudentGuardianLocalizations.AddRange(localizations);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapStudentGuardianViewModel(model, localizations);

            return response;
        }

        public IEnumerable<StudentGuardianViewModel> GetByStudentId(Guid studentId)
        {
            var guardians = _dbContext.StudentGuardians.AsNoTracking()
                                                       .Include(x => x.Localizations)
                                                       .Where(x => x.StudentId == studentId)
                                                       .ToList();

            var response = (from guardian in guardians
                            select MapStudentGuardianViewModel(guardian, guardian.Localizations))
                           .ToList();

            return response;
        }

        public StudentGuardianViewModel GetById(Guid id)
        {
            var guardian = _dbContext.StudentGuardians.AsNoTracking()
                                                      .Include(x => x.Localizations)
                                                      .SingleOrDefault(x => x.Id == id);

            if (guardian is null)
            {
                throw new StudentException.GuardianNotFound(id);
            }

            var response = MapStudentGuardianViewModel(guardian, guardian.Localizations);

            return response;
        }

        public StudentGuardianViewModel Update(Guid id, CreateStudentGuardianViewModel request, Guid userId)
        {
            var guardian = _dbContext.StudentGuardians.Include(x => x.Localizations)
                                                      .SingleOrDefault(x => x.Id == id);

            if (guardian is null)
            {
                throw new StudentException.GuardianNotFound(id);
            }

            var localizations = MapGuardianLocalizationsModel(request.Localizations, guardian);

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                guardian.FirstName = request.FirstName;
                guardian.MiddleName = request.MiddleName;
                guardian.LastName = request.LastName;
                guardian.Relationship = request.Relationship;
                guardian.CitizenNo = request.CitizenNo;
                guardian.PhoneNumber = request.PhoneNumber;
                guardian.EmailAddress = request.EmailAddress;
                guardian.IsMainContact = request.IsMainContact;
                guardian.IsEmergencyContact = request.IsEmergencyContact;
                guardian.UpdatedAt = DateTime.UtcNow;
                guardian.UpdatedBy = ""; // TODO : Add requester

                _dbContext.StudentGuardianLocalizations.RemoveRange(guardian.Localizations);

                if (localizations.Any())
                {
                    _dbContext.StudentGuardianLocalizations.AddRange(localizations);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapStudentGuardianViewModel(guardian, guardian.Localizations);

            return response;
        }

        public void Delete(Guid id)
        {
            var guardian = _dbContext.StudentGuardians.SingleOrDefault(x => x.Id == id);

            if (guardian is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.StudentGuardians.Remove(guardian);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        private static StudentGuardianViewModel MapStudentGuardianViewModel(StudentGuardian model, IEnumerable<StudentGuardianLocalization>? localizations)
        {
            return new StudentGuardianViewModel
            {
                Id = model.Id,
                StudentId = model.StudentId,
                Relationship = model.Relationship,
                CitizenNo = model.CitizenNo,
                PhoneNumber = model.PhoneNumber,
                EmailAddress = model.EmailAddress,
                IsMainContact = model.IsMainContact,
                IsEmergencyContact = model.IsEmergencyContact,
                Localizations = localizations is null ? Enumerable.Empty<StudentGuardianLocalizationViewModel>()
                                                      : (from locale in localizations
                                                         orderby locale.Language
                                                         select new StudentGuardianLocalizationViewModel
                                                         {
                                                             Language = locale.Language,
                                                             FirstName = locale.FirstName,
                                                             MiddleName = locale.MiddleName,
                                                             LastName = locale.LastName
                                                         })
                                                         .ToList(),
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt
            };
        }

        private static IEnumerable<StudentGuardianLocalization> MapGuardianLocalizationsModel(IEnumerable<StudentGuardianLocalizationViewModel>? localizations, StudentGuardian model)
        {
            if (localizations is null)
            {
                return Enumerable.Empty<StudentGuardianLocalization>();
            }

            var response = (from locale in localizations
                            orderby locale.Language
                            select new StudentGuardianLocalization
                            {
                                StudentGuardian = model,
                                Language = locale.Language,
                                FirstName = locale.FirstName,
                                MiddleName = locale.MiddleName,
                                LastName = locale.LastName
                            })
                            .ToList();

            return response;
        }
    }
}