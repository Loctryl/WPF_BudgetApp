using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WPF_BudgetApp.Data.Models;

namespace WPF_BudgetApp.Data.Configurations;

public class ArchivedTransferConfiguration : IEntityTypeConfiguration<ArchivedTransfer>
{
	public void Configure(EntityTypeBuilder<ArchivedTransfer> builder)
	{
		// Table Name
		builder.ToTable("ArchivedTransfers");

		// Primary key
		builder.HasKey(a => a.Id);

		// Properties
		builder.Property(a => a.Amount)
			.HasColumnType("decimal(18,2)")
			.IsRequired();
		
		builder.Property(a => a.OperationDate)
			.IsRequired()
			.HasColumnType("datetime");
		
		builder.Property(a => a.AccountName)
			.IsRequired()
			.HasMaxLength(16);
		
		builder.Property(a => a.CategoryName)
			.IsRequired()
			.HasMaxLength(32);
		
		builder.Property(a => a.UserId)
			.IsRequired();
		
		builder.Property(a => a.UserName)
			.IsRequired()
			.HasMaxLength(32);
		
		builder.Property(a => a.CreationDate)
			.IsRequired()
			.HasColumnType("datetime");
		
		builder.Property(a => a.LastUpdateDate)
			.IsRequired()
			.HasColumnType("datetime");
	}
}