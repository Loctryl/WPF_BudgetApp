using WPF_BudgetApp.Data.Models;

namespace WPF_BudgetApp.Services.Interfaces;

public interface IDebtService
{
	Task<List<Debt>> GetAllDebtAsync(string userId);
	Task<Debt?> GetDebtByIdAsync(string userId, uint debtId);
	Task<Debt> CreateDebtAsync(Debt debt);
	Task<Debt?> UpdateDebtAsync(string userId, uint debtId);
	Task<Debt?> DeleteDebtAsync(string userId, uint debtId);
}