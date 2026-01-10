using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RegisterSystem.Domain.Entities;

namespace RegisterSystem.Infrastructure.Data.Configurations
{
  public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
  {
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
      builder.Property((a) => a.FirstName)
        .HasMaxLength(100)
        .IsRequired();

      builder.Property((a) => a.LastName)
        .HasMaxLength(100)
        .IsRequired();
    }
  }
}