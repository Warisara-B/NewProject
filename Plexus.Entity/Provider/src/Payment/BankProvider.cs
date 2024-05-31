using System;
using Microsoft.EntityFrameworkCore;
using Plexus.Database;
using Plexus.Database.Model.Payment;
using Plexus.Entity.DTO.Payment;
using Plexus.Entity.Exception;

namespace Plexus.Entity.Provider.src.Payment
{
	public class BankProvider
	{
		private readonly DatabaseContext _dbContext;

		public BankProvider(DatabaseContext dbContext)
		{
			_dbContext = dbContext;
		}

		//public BankDTO Create(CreateBankDTO request)
  //      {
		//	var code = request.Code.ToUpper();
		//	var name = request.Name.ToLower();

		//	var duplicateBank = _dbContext.Banks.AsNoTracking()
		//										.Where(x => x.Code == code
		//													|| x.Name == name)
		//										.ToList();

  //          if (!duplicateBank.Any())
  //          {
		//		if(duplicateBank.Any(x => x.Code == code))
		//		{
		//			throw new BankException.Duplicate(code: code);
		//		}
  //              else
  //              {
		//			throw new BankException.Duplicate(name: name);
  //              }
  //          }

		//	var model = new Bank
		//	{
		//		Name = name,
		//		Code = code,
		//		IsActive = true,
		//		IconFilePath = request.IconImageFilePath,
		//		CreatedAt = DateTime.UtcNow,
		//		UpdatedAt = DateTime.UtcNow
		//	};

		//	using(var transaction = _dbContext.Database.BeginTransaction())
  //          {
		//		_dbContext.Banks.Add(model);

		//		transaction.Commit();
  //          }

		//	_dbContext.SaveChanges();

		//	var response = MapModelToDTO(model);

		//	return response;
  //      }

		//public IEnumerable<BankDTO> GetAll()
  //      {
		//	var banks = _dbContext.Banks.AsNoTracking()
		//								.ToList();

		//	var response = (from bank in banks
		//					orderby bank.Code
		//					select MapModelToDTO(bank))
		//				   .ToList();

		//	return response;
  //      }

		//public BankDTO GetById(Guid bankId)
  //      {
		//	var bank = _dbContext.Banks.AsNoTracking()
		//							   .SingleOrDefault(x => x.Id == bankId);

		//	if(bank is null)
  //          {
		//		throw new BankException.NotFound(bankId);
  //          }

		//	var response = MapModelToDTO(bank);

		//	return response;
  //      }

		//public IEnumerable<BankDTO> GetById(IEnumerable<Guid> bankIds)
		//{
		//	var banks = _dbContext.Banks.AsNoTracking()
		//							    .Where(x => bankIds.Contains(x.Id))
		//							    .ToList();

		//	var response = (from bank in banks
		//					orderby bank.Code
		//					select MapModelToDTO(bank))
		//				   .ToList();

		//	return response;
		//}

		//public BankDTO Update(BankDTO request)
  //      {
		//	var code = request.Code.ToUpper();
		//	var name = request.Name.ToLower();

		//	var banks = _dbContext.Banks.Where(x => x.Id == request.Id
		//											|| x.Code == code
		//											|| x.Name == name)
		//								.ToList();

		//	var updateBank = banks.SingleOrDefault(x => x.Id == request.Id);

		//	if(updateBank is null)
  //          {
		//		throw new BankException.NotFound(request.Id);
  //          }

		//	var duplicateData = banks.Where(x => x.Id != request.Id).ToList();

  //          if (duplicateData.Any())
  //          {
		//		if (duplicateData.Any(x => x.Code == code))
		//		{
		//			throw new BankException.Duplicate(code: code);
		//		}
		//		else
		//		{
		//			throw new BankException.Duplicate(name: name);
		//		}
		//	}

		//	updateBank.Name = name;
		//	updateBank.Code = code;
		//	updateBank.IconFilePath = request.IconImageFilePath;
		//	updateBank.IsActive = request.IsActive;
		//	updateBank.UpdatedAt = DateTime.UtcNow;

		//	_dbContext.SaveChanges();

		//	var response = MapModelToDTO(updateBank);

		//	return response;
  //      }

		private static BankDTO MapModelToDTO(Bank model)
        {
			return new BankDTO
			{
				Id = model.Id,
				Name = model.Name,
				Code = model.Code,
				IsActive = model.IsActive,
				IconImageFilePath = model.IconFilePath
			};
        }
	}
}

