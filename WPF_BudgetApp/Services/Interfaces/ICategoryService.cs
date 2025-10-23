﻿using WPF_BudgetApp.Data.Models;

namespace WPF_BudgetApp.Services.Interfaces;

public interface ICategoryService
{
	Task<List<Category>> GetAllCategoryAsync(uint userId);
	Task<Category?> GetCategoryByIdAsync(uint userId, uint categoryId);
	Task<Category?> GetCategoryByNameAsync(uint userId, string name);
	Task<Category> CreateCategoryAsync(Category category);
	Task<Category?> UpdateCategoryAsync(uint userId, uint categoryId);
	Task<Category?> DeleteCategoryAsync(uint userId, uint categoryId);
}