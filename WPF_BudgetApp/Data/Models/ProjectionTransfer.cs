using System.ComponentModel.DataAnnotations.Schema;

namespace WPF_BudgetApp.Data.Models;

[Table("ProjectionTransfer")]
public class ProjectionTransfer : TransferTemplate
{
	public bool IsMonthly { get; set; } = false;
	public bool IsPassed { get; set; } = false;
}