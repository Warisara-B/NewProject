using Microsoft.EntityFrameworkCore;
using Plexus.Database;
using Plexus.Database.Model;
using Plexus.Entity.DTO;
using Plexus.Entity.Exception;

namespace Plexus.Entity.Provider.src
{
    public class StudentGuardianProvider : IStudentGuardianProvider
    {
        private readonly DatabaseContext _dbContext;

        public StudentGuardianProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public StudentGuardianDTO Create(CreateStudentGuardianDTO request, string requester)
        {
            var model = new StudentGuardian
            {
                StudentId = request.StudentId,
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
                CreatedBy = requester,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = requester
            };

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.StudentGuardians.Add(model);

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(model);

            return response;
        }

        public IEnumerable<StudentGuardianDTO> GetByStudentId(Guid studentId)
        {
            var guardians = _dbContext.StudentGuardians.AsNoTracking()
                                                       .Where(x => x.StudentId == studentId)
                                                       .ToList();

            var response = (from guardian in guardians
                            select MapModelToDTO(guardian))
                           .ToList();

            return response;
        }

        public StudentGuardianDTO GetById(Guid id)
        {
            var guardian = _dbContext.StudentGuardians.AsNoTracking()
                                                      .SingleOrDefault(x => x.Id == id);

            if (guardian is null)
            {
                throw new StudentException.GuardianNotFound(id);
            }

            var response = MapModelToDTO(guardian);

            return response;
        }

        public StudentGuardianDTO Update(StudentGuardianDTO request, string requester)
        {
            var guardian = _dbContext.StudentGuardians.SingleOrDefault(x => x.Id == request.Id);

            if (guardian is null)
            {
                throw new StudentException.GuardianNotFound(request.Id);
            }

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
                guardian.UpdatedBy = requester;

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(guardian);

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

        private static StudentGuardianDTO MapModelToDTO(StudentGuardian model)
        {
            return new StudentGuardianDTO
            {
                Id = model.Id,
                StudentId = model.StudentId,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                Relationship = model.Relationship,
                CitizenNo = model.CitizenNo,
                PhoneNumber = model.PhoneNumber,
                EmailAddress = model.EmailAddress,
                IsMainContact = model.IsMainContact,
                IsEmergencyContact = model.IsEmergencyContact,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt
            };
        }
    }
}