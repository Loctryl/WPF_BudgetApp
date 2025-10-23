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

	protected override IQueryable<Transfer> CheckedListWithUser(string userId)
		=> _context.Transfers.Include(x => x.Category)
			.Include(x => x.Account)
			.ThenInclude(x => x.AppUser)
			.AsQueryable().Where(s => s.Account.AppUserId == userId);
	
	public async Task<List<Transfer>> GetAllTransfersAsync(string userId, TransferQueryObject query)
	{
		var queryable = CheckedListWithUser(userId);
		
		if(!string.IsNullOrWhiteSpace(query.Source))
			queryable = queryable.Where(s => s.SourceName.Contains(query.Source));
		
		if(query.BankId != uint.MaxValue)
			queryable = queryable.Where(s => s.AccountId == query.BankId);
		
		if(query.CategoryId != uint.MaxValue)
			queryable = queryable.Where(s => s.CategoryId == query.CategoryId);
		
		queryable = query.Reviewed ? queryable.Where(s => s.Reviewed) : queryable.Where(s => !s.Reviewed);
		
		queryable = queryable.OrderByProperty(query.OrderBy);
		
		var skipNumber = (query.PageNumber - 1) * query.PageSize;
		return await queryable.Skip(skipNumber).Take(query.PageSize).ToListAsync();
	}

	public async Task<Transfer?> GetTransferByIdAsync(string userId, uint transferId) 
		=> await CheckedListWithUser(userId).FirstOrDefaultAsync(x => x.Id == transferId);
	
	public async Task<List<Transfer>> GetTransfersByAccountAsync(string userId, uint accountId)
		=> await CheckedListWithUser(userId).Where(x => x.AccountId == accountId).ToListAsync();

	public async Task<Transfer> CreateTransferAsync(Transfer transfer)
	{
		await _context.Transfers.AddAsync(transfer);
		await _context.SaveChangesAsync();
		return transfer;
	}

	public Task<Transfer?> UpdateTransferAsync(string userId, uint transferId)
	{
		throw new NotImplementedException();
	}

	public async Task<Transfer?> DeleteTransferAsync(string userId, uint transferId)
	{
		var transfer = GetTransferByIdAsync(userId, transferId).Result;
		if (transfer == null)
			return null;
		
		_context.Transfers.Remove(transfer);
		await _context.SaveChangesAsync();
		return transfer;
	}
}