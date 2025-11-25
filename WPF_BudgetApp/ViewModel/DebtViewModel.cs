using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Input;
using WPF_BudgetApp.Commands;
using WPF_BudgetApp.Data.DTOs;
using WPF_BudgetApp.Data.Models;
using WPF_BudgetApp.Resources;
using WPF_BudgetApp.Windows;

namespace WPF_BudgetApp.ViewModel;

public class DebtViewModel : BaseMenuViewModel
{
	#region Commands

	public ICommand AddDebtCommand { get; } 
	public ICommand UpdateDebtCommand { get; } 
	public ICommand DeleteDebtCommand { get; } 
	
	#endregion
	
	#region Debt management
	
	private DebtForm DebtForm { get; set; }
	public DebtFormDTO DebtFormDTO { get; } = new DebtFormDTO();
	public DebtDisplayDTO SelectedDebt { get; set; }
	public List<Debt> Debts { get; set; } = new List<Debt>();
	public ObservableCollection<DebtDisplayDTO> DebtsDTOs { get; set; } = new ObservableCollection<DebtDisplayDTO>();
	
	#endregion
	
	public DebtViewModel(MainViewModel mainVM) : base(mainVM)
	{
		AddDebtCommand = new RelayCommand(_ => DebtFormCall(FormType.ADD, ReceiveDebtForm));
		UpdateDebtCommand = new RelayCommand(_ => DebtFormCall(FormType.EDIT, ReceiveDebtForm));
		DeleteDebtCommand = new RelayCommand(_ => DebtFormCall(FormType.DELETE, ReceiveDebtForm));
	}
	
	#region Updating Data
	
	public override void UpdateData()
	{
		base.UpdateData();
		
		UpdateDebts();
	}
	
	private void UpdateDebts()
	{
		Debts.Clear();
		DebtsDTOs.Clear();
		
		Debts.AddRange(mainVM.debtService.GetAllDebtAsync(mainVM.CurrentUser.Id).Result);

		foreach (var debt in Debts)
			DebtsDTOs.Add(new DebtDisplayDTO(debt));
	}
	
	#endregion
	
	#region TransferForm
	
	private void DebtFormCall(FormType formType, EventHandler<bool> func)
	{
		DebtFormDTO.Reset();

		switch (formType)
		{
			case FormType.ADD:
				DebtFormDTO.AccountsOptions = mainVM.accountService.GetAllAccountAsync(mainVM.CurrentUser.Id).Result;
				break;
			
			case FormType.EDIT:
				DebtFormDTO.DebtName = SelectedDebt.DebtName;
				DebtFormDTO.DebtInitialAmount = SelectedDebt.DebtInitialAmount;
				DebtFormDTO.DebtCurrentAmount = SelectedDebt.DebtCurrentAmount;
				DebtFormDTO.DebtInterestRate = SelectedDebt.DebtInterestRate;
				DebtFormDTO.DebtLimitDate = SelectedDebt.DebtLimitDate;
				DebtFormDTO.CreationDate = SelectedDebt.CreationDate;
				
				DebtFormDTO.AccountsOptions = mainVM.accountService.GetAllAccountAsync(mainVM.CurrentUser.Id).Result;
				break;
			
			case FormType.DELETE:
				break;
		}
		
		DebtForm = new DebtForm(DebtFormDTO, formType);
		DebtForm.ConfirmEvent += func;
		DebtForm.Show();
	}

	private async void ReceiveDebtForm(object? sender, bool isConfirmed)
	{
		if (!isConfirmed) return;

		switch (DebtForm.FormType)
		{
			case FormType.ADD:
				await AddDebt();
				break;
			case FormType.EDIT:
				await EditDebt();
				break;
			case FormType.DELETE:
				await DeleteDebt();
				break;
		}
		
		UpdateDebts();
	}
	
	private async Task AddDebt()
	{
		Debt debt = Helpers.SetNewDebt(
			DebtFormDTO.DebtName, 
			DebtFormDTO.DebtInitialAmount,
			DebtFormDTO.DebtCurrentAmount,
			DebtFormDTO.DebtInterestRate,
			0,
			DebtFormDTO.DebtLimitDate
		);
		Category category = Helpers.SetNewCategory(mainVM.CurrentUser.Id, debt.SourceName, Helpers.GetRandomColorInString());
		await mainVM.categoryService.CreateCategoryAsync(category);
		debt.CategoryId = category.Id;
			
		Transfer transfer = Helpers.SetNewTransfer(debt.SourceName+" Initial Transfer", debt.InitialAmount, category.Id, DebtFormDTO.BeneficiaryAccount.Id, DateTime.Now);
		await mainVM.transferService.CreateTransferAsync(transfer);
		DebtFormDTO.BeneficiaryAccount.Balance += transfer.Amount;
			
		await mainVM.debtService.CreateDebtAsync(debt);
	}
	
	private async Task EditDebt()
	{
		Debt debt = Debts.First(c => c.Id == DebtFormDTO.DebtId);
		debt.SourceName = DebtFormDTO.DebtName;
		debt.CurrentDebt = DebtFormDTO.DebtCurrentAmount;
		debt.InterestRate = DebtFormDTO.DebtInterestRate;
		debt.LimitDate = DebtFormDTO.DebtLimitDate;
		debt.LastUpdateDate = DateTime.Now;
		
		await mainVM.categoryService.UpdateCategoryAsync();
	}

	private async Task DeleteDebt()
		=> await mainVM.debtService.DeleteDebtAsync(mainVM.CurrentUser.Id, DebtFormDTO.DebtId);
	
	#endregion
}