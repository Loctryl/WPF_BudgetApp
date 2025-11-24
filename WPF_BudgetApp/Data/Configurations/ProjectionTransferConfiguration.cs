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
		
		builder.Property(a => a.IsMonthly)
			.IsRequired()
			.HasColumnType("boolean");
		
		builder.Property(a => a.ScheduledDate)
			.IsRequired()
			.HasColumnType("datetime");
		
		builder.Property(a => a.CreationDate)
			.IsRequired()
			.HasColumnType("datetime");
		
		builder.Property(a => a.LastUpdateDate)
			.IsRequired()
			.HasColumnType("datetime");

		// Relations
		builder.HasOne(a => a.Category)
			.WithMany()
			.HasForeignKey(a => a.CategoryId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasOne(a => a.Account)
			.WithMany()
			.HasForeignKey(a => a.AccountId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}