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
		
		builder.Property(a => a.Color)
			.IsRequired()
			.HasMaxLength(9);

		builder.Property(a => a.Balance)
			.IsRequired()
			.HasColumnType("decimal(18,2)");
		
		builder.Property(a => a.CreationDate)
			.IsRequired()
			.HasColumnType("datetime");
		
		builder.Property(a => a.LastUpdateDate)
			.IsRequired()
			.HasColumnType("datetime");

		// Relations
		builder.HasOne(a => a.AppUser)
			.WithMany()
			.HasForeignKey(a => a.AppUserId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}