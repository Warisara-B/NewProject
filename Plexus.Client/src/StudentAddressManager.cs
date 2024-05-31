using Plexus.Client.ViewModel;
using Plexus.Database;
using Plexus.Database.Model;
using Plexus.Entity.Exception;
using Plexus.Entity.Provider;

namespace Plexus.Client.src
{
    public class StudentAddressManager : IStudentAddressManager
    {
        private DatabaseContext _dbContext;

        public StudentAddressManager(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public StudentAddressViewModel Create(Guid studentId, CreateStudentAddressViewModel request, Guid userId)
        {
            var student = _dbContext.Students.FirstOrDefault(x => x.Id == studentId);

            if (student is null)
            {
                throw new StudentException.NotFound(studentId);
            }

            var address = new StudentAddress
            {
                Student = student,
                Type = request.Type,
                Address1 = request.Address1,
                Address2 = request.Address2,
                HouseNumber = request.HouseNumber,
                Moo = request.Moo,
                Soi = request.Soi,
                Road = request.Road,
                Province = request.Province,
                District = request.District,
                SubDistrict = request.SubDistrict,
                Country = request.Country,
                PostalCode = request.PostalCode,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "", // TODO : Add requester.
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = "" // TODO : Add requester.
            };

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.StudentAddresses.Add(address);

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToViewModel(address);

            return response;
        }

        public IEnumerable<StudentAddressViewModel> GetByStudentId(Guid studentId)
        {
            var addresses = _dbContext.StudentAddresses.Where(x => x.StudentId == studentId)
                                                       .ToList();

            var response = (from address in addresses
                            select MapModelToViewModel(address))
                           .ToList();

            return response;
        }

        public StudentAddressViewModel GetById(Guid id)
        {
            var address = _dbContext.StudentAddresses.SingleOrDefault(x => x.Id == id);

            if (address is null)
            {
                throw new StudentException.AddressNotFound(id);
            }

            var response = MapModelToViewModel(address);

            return response;
        }

        public StudentAddressViewModel Update(Guid id, CreateStudentAddressViewModel request, Guid userId)
        {
            var address = _dbContext.StudentAddresses.SingleOrDefault(x => x.Id == id);

            if (address is null)
            {
                throw new StudentException.AddressNotFound(id);
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                address.Type = request.Type;
                address.Address1 = request.Address1;
                address.Address2 = request.Address2;
                address.HouseNumber = request.HouseNumber;
                address.Moo = request.Moo;
                address.Soi = request.Soi;
                address.Road = request.Road;
                address.Province = request.Province;
                address.District = request.District;
                address.SubDistrict = request.SubDistrict;
                address.Country = request.Country;
                address.PostalCode = request.PostalCode;
                address.UpdatedAt = DateTime.UtcNow;
                address.UpdatedBy = ""; // TODO : Add requester

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToViewModel(address);

            return response;
        }

        public void Delete(Guid id)
        {
            var address = _dbContext.StudentAddresses.SingleOrDefault(x => x.Id == id);

            if (address is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.StudentAddresses.Remove(address);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        private static StudentAddressViewModel MapModelToViewModel(StudentAddress model)
        {
            var response = new StudentAddressViewModel
            {
                Id = model.Id,
                StudentId = model.StudentId,
                Type = model.Type,
                Address1 = model.Address1,
                Address2 = model.Address2,
                HouseNumber = model.HouseNumber,
                Moo = model.Moo,
                Soi = model.Soi,
                Road = model.Road,
                Province = model.Province,
                District = model.District,
                SubDistrict = model.SubDistrict,
                Country = model.Country,
                PostalCode = model.PostalCode,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt
            };

            return response;
        }
    }
}