using WPF_BudgetApp.Data.Models;
using WPF_BudgetApp.Services.Interfaces;

namespace WPF_BudgetApp.ViewModel;

public class MainViewModel : BaseViewModel
{
	public readonly IAccountService accountService;
	public readonly IAppUserService appUserService;
	public readonly ICategoryService categoryService;
	public readonly IDebtService debtService;
	public readonly ITransferService transferService;
	public readonly IProjectionTransferService projectionTransferService;
	public readonly IArchivedTransferService archiveService;
	
	public AppUser CurrentUser { get; set; }
	public BaseViewModel CurrentVM {get; set;}
	private LoginViewModel LoginVM { get; set; }
	private DashboardViewModel DashboardVM { get; set; }
	private AccountViewModel AccountVM { get; set; }
	private DebtViewModel DebtVM { get; set; }
	private ArchiveViewModel ArchiveVM { get; set; }
	
	//debug
	private DebugViewModel DebugVM { get; set; }

	public MainViewModel(
	IAccountService accountService, IAppUserService appUserService, 
	ICategoryService categoryService, IDebtService debtService, 
	ITransferService transferService,  IProjectionTransferService projectionTransferService, 
	IArchivedTransferService archiveService)
	{
		this.accountService = accountService;
		this.appUserService = appUserService;
		this.categoryService = categoryService;
		this.debtService = debtService;
		this.transferService = transferService;
		this.projectionTransferService = projectionTransferService;
		this.archiveService = archiveService;
		
		LoginVM = new LoginViewModel(this);
		DashboardVM = new DashboardViewModel(this);
		AccountVM = new AccountViewModel(this);
		DebtVM = new DebtViewModel(this);
		ArchiveVM = new ArchiveViewModel(this);
		
		// Debug
		DebugVM = new DebugViewModel(this);
		//SwitchToDebug();
		
		// temporary login
		Task.Run(SkipLoginAsync);
	}
	
	private async Task SkipLoginAsync()
	{
		AppUser user = await this.appUserService.AuthenticateAppUserAsync("Loctryl", "1234");
		
		if (user == null)
		{
			Logout();
			return;
		}
		
		SetCurrentUser(user);
		SwitchToDashBoard();
	}
	
	public void SetCurrentUser(AppUser user) => CurrentUser = user;
	
	private void SwitchToDebug()
	{
		CurrentVM = DebugVM;
		DebugVM.UpdateData();
	}

	public void SwitchToDashBoard()
	{
		DashboardVM.UpdateData();
		CurrentVM = DashboardVM;
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