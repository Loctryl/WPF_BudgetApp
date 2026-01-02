using Microsoft.EntityFrameworkCore;
using WPF_BudgetApp.Data;
using WPF_BudgetApp.Data.Models;
using WPF_BudgetApp.Resources;
using WPF_BudgetApp.Services.Interfaces;

namespace WPF_BudgetApp.Services;

public class TransferService : ServiceBase<Transfer>, ITransferService
{
	public TransferService(AppDbContext context) : base(context)
	{
	}

	protected override IQueryable<Transfer> CheckedListWithUser(uint userId)
		=> _context.Transfers.Include(x => x.Category)
			.Include(x => x.Account)
			.ThenInclude(x => x.AppUser)
			.AsQueryable().Where(s => s.Account.AppUserId == userId);

	public Task<List<Transfer>> DebugGetAllTransferAsync()
		=> _context.Transfers.Include(x => x.Category)
			.Include(x => x.Account)
			.ThenInclude(x => x.AppUser)
			.AsQueryable().ToListAsync();

	public async Task<List<Transfer>> GetAllTransfersAsync(uint userId)
		=> await CheckedListWithUser(userId).ToListAsync();
	

	public async Task<Transfer?> GetTransferByIdAsync(uint userId, uint transferId) 
		=> await CheckedListWithUser(userId).FirstOrDefaultAsync(x => x.Id == transferId);
	
	public async Task<List<Transfer>> GetTransfersByAccountAsync(uint userId, uint accountId)
		=> await CheckedListWithUser(userId).Where(x => x.AccountId == accountId).ToListAsync();

	public async Task<Transfer> CreateTransferAsync(Transfer transfer)
	{
		await _context.Transfers.AddAsync(transfer);
		await _context.SaveChangesAsync();
		return transfer;
	}

	public async Task UpdateTransferAsync() => await _context.SaveChangesAsync();

	public async Task<Transfer?> DeleteTransferAsync(uint userId, uint transferId)
	{
		var transfer = GetTransferByIdAsync(userId, transferId).Result;
		if (transfer == null)
			return null;
		
		_context.Transfers.Remove(transfer);
		await _context.SaveChangesAsync();
		return transfer;
	}
}