using System.ComponentModel.DataAnnotations.Schema;

namespace WPF_BudgetApp.Data.Models;

public class TransferTemplate : DBTable
{
	[Column(TypeName = "decimal(18,2)")]
	public float Amount { get; set; }
	public uint CategoryId { get; set; }
	public Category? Category { get; set; }
	public uint BankAccountId { get; set; }
	public Account? BankAccount { get; set; }
	public DateTime OperationDate { get; set; }
}