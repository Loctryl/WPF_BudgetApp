using System.Windows.Media;
using WPF_BudgetApp.Data.Models;

namespace WPF_BudgetApp.Data.DTOs;

public class DebtFormDTO
{
	public string DebtName { get; set; }
	public decimal DebtInitialAmount { get; set; }
	public decimal DebtCurrentAmount { get; set; }
	public decimal DebtInterestRate { get; set; }
	public DateTime DebtLimitDate { get; set; }
	public DateTime CreationDate { get; set; }
	public DateTime LastUpdateDate { get; set; }

	public void Reset()
	{
		DebtName = string.Empty;
		DebtInitialAmount = 0;
		DebtCurrentAmount = 0;
		DebtInterestRate = 0;
		DebtLimitDate = DateTime.Now;
		CreationDate = DateTime.Now;
		LastUpdateDate = DateTime.Now;
	}
}

public class DebtDisplayDTO(Debt debt)
{
	public uint DebtId { get; set; } = debt.Id;
	public string DebtName { get; set; } = debt.SourceName;
	public decimal DebtInitialAmount { get; set; } = debt.InitialAmount;
	public decimal DebtCurrentAmount { get; set; } = debt.CurrentDebt;
	public decimal DebtInterestRate { get; set; } = debt.InterestRate;
	public uint DebtCategory { get; set; } = debt.CategoryId;
	public string DebtCategoryName { get; set; } = debt.Category.SourceName;
	public string DebtCategoryColor { get; set; } = debt.Category.Color;
	public DateTime DebtLimitDate { get; set; } = debt.LimitDate;
	public DateTime CreationDate { get; set; } = debt.CreationDate;
	public DateTime LastUpdateDate { get; set; } =  debt.LastUpdateDate;
}