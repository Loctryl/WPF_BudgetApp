using WPF_BudgetApp.Data.Models;

namespace WPF_BudgetApp.Services.Interfaces;

public interface IArchivedTransferService
{
	Task<List<ArchivedTransfer>> GetAllArchivedTransfersAsync(string userId);
	Task<ArchivedTransfer?> GetArchivedTransferByIdAsync(string userId, uint archivedTransferId);
	Task<ArchivedTransfer> CreateArchivedTransferAsync(ArchivedTransfer transfer);
}