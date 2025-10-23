namespace WPF_BudgetApp.Data.Models;

public class AppUser : DBTable
{
	public string Password { get; set; } = string.Empty;
	public List<Account> Accounts { get; set; } = new();
}