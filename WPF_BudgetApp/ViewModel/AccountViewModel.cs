using System.Collections.ObjectModel;
using System.Windows.Input;
using WPF_BudgetApp.Commands;
using WPF_BudgetApp.Data.DTOs;
using WPF_BudgetApp.Data.Models;
using WPF_BudgetApp.Windows;

namespace WPF_BudgetApp.ViewModel;

public class AccountViewModel : BaseMenuViewModel
{
	public Account CurrentSelectedAccount { get; set; }
	public string SelectedBalance { get; set; }

	public List<Account> Accounts { get; set; } = new List<Account>();
	public AccountDisplayDTO AccountsDTO { get; set; }
	
	#region Commands

	public ICommand AddTransferCommand { get; } 
	public ICommand UpdateTransferCommand { get; } 
	public ICommand DeleteTransferCommand { get; } 
	
	
	#endregion
	
	#region Transfer management
	
	private TransferForm TransferForm { get; set; }
	public TransferFormDTO TransFormDTO { get; } = new TransferFormDTO();
	public TransferDisplayDTO SelectedTransfer { get; set; }
	public List<Transfer> Transfers { get; set; } = new List<Transfer>();
	public ObservableCollection<TransferDisplayDTO> TransfersDTOs { get; set; } = new ObservableCollection<TransferDisplayDTO>();
	
	#endregion
	
	public ObservableCollection<CategoryDisplayDTO> CategoriesDTOs { get; set; } = new ObservableCollection<CategoryDisplayDTO>();
	
	
	public AccountViewModel(MainViewModel mainVM) : base(mainVM)
	{
		AddTransferCommand = new RelayCommand(_ => TransferFormCall(false, ReceiveTransferForm));
		UpdateTransferCommand = new RelayCommand(_ => TransferFormCall(true, ReceiveTransferForm));
		DeleteTransferCommand = new RelayCommand(_ => ConfirmationWindowCall(DeleteTransfer));
	}
	
	#region Updating Data
	
	public override void UpdateData()
	{
		base.UpdateData();
		
		UpdateAccounts();
	}

	private void UpdateAccounts()
	{
		Accounts.Clear();
		Accounts.AddRange(mainVM.accountService.GetAllAccountAsync(mainVM.CurrentUser.Id).Result);
	}

	public void UpdateSelectedAccount()
	{
		AccountsDTO = new AccountDisplayDTO(CurrentSelectedAccount);
		SelectedBalance = AccountsDTO.AccountBalance.ToString("c2");
		UpdateTransfers();
	}
	
	private void UpdateTransfers()
	{
		Transfers.Clear();
		TransfersDTOs.Clear();
		CategoriesDTOs.Clear();
		
		if(CurrentSelectedAccount == null) return;
		
		Transfers.AddRange(mainVM.transferService.GetTransfersByAccountAsync(mainVM.CurrentUser.Id, CurrentSelectedAccount.Id).Result);

		foreach (var transfer in Transfers)
		{
			TransfersDTOs.Add(new TransferDisplayDTO(transfer));

			if (!(transfer.OperationDate.Month == DateTime.Now.Month || transfer.OperationDate.Month == DateTime.Now.Month-1)) return;

			var cat = CategoriesDTOs.Where(c => c.CategoryId == transfer.CategoryId).FirstOrDefault();

			if (cat == null)
			{
				cat = new CategoryDisplayDTO(transfer.Category);
				CategoriesDTOs.Add(cat);
			}
			
			if(transfer.OperationDate.Month == DateTime.Now.Month)
				cat.CategoryCurrentMonth += transfer.Amount;
			else
				cat.CategoryLastMonth += transfer.Amount;
		}
	}
	
	#endregion
	
	#region TransferForm
	
	private void TransferFormCall(bool isUpdate, EventHandler<bool> func)
	{
		if (isUpdate)
		{
			TransFormDTO.TransferName = SelectedTransfer.TransferName;
			TransFormDTO.TransferAmount = SelectedTransfer.TransferAmount;
			TransFormDTO.TransferCategory = SelectedTransfer.TransferCategory;
			TransFormDTO.TransferAccount = SelectedTransfer.TransferAccount;
			TransFormDTO.TransferDate = SelectedTransfer.TransferDate;
		}
		
		TransferForm = new TransferForm(this, isUpdate);
		TransferForm.ConfirmEvent += func;
		TransferForm.Show();
	}

	private async void ReceiveTransferForm(object? sender, bool isConfirmed)
	{
		if (!isConfirmed)
		{
			TransFormDTO.Reset();
			return;
		}
		
		Transfer trans = new Transfer();
		if (TransferForm.IsUpdate)
		{
			trans = Transfers.Where(c => c.Id == SelectedTransfer.TransferId).FirstOrDefault();
			if (trans == null)
				return;
		}
		
		trans.SourceName = TransFormDTO.TransferName;
		
		if (TransferForm.IsUpdate)
			await mainVM.transferService.UpdateTransferAsync();
		else
			await mainVM.transferService.CreateTransferAsync(trans);
		
		TransferForm.ConfirmEvent -= ReceiveTransferForm;
		TransferForm.Close();
		TransFormDTO.Reset();
		UpdateTransfers();
	}

	private async void DeleteTransfer(object? sender, bool isConfirmed)
	{
		if (!isConfirmed) return;
		await mainVM.transferService.DeleteTransferAsync(mainVM.CurrentUser.Id, SelectedTransfer.TransferId);
		UpdateTransfers();
	}
	
	#endregion
}