namespace WPF_BudgetApp.Data.Models;

public class Account : DBTable
{
	public string Symbol { get; set; } = string.Empty;
	public string Color { get; set; } = string.Empty;
	public float Balance { get; set; }
	public string AppUserId { get; set; } = string.Empty;
	public AppUser? AppUser { get; set; }
	
	public List<Transfer> Transfers { get; set; } = new();
	public List<ProjectionTransfer> ProjectionTransfers { get; set; } = new();
}