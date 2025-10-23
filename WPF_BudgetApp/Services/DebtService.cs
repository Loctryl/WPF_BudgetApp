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

	protected override IQueryable<Debt> CheckedListWithUser(string userId) 
		=> _context.Debts.Include(x => x.AppUser).AsQueryable().Where(s => s.AppUserId == userId);
	
	public async Task<List<Debt>> GetAllDebtAsync(string userId) 
		=> await CheckedListWithUser(userId).ToListAsync();

	public async Task<Debt?> GetDebtByIdAsync(string userId, uint debtId) 
		=> await CheckedListWithUser(userId).FirstOrDefaultAsync(x => x.Id == debtId);

	public async Task<Debt> CreateDebtAsync(Debt debt)
	{
		await _context.Debts.AddAsync(debt);
		await _context.SaveChangesAsync();
		return debt;
	}

	public Task<Debt?> UpdateDebtAsync(string userId, uint debtId)
	{
		throw new NotImplementedException();
	}

	public async Task<Debt?> DeleteDebtAsync(string userId, uint debtId)
	{
		var debt = GetDebtByIdAsync(userId, debtId).Result;
		if (debt == null)
			return null;
		
		_context.Debts.Remove(debt);
		await _context.SaveChangesAsync();
		return debt;
	}
}