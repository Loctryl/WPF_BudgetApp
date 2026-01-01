namespace WPF_BudgetApp.Data.Models;

public class ArchivedTransfer : DBTable
{
	public double Amount {get; set;}
	public string CategoryName { get; set; } = string.Empty;
	public string AccountName {get; set;} = string.Empty;
	public DateTime OperationDate {get; set;}
	public uint UserId {get; set;}
	public string UserName {get; set;} = string.Empty;
}