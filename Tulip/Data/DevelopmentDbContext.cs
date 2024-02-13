using Tulip.Configurations.Entities;
using Tulip.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Tulip.Data
{
  public class DevelopmentDbContext : ApplicationDbContext
  {
    public DevelopmentDbContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      builder.ApplyConfiguration(new RoleSeedConfiguration());
      builder.ApplyConfiguration(new UserSeedConfiguration());
      builder.ApplyConfiguration(new UserRoleSeedConfiguration());
    }

  }


}

