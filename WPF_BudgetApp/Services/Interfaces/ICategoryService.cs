using WPF_BudgetApp.Data.Models;

namespace WPF_BudgetApp.Services.Interfaces;

public interface ICategoryService
{
	Task<List<Category>> GetAllCategoryAsync(string userId);
	Task<Category?> GetCategoryByIdAsync(string userId, uint categoryId);
	Task<Category?> GetCategoryByNameAsync(string userId, string name);
	Task<Category> CreateCategoryAsync(Category category);
	Task<Category?> UpdateCategoryAsync(string userId, uint categoryId);
	Task<Category?> DeleteCategoryAsync(string userId, uint categoryId);
}