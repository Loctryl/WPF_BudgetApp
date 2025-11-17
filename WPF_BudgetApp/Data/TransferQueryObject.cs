namespace WPF_BudgetApp.Data;

public class TransferQueryObject
{
	public string SourceName { get; set; } = string.Empty;
	public uint CategoryId { get; set; }= uint.MaxValue;
	public float MinAmount { get; set; } = float.MinValue;
	public float MaxAmount { get; set; } = float.MaxValue;
	
	public string OrderBy { get; set; } = string.Empty;
	
	public int PageNumber { get; set; } = 1;
	public int PageSize { get; set; } = 20;
}