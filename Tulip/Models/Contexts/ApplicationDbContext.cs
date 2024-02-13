using Tulip.Configurations.Entities;
using Tulip.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Configuration;

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

  }


}
