using Gamification.UI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Gamification.UI.Models
{

    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // Look for any Tasks.
                if (context.Tasks.Any())
                {
                    return;   // DB has been seeded
                }

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
    }
}
