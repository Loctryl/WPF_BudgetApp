using System.Windows.Input;
using WPF_BudgetApp.Commands;
using WPF_BudgetApp.Data.Models;
using WPF_BudgetApp.Windows;

namespace WPF_BudgetApp.ViewModel;

public class DashboardViewModel : BaseMenuViewModel
{
	public ICommand AddCategoryCommand { get; } 
	
	private List<Account> Accounts { get; set; } = new List<Account>();
	private List<Category> Categories { get; set; } = new List<Category>();

	public DashboardViewModel(MainViewModel mainVM) : base(mainVM)
	{ 
		AddCategoryCommand = new RelayCommand(_ => AddCategoryForm());
	}

	public override void UpdateData()
	{
		base.UpdateData();
		
		Accounts.Clear();
		Categories.Clear();
		
		Accounts.AddRange(mainVM.accountService.GetAllAccountAsync(mainVM.CurrentUser.Id).Result);
		Categories.AddRange(mainVM.categoryService.GetAllCategoryAsync(mainVM.CurrentUser.Id).Result);
		
		foreach (var cat in Categories)
		{
			cat.CurrentMonthValue = 0;
			foreach (var transfer in cat.Transfers.Where(t => t.OperationDate.Month == DateTime.Now.Month))
				cat.CurrentMonthValue += transfer.Amount;
			
			cat.LastMonthValue = 0;
			foreach (var transfer in cat.Transfers.Where(t => t.OperationDate.Month == DateTime.Now.Month-1))
				cat.LastMonthValue += transfer.Amount;
		}
	}

	private void AddCategoryForm()
	{
		var categoryForm = new CategoryForm(this);
		categoryForm.Show();
	}

	private void ReceiveCategoryForm(string name, string symbol, string color)
	{
		
	}
}