using WPF_BudgetApp.Services.Interfaces;

namespace WPF_BudgetApp.ViewModel;

public class MainViewModel : BaseViewModel
{
	public readonly IAccountService accountService;
	public readonly IAppUserService appUserService;
	public BaseViewModel CurrentVM {get; set;}
	
	private LoginViewModel LoginVM { get; set; }
	private DashboardViewModel DashboardVM { get; set; }

	public MainViewModel(IAccountService accountService, IAppUserService appUserService)
	{
		this.accountService = accountService;
		this.appUserService = appUserService;
		
		LoginVM = new LoginViewModel(this);
		DashboardVM = new DashboardViewModel(this);
		
		CurrentVM = DashboardVM;
	}

	public void SwitchToDashBoard()
	{
		CurrentVM = DashboardVM;
		DashboardVM.RefreshData();
	}
	public void SwitchToLogin() => CurrentVM = LoginVM;
}