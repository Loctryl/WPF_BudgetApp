namespace WPF_BudgetApp.Data.Models;

public class ProjectionTransfer : TransferTemplate
{
	public bool IsMonthly { get; set; } = false;
	public bool IsPassed { get; set; } = false;
}