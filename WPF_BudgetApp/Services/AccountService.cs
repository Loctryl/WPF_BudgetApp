using Microsoft.EntityFrameworkCore;
using WPF_BudgetApp.Data;
using WPF_BudgetApp.Data.Models;
using WPF_BudgetApp.Services.Interfaces;

namespace WPF_BudgetApp.Services;

public class AccountService : ServiceBase<Account>, IAccountService
{
	public AccountService(AppDbContext context) : base(context)
	{
	}
	
	protected override IQueryable<Account> CheckedListWithUser(uint userId) 
		=> _context.BankAccounts.Include(x => x.AppUser)
			.Include(x => x.Transfers)
			.Include(x => x.ProjectionTransfers)
			.AsQueryable().Where(s => s.AppUserId == userId);
	
	public async Task<List<Account>> GetAllAccountAsync(uint userId) 
		=> await CheckedListWithUser(userId).ToListAsync();

	public async Task<Account?> GetAccountByIdAsync(uint userId, uint accountId) 
		=> await CheckedListWithUser(userId).FirstOrDefaultAsync(x => x.Id == accountId);

	public async Task<Account> CreateAccountAsync(Account bankAccount)
	{
		await _context.BankAccounts.AddAsync(bankAccount);
		await _context.SaveChangesAsync();
		return bankAccount;
	}

	public Task<Account?> UpdateAccountAsync(uint userId, uint accountId)
	{
		throw new NotImplementedException();
	}

	public Task<Account?> UpdateCashAccountAsync(uint userId, uint accountId)
	{
		throw new NotImplementedException();
	}

	public async Task<Account?> DeleteAccountAsync(uint userId, uint accountId)
	{
		var bankAccount = GetAccountByIdAsync(userId, accountId).Result;
		if (bankAccount == null)
			return null;
		
		_context.BankAccounts.Remove(bankAccount);
		await _context.SaveChangesAsync();
		return bankAccount;
	}
}