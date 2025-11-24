namespace WPF_BudgetApp.Data.Models;

public class Account : DBTable
{
	public string Color { get; set; } = string.Empty;
	public decimal Balance { get; set; }
	public uint AppUserId { get; set; }
	public AppUser? AppUser { get; set; }
}