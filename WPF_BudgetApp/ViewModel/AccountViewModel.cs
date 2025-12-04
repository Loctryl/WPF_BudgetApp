using System.Collections.ObjectModel;
using System.Windows.Input;
using WPF_BudgetApp.Commands;
using WPF_BudgetApp.Data.DTOs;
using WPF_BudgetApp.Data.Models;
using WPF_BudgetApp.Resources;
using WPF_BudgetApp.Windows;

namespace WPF_BudgetApp.ViewModel;

public class AccountViewModel : BaseMenuViewModel
{
	public Account CurrentSelectedAccount { get; set; }
	public string SelectedBalance { get; set; }
	public int SelectedMonth { get; set; }

	public List<Account> Accounts { get; set; } = new List<Account>();
	public AccountDisplayDTO AccountsDTO { get; set; }
	
	#region Commands

	public ICommand AddTransferCommand { get; } 
	public ICommand UpdateTransferCommand { get; } 
	public ICommand DeleteTransferCommand { get; } 
	
	public ICommand AddProjectionTransferCommand { get; } 
	public ICommand UpdateProjectionTransferCommand { get; } 
	public ICommand DeleteProjectionTransferCommand { get; } 
	
	public ICommand TransferMonthCommand { get; }
	
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
		AddTransferCommand = new RelayCommand(_ => TransferFormCall(FormType.ADD, ReceiveTransferForm));
		UpdateTransferCommand = new RelayCommand(_ => TransferFormCall(FormType.EDIT, ReceiveTransferForm));
		DeleteTransferCommand = new RelayCommand(_ => TransferFormCall(FormType.DELETE, ReceiveTransferForm));
		
		TransferMonthCommand = new RelayCommand(TransferMonthSwitch);
	}

	private void TransferMonthSwitch(object parameter)
	{
		if(CurrentSelectedAccount == null) return;
		SelectedMonth = Convert.ToInt32(parameter);
		UpdateTransfers();
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
		SelectedBalance = AccountsDTO.AccountBalance >= 0 ? "+" : "-";
		SelectedBalance += AccountsDTO.AccountBalance.ToString("c2");
		UpdateTransfers();
	}
	
	private void UpdateTransfers()
	{
		Transfers.Clear();
		TransfersDTOs.Clear();
		CategoriesDTOs.Clear();

		var month = DateTime.Now.Month + SelectedMonth;
		int month2;
		if (month == 13) month = 1;
		
		if (SelectedMonth == 1)
		{
			Transfers.AddRange(
				(mainVM.transferService.GetTransfersByAccountAsync(mainVM.CurrentUser.Id, CurrentSelectedAccount.Id).Result)
				.Where(t => t.OperationDate >= DateTime.Now)
			);
		}
		else
		{
			Transfers.AddRange(
				(mainVM.transferService.GetTransfersByAccountAsync(mainVM.CurrentUser.Id, CurrentSelectedAccount.Id).Result)
				.Where(t => t.OperationDate.Month == month || t.OperationDate.Month == month-1)
			);
		}
		
		

		foreach (var transfer in Transfers)
		{
			if(transfer.OperationDate.Month == month)
				TransfersDTOs.Add(new TransferDisplayDTO(transfer));

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
	
	private void TransferFormCall(FormType formType, EventHandler<bool> func)
	{
		TransFormDTO.Reset();
		
		switch (formType)
		{
			case FormType.ADD:
				TransFormDTO.CategoriesOptions = mainVM.categoryService.GetAllCategoryAsync(mainVM.CurrentUser.Id).Result;
				break;
			case FormType.EDIT:
				TransFormDTO.TransferId = SelectedTransfer.TransferId;
				TransFormDTO.TransferName = SelectedTransfer.TransferName;
				TransFormDTO.TransferAmount = SelectedTransfer.TransferAmount;
				TransFormDTO.FirstTransferAmount = SelectedTransfer.TransferAmount;
				TransFormDTO.TransferCategory = mainVM.categoryService.GetCategoryByIdAsync(mainVM.CurrentUser.Id, SelectedTransfer.TransferCategory).Result;
				TransFormDTO.TransferDate = SelectedTransfer.TransferDate;
				TransFormDTO.CreationDate = SelectedTransfer.CreationDate;
				
				TransFormDTO.CategoriesOptions = mainVM.categoryService.GetAllCategoryAsync(mainVM.CurrentUser.Id).Result;
				break;
			case FormType.DELETE:
				TransFormDTO.TransferId = SelectedTransfer.TransferId;
				TransFormDTO.TransferAmount = SelectedTransfer.TransferAmount;
				break;
		}
		
		TransferForm = new TransferForm(TransFormDTO, formType);
		TransferForm.ConfirmEvent += func;
		TransferForm.Show();
	}

	private async void ReceiveTransferForm(object? sender, bool isConfirmed)
	{
		if (!isConfirmed) return;
		
		switch (TransferForm.FormType)
		{
			case FormType.ADD:
				await AddTransfer();
				break;
			case FormType.EDIT:
				await EditTransfer();
				break;
			case FormType.DELETE:
				await DeleteTransfer();
				break;
		}
		
		UpdateSelectedAccount();
	}
	
	private async Task AddTransfer()
	{
		Transfer trans = Helpers.SetNewTransfer(
			TransFormDTO.TransferName, 
			TransFormDTO.TransferAmount,
			TransFormDTO.TransferCategory.Id,
			CurrentSelectedAccount.Id,
			TransFormDTO.TransferDate,
			TransFormDTO.TransferIsMonthly
		);
		
		CurrentSelectedAccount.Balance += trans.Amount;
		await mainVM.transferService.CreateTransferAsync(trans);
	}
	
	private async Task EditTransfer()
	{
		Transfer trans = Transfers.First(c => c.Id == TransFormDTO.TransferId);
		trans.SourceName = TransFormDTO.TransferName;
		trans.Amount = TransFormDTO.TransferAmount;
		trans.CategoryId = TransFormDTO.TransferCategory.Id;
		trans.OperationDate = TransFormDTO.TransferDate;
		trans.LastUpdateDate = DateTime.Now;
		
		CurrentSelectedAccount.Balance -= TransFormDTO.FirstTransferAmount;
		CurrentSelectedAccount.Balance += trans.Amount;
		await mainVM.transferService.UpdateTransferAsync();
	}

	private async Task DeleteTransfer()
	{
		CurrentSelectedAccount.Balance -= TransFormDTO.TransferAmount;
		await mainVM.transferService.DeleteTransferAsync(mainVM.CurrentUser.Id, TransFormDTO.TransferId);
	}
	
	#endregion
	
}