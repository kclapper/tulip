using Tulip.Data;
using Tulip.Services.Implementations;
using Tulip.Services.Interfaces;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

/* Configure Logging */
builder.Logging.AddConsole();

/* Add Services */
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<ApplicationDbContext, DevelopmentDbContext>(
        options => options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection")
        )
    );
}
else
{
    builder.Services.AddDbContext<ApplicationDbContext>(
        options => options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection")
        )
    );
}

//builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddScoped<ITasksServices, TasksService>();

builder.Services.AddScoped(sp => new HttpClient { 
    BaseAddress = new Uri(builder.Configuration.GetValue<string>("APIKey"))
});

var identityService = builder.Services.AddIdentity<IdentityUser, IdentityRole>();
if (builder.Environment.IsDevelopment())
{
    identityService.AddEntityFrameworkStores<DevelopmentDbContext>();
}
else
{
    identityService.AddEntityFrameworkStores<ApplicationDbContext>();
}
identityService
    .AddDefaultTokenProviders()
    .AddDefaultUI();

//builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

/* Configure Routes */
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
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