namespace WPF_BudgetApp.Data;

public class TransferQueryObject
{
	public string SourceName { get; set; } = string.Empty;
	public uint CategoryId { get; set; }= uint.MaxValue;
	public decimal MinAmount { get; set; } = decimal.MinValue;
	public decimal MaxAmount { get; set; } = decimal.MaxValue;
	
	public string OrderBy { get; set; } = string.Empty;
	
	public int PageNumber { get; set; } = 1;
	public int PageSize { get; set; } = 20;
}