using System.Drawing;
using System.Linq.Expressions;
using WPF_BudgetApp.Data.Models;

namespace WPF_BudgetApp.Resources;

public static class Helpers
{
	public static IQueryable<T> OrderByProperty<T>(this IQueryable<T> queryable, string propertyName, bool isDescending = false)
	{
		if(string.IsNullOrWhiteSpace(propertyName))
			return queryable;
		
		var parameter = Expression.Parameter(typeof(T), "x");
		var property = Expression.Property(parameter, propertyName);
		var lambda = Expression.Lambda(property, parameter);

		var methodName = isDescending ? "OrderByDescending" : "OrderBy";
		var resultExpression = Expression.Call(typeof(Queryable), methodName, new Type[] { typeof(T), property.Type }, queryable.Expression, Expression.Quote(lambda));

		return queryable.Provider.CreateQuery<T>(resultExpression);
	}

	/*public static IQueryable<T> FilterWith<T>(this IQueryable<T> queryable, string propertyName, uint value)
	{ 
		
		var parameter = Expression.Parameter(typeof(T), "x");
		var property = Expression.Property(parameter, propertyName);
		//var lambda = Expression.Lambda(property, parameter);
		
		return value != 0 ? queryable : queryable.Where(x =>  == value);
		
		switch (valueType)
		{
			case TypeCode.String:
				return string.IsNullOrWhiteSpace(valueDy) ? queryable : queryable.Where(s => property == valueDy.value);
			case TypeCode.Boolean:
				return !valueDy.value ? queryable : queryable.Where(s => property == valueDy.value);
			case TypeCode.Int16:
		}
		
		return queryable;
	}*/
	
	public static string ToHex(System.Windows.Media.Color c)
		=> $"#{c.R:X2}{c.G:X2}{c.B:X2}";
	
	public static string ToHex(System.Drawing.Color c)
		=> $"#{c.R:X2}{c.G:X2}{c.B:X2}";
	
	public static Account SetNewAccount(uint userId, string sourceName, string symbol, decimal balance, string color = "", 
	DateTime? creationDate = null, DateTime? lastUpdateDate = null)
	{
		Account account = new Account();
		account.AppUserId = userId;
		account.SourceName = sourceName;
		account.Symbol = symbol;
		account.Balance = balance;
		account.Color = color != "" ? color : GetRandomColor();
		account.CreationDate = creationDate ?? DateTime.Now;
		account.LastUpdateDate = lastUpdateDate ?? DateTime.Now;
		
		return account;
	}
	
	public static Category SetNewCategory(uint userId, string sourceName, string symbol, string color = "",
	DateTime? creationDate = null, DateTime? lastUpdateDate = null)
	{
		Category category = new Category();
		category.AppUserId = userId;
		category.SourceName = sourceName;
		category.Symbol = symbol;
		category.Color = color != "" ? color : GetRandomColor();
		category.CreationDate = creationDate ?? DateTime.Now;
		category.LastUpdateDate = lastUpdateDate ?? DateTime.Now;

		return category;
	}

	public static Transfer SetNewTransfer(string sourceName, decimal amount, uint category, uint account, DateTime operationDate,
	DateTime? creationDate = null, DateTime? lastUpdateDate = null)
	{
		Transfer transfer = new Transfer();
		transfer.SourceName = sourceName;
		transfer.Amount = amount;
		transfer.CategoryId = category;
		transfer.AccountId = account;
		transfer.OperationDate = operationDate;
		transfer.CreationDate = creationDate ?? DateTime.Now;
		transfer.LastUpdateDate = lastUpdateDate ?? DateTime.Now;
		
		return transfer;
	}
	
	public static Debt SetNewDebt(string sourceName, decimal initialAmount, decimal currentDebt, decimal interestRate, uint category, DateTime limitDate,
	DateTime? creationDate = null, DateTime? lastUpdateDate = null)
	{
		Debt debt = new Debt();
		debt.SourceName = sourceName;
		debt.InitialAmount = initialAmount;
		debt.CurrentDebt = currentDebt;
		debt.InterestRate = interestRate;
		debt.CategoryId = category;
		debt.LimitDate = limitDate;
		debt.CreationDate = creationDate ?? DateTime.Now;
		debt.LastUpdateDate = lastUpdateDate ?? DateTime.Now;
		
		return debt;
	}
	
	public static string GetRandomString(int length)
	{
		Random rnd = new Random();
		
		const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		return new string(Enumerable.Repeat(chars, length)
			.Select(s => s[rnd.Next(s.Length)]).ToArray());
	}
	
	public static DateTime GetRandomDate(bool todayLimit = true, int minMonth = 1, int maxMonth = 13, int year = 0)
	{
		Random rnd = new Random();
		
		int y = year == 0 ? DateTime.Now.Year : year;
		int m = rnd.Next(minMonth, maxMonth);
		int noOfDaysInMonth = DateTime.DaysInMonth(y, m);
		int d = todayLimit ? rnd.Next(1, noOfDaysInMonth == DateTime.Now.Month ? DateTime.Now.Day : noOfDaysInMonth) : rnd.Next(1, noOfDaysInMonth);

		return new DateTime(y, m, d);
	}

	public static string GetRandomColor()
	{
		Random rnd = new Random();
		return ToHex(Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256)));
	}
}