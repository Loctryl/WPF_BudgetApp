using System.ComponentModel.DataAnnotations.Schema;

namespace WPF_BudgetApp.Data.Models;

[Table("Category")]
public class Category : DBTable
{
	public string Symbol { get; set; } = string.Empty;
	public string Color { get; set; } = string.Empty;
	public float CurrentMonthValue { get; set; } = 0f;
	public float LastMonthValue { get; set; } = 0f;
	public float GoalPerMonth { get; set; } = 0f;
	public bool IsEarning { get; set; } = false;
	
	public string AppUserId { get; set; } = string.Empty;
	public AppUser? AppUser { get; set; }
}