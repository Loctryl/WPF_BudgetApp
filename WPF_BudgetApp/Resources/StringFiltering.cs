using System.Linq.Expressions;

namespace WPF_BudgetApp.Resources;

public static class StringFiltering
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
}