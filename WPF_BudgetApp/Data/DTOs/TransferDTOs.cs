using WPF_BudgetApp.Data.Models;

namespace WPF_BudgetApp.Data.DTOs;

public class TransferFormDTO
{
	public string TransferName { get; set; }
	public decimal TransferAmount { get; set; }
	public Category TransferCategory { get; set; }
	public uint TransferAccount { get; set; }
	public DateTime TransferDate { get; set; }
	public DateTime CreationDate { get; set; }
	public DateTime LastUpdateDate { get; set; }

	public void Reset()
	{
		TransferName = string.Empty;
		TransferAmount = 0;
		TransferCategory = null;
		TransferAccount = 0;
		TransferDate = DateTime.Now;
		CreationDate = DateTime.Now;
		LastUpdateDate = DateTime.Now;
	}
	
	public List<Category> Categories { get; set; }
}

public class TransferDisplayDTO(Transfer transfer)
{
	public uint TransferId { get; set; } = transfer.Id;
	public string TransferName { get; set; } = transfer.SourceName;
	public decimal TransferAmount { get; set; } = transfer.Amount;
	public uint TransferCategory { get; set; } = transfer.CategoryId;
	public string TransferCategoryName { get; set; } = transfer.Category.SourceName;
	public string TransferCategoryColor { get; set; } = transfer.Category.Color;
	public uint TransferAccount { get; set; } = transfer.AccountId;
	public DateTime TransferDate { get; set; } = transfer.OperationDate;
	public DateTime CreationDate { get; set; } = transfer.CreationDate;
	public DateTime LastUpdateDate { get; set; } =  transfer.LastUpdateDate;
}