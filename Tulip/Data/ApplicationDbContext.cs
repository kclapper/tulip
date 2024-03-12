using Tulip.Models;
using Tulip.Configurations.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Tulip.Data
{

  public class ApplicationDbContext : IdentityDbContext
  {
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<Tasks> Tasks { get; set; }
    public DbSet<TasksResponse> Responses { get; set; }
    public DbSet<Scores> Scores { get; set; }
    public DbSet<LeaderBoader> LeaderBoaders { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }

    public DbSet<ChatMessage> ChatMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      builder.ApplyConfiguration(new RoleSeedConfiguration());
    }

  }


}
