using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using WPF_BudgetApp.Commands;
using WPF_BudgetApp.Data.DTOs;
using WPF_BudgetApp.Data.Models;
using WPF_BudgetApp.Windows;

namespace WPF_BudgetApp.ViewModel;

public class DashboardViewModel : BaseMenuViewModel
{
	#region Commands

	public ICommand AddCategoryCommand { get; } 
	public ICommand UpdateCategoryCommand { get; } 
	public ICommand DeleteCategoryCommand { get; } 
	
	public ICommand AddAccountCommand { get; } 
	public ICommand UpdateAccountCommand { get; } 
	public ICommand DeleteAccountCommand { get; } 
	
	#endregion
	
	#region Category management
	
	private CategoryForm CategoryForm { get; set; }
	public CategoryFormDTO CatFormDTO { get; } = new CategoryFormDTO();
	public CategoryDisplayDTO SelectedCategory { get; set; }
	public List<Category> Categories { get; set; } = new List<Category>();
	public ObservableCollection<CategoryDisplayDTO> CategoriesDTOs { get; set; } = new ObservableCollection<CategoryDisplayDTO>();
	
	#endregion
	
	#region Account management
	
	private AccountForm AccountForm { get; set; }
	public AccountFormDTO AccFormDTO { get; } = new AccountFormDTO();
	public AccountDisplayDTO SelectedAccount { get; set; }
	public List<Account> Accounts { get; set; } = new List<Account>();
	public ObservableCollection<AccountDisplayDTO> AccountsDTOs { get; set; } = new ObservableCollection<AccountDisplayDTO>();
	
	#endregion
	
	public string GlobalBalance { get; set; }
	
	public DashboardViewModel(MainViewModel mainVM) : base(mainVM)
	{ 
		AddCategoryCommand = new RelayCommand(_ => CategoryFormCall(false, ReceiveCategoryForm));
		UpdateCategoryCommand = new RelayCommand(_ => CategoryFormCall(true, ReceiveCategoryForm));
		DeleteCategoryCommand = new RelayCommand(_ => ConfirmationWindowCall(DeleteCategory));
		
		AddAccountCommand = new RelayCommand(_ => AccountFormCall(false, ReceiveAccountForm));
		UpdateAccountCommand = new RelayCommand(_ => AccountFormCall(true, ReceiveAccountForm));
		DeleteAccountCommand = new RelayCommand(_ => ConfirmationWindowCall(DeleteAccount));
	}
	
	#region Updating Data
	
	public override void UpdateData()
	{
		base.UpdateData();
		
		UpdateAccounts();
		UpdateCategories();
	}
	
	private void UpdateAccounts()
	{
		GlobalBalance = string.Empty;
		Accounts.Clear();
		AccountsDTOs.Clear();
		Accounts.AddRange(mainVM.accountService.GetAllAccountAsync(mainVM.CurrentUser.Id).Result);

		float balance = 0;
		
		foreach (var acc in Accounts)
		{
			AccountsDTOs.Add(new AccountDisplayDTO(acc));
			balance += acc.Balance;
		}
		if (balance >= 0)
			GlobalBalance = "+";
		
		GlobalBalance += balance.ToString("c2");
	}

	private void UpdateCategories()
	{
		Categories.Clear();
		CategoriesDTOs.Clear();
		Categories.AddRange(mainVM.categoryService.GetAllCategoryAsync(mainVM.CurrentUser.Id).Result);
		
		foreach (var cat in Categories)
		{
			CategoryDisplayDTO disp = new CategoryDisplayDTO(cat);
			disp.CategoryCurrentMonth = 0;
			foreach (var transfer in cat.Transfers.Where(t => t.OperationDate.Month == DateTime.Now.Month))
				disp.CategoryCurrentMonth += transfer.Amount;
			
			disp.CategoryLastMonth = 0;
			foreach (var transfer in cat.Transfers.Where(t => t.OperationDate.Month == DateTime.Now.Month-1))
				disp.CategoryLastMonth += transfer.Amount;
			
			CategoriesDTOs.Add(disp);
		}
	}
	
