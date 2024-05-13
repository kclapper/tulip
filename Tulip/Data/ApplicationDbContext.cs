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
    public DbSet<AIChatMessage> AIChatMessages { get; set; }
    public DbSet<FloatingChat> FloatingChats { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      builder.ApplyConfiguration(new RoleSeedConfiguration());

      /* User Chat Relationships */

      builder.Entity<ApplicationUser>()
        .HasMany(e => e.SentMessages)
        .WithOne(e => e.Sender)
        .HasForeignKey(e => e.SenderId)
        .IsRequired();

      builder.Entity<ApplicationUser>()
        .HasMany(e => e.ReceivedMessages)
        .WithOne(e => e.Receiver)
        .HasForeignKey(e => e.ReceiverId)
        .IsRequired();

      builder.Entity<ChatMessage>()
        .Navigation(e => e.Sender)
        .AutoInclude();
      builder.Entity<ChatMessage>()
        .Navigation(e => e.Receiver)
        .AutoInclude();

      /* AI Chat Relationships */

        builder.Entity<AIChatMessage>()
          .HasOne(e => e.User)
          .WithMany()
          .HasForeignKey(e => e.UserId)
          .IsRequired();
        builder.Entity<AIChatMessage>()
          .Navigation(e => e.User)
          .AutoInclude();

      /* User FloatingChat Relationships */

      builder.Entity<ApplicationUser>()
        .HasMany(e => e.FloatingChats)
        .WithOne(e => e.User)
        .HasForeignKey(e => e.UserId)
        .IsRequired();

      builder.Entity<FloatingChat>()
        .Navigation(e => e.User)
        .AutoInclude();
    }

  }


}
