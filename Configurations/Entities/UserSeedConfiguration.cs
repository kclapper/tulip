using Gamification.UI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gamification.UI.Configurations.Entities
{
	public class UserSeedConfiguration : IEntityTypeConfiguration<ApplicationUser>
	{
		public void Configure(EntityTypeBuilder<ApplicationUser> builder)
		{
			var hasher = new PasswordHasher<ApplicationUser>();
			builder.HasData(
				new ApplicationUser
				{
					Id = "8e445865-a24d-4543-a6c6-9443d048cdb9",
					Email = "admin@localhost.com",
					NormalizedEmail = "ADMIN@LOCALHOST.COM",
					FirstName = "System",
					LastName = "Admin",
					UserName = "admin",
					NormalizedUserName = "ADMIN",
					ApplicationServer = "e45z.4.ucc.md",
					ClientId = 111,
					UserId = "Learn-031",
					PasswordHash = hasher.HashPassword(null, "Password@1")
				},
				new ApplicationUser
				{
					Id = "9e224968-33e4-4652-b7b7-8574d048cdb9",
					Email = "user@localhost.com",
					NormalizedEmail = "USER@LOCALHOST.COM",
					FirstName = "System",
					LastName = "User",
					UserName = "user",
					NormalizedUserName = "USER",
					ApplicationServer = "e45z.4.ucc.md",
					ClientId = 111,
					UserId = "Learn-031",
					PasswordHash = hasher.HashPassword(null, "User@123")
				}
			);
		}
	}

}
