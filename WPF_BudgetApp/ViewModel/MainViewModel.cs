using WPF_BudgetApp.Data.Models;
using WPF_BudgetApp.Services.Interfaces;

namespace WPF_BudgetApp.ViewModel;

public class MainViewModel : BaseViewModel
{
	public readonly IAccountService accountService;
	public readonly IAppUserService appUserService;
	
	public AppUser CurrentUser { get; set; }
	public BaseViewModel CurrentVM {get; set;}
	private LoginViewModel LoginVM { get; set; }
	private DashboardViewModel DashboardVM { get; set; }
	private AccountViewModel AccountVM { get; set; }
	private DebtViewModel DebtVM { get; set; }
	private ArchiveViewModel ArchiveVM { get; set; }

	public MainViewModel(IAccountService accountService, IAppUserService appUserService)
	{
		this.accountService = accountService;
		this.appUserService = appUserService;
		
		LoginVM = new LoginViewModel(this);
		DashboardVM = new DashboardViewModel(this);
		AccountVM = new AccountViewModel(this);
		DebtVM = new DebtViewModel(this);
		ArchiveVM = new ArchiveViewModel(this);
		
		Logout();
	}
	
	public void SetCurrentUser(AppUser user) => CurrentUser = user;

	public void SwitchToDashBoard()
	{
		CurrentVM = DashboardVM;
		DashboardVM.UpdateData();
	}

	public void Logout()
	{
		CurrentUser = null;
		CurrentVM = LoginVM;
	}

	public void SwitchToAccount()
	{
		CurrentVM = AccountVM;
		AccountVM.UpdateData();
	}
	
	public void SwitchToDebt()
	{
		CurrentVM = DebtVM;
		DebtVM.UpdateData();
	}
	
	public void SwitchToArchive()
	{
		CurrentVM = ArchiveVM;
		ArchiveVM.UpdateData();
	}
}