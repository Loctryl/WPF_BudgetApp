using Microsoft.EntityFrameworkCore;
using WPF_BudgetApp.Data;
using WPF_BudgetApp.Data.Models;
using WPF_BudgetApp.Services.Interfaces;

namespace WPF_BudgetApp.Services;

public class AppUserService : ServiceBase<AppUser>, IAppUserService
{
	public AppUserService(AppDbContext context) : base(context)
	{
	}
	
	protected override IQueryable<AppUser> CheckedListWithUser(uint userId)
	{
		throw new NotImplementedException();
	}


	public async Task<AppUser?> AuthenticateAppUserAsync(string username, string password) 
		=> await _context.AppUsers.FirstOrDefaultAsync(u => u.SourceName == username && u.Password == password);

	public async Task<List<AppUser>> DebugGetAllAppUsersAsync() 
		=> await _context.AppUsers.ToListAsync();
	
	public async Task<AppUser?> GetAppUserByIdAsync(uint id) 
		=> await _context.AppUsers.FirstOrDefaultAsync(u => u.Id == id);
	
	public async Task<AppUser> CreateAppUserAsync(AppUser user)
	{
		await _context.AppUsers.AddAsync(user);
		await _context.SaveChangesAsync();
		return user;
	}
	
	public async Task UpdateAppUserAsync() => await _context.SaveChangesAsync();

	public async Task DeleteAppUserAsync(uint userId)
	{
		var user = GetAppUserByIdAsync(userId).Result;
		if (user == null)
			return;
		
		_context.AppUsers.Remove(user);
		await _context.SaveChangesAsync();
	}
}