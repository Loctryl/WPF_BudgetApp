using WPF_BudgetApp.Data.Models;

namespace WPF_BudgetApp.Services.Interfaces;

public interface IAccountService
{
	Task<List<Account>> GetAllAccountAsync(uint userId);
	Task<Account?> GetAccountByIdAsync(uint userId, uint accountId);
	Task<Account> CreateAccountAsync(Account bankAccount);
	Task<Account?> UpdateAccountAsync(uint userId, uint accountId);
	Task<Account?> UpdateCashAccountAsync(uint userId, uint accountId);
	Task<Account?> DeleteAccountAsync(uint userId, uint accountId);
}