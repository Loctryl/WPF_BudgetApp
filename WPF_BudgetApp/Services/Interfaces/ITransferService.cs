using WPF_BudgetApp.Data;
using WPF_BudgetApp.Data.Models;

namespace WPF_BudgetApp.Services.Interfaces;

public interface ITransferService
{
	Task<List<Transfer>> DebugGetAllTransferAsync();

	Task<List<Transfer>> GetAllTransfersAsync(uint userId);
	Task<Transfer?> GetTransferByIdAsync(uint userId, uint transferId);
	Task<List<Transfer>> GetTransfersByAccountAsync(uint userId, uint accountId);
	Task<Transfer> CreateTransferAsync(Transfer transfer);
	Task UpdateTransferAsync();
	Task<Transfer?> DeleteTransferAsync(uint userId, uint transferId);
}