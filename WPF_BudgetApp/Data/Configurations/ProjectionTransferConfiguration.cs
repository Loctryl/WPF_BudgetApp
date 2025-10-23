using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WPF_BudgetApp.Data.Models;

namespace WPF_BudgetApp.Data.Configurations;

public class ProjectionTransferConfiguration : IEntityTypeConfiguration<ProjectionTransfer>
{
	public void Configure(EntityTypeBuilder<ProjectionTransfer> builder)
	{
		// Table Name
		builder.ToTable("ProjectionTransfers");

		// Primary key
		builder.HasKey(a => a.Id);

		// Properties
		builder.Property(a => a.SourceName)
			.IsRequired()
			.HasMaxLength(16);

		builder.Property(a => a.Amount)
			.IsRequired()
			.HasColumnType("decimal(18,2)");

		// Relations
		builder.HasOne(a => a.Category)
			.WithMany(u => u.ProjectionTransfers)
			.HasForeignKey(a => a.CategoryId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasOne(a => a.Account)
			.WithMany(u => u.ProjectionTransfers)
			.HasForeignKey(a => a.AccountId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}