using Microsoft.EntityFrameworkCore;
using WPF_BudgetApp.Data;
using WPF_BudgetApp.Data.Models;
using WPF_BudgetApp.Services.Interfaces;

namespace WPF_BudgetApp.Services;

public class AppUserService : IAppUserService
{
	private readonly AppDbContext _context;

	public AppUserService(AppDbContext context)
	{
		_context = context;
	}
	
	public async Task<AppUser?> AuthenticateAppUserAsync(string username, string password)
	{
		return await _context.AppUsers.FirstOrDefaultAsync(u => u.SourceName == username && u.Password == password);
	}

	public async Task<AppUser?> GetAppUserByIdAsync(int id)
	{
		return await _context.AppUsers.Include(u=>u.Accounts).FirstOrDefaultAsync(u => u.Id == id);
	}

	public async Task<AppUser> CreateAppUserAsync(AppUser user)
	{
		_context.AppUsers.Add(user);
		await _context.SaveChangesAsync();
		return user;
	}

	public async Task<AppUser> DeleteAppUserAsync(AppUser user)
	{
		_context.AppUsers.Remove(user);
		await _context.SaveChangesAsync();
		return user;
	}
}