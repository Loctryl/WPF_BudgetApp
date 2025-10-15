using Microsoft.EntityFrameworkCore;
using WPF_BudgetApp.Data.Models;

namespace WPF_BudgetApp.Data;

public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions options) : base(options)
	{
		
	}
	
	public DbSet<AppUser> AppUsers { get; set; }
	public DbSet<Transfer> Transfers { get; set; }
	//public DbSet<ProjectionTransfer> ProjectionTransfers { get; set; }
	public DbSet<Account> BankAccounts { get; set; }
	public DbSet<Category> Categories { get; set; }
	//public DbSet<Debt> Debts { get; set; }
	//public DbSet<ArchivedTransfer> ArchivedTransfers { get; set; }
	
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		
		modelBuilder.Entity<AppUser>().HasIndex(u => u.SourceName).IsUnique();

		modelBuilder.Entity<Transfer>().HasOne(e => e.Category).WithMany().OnDelete(DeleteBehavior.Restrict);
		modelBuilder.Entity<Transfer>().HasOne(e => e.BankAccount).WithMany().OnDelete(DeleteBehavior.Restrict);
		
		modelBuilder.Entity<Account>().HasOne(e => e.AppUser).WithMany().OnDelete(DeleteBehavior.Restrict);
		modelBuilder.Entity<Category>().HasOne(e => e.AppUser).WithMany().OnDelete(DeleteBehavior.Restrict);
	}
}