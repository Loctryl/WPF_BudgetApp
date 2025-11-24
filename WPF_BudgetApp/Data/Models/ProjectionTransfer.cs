namespace WPF_BudgetApp.Data.Models;

public class ProjectionTransfer : DBTable
{
	public decimal Amount { get; set; }
	public bool IsMonthly { get; set; }
	public DateTime ScheduledDate { get; set; }
	public uint CategoryId { get; set; }
	public Category? Category { get; set; }
	public uint AccountId { get; set; }
	public Account? Account { get; set; }
}