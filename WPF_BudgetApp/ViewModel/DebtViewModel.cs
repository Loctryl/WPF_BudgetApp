using System.Collections.ObjectModel;
using System.Windows.Input;
using WPF_BudgetApp.Commands;
using WPF_BudgetApp.Data.DTOs;
using WPF_BudgetApp.Data.Models;
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
		AddDebtCommand = new RelayCommand(_ => DebtFormCall(false, ReceiveDebtForm));
		UpdateDebtCommand = new RelayCommand(_ => DebtFormCall(true, ReceiveDebtForm));
		DeleteDebtCommand = new RelayCommand(_ => ConfirmationWindowCall(DeleteDebt));
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
	
	private void DebtFormCall(bool isUpdate, EventHandler<bool> func)
	{
		DebtFormDTO.Reset();
		
		if (isUpdate)
		{
			DebtFormDTO.DebtName = SelectedDebt.DebtName;
			DebtFormDTO.DebtInitialAmount = SelectedDebt.DebtInitialAmount;
			DebtFormDTO.DebtCurrentAmount = SelectedDebt.DebtCurrentAmount;
			DebtFormDTO.DebtInterestRate = SelectedDebt.DebtInterestRate;
			DebtFormDTO.DebtLimitDate = SelectedDebt.DebtLimitDate;
			DebtFormDTO.CreationDate = SelectedDebt.CreationDate;
		}
		
		DebtForm = new DebtForm(this, isUpdate);
		DebtForm.ConfirmEvent += func;
		DebtForm.Show();
	}

	private async void ReceiveDebtForm(object? sender, bool isConfirmed)
	{
		if (!isConfirmed) return;
		
		Debt debt = new Debt();
		if (DebtForm.IsUpdate)
		{
			debt = Debts.Where(c => c.Id == SelectedDebt.DebtId).FirstOrDefault();
			if (debt == null)
				return;
		}
		
		debt.SourceName = DebtFormDTO.DebtName;
		debt.InitialAmount = DebtFormDTO.DebtInitialAmount;
		debt.CurrentDebt = DebtFormDTO.DebtCurrentAmount;
		debt.InterestRate = DebtFormDTO.DebtInterestRate;
		debt.LimitDate = DebtFormDTO.DebtLimitDate;
		debt.CreationDate = DebtFormDTO.CreationDate;
		debt.LastUpdateDate = DateTime.Now;

		if (DebtForm.IsUpdate)
		{
			await mainVM.debtService.UpdateDebtAsync();
		}
		else
		{
			//TODO: creating a category with the debt
			
			await mainVM.debtService.CreateDebtAsync(debt);
		}
		
		DebtFormDTO.Reset();
		UpdateDebts();
	}

	private async void DeleteDebt(object? sender, bool isConfirmed)
	{
		if (!isConfirmed) return;
		await mainVM.debtService.DeleteDebtAsync(mainVM.CurrentUser.Id, SelectedDebt.DebtId);
		UpdateDebts();
	}
	
	#endregion
}