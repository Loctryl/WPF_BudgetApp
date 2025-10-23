using System.ComponentModel.DataAnnotations.Schema;

namespace WPF_BudgetApp.Data.Models;

[Table("ArchivedTransfer")]
public class ArchivedTransfer : DBTable
{
	public float Amount {get; set;}
	public string Category { get; set; } = string.Empty;
	public DateTime OperationDate {get; set;}
	public string BankAccount {get; set;} = string.Empty;
	public string UserId {get; set;} = string.Empty;
	public string UserName {get; set;} = string.Empty;
}