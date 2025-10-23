using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WPF_BudgetApp.Data.Models;

namespace WPF_BudgetApp.Data.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
	public void Configure(EntityTypeBuilder<Account> builder)
	{
		// Table Name
		builder.ToTable("Accounts");

		// Primary key
		builder.HasKey(a => a.Id);

		// Properties
		builder.Property(a => a.SourceName)
			.IsRequired()
			.HasMaxLength(16);
		
		builder.Property(a => a.Symbol)
			.IsRequired()
			.HasMaxLength(5);

		builder.Property(a => a.Balance)
			.IsRequired()
			.HasColumnType("decimal(18,2)");

		// Relations
		builder.HasOne(a => a.AppUser)
			.WithMany(u => u.Accounts)
			.HasForeignKey(a => a.AppUserId)
			.OnDelete(DeleteBehavior.Cascade);
		
		builder.HasMany(a => a.Transfers)
			.WithOne(t => t.Account)
			.HasForeignKey(t => t.AccountId)
			.OnDelete(DeleteBehavior.Restrict);
		
		builder.HasMany(a => a.ProjectionTransfers)
			.WithOne(t => t.Account)
			.HasForeignKey(t => t.AccountId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}