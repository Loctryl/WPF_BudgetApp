using WPF_BudgetApp.Data;
using WPF_BudgetApp.Data.Models;

namespace WPF_BudgetApp.Services;

public abstract class ServiceBase<T>
{
	protected readonly AppDbContext _context;

	protected ServiceBase(AppDbContext context)
	{
		_context = context;
	}

	protected abstract IQueryable<T> CheckedListWithUser(uint userId);
}