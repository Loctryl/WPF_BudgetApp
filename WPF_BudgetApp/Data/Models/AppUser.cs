using System.ComponentModel.DataAnnotations.Schema;

namespace WPF_BudgetApp.Data.Models;

[Table("AppUser")]
public class AppUser : DBTable
{
	public string Password { get; set; } = string.Empty;
	
	public List<Account> Accounts { get; set; } = new();
}