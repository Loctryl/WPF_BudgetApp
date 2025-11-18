using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WPF_BudgetApp.Data.Models;

namespace WPF_BudgetApp.Data.Configurations;

public class TransferConfiguration : IEntityTypeConfiguration<Transfer>
{
	public void Configure(EntityTypeBuilder<Transfer> builder)
	{
		// Table Name
		builder.ToTable("Transfers");

		// Primary key
		builder.HasKey(a => a.Id);

		// Properties
		builder.Property(a => a.SourceName)
			.IsRequired()
			.HasMaxLength(16);

		builder.Property(a => a.Amount)
			.IsRequired()
			.HasColumnType("decimal(18,2)");
		
		builder.Property(a => a.OperationDate)
			.IsRequired();

		// Relations
		builder.HasOne(a => a.Category)
			.WithMany(c => c.Transfers)
			.HasForeignKey(a => a.CategoryId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasOne(a => a.Account)
			.WithMany(u => u.Transfers)
			.HasForeignKey(a => a.AccountId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}