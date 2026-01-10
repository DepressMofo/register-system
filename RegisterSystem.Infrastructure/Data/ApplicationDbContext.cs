using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RegisterSystem.Domain.Entities;

namespace RegisterSystem.Infrastructure.Data
{
  public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
  {
    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);
      builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
  }
}