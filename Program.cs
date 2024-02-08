using Tulip.Data;
using Tulip.Services.Implementations;
using Tulip.Services.Interfaces;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

/* Configure Logging */
builder.Logging.AddConsole();

/* Add Services */
builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

//builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddScoped<ITasksServices, TasksService>();

builder.Services.AddScoped(sp => new HttpClient { 
    BaseAddress = new Uri(builder.Configuration.GetValue<string>("APIKey"))
});

builder.Services
    .AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
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