using WPF_BudgetApp.Data.Models;

namespace WPF_BudgetApp.ViewModel;

public class DebugViewModel : BaseMenuViewModel
{
	public List<AppUser> Users { get; set; } = new List<AppUser>();
	public List<Account> Accounts { get; set; } = new List<Account>();
	public List<Category> Categories { get; set; } = new List<Category>();
	public List<Debt> Debts { get; set; } = new List<Debt>();
	public List<Transfer> Transfers { get; set; } = new List<Transfer>();

	public DebugViewModel(MainViewModel mainVM) : base(mainVM)
	{
	}

	public override void UpdateData()
	{
		Users.Clear();
		Accounts.Clear();
		Categories.Clear();
		Debts.Clear();
		Transfers.Clear();

		Users.AddRange(mainVM.appUserService.DebugGetAllAppUsersAsync().Result);
		Accounts.AddRange(mainVM.accountService.DebugGetAllAccountsAsync().Result);
		Categories.AddRange(mainVM.categoryService.DebugGetAllCategoryAsync().Result);
		Debts.AddRange(mainVM.debtService.DebugGetAllDebtAsync().Result);
		Transfers.AddRange(mainVM.transferService.DebugGetAllTransferAsync().Result);
	}
}