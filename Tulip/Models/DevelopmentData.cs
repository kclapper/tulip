using Tulip.Data;
using Microsoft.AspNetCore.Identity;

namespace Tulip.Models
{

    public static class DevelopmentData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = serviceProvider.GetRequiredService<ApplicationDbContext>())
            {
                    Console.WriteLine("GETS HERE");
                // Look for existing data.
                if (context.ApplicationUsers.Any())
                {
                    return;   // DB has alrady been seeded
                }

                AddAdminUser(context);
                AddNormalUser(context);

                context.Tasks.AddRange(
                    new Tasks { Description = "click on complete for 5 points double click for 10 points", StepNumber = "Step 1" },
                     new Tasks { Description = "click on complete for 5 points double click for 10 points", StepNumber = "Step 2" },
                      new Tasks { Description = "click on complete for 5 points double click for 10 points", StepNumber = "Step 3" },
                       new Tasks { Description = "click on complete for 5 points double click for 10 points", StepNumber = "Step 4" },
                        new Tasks { Description = "click on complete for 5 points double click for 10 points", StepNumber = "Step 5" }

                );
                context.SaveChanges();
            }
        }

        private static void AddAdminUser(ApplicationDbContext context)
        {
			var hasher = new PasswordHasher<ApplicationUser>();

            context.ApplicationUsers.Add(
                new ApplicationUser 
                {
					Id = "8e445865-a24d-4543-a6c6-9443d048cdb9",
					Email = "admin@localhost.com",
					NormalizedEmail = "ADMIN@LOCALHOST.COM",
					FirstName = "System",
					LastName = "Admin",
					UserName = "admin",
					NormalizedUserName = "ADMIN",
					ApplicationServer = "trek.ucc.uwm.edu",
					ClientId = 101,
					UserId = "Learn-031",
					PasswordHash = hasher.HashPassword(null, "Password@1")
                }
            );

            context.UserRoles.Add(
                new IdentityUserRole<string>
				{
					RoleId = "cbc43a8e-f7bb-4445-baaf-1add431ffbbf",
					UserId = "8e445865-a24d-4543-a6c6-9443d048cdb9"
				}
            );
        }

        private static void AddNormalUser(ApplicationDbContext context) 
        {
			var hasher = new PasswordHasher<ApplicationUser>();

            context.ApplicationUsers.Add(
				new ApplicationUser
				{
					Id = "9e224968-33e4-4652-b7b7-8574d048cdb9",
					Email = "user@localhost.com",
					NormalizedEmail = "USER@LOCALHOST.COM",
					FirstName = "System",
					LastName = "User",
					UserName = "user",
					NormalizedUserName = "USER",
					ApplicationServer = "trek.ucc.uwm.edu",
					ClientId = 101,
					UserId = "Learn-031",
					PasswordHash = hasher.HashPassword(null, "User@123")
				}
            );

            context.UserRoles.Add(
				new IdentityUserRole<string>
				{
					RoleId = "cac43a6e-f7bb-4448-baaf-1add431ccbbf",
					UserId = "9e224968-33e4-4652-b7b7-8574d048cdb9"
				}
            );
        }
    }
}