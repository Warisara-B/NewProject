using Microsoft.EntityFrameworkCore;
using Plexus.Database;
using Plexus.Database.Model;
using Plexus.Entity.DTO;
using Plexus.Entity.Exception;

namespace Plexus.Entity.Provider.src
{
    public class StudentAddressProvider : IStudentAddressProvider
    {
        private readonly DatabaseContext _dbContext;

        public StudentAddressProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public StudentAddressDTO Create(CreateStudentAddressDTO request, string requester)
        {
            var model = new StudentAddress
            {
                StudentId = request.StudentId,
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
                CreatedBy = requester,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = requester
            };

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.StudentAddresses.Add(model);

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(model);

            return response;
        }

        public IEnumerable<StudentAddressDTO> GetByStudentId(Guid stduentId)
        {
            var addresses = _dbContext.StudentAddresses.AsNoTracking()
                                                             .Where(x => x.StudentId == stduentId)
                                                             .ToList();

            var response = (from address in addresses
                            select MapModelToDTO(address))
                           .ToList();

            return response;
        }

        public StudentAddressDTO GetById(Guid id)
        {
            var address = _dbContext.StudentAddresses.AsNoTracking()
                                                            .SingleOrDefault(x => x.Id == id);

            if (address is null)
            {
                throw new StudentException.AddressNotFound(id);
            }

            var response = MapModelToDTO(address);

            return response;
        }

        public StudentAddressDTO Update(StudentAddressDTO request, string requester)
        {
            var address = _dbContext.StudentAddresses.SingleOrDefault(x => x.Id == request.Id);

            if (address is null)
            {
                throw new StudentException.AddressNotFound(request.Id);
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
                address.UpdatedBy = requester;

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(address);

            return response;
        }

        // public void ClearMainAddress(Guid studentId, string requester)
        // {
        //     var contacts = _dbContext.StudentAddresses.Where(x => x.StudentId == studentId
        //                                                                  && x.IsMainAddress)
        //                                                      .ToList();

        //     using (var transaction = _dbContext.Database.BeginTransaction())
        //     {
        //         foreach (var contact in contacts)
        //         {
        //             contact.IsMainAddress = false;
        //             contact.UpdatedAt = DateTime.UtcNow;
        //             contact.UpdatedBy = requester;
        //         }

        //         transaction.Commit();
        //     }

        //     _dbContext.SaveChanges();
        // }

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

        private static StudentAddressDTO MapModelToDTO(StudentAddress model)
        {
            var response = new StudentAddressDTO
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