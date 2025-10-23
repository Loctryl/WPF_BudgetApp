using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WPF_BudgetApp.Data.Models;

namespace WPF_BudgetApp.Data.Configurations;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
	public void Configure(EntityTypeBuilder<AppUser> builder)
	{
		// Table Name
		builder.ToTable("AppUsers");

		// Primary key
		builder.HasKey(a => a.Id);

		// Properties
		builder.Property(a => a.SourceName)
			.IsRequired()
			.HasMaxLength(32);
		
		builder.Property(a => a.Password)
			.IsRequired()
			.HasMaxLength(32);
		
		//Relations
		builder.HasMany(a => a.Accounts)
			.WithOne(t => t.AppUser)
			.HasForeignKey(t => t.AppUserId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}