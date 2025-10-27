using WPF_BudgetApp.Data.Models;

namespace WPF_BudgetApp.Services.Interfaces;

public interface IDebtService
{
	Task<List<Debt>> GetAllDebtAsync(uint userId);
	Task<Debt?> GetDebtByIdAsync(uint userId, uint debtId);
	Task<Debt> CreateDebtAsync(Debt debt);
	Task UpdateDebtAsync();
	Task<Debt?> DeleteDebtAsync(uint userId, uint debtId);
}