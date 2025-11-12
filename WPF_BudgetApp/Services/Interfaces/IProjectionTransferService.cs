using WPF_BudgetApp.Data.Models;

namespace WPF_BudgetApp.Services.Interfaces;

public interface IProjectionTransferService
{
	Task<List<ProjectionTransfer>> DebugGetAllProjectionTransfersAsync();

	Task<List<ProjectionTransfer>> GetAllProjectionTransferAsync(uint userId);
	Task<ProjectionTransfer?> GetProjectionTransferByIdAsync(uint userId, uint projectionTransferId);
	Task<List<ProjectionTransfer>> GetProjectionTransfersByAccountAsync(uint userId, uint accountId);
	Task<ProjectionTransfer> CreateProjectionTransferAsync(ProjectionTransfer transfer);
	Task UpdateProjectionTransferAsync();
	Task<ProjectionTransfer?> DeleteProjectionTransferAsync(uint userId, uint projectionTransferId);
}