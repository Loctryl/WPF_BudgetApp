namespace WPF_BudgetApp.Data.Models;

public class AppUser : DBTable
{
	public string Password { get; set; } = string.Empty;
	public string PrimaryColor { get; set; } = string.Empty;
	public string SecondaryColor { get; set; } = string.Empty;
	public string TertiaryColor { get; set; } = string.Empty;
	
}