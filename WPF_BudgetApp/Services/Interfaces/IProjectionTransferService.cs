using WPF_BudgetApp.Data.Models;

namespace WPF_BudgetApp.Services.Interfaces;

public interface IProjectionTransferService
{
	Task<List<ProjectionTransfer>> GetAllProjectionTransferAsync(string userId);
	Task<ProjectionTransfer?> GetProjectionTransferByIdAsync(string userId, uint projectionTransferId);
	Task<List<ProjectionTransfer>> GetProjectionTransfersByAccountAsync(string userId, uint accountId);
	Task<ProjectionTransfer> CreateProjectionTransferAsync(ProjectionTransfer transfer);
	Task<ProjectionTransfer?> UpdateProjectionTransferAsync(string userId, uint projectionTransferId);
	Task<ProjectionTransfer?> DeleteProjectionTransferAsync(string userId, uint projectionTransferId);
}