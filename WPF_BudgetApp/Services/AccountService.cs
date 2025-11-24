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
		=> _context.Accounts.Include(x => x.AppUser)
			.AsQueryable().Where(s => s.AppUserId == userId);

	//Debug Func
	public Task<List<Account>> DebugGetAllAccountsAsync()
		=> _context.Accounts.Include(x => x.AppUser)
			.AsQueryable().ToListAsync();

	public async Task<List<Account>> GetAllAccountAsync(uint userId) 
		=> await CheckedListWithUser(userId).ToListAsync();

	public async Task<Account?> GetAccountByIdAsync(uint userId, uint accountId) 
		=> await CheckedListWithUser(userId).FirstOrDefaultAsync(x => x.Id == accountId);

	public async Task<Account> CreateAccountAsync(Account account)
	{
		await _context.Accounts.AddAsync(account);
		await _context.SaveChangesAsync();
		return account;
	}

	public async Task UpdateAccountAsync() => await _context.SaveChangesAsync();
	
	public async Task DeleteAccountAsync(uint userId, uint accountId)
	{
		var account = GetAccountByIdAsync(userId, accountId).Result;
		if (account == null)
			return;
		
		_context.Accounts.Remove(account);
		await _context.SaveChangesAsync();
	}
}