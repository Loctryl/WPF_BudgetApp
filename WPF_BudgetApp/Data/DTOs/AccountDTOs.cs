using System.Windows.Media;
using WPF_BudgetApp.Data.Models;

namespace WPF_BudgetApp.Data.DTOs;

public class AccountFormDTO
{
	public string AccountName { get; set; }
	public string AccountSymbol { get; set; }
	public Color AccountColor { get; set; }
	public float AccountBalance { get; set; }

	public void Reset()
	{
		AccountName = string.Empty;
		AccountSymbol = string.Empty;
		AccountColor = new Color();
		AccountBalance = 0f;
	}
}

public class AccountDisplayDTO(Account account)
{
	public uint AccountId { get; set; } = account.Id;
	public string AccountName { get; set; } = account.SourceName;
	public string AccountSymbol { get; set; } = account.Symbol;
	public string AccountColor { get; set; } = account.Color;
	public float AccountBalance { get; set; } = account.Balance;
}