using WPF_BudgetApp.Data.Models;

namespace WPF_BudgetApp.Services.Interfaces;

public interface IAccountService
{
	Task<List<Account>> GetAllAccountAsync(string userId);
	Task<Account?> GetAccountByIdAsync(string userId, uint accountId);
	Task<Account> CreateAccountAsync(Account bankAccount);
	Task<Account?> UpdateAccountAsync(string userId, uint accountId);
	Task<Account?> UpdateCashAccountAsync(string userId, uint accountId);
	Task<Account?> DeleteAccountAsync(string userId, uint accountId);
}