using Microsoft.EntityFrameworkCore;
using WPF_BudgetApp.Data;
using WPF_BudgetApp.Data.Models;
using WPF_BudgetApp.Services.Interfaces;

namespace WPF_BudgetApp.Services;

public class DebtService : ServiceBase<Debt>, IDebtService
{
	public DebtService(AppDbContext context) : base(context)
	{
	}

	protected override IQueryable<Debt> CheckedListWithUser(uint userId) 
		=> _context.Debts.Include(x => x.AppUser).Include(x => x.Category).AsQueryable().Where(s => s.AppUserId == userId);

	public Task<List<Debt>> DebugGetAllDebtAsync()
		=> _context.Debts.Include(x => x.AppUser).Include(x => x.Category).AsQueryable().ToListAsync();

	public async Task<List<Debt>> GetAllDebtAsync(uint userId) 
		=> await CheckedListWithUser(userId).ToListAsync();

	public async Task<Debt?> GetDebtByIdAsync(uint userId, uint debtId) 
		=> await CheckedListWithUser(userId).FirstOrDefaultAsync(x => x.Id == debtId);

	public async Task<Debt> CreateDebtAsync(Debt debt)
	{
		await _context.Debts.AddAsync(debt);
		await _context.SaveChangesAsync();
		return debt;
	}

	public async Task UpdateDebtAsync() => await _context.SaveChangesAsync();

	public async Task<Debt?> DeleteDebtAsync(uint userId, uint debtId)
	{
		var debt = GetDebtByIdAsync(userId, debtId).Result;
		if (debt == null)
			return null;
		
		_context.Debts.Remove(debt);
		await _context.SaveChangesAsync();
		return debt;
	}
}