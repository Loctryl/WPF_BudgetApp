using WPF_BudgetApp.Data.Models;

namespace WPF_BudgetApp.ViewModel;

public class DashboardViewModel : BaseViewModel
{
	private readonly MainViewModel mainVM;
	
	public AppUser CurrentUser { get; set; }
	
	public List<AppUser> Users { get; set; } = new List<AppUser>();
	public List<Account> Accounts { get; set; } = new List<Account>();

	public DashboardViewModel(MainViewModel mainVM)
	{
		this.mainVM = mainVM;
		RefreshData();
	}

	public void RefreshData()
	{
		Users.Clear();
		Accounts.Clear();
		
		Users = mainVM.appUserService.GetAllAppUserAsync().Result;

		foreach (var user in Users)
		{
			Accounts.AddRange(mainVM.accountService.GetAllAccountAsync(user.Id).Result);
		}
	}
}