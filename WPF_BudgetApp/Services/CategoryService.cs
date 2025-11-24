using System.Windows.Documents;
using Microsoft.EntityFrameworkCore;
using WPF_BudgetApp.Data;
using WPF_BudgetApp.Data.Models;
using WPF_BudgetApp.Services.Interfaces;

namespace WPF_BudgetApp.Services;

public class CategoryService : ServiceBase<Category>, ICategoryService
{
	public CategoryService(AppDbContext context) : base(context)
	{
	}
	
	protected override IQueryable<Category> CheckedListWithUser(uint userId) 
		=> _context.Categories.Include(x => x.AppUser).AsQueryable().Where(s => s.AppUserId == userId);

	public Task<List<Category>> DebugGetAllCategoryAsync()
		=> _context.Categories.Include(x => x.AppUser).AsQueryable().ToListAsync();

	public async Task<List<Category>> GetAllCategoryAsync(uint userId) 
		=> await CheckedListWithUser(userId).ToListAsync();
	
	public async Task<Category?> GetCategoryByIdAsync(uint userId, uint categoryId) 
		=> await CheckedListWithUser(userId).FirstOrDefaultAsync(x => x.Id == categoryId);

	public async Task<Category?> GetCategoryByNameAsync(uint userId, string name) 
		=> await CheckedListWithUser(userId).FirstOrDefaultAsync(x=> x.SourceName.Equals(name));

	public async Task<Category> CreateCategoryAsync(Category category)
	{
		await _context.Categories.AddAsync(category);
		await _context.SaveChangesAsync();
		return category;
	}

	public async Task UpdateCategoryAsync() => await _context.SaveChangesAsync();

	public async Task<Category?> DeleteCategoryAsync(uint userId, uint categoryId, uint replaceCategoryId)
	{
		var category = GetCategoryByIdAsync(userId, categoryId).Result;
		var replaceCategory = GetCategoryByIdAsync(userId, replaceCategoryId).Result;
		if (category == null || replaceCategory == null)
			return null;

		List<Transfer> missingCat = _context.Transfers.AsQueryable().Where(x => x.CategoryId == category.Id).ToList();
		foreach (var transfer in missingCat)
			transfer.CategoryId = replaceCategory.Id;
		
		_context.Categories.Remove(category);
		await _context.SaveChangesAsync();
		return category;
	}
}