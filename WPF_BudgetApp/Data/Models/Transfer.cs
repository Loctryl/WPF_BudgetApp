using System.ComponentModel.DataAnnotations.Schema;

namespace WPF_BudgetApp.Data.Models;

[Table("Transfer")]
public class Transfer : TransferTemplate
{
	public DateTime DebitDate { get; set; }
	public bool Reviewed { get; set; } = false;
}