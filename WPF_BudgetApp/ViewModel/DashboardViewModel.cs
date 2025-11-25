using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using WPF_BudgetApp.Commands;
using WPF_BudgetApp.Data.DTOs;
using WPF_BudgetApp.Data.Models;
using WPF_BudgetApp.Resources;
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
		AddCategoryCommand = new RelayCommand(_ => CategoryFormCall(FormType.ADD, ReceiveCategoryForm));
		UpdateCategoryCommand = new RelayCommand(_ => CategoryFormCall(FormType.EDIT, ReceiveCategoryForm));
		DeleteCategoryCommand = new RelayCommand(_ => CategoryFormCall(FormType.DELETE, ReceiveCategoryForm));
		
		AddAccountCommand = new RelayCommand(_ => AccountFormCall(FormType.ADD, ReceiveAccountForm));
		UpdateAccountCommand = new RelayCommand(_ => AccountFormCall(FormType.EDIT, ReceiveAccountForm));
		DeleteAccountCommand = new RelayCommand(_ => AccountFormCall(FormType.DELETE, ReceiveAccountForm));
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

		decimal balance = 0;
		
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
	
	private async void CategoryFormCall(FormType formType, EventHandler<bool> func)
	{
		CatFormDTO.Reset();

		switch (formType)
		{
			case FormType.ADD:
				CatFormDTO.CategoryColor = Helpers.GetRandomColor();
				break;
			case FormType.EDIT:
				CatFormDTO.CategoryId = SelectedCategory.CategoryId;
				CatFormDTO.CategoryName = SelectedCategory.CategoryName;
				CatFormDTO.CategoryColor = (Color)ColorConverter.ConvertFromString(SelectedCategory.CategoryColor);
				CatFormDTO.CreationDate = SelectedCategory.CreationDate;
				break;
			case FormType.DELETE:
				if ((await mainVM.debtService.GetAllDebtAsync(mainVM.CurrentUser.Id)).Any(d => d.CategoryId == SelectedCategory.CategoryId))
				{
					//todo : Message box
					return;
				}
				
				CatFormDTO.CategoryId = SelectedCategory.CategoryId;
				CatFormDTO.CategoriesOptions.AddRange(Categories);
				CatFormDTO.CategoriesOptions.Remove(Categories.First(c => c.Id == SelectedCategory.CategoryId));
				break;
		}
		
		CategoryForm = new CategoryForm(CatFormDTO, formType);
		CategoryForm.ConfirmEvent += func;
		CategoryForm.Show();
	}

	private async void ReceiveCategoryForm(object? sender, bool isConfirmed)
	{
		if (!isConfirmed) return;
		
		switch (CategoryForm.FormType)
		{
			case FormType.ADD:
				await AddCategory();
				break;
			case FormType.EDIT:
				await EditCategory();
				break;
			case FormType.DELETE:
				await DeleteCategory();
				break;
		}
		
		UpdateCategories();
	}

	private async Task AddCategory()
	{
		Category cat = Helpers.SetNewCategory(
			mainVM.CurrentUser.Id, 
			CatFormDTO.CategoryName, 
			CatFormDTO.CategoryColor.ToString()
		);
		await mainVM.categoryService.CreateCategoryAsync(cat);
	}
	
	private async Task EditCategory()
	{
		Category cat = Categories.First(c => c.Id == CatFormDTO.CategoryId);
		cat.SourceName = CatFormDTO.CategoryName;
		cat.Color = CatFormDTO.CategoryColor.ToString();
		cat.LastUpdateDate = DateTime.Now;
		
		await mainVM.categoryService.UpdateCategoryAsync();
	}

	private async Task DeleteCategory() =>
		await mainVM.categoryService.DeleteCategoryAsync(mainVM.CurrentUser.Id, CatFormDTO.CategoryId, CatFormDTO.ReplaceCategory.Id);
	
	#endregion
	
	#region AccountForm
	
	private void AccountFormCall(FormType formType, EventHandler<bool> func)
	{
		AccFormDTO.Reset();
		
		switch (formType)
		{
			case FormType.ADD:
				AccFormDTO.AccountColor = Helpers.GetRandomColor();
				break;
			case FormType.EDIT:
				AccFormDTO.AccountId = SelectedAccount.AccountId;
				AccFormDTO.AccountName = SelectedAccount.AccountName;
				AccFormDTO.AccountColor = (Color)ColorConverter.ConvertFromString(SelectedAccount.AccountColor);
				AccFormDTO.CreationDate = SelectedAccount.CreationDate;
				break;
			case FormType.DELETE:
				AccFormDTO.AccountId = SelectedAccount.AccountId;
				break;
		}
		
		AccountForm = new AccountForm(AccFormDTO, formType);
		AccountForm.ConfirmEvent += func;
		AccountForm.Show();
	}

	private async void ReceiveAccountForm(object? sender, bool isConfirmed)
	{
		if (!isConfirmed) return;
		
		switch (AccountForm.FormType)
		{
			case FormType.ADD:
				await AddAccount();
				break;
			case FormType.EDIT:
				await EditAccount();
				break;
			case FormType.DELETE:
				await DeleteAccount();
				break;
		}
		
		UpdateAccounts();
	}

	private async Task AddAccount()
	{
		Account acc = Helpers.SetNewAccount(
			mainVM.CurrentUser.Id, 
			AccFormDTO.AccountName, 
			AccFormDTO.AccountBalance,
			AccFormDTO.AccountColor.ToString()
		);
		await mainVM.accountService.CreateAccountAsync(acc);
	}

	private async Task EditAccount()
	{
		Account acc = Accounts.First(c => c.Id == AccFormDTO.AccountId);
		acc.SourceName = AccFormDTO.AccountName;
		acc.Color = AccFormDTO.AccountColor.ToString();
		acc.LastUpdateDate = DateTime.Now;
		await mainVM.accountService.UpdateAccountAsync();
	}

	private async Task DeleteAccount() 
		=> await mainVM.accountService.DeleteAccountAsync(mainVM.CurrentUser.Id, AccFormDTO.AccountId);
	
	#endregion
}