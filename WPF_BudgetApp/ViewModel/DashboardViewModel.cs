using System.Windows;
using System.Windows.Input;
using WPF_BudgetApp.Commands;
using WPF_BudgetApp.Data.DTOs;
using WPF_BudgetApp.Data.Models;
using WPF_BudgetApp.Windows;

namespace WPF_BudgetApp.ViewModel;

public class DashboardViewModel : BaseMenuViewModel
{
	public ICommand AddCategoryCommand { get; } 
	public ICommand UpdateCategoryCommand { get; } 
	
	private CategoryForm CategoryForm { get; set; }
	public CategoryFormDTO CatFormDTO { get; } = new CategoryFormDTO();
	public CategoryDisplayDTO SelectedCategory { get; set; }
	
	public List<Account> Accounts { get; set; } = new List<Account>();
	public List<Category> Categories { get; set; } = new List<Category>();
	public List<CategoryDisplayDTO> CategoriesDTOs { get; set; } = new List<CategoryDisplayDTO>();

	public DashboardViewModel(MainViewModel mainVM) : base(mainVM)
	{ 
		AddCategoryCommand = new RelayCommand(_ => CategoryFormCall(false));
		UpdateCategoryCommand = new RelayCommand(_ => CategoryFormCall(true));
	}

	public override void UpdateData()
	{
		base.UpdateData();
		
		Accounts.Clear();
		Categories.Clear();
		CategoriesDTOs.Clear();
		
		Accounts.AddRange(mainVM.accountService.GetAllAccountAsync(mainVM.CurrentUser.Id).Result);
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

	private void CategoryFormCall(bool isUpdate)
	{
		if (isUpdate)
		{
			CatFormDTO.CategoryName = SelectedCategory.CategoryName;
			CatFormDTO.CategorySymbol = SelectedCategory.CategorySymbol;
			CatFormDTO.CategoryColor = SelectedCategory.CategoryColor;
		}
		
		CategoryForm = new CategoryForm(this, isUpdate);
		CategoryForm.ConfirmEvent += ReceiveCategoryForm;
		CategoryForm.Show();
	}

	private void ReceiveCategoryForm(object? sender, EventArgs e)
	{
		Category cat = new Category();
		if (CategoryForm.IsUpdate)
		{
			cat = Categories.Where(c => c.Id == SelectedCategory.CategoryId).FirstOrDefault();
			if (cat == null)
				return;
		}
		
		cat.SourceName = CatFormDTO.CategoryName;
		cat.Symbol = CatFormDTO.CategorySymbol;
		cat.Color = CatFormDTO.CategoryColor;
		
		if (CategoryForm.IsUpdate)
			mainVM.categoryService.UpdateCategoryAsync();
		else
		{
			cat.AppUserId = mainVM.CurrentUser.Id;
			mainVM.categoryService.CreateCategoryAsync(cat);
		}
		
		CategoryForm.ConfirmEvent -= ReceiveCategoryForm;
		UpdateData();
	}
}