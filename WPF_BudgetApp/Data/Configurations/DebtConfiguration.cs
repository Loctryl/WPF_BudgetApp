using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WPF_BudgetApp.Data.Models;

namespace WPF_BudgetApp.Data.Configurations;

public class DebtConfiguration : IEntityTypeConfiguration<Debt>
{
	public void Configure(EntityTypeBuilder<Debt> builder)
	{
		// Table Name
		builder.ToTable("Debt");

		// Primary key
		builder.HasKey(a => a.Id);

		// Properties
		builder.Property(a => a.InitialAmount)
			.IsRequired()
			.HasColumnType("decimal(18,2)");

		builder.Property(a => a.CurrentDebt)
			.IsRequired()
			.HasColumnType("decimal(18,2)");
		
		builder.Property(a => a.InterestRate)
			.IsRequired()
			.HasColumnType("decimal(18,2)");

		// Relations
		builder.HasOne(a => a.AppUser)
			.WithMany()
			.HasForeignKey(a => a.AppUserId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasOne(a => a.Category)
			.WithMany()
			.HasForeignKey(a => a.CategoryId)
			.OnDelete(DeleteBehavior.Restrict);;
	}
}