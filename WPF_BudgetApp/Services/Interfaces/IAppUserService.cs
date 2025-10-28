using WPF_BudgetApp.Data.Models;

namespace WPF_BudgetApp.Services.Interfaces;

public interface IAppUserService
{
	Task<AppUser?> AuthenticateAppUserAsync(string username, string password);
	Task<List<AppUser>> GetAllAppUserAsync();
	Task<AppUser?> GetAppUserByIdAsync(uint id);
	Task<AppUser> CreateAppUserAsync(AppUser user);
	Task UpdateAppUserAsync();
	Task<AppUser> DeleteAppUserAsync(AppUser user);
}