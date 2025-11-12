using WPF_BudgetApp.Data.Models;

namespace WPF_BudgetApp.Services.Interfaces;

public interface IArchivedTransferService
{
	Task<List<ArchivedTransfer>> DebugGetAllArchivedTransfersAsync();
	
	Task<List<ArchivedTransfer>> GetAllArchivedTransfersAsync(uint userId);
	Task<ArchivedTransfer?> GetArchivedTransferByIdAsync(uint userId, uint archivedTransferId);
	Task<ArchivedTransfer> CreateArchivedTransferAsync(ArchivedTransfer transfer);
}