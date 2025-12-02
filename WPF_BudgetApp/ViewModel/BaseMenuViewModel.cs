using System.Windows.Input;
using WPF_BudgetApp.Commands;
using WPF_BudgetApp.Data.DTOs;
using WPF_BudgetApp.Windows;
using MessageBox = WPF_BudgetApp.Windows.MessageBox;

namespace WPF_BudgetApp.ViewModel;

public abstract class BaseMenuViewModel : BaseViewModel
{
	public readonly MainViewModel mainVM;
	public ICommand MenuCommand { get; }
	public ICommand LogoutCommand { get; }
	public ICommand UserCommand { get; }
	
	public string Name { get; set; } = string.Empty;
	protected MessageBox MessageBox { get; set; }
	
	
	private UserForm UserForm { get; set; }
	private UserFormDTO UserFormDTO { get; set; } = new UserFormDTO();

	protected BaseMenuViewModel(MainViewModel mainVM)
	{
		this.mainVM = mainVM;
		MenuCommand = new RelayCommand(RadioSwitch);
		LogoutCommand = new RelayCommand(Logout);
		UserCommand = new RelayCommand(_ => UserFormCall(ReceiveUserForm));
	}

	public virtual void UpdateData() => Name = mainVM.CurrentUser.SourceName;

	private void RadioSwitch(object parameter)
	{
		string button = (string)parameter;

		switch (button)
		{
			case "DashBoardButton":
				mainVM.SwitchToDashBoard();
				break;
			case "AccountButton":
				mainVM.SwitchToAccount();
				break;
			case "DebtButton":
				mainVM.SwitchToDebt();
				break;
			case "ArchiveButton":
				mainVM.SwitchToArchive();
				break;
		}
	}

	private void Logout(object parameter) => mainVM.Logout();

	private void UserFormCall(EventHandler<bool> func)
	{
		UserFormDTO.Reset();
		
		UserFormDTO.Username = mainVM.CurrentUser.SourceName;
		UserFormDTO.Password = mainVM.CurrentUser.Password;
		UserFormDTO.PrimaryColor = mainVM.CurrentUser.PrimaryColor;
		UserFormDTO.SecondaryColor = mainVM.CurrentUser.SecondaryColor;
		UserFormDTO.TertiaryColor = mainVM.CurrentUser.TertiaryColor;
		
		UserForm = new UserForm(UserFormDTO);
		UserForm.ConfirmEvent += func;
		UserForm.Show();
	}

	private async void ReceiveUserForm(object? sender, bool isConfirmed)
	{
		if (!isConfirmed) return;
		
		mainVM.CurrentUser.SourceName = UserFormDTO.Username;
		mainVM.CurrentUser.Password = UserFormDTO.Password;
		mainVM.CurrentUser.PrimaryColor = UserFormDTO.PrimaryColor;
		mainVM.CurrentUser.SecondaryColor = UserFormDTO.SecondaryColor;
		mainVM.CurrentUser.TertiaryColor = UserFormDTO.TertiaryColor;
		
		await mainVM.appUserService.UpdateAppUserAsync();
	}
}