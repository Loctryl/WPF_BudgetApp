namespace WPF_BudgetApp.Data.Models;

public class Debt : DBTable
{
	public float InitialAmount { get; set; }
	public float CurrentDebt { get; set; }
	public float InterestRate { get; set; }
	public DateTime LimitDate { get; set; }
	public string AppUserId { get; set; } = string.Empty;
	public AppUser? AppUser { get; set; }
	
	public string CategoryId { get; set; } = string.Empty;
	public Category? Category { get; set; }
}