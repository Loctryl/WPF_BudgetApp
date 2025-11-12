using WPF_BudgetApp.Data.Models;

namespace WPF_BudgetApp.Data.DTOs;

public class CategoryFormDTO
{
	public string CategoryName { get; set; }
	public string CategorySymbol { get; set; }
	public string CategoryColor { get; set; }
}

public class CategoryDisplayDTO(Category category)
{
	public uint CategoryId { get; set; } = category.Id;
	public string CategoryName { get; set; } = category.SourceName;
	public string CategorySymbol { get; set; } = category.Symbol;
	public string CategoryColor { get; set; } = category.Color;
	public float CategoryCurrentMonth { get; set; }
	public float CategoryLastMonth { get; set; }
}