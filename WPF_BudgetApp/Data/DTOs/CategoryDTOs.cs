using System.Windows.Media;
using WPF_BudgetApp.Data.Models;

namespace WPF_BudgetApp.Data.DTOs;

public class CategoryFormDTO
{
	public string CategoryName { get; set; }
	public Color CategoryColor { get; set; }

	public void Reset()
	{
		CategoryName = string.Empty;
		CategoryColor = new Color();
	}
}

public class CategoryDisplayDTO(Category category)
{
	public uint CategoryId { get; set; } = category.Id;
	public string CategoryName { get; set; } = category.SourceName;
	public string CategoryColor { get; set; } = category.Color;
	public DateTime CreationDate { get; set; } = category.CreationDate;
	public DateTime LastUpdateDate { get; set; } =  category.LastUpdateDate;
	public decimal CategoryCurrentMonth { get; set; }
	public decimal CategoryLastMonth { get; set; }
}