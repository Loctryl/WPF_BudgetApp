using Microsoft.EntityFrameworkCore;
using WPF_BudgetApp.Data;
using WPF_BudgetApp.Data.DTOs;
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

	public Task<List<ArchivedTransfer>> DebugGetAllArchivedTransfersAsync()
		=> _context.ArchivedTransfers.AsQueryable().ToListAsync();

	public async Task<List<ArchivedTransfer>> GetAllArchivedTransfersAsync(uint userId) 
		=> await CheckedListWithUser(userId).ToListAsync();

	public Task<List<ArchivedTransfer>> GetAllArchivedTransfersWithQueryAsync(uint userId, ArchiveQuery query)
	{
		var queryable = CheckedListWithUser(userId);
		
		if(!string.IsNullOrWhiteSpace(query.QueryName))
			queryable = queryable.Where(s => s.SourceName.Contains(query.QueryName));
		
		if(!string.IsNullOrWhiteSpace(query.QueryCategory))
			queryable = queryable.Where(s => s.CategoryName.Contains(query.QueryCategory));
		
		if(!string.IsNullOrWhiteSpace(query.QueryAccount))
			queryable = queryable.Where(s => s.AccountName.Contains(query.QueryAccount));

		if (query.QueryAmount != 0)
		{
			double marginError = Math.Abs(query.QueryAmount) * 0.2;
			queryable = queryable.Where(s => (s.Amount <= query.QueryAmount + marginError && s.Amount >= query.QueryAmount - marginError));
		}
		
		if(query.QueryOperationDate != DateTime.Now)
			queryable = queryable.Where(s => s.OperationDate.Year == query.QueryOperationDate.Year && s.OperationDate.Month == query.QueryOperationDate.Month);
		
		if(query.QueryCreationDate != DateTime.Now)
			queryable = queryable.Where(s => s.CreationDate.Year == query.QueryOperationDate.Year && s.OperationDate.Month == query.QueryOperationDate.Month);
		
		return queryable.ToListAsync();
	}

	public async Task<ArchivedTransfer?> GetArchivedTransferByIdAsync(uint userId, uint archivedTransferId) 
		=> await CheckedListWithUser(userId).FirstOrDefaultAsync(x => x.Id == archivedTransferId);

	public async Task<ArchivedTransfer> CreateArchivedTransferAsync(ArchivedTransfer transfer)
	{
		await _context.ArchivedTransfers.AddAsync(transfer);
		await _context.SaveChangesAsync();
		return transfer;
	}
}