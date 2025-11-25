using System.Windows.Media;
using WPF_BudgetApp.Data.Models;

namespace WPF_BudgetApp.Data.DTOs;

public class AccountFormDTO
{
	public uint AccountId { get; set; }
	public string AccountName { get; set; }
	public Color AccountColor { get; set; }
	public decimal AccountBalance { get; set; }
	public DateTime CreationDate { get; set; }
	
	public void Reset()
	{
		AccountName = string.Empty;
		AccountColor = new Color();
		AccountBalance = 0;
	}
}

public class AccountDisplayDTO(Account account)
{
	public uint AccountId { get; set; } = account.Id;
	public string AccountName { get; set; } = account.SourceName;
	public string AccountColor { get; set; } = account.Color;
	public decimal AccountBalance { get; set; } = account.Balance;
	public DateTime CreationDate { get; set; } = account.CreationDate;
	public DateTime LastUpdateDate { get; set; } =  account.LastUpdateDate;
}