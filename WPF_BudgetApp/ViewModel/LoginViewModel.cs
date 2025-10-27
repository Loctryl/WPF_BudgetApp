using System.Windows.Input;
using WPF_BudgetApp.Commands;
using WPF_BudgetApp.Data.Models;

namespace WPF_BudgetApp.ViewModel;

public class LoginViewModel : BaseViewModel
{
	private readonly MainViewModel mainVM;
	public ICommand LoginCommand { get; }
	public ICommand RegisterCommand { get; }
	public string Username {get; set;}
	public string Password {get; set;}

	private string errorMessage;

	public LoginViewModel(MainViewModel mainVM)
	{
		LoginCommand = new RelayCommand(async _ => await LoginAsync());
		RegisterCommand = new RelayCommand(async _ => await RegisterAsync());
		this.mainVM = mainVM;
	}
	
	private async Task LoginAsync()
	{
		errorMessage = string.Empty;

		if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
		{
			errorMessage = "Veuillez remplir tous les champs.";
			return;
		}

		var user = await mainVM.appUserService.AuthenticateAppUserAsync(Username, Password);

		if (user is null)
		{
			errorMessage = "Identifiants incorrects.";
			return;
		}

		// login successful
		mainVM.SwitchToDashBoard();
	}
	
	private async Task RegisterAsync()
	{
		errorMessage = string.Empty;

		if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
		{
			errorMessage = "Veuillez remplir tous les champs.";
			return;
		}

		AppUser appuser = new AppUser();
		appuser.SourceName = Username;
		appuser.Password = Password;
		
		Account account = new Account();
		account.SourceName = "Cash";
		account.Symbol = "CASH";
		
		await mainVM.appUserService.CreateAppUserAsync(appuser);
		await mainVM.accountService.CreateAccountAsync(appuser.Id, account);
		
		appuser.Accounts.Add(account);
		await mainVM.appUserService.UpdateAppUserAsync();

		// register successful
		mainVM.SwitchToDashBoard();
	}
}