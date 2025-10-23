using Microsoft.EntityFrameworkCore;
using WPF_BudgetApp.Data;
using WPF_BudgetApp.Data.Models;
using WPF_BudgetApp.Services.Interfaces;

namespace WPF_BudgetApp.Services;

public class ArchivedTransferService : ServiceBase<ArchivedTransfer>, IArchivedTransferService
{
	public ArchivedTransferService(AppDbContext context) : base(context)
	{
	}
	
	protected override IQueryable<ArchivedTransfer> CheckedListWithUser(uint userId)
		=> _context.ArchivedTransfers.AsQueryable().Where(s => s.UserId == userId);

	public async Task<List<ArchivedTransfer>> GetAllArchivedTransfersAsync(uint userId) 
		=> await CheckedListWithUser(userId).ToListAsync();

	public async Task<ArchivedTransfer?> GetArchivedTransferByIdAsync(uint userId, uint archivedTransferId) 
		=> await CheckedListWithUser(userId).FirstOrDefaultAsync(x => x.Id == archivedTransferId);

	public async Task<ArchivedTransfer> CreateArchivedTransferAsync(ArchivedTransfer transfer)
	{
		await _context.ArchivedTransfers.AddAsync(transfer);
		await _context.SaveChangesAsync();
		return transfer;
	}
}