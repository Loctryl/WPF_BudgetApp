using WPF_BudgetApp.Data;
using WPF_BudgetApp.Data.Models;

namespace WPF_BudgetApp.Services.Interfaces;

public interface ITransferService
{
	Task<List<Transfer>> GetAllTransfersAsync(string userId, TransferQueryObject query);
	Task<Transfer?> GetTransferByIdAsync(string userId, uint transferId);
	Task<List<Transfer>> GetTransfersByAccountAsync(string userId, uint accountId);
	Task<Transfer> CreateTransferAsync(Transfer transfer);
	Task<Transfer?> UpdateTransferAsync(string userId, uint transferId);
	Task<Transfer?> DeleteTransferAsync(string userId, uint transferId);
}