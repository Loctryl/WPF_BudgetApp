using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WPF_BudgetApp.Data.Models;

namespace WPF_BudgetApp.Data.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
	public void Configure(EntityTypeBuilder<Category> builder)
	{
		// Table Name
		builder.ToTable("Category");

		// Primary key
		builder.HasKey(a => a.Id);

		// Properties
		builder.Property(a => a.Symbol)
			.IsRequired()
			.HasMaxLength(5);

		builder.Property(a => a.CurrentMonthValue)
			.HasColumnType("decimal(18,2)")
			.IsRequired();

		builder.Property(a => a.LastMonthValue)
			.IsRequired()
			.HasColumnType("decimal(18,2)");
		
		builder.Property(a => a.GoalPerMonth)
			.HasColumnType("decimal(18,2)");

		// Relations
		builder.HasOne(a => a.AppUser)
			.WithMany()
			.HasForeignKey(a => a.AppUserId)
			.OnDelete(DeleteBehavior.Cascade);
		
		builder.HasMany(a => a.Transfers)
			.WithOne(t => t.Category)
			.HasForeignKey(t => t.CategoryId)
			.OnDelete(DeleteBehavior.Restrict);
		
		builder.HasMany(a => a.ProjectionTransfers)
			.WithOne(t => t.Category)
			.HasForeignKey(t => t.CategoryId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}