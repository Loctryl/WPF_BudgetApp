using System.Windows.Input;
using WPF_BudgetApp.Commands;
using WPF_BudgetApp.Data.Models;
using WPF_BudgetApp.Services.Interfaces;

namespace WPF_BudgetApp.ViewModel;

public class LoginViewModel : BaseViewModel
{
	private readonly IAppUserService appUserService;
	private readonly IAccountService accountService;
	//private readonly INavigationService _navigationService;
	public ICommand LoginCommand { get; }
	public ICommand RegisterCommand { get; }
	public string Username {get; set;}
	public string Password {get; set;}

	private string errorMessage;

	public LoginViewModel(IAppUserService appUserService, IAccountService accountService /*, INavigationService navigationService*/)
	{
		this.appUserService = appUserService;
		this.accountService = accountService;
		//_navigationService = navigationService;
		LoginCommand = new RelayCommand(async _ => await LoginAsync());
		RegisterCommand = new RelayCommand(async _ => await RegisterAsync());
	}
	
	private async Task LoginAsync()
	{
		errorMessage = string.Empty;

		if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
		{
			errorMessage = "Veuillez remplir tous les champs.";
			return;
		}

		var user = await appUserService.AuthenticateAppUserAsync(Username, Password);

		if (user is null)
		{
			errorMessage = "Identifiants incorrects.";
			return;
		}

		// Connexion réussie : navigation vers la vue principale
		//_navigationService.NavigateTo<DashboardViewModel>();
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
		
		await appUserService.CreateAppUserAsync(appuser);
		await accountService.CreateAccountAsync(appuser.Id, account);
		
		appuser.Accounts.Add(account);
		await appUserService.UpdateAppUserAsync();

		// Connexion réussie : navigation vers la vue principale
		//_navigationService.NavigateTo<DashboardViewModel>();
	}
}