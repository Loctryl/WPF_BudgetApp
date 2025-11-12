using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using WPF_BudgetApp.Data.Models;

namespace WPF_BudgetApp.Data;

public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions options) : base(options)
	{
		Database.EnsureCreated();
	}
	
	public DbSet<AppUser> AppUsers { get; set; }
	public DbSet<Transfer> Transfers { get; set; }
	public DbSet<ProjectionTransfer> ProjectionTransfers { get; set; }
	public DbSet<Account> Accounts { get; set; }
	public DbSet<Category> Categories { get; set; }
	public DbSet<Debt> Debts { get; set; }
	public DbSet<ArchivedTransfer> ArchivedTransfers { get; set; }
	
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
	}
}

public class DbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
	public AppDbContext CreateDbContext(string[] args)
	{
		var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
		optionsBuilder.UseSqlite("DataSource=BudgetDataBase.db");

		return new AppDbContext(optionsBuilder.Options);
	}
}