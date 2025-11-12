using Microsoft.EntityFrameworkCore;
using WPF_BudgetApp.Data;
using WPF_BudgetApp.Data.Models;
using WPF_BudgetApp.Services.Interfaces;

namespace WPF_BudgetApp.Services;

public class ProjectionTransferService : ServiceBase<ProjectionTransfer>, IProjectionTransferService
{
	public ProjectionTransferService(AppDbContext context) : base(context)
	{
	}

	protected override IQueryable<ProjectionTransfer> CheckedListWithUser(uint userId) 
		=> _context.ProjectionTransfers.Include(x => x.Category)
			.Include(x => x.Account)
			.ThenInclude(x => x.AppUser)
			.AsQueryable()
			.Where(s => s.Account.AppUserId == userId);

	public Task<List<ProjectionTransfer>> DebugGetAllProjectionTransfersAsync()
		=> _context.ProjectionTransfers.Include(x => x.Category)
			.Include(x => x.Account)
			.ThenInclude(x => x.AppUser)
			.AsQueryable()
			.ToListAsync();

	public async Task<List<ProjectionTransfer>> GetAllProjectionTransferAsync(uint userId) 
		=> await CheckedListWithUser(userId).ToListAsync();
	
	public async Task<ProjectionTransfer?> GetProjectionTransferByIdAsync(uint userId, uint projectionTransferId) 
		=> await CheckedListWithUser(userId).FirstOrDefaultAsync(x => x.Id == projectionTransferId);

	public async Task<List<ProjectionTransfer>> GetProjectionTransfersByAccountAsync(uint userId, uint accountId) 
		=> await CheckedListWithUser(userId).Where(x => x.AccountId == accountId).ToListAsync();
	

	public async Task<ProjectionTransfer> CreateProjectionTransferAsync(ProjectionTransfer transfer)
	{
		await _context.ProjectionTransfers.AddAsync(transfer);
		await _context.SaveChangesAsync();
		return transfer;
	}

	public async Task UpdateProjectionTransferAsync() => await _context.SaveChangesAsync();

	public async Task<ProjectionTransfer?> DeleteProjectionTransferAsync(uint userId, uint projectionTransferId)
	{
		var projectionTransfer = GetProjectionTransferByIdAsync(userId, projectionTransferId).Result;
		if (projectionTransfer == null)
			return null;
		
		_context.ProjectionTransfers.Remove(projectionTransfer);
		await _context.SaveChangesAsync();
		return projectionTransfer;
	}
}