	#endregion
	
	#region CategoryForm
	
	private void CategoryFormCall(bool isUpdate, EventHandler<bool> func)
	{
		if (isUpdate)
		{
			CatFormDTO.CategoryName = SelectedCategory.CategoryName;
			CatFormDTO.CategorySymbol = SelectedCategory.CategorySymbol;
			CatFormDTO.CategoryColor = (Color)ColorConverter.ConvertFromString(SelectedCategory.CategoryColor);
		}
		
		CategoryForm = new CategoryForm(this, isUpdate);
		CategoryForm.ConfirmEvent += func;
		CategoryForm.Show();
	}

	private async void ReceiveCategoryForm(object? sender, bool isConfirmed)
	{
		if (!isConfirmed)
		{
			CatFormDTO.Reset();
			return;
		}
		
		Category cat = new Category();
		if (CategoryForm.IsUpdate)
		{
			cat = Categories.Where(c => c.Id == SelectedCategory.CategoryId).FirstOrDefault();
			if (cat == null)
				return;
		}
		
		cat.SourceName = CatFormDTO.CategoryName;
		cat.Symbol = CatFormDTO.CategorySymbol;
		cat.Color = CatFormDTO.CategoryColor.ToString();
		
		if (CategoryForm.IsUpdate)
			await mainVM.categoryService.UpdateCategoryAsync();
		else
		{
			cat.AppUserId = mainVM.CurrentUser.Id;
			await mainVM.categoryService.CreateCategoryAsync(cat);
		}
		
		CategoryForm.ConfirmEvent -= ReceiveCategoryForm;
		CategoryForm.Close();
		CatFormDTO.Reset();
		UpdateCategories();
	}

	private async void DeleteCategory(object? sender, bool isConfirmed)
	{
		if (!isConfirmed) return;
		await mainVM.categoryService.DeleteCategoryAsync(mainVM.CurrentUser.Id, SelectedCategory.CategoryId);
		UpdateCategories();
	}
	
	#endregion
	
	#region AccountForm
	
	private void AccountFormCall(bool isUpdate, EventHandler<bool> func)
	{
		if (isUpdate)
		{
			AccFormDTO.AccountName = SelectedAccount.AccountName;
			AccFormDTO.AccountSymbol = SelectedAccount.AccountSymbol;
			AccFormDTO.AccountColor = (Color)ColorConverter.ConvertFromString(SelectedAccount.AccountColor);
		}
		
		AccountForm = new AccountForm(this, isUpdate);
		AccountForm.ConfirmEvent += func;
		AccountForm.Show();
	}

	private async void ReceiveAccountForm(object? sender, bool isConfirmed)
	{
		if (!isConfirmed)
		{
			AccFormDTO.Reset();
			return;
		}
		
		Account acc = new Account();
		if (AccountForm.IsUpdate)
		{
			acc = Accounts.Where(a => a.Id == SelectedAccount.AccountId).FirstOrDefault();
			if (acc == null)
				return;
		}
		
		acc.SourceName = AccFormDTO.AccountName;
		acc.Symbol = AccFormDTO.AccountSymbol;
		acc.Color = AccFormDTO.AccountColor.ToString();
		
		if (AccountForm.IsUpdate)
			await mainVM.accountService.UpdateAccountAsync();
		else
		{
			acc.Balance = AccFormDTO.AccountBalance;
			acc.AppUserId = mainVM.CurrentUser.Id;
			await mainVM.accountService.CreateAccountAsync(acc);
		}
		
		AccountForm.ConfirmEvent -= ReceiveAccountForm;
		AccountForm.Close();
		AccFormDTO.Reset();
		UpdateAccounts();
	}

	private async void DeleteAccount(object? sender, bool isConfirmed)
	{
		if (!isConfirmed) return;
		await mainVM.accountService.DeleteAccountAsync(mainVM.CurrentUser.Id, SelectedAccount.AccountId);
		UpdateAccounts();
	}
	
	#endregion
}