using System.Windows;
using WPF_BudgetApp.Data.Models;
using WPF_BudgetApp.Resources;
using WPF_BudgetApp.Services.Interfaces;

namespace WPF_BudgetApp.ViewModel;

public class MainViewModel : BaseViewModel
{
	public readonly IAccountService accountService;
	public readonly IAppUserService appUserService;
	public readonly ICategoryService categoryService;
	public readonly IDebtService debtService;
	public readonly ITransferService transferService;
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
	ITransferService transferService, 
	IArchivedTransferService archiveService)
	{
		this.accountService = accountService;
		this.appUserService = appUserService;
		this.categoryService = categoryService;
		this.debtService = debtService;
		this.transferService = transferService;
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
		//Logout();
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
		await LogInCheck();
		SwitchToDashBoard();
	}
	
	public async Task LogInCheck()
	{
		List<Transfer> transfers = (await transferService.GetAllTransfersAsync(CurrentUser.Id));

		var Limit = DateTime.Now.AddMonths(-6);
		
		foreach (var transfer in transfers.Where(t => t.OperationDate.Month <= Limit.Month && t.OperationDate.Year <= Limit.Year).ToList())
		{
			ArchivedTransfer newArchive = new ArchivedTransfer();
			newArchive.Id = transfer.Id;
			newArchive.SourceName = transfer.SourceName;
			newArchive.UserId = CurrentUser.Id;
			newArchive.UserName = CurrentUser.SourceName;
			newArchive.AccountName = transfer.Account.SourceName;
			newArchive.CategoryName = transfer.Category.SourceName;
			newArchive.Amount = transfer.Amount;
			newArchive.OperationDate = transfer.OperationDate;
			newArchive.CreationDate = transfer.CreationDate;
			newArchive.LastUpdateDate = DateTime.Now;
				
			await archiveService.CreateArchivedTransferAsync(newArchive);
			await transferService.DeleteTransferAsync(CurrentUser.Id, transfer.Id);
		}

		foreach (var transfer in transfers.Where(t => t.OperationDate > CurrentUser.LastUpdateDate && t.OperationDate <= DateTime.Now && t.IsMonthly).ToList())
		{
			Transfer trans = Helpers.SetNewTransfer(
				transfer.SourceName, 
				transfer.Amount,
				transfer.CategoryId,
				transfer.AccountId,
				transfer.OperationDate.AddMonths(1),
				transfer.IsMonthly
			);
			await transferService.CreateTransferAsync(trans);
		}
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
		CurrentUser = new AppUser();
		CurrentUser.PrimaryColor = "#FFF7F3E3";
		CurrentUser.SecondaryColor = "#FF57504C";
		CurrentUser.TertiaryColor = "#FF57504C";
		CurrentUser.WritingColor = "#FF000000";
		
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