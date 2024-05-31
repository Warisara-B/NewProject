using System;
using Microsoft.EntityFrameworkCore;
using Plexus.Database;
using Plexus.Database.Model.Payment;
using Plexus.Entity.DTO.Payment;
using Plexus.Entity.Exception;

namespace Plexus.Entity.Provider.src.Payment
{
	public class StudentBankAccountProvider
	{
		private readonly DatabaseContext _dbContext;

		public StudentBankAccountProvider(DatabaseContext dbContext)
        {
			_dbContext = dbContext;
        }

		//public StudentBankAccountDTO Create(CreateStudentBankAccountDTO request)
  //      {
		//	var bank = _dbContext.Banks.AsNoTracking()
		//							   .SingleOrDefault(x => x.Id == request.BankId);

		//	if(bank is null || !bank.IsActive)
  //          {
		//		throw new BankException.NotFound(request.BankId);
  //          }

		//	var activeStudentBankAccounts = _dbContext.StudentBankAccounts.Where(x => x.StudentId == request.StudentId
		//																		      && x.IsActive)
		//															      .ToList();

		//	var model = new StudentBankAccount
		//	{
		//		BankId = request.BankId,
		//		StudentId = request.StudentId,
		//		AccountNumber = request.AccountNumber,
		//		AccountHolderName = request.AccountHolderName,
		//		IsActive = true,
		//		CreatedAt = DateTime.UtcNow,
		//		UpdatedAt = DateTime.UtcNow
		//	};

		//	using(var transaction = _dbContext.Database.BeginTransaction())
  //          {
		//		// ONLY NEW ONE IS ACTIVE ONLY
		//		foreach(var account in activeStudentBankAccounts)
  //              {
		//			account.IsActive = false;
		//			account.UpdatedAt = DateTime.UtcNow;
  //              }

		//		_dbContext.StudentBankAccounts.Add(model);

		//		transaction.Commit();
  //          }

		//	_dbContext.SaveChanges();

		//	var response = MapModelToDTO(model, bank);

		//	return response;
  //      }

		//public IEnumerable<StudentBankAccountDTO> GetByStudentId(Guid studentId)
  //      {
		//	var bankAccounts = _dbContext.StudentBankAccounts.AsNoTracking()
		//													 .Include(x => x.Bank)
		//													 .Where(x => x.StudentId == studentId)
		//													 .ToList();

		//	var response = (from account in bankAccounts
		//					orderby account.IsActive descending
		//					select MapModelToDTO(account, account.Bank))
		//				   .ToList();

		//	return response;
  //      }

		//public IEnumerable<StudentBankAccountDTO> GetByStudentId(IEnumerable<Guid> studentIds)
		//{
		//	var bankAccounts = _dbContext.StudentBankAccounts.AsNoTracking()
		//													 .Include(x => x.Bank)
		//													 .Where(x => studentIds.Contains(x.StudentId))
		//													 .ToList();

		//	var response = (from account in bankAccounts
		//					orderby account.StudentId, account.IsActive descending
		//					select MapModelToDTO(account, account.Bank))
		//				   .ToList();

		//	return response;
		//}

		private static StudentBankAccountDTO MapModelToDTO(StudentBankAccount model, Bank bank)
        {
			return new StudentBankAccountDTO
			{
				BankId = bank.Id,
				BankName = bank.Name,
				BankCode = bank.Code,
				BankIconImageFilePath = bank.IconFilePath,
				StudentId = model.StudentId,
				AccountHolderName = model.AccountHolderName,
				AccountNumber = model.AccountNumber,
				IsActive = model.IsActive
			};
        }
	}
}

