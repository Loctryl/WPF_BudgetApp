namespace WPF_BudgetApp.Data.Models;

public class Transfer : TransferTemplate
{
	public DateTime DebitDate { get; set; }
	public bool Reviewed { get; set; } = false;
}