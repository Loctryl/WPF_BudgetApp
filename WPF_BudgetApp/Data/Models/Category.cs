namespace WPF_BudgetApp.Data.Models;

public class Category : DBTable
{
	public string Symbol { get; set; } = string.Empty;
	public string Color { get; set; } = string.Empty;
	public float CurrentMonthValue { get; set; } = 0f;
	public float LastMonthValue { get; set; } = 0f;
	public float GoalPerMonth { get; set; } = 0f;
	public bool IsEarning { get; set; } = false;
	
	public uint AppUserId { get; set; }
	public AppUser? AppUser { get; set; }
	
	public List<Transfer> Transfers { get; set; } = new();
	public List<ProjectionTransfer> ProjectionTransfers { get; set; } = new();
}