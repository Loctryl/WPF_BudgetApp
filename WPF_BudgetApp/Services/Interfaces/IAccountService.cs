using WPF_BudgetApp.Data.Models;

namespace WPF_BudgetApp.Services.Interfaces;

public interface IAccountService
{
	Task<List<Account>> DebugGetAllAccountsAsync();
	
	Task<List<Account>> GetAllAccountAsync(uint userId);
	Task<Account?> GetAccountByIdAsync(uint userId, uint accountId);
	Task<Account> CreateAccountAsync(uint userId, Account account);
	Task UpdateAccountAsync();
	
	Task<Account?> DeleteAccountAsync(uint userId, uint accountId);
}