using System.ComponentModel.DataAnnotations.Schema;

namespace WPF_BudgetApp.Data.Models;

public class Transfer : DBTable
{
	public float Amount { get; set; }
	public uint CategoryId { get; set; }
	public Category? Category { get; set; }
	public uint AccountId { get; set; }
	public Account? Account { get; set; }
	public DateTime OperationDate { get; set; }
}