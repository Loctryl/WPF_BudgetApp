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
		builder.Property(a => a.SourceName)
			.IsRequired()
			.HasMaxLength(16);
		
		builder.Property(a => a.Color)
			.IsRequired()
			.HasMaxLength(9);
		
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
		
		builder.HasMany(a => a.Transfers)
			.WithOne(t => t.Category)
			.HasForeignKey(t => t.CategoryId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}