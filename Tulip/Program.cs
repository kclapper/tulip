using Tulip.Data;
using Tulip.Models;
using Tulip.Services.Implementations;
using Tulip.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

/* Configure Logging */
builder.Logging.AddConsole();

/* Add Services */
builder.Services.AddDbContext<ApplicationDbContext>(
    options => {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        if (builder.Environment.IsProduction())
        {
            options.UseSqlServer(connectionString);
        }
        else 
        {
            options.UseSqlite(connectionString);
        }
    }
);

//builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddScoped<ISAPBuilder, SAPBuilder>();
builder.Services.AddScoped<ITasksServices, TasksService>();

builder.Services.AddScoped(sp => new HttpClient { 
    BaseAddress = new Uri(builder.Configuration.GetValue<string>("APIKey"))
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();

//builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

/* Configure Routes */
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        DevelopmentData.Initialize(scope.ServiceProvider);
    }
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
} 
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(
    endpoints => {
        endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Account}/{action=Login}/{id?}"
        );
        endpoints.MapRazorPages();
    }
);

app.Run();