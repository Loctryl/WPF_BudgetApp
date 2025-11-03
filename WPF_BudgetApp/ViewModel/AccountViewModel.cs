using WPF_BudgetApp.Data.Models;

namespace WPF_BudgetApp.ViewModel;

public class AccountViewModel : BaseMenuViewModel
{
	public AppUser CurrentUser { get; set; }
	
	public Account CurrentAccount { get; set; }
	
	public AccountViewModel(MainViewModel mainVM) : base(mainVM)
	{
	}
}