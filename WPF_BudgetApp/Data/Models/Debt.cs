namespace WPF_BudgetApp.Data.Models;

public class Debt : DBTable
{
	public decimal InitialAmount { get; set; }
	public decimal CurrentDebt { get; set; }
	public decimal InterestRate { get; set; }
	public DateTime LimitDate { get; set; }
	public uint CategoryId { get; set; }
	public Category? Category { get; set; }
}