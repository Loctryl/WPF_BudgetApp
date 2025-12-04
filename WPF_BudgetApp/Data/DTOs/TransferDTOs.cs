using WPF_BudgetApp.Data.Models;

namespace WPF_BudgetApp.Data.DTOs;

public class TransferFormDTO
{
	public uint TransferId { get; set; }
	public string TransferName { get; set; }
	public decimal TransferAmount { get; set; }
	public decimal FirstTransferAmount { get; set; }
	public bool TransferIsMonthly { get; set; }
	public Category TransferCategory { get; set; }
	public DateTime TransferDate { get; set; }
	public DateTime CreationDate { get; set; }
	public DateTime LastUpdateDate { get; set; }

	public virtual void Reset()
	{
		TransferId = uint.MaxValue;
		TransferName = string.Empty;
		TransferAmount = 0;
		FirstTransferAmount = 0;
		TransferIsMonthly = false;
		TransferCategory = null;
		TransferDate = DateTime.Now;
		CreationDate = DateTime.Now;
		LastUpdateDate = DateTime.Now;
		CategoriesOptions.Clear();
	}

	public List<Category> CategoriesOptions { get; set; } = new List<Category>();
}

public class TransferDisplayDTO(Transfer transfer)
{
	public uint TransferId { get; set; } = transfer.Id;
	public string TransferName { get; set; } = transfer.SourceName;
	public decimal TransferAmount { get; set; } = transfer.Amount;
	public bool TransferIsMonthly { get; set; } = transfer.IsMonthly;
	public uint TransferCategory { get; set; } = transfer.CategoryId;
	public string TransferCategoryName { get; set; } = transfer.Category.SourceName;
	public string TransferCategoryColor { get; set; } = transfer.Category.Color;
	public DateTime TransferDate { get; set; } = transfer.OperationDate;
	public DateTime CreationDate { get; set; } = transfer.CreationDate;
	public DateTime LastUpdateDate { get; set; } =  transfer.LastUpdateDate;
}