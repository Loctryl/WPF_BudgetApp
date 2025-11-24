namespace WPF_BudgetApp.Data.Models;

public class Category : DBTable
{
	public string Color { get; set; } = string.Empty;
	public uint AppUserId { get; set; }
	public AppUser? AppUser { get; set; }
	public List<Transfer> Transfers { get; set; } = new();
}