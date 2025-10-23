namespace WPF_BudgetApp.Data;

public class TransferQueryObject
{
	public uint BankId { get; set; } = uint.MaxValue;
	public string Source { get; set; } = string.Empty;
	public uint CategoryId { get; set; }= uint.MaxValue;
	public bool Reviewed { get; set; }
	
	public string OrderBy { get; set; } = string.Empty;
	
	public int PageNumber { get; set; } = 1;
	public int PageSize { get; set; } = 20;
}