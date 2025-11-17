namespace WPF_BudgetApp.Data.Models;

public class ProjectionTransfer : Transfer
{
	public bool IsMonthly { get; set; } = false;
	public bool IsPassed { get; set; } = false;
}