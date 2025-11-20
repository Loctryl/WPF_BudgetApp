using System.Drawing;
using System.Windows.Input;
using WPF_BudgetApp.Commands;
using WPF_BudgetApp.Data.Models;
using WPF_BudgetApp.Resources;

namespace WPF_BudgetApp.ViewModel;

public class LoginViewModel : BaseViewModel
{
	private readonly MainViewModel mainVM;
	public ICommand LoginCommand { get; }
	public ICommand RegisterCommand { get; }
	public string Username {get; set;}
	public string Password { private get; set;}

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

		var user = await mainVM.appUserService.AuthenticateAppUserAsync(Username, Password);

		if (user is null)
		{
			errorMessage = "Identifiants incorrects.";
			return;
		}

		Username = String.Empty;
		Password = String.Empty;
		
		user.LastUpdateDate = DateTime.Now;
		await mainVM.appUserService.UpdateAppUserAsync();
		
		// login successful
		mainVM.SetCurrentUser(user);
		mainVM.SwitchToDashBoard();
	}
	
	private async Task RegisterAsync()
	{
		errorMessage = string.Empty;

		AppUser appuser = new AppUser();
		appuser.SourceName = Username;
		appuser.Password = Password;
		appuser.CreationDate = DateTime.Now;
		
		Account account = new Account();
		account.SourceName = "Cash";
		account.Symbol = "CASH";
		account.Color = "#ffffff";
		account.CreationDate = DateTime.Now;
		account.LastUpdateDate = DateTime.Now;
		
		var user = await mainVM.appUserService.CreateAppUserAsync(appuser);
		account.AppUserId = appuser.Id;
		await mainVM.accountService.CreateAccountAsync(account);
		
		appuser.Accounts.Add(account);
		appuser.LastUpdateDate = DateTime.Now;
		await mainVM.appUserService.UpdateAppUserAsync();
		
		Username = String.Empty;
		Password = String.Empty;

		// register successful
		mainVM.SetCurrentUser(user);

		await PopulateNewUser(user);
		
		mainVM.SwitchToDashBoard();
	}
	
	private async Task PopulateNewUser(AppUser user)
	{
		Random rnd = new Random();
		
		await CreateNewAccount(user.Id, "Caisse d'Epargne", "CE");
		await CreateNewAccount(user.Id, "Credit Mutuel", "CM");
		await CreateNewAccount(user.Id, "Credit Agricole", "CA");
		
		await CreateNewCategory(user.Id, "Food", "FD");
		await CreateNewCategory(user.Id, "Gasoline", "GAS");
		await CreateNewCategory(user.Id, "Sport", "SP");
		await CreateNewCategory(user.Id, "Sanitary", "SNT");
		await CreateNewCategory(user.Id, "Clothes", "CLT");
		await CreateNewCategory(user.Id, "Activities", "ACT");
		await CreateNewCategory(user.Id, "Others", "OTR");
		
		List<Account> accs = mainVM.accountService.GetAllAccountAsync(user.Id).Result;
		List<Category> cats = mainVM.categoryService.GetAllCategoryAsync(user.Id).Result;

		for (int i = 0; i < 250; i++)
		{
			string str = RandomString(3);
			decimal amount = (decimal)(rnd.Next(20000)) / (decimal)100;
			amount = rnd.Next(0,2) == 0 ? amount : -amount;
			await CreateNewTransfer(str, amount, cats[rnd.Next(cats.Count)].Id, accs[rnd.Next(accs.Count)].Id, GetRandomDate());
		}
	}
	
	private async Task CreateNewAccount(uint userId, string sourceName, string symbol)
	{
		Random rnd = new Random();
		
		Account account = new Account();
		account.SourceName = sourceName;
		account.Symbol = symbol;
		account.Color = Helpers.ToHex(Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256)));
		account.Balance = (decimal)(rnd.Next(200000)) / (decimal)100;
		account.CreationDate = DateTime.Now;
		account.LastUpdateDate = DateTime.Now;
		account.AppUserId = userId;
		
		await mainVM.accountService.CreateAccountAsync(account);
	}
	
	private async Task CreateNewCategory(uint userId, string sourceName, string symbol)
	{
		Random rnd = new Random();
		
		Category category = new Category();
		category.SourceName = sourceName;
		category.Symbol = symbol;
		category.Color = Helpers.ToHex(Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256)));
		category.CreationDate = DateTime.Now;
		category.LastUpdateDate = DateTime.Now;
		category.AppUserId = userId;
		
		await mainVM.categoryService.CreateCategoryAsync(category);
	}

	private async Task CreateNewTransfer(string sourceName, decimal amount, uint category, uint account, DateTime date)
	{
		Transfer transfer = new Transfer();
		transfer.SourceName = sourceName;
		transfer.Amount = amount;
		transfer.CategoryId = category;
		transfer.AccountId = account;
		transfer.OperationDate = date;
		transfer.CreationDate = DateTime.Now;
		transfer.LastUpdateDate = DateTime.Now;
		
		await mainVM.transferService.CreateTransferAsync(transfer);
	}
	
	private static string RandomString(int length)
	{
		Random rnd = new Random();
		
		const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		return new string(Enumerable.Repeat(chars, length)
			.Select(s => s[rnd.Next(s.Length)]).ToArray());
	}
	
	private static DateTime GetRandomDate()
	{
		Random rnd = new Random();
		
		var year = DateTime.Now.Year;
		var month = rnd.Next(6, 12);
		var noOfDaysInMonth = DateTime.DaysInMonth(year, month);
		var day = rnd.Next(1, noOfDaysInMonth == 11 ? 21 : noOfDaysInMonth);

		return new DateTime(year, month, day);
	}
}