namespace WPF_BudgetApp.Data.Models;

public class Debt : DBTable
{
	public float InitialAmount { get; set; }
	public float CurrentDebt { get; set; }
	public float InterestRate { get; set; }
	public DateTime LimitDate { get; set; }
	public uint AppUserId { get; set; }
	public AppUser? AppUser { get; set; }
	
	public uint CategoryId { get; set; }
	public Category? Category { get; set; }
}