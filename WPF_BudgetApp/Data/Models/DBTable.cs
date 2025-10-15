namespace WPF_BudgetApp.Data.Models;

public abstract class DBTable
{
	public uint Id { get; set; }
	public string SourceName { get; set; } = string.Empty;
	public string Comment { get; set; } = string.Empty;
}