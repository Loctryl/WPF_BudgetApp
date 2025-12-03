using WPF_BudgetApp.Data.Models;

namespace WPF_BudgetApp.Data.DTOs;

public class ProjectionTransferFormDTO : TransferFormDTO
{
	public bool ProjTransferIsMonthly { get; set; }

	public override void Reset()
	{
		base.Reset();
		ProjTransferIsMonthly = false;
	}
}

public class ProjectionTransferDisplayDTO(ProjectionTransfer projTransfer)
{
	public uint ProjTransferId { get; set; } = projTransfer.Id;
	public string ProjTransferName { get; set; } = projTransfer.SourceName;
	public decimal ProjTransferAmount { get; set; } = projTransfer.Amount;
	public bool ProjTransferIsMonthly { get; set; } = projTransfer.IsMonthly;
	public uint ProjTransferCategory { get; set; } = projTransfer.CategoryId;
	public string ProjTransferCategoryName { get; set; } = projTransfer.Category.SourceName;
	public string ProjTransferCategoryColor { get; set; } = projTransfer.Category.Color;
	public DateTime ProjTransferDate { get; set; } = projTransfer.ScheduledDate;
	public DateTime CreationDate { get; set; } = projTransfer.CreationDate;
	public DateTime LastUpdateDate { get; set; } =  projTransfer.LastUpdateDate;
}