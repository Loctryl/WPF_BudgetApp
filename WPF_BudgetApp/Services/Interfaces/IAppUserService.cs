using WPF_BudgetApp.Data.Models;

namespace WPF_BudgetApp.Services.Interfaces;

public interface IAppUserService
{
	Task<AppUser?> AuthenticateAppUserAsync(string username, string password);
	Task<AppUser?> GetAppUserByIdAsync(int id);
	Task<AppUser> CreateAppUserAsync(AppUser user);
	Task<AppUser> DeleteAppUserAsync(AppUser user);
}