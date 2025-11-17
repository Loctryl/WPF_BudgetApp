namespace WPF_BudgetApp.Data.Models;

public class ProjectionTransfer : DBTable
{
	public float Amount { get; set; }
	public uint CategoryId { get; set; }
	public Category? Category { get; set; }
	public uint AccountId { get; set; }
	public Account? Account { get; set; }
	public DateTime OperationDate { get; set; }
	
	public bool IsMonthly { get; set; } = false;
	public bool IsPassed { get; set; } = false;
}