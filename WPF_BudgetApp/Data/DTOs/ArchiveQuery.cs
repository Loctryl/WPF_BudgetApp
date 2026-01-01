namespace WPF_BudgetApp.Data.DTOs;

public class ArchiveQuery
{
	public string QueryName { get; set; } = string.Empty;
	public string QueryAccount { get; set; } = string.Empty;
	public string QueryCategory { get; set; } = string.Empty;
	public double QueryAmount { get; set; }
	public DateTime QueryOperationDate { get; set; } = DateTime.Now;
	public DateTime QueryCreationDate { get; set; } = DateTime.Now;
}