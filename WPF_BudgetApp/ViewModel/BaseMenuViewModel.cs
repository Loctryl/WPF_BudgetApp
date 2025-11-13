using System.Windows;
using System.Windows.Input;
using WPF_BudgetApp.Commands;
using WPF_BudgetApp.Windows;

namespace WPF_BudgetApp.ViewModel;

public abstract class BaseMenuViewModel : BaseViewModel
{
	protected readonly MainViewModel mainVM;
	public ICommand MenuCommand { get; }
	public ICommand LogoutCommand { get; }
	
	public string Name { get; set; } = string.Empty;
	
	private ConfirmWindow ConfirmWindow { get; set; }

	protected BaseMenuViewModel(MainViewModel mainVM)
	{
		this.mainVM = mainVM;
		MenuCommand = new RelayCommand(RadioSwitch);
		LogoutCommand = new RelayCommand(Logout);
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
	
	protected void ConfirmationWindowCall(EventHandler<bool> func)
	{
		ConfirmWindow = new ConfirmWindow();
		ConfirmWindow.ConfirmEvent += func;
		ConfirmWindow.Show();
	}
}