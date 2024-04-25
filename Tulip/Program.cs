using Tulip.Data;
using Tulip.Hubs;
using Tulip.Services.Implementations;
using Tulip.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

/* Configure Logging */
builder.Logging.AddConsole();

/* Add Configuration Providers */
builder.Configuration.AddInMemoryCollection(new Dictionary<string, string>{
    { "AIChatModelPath", "" }
});

/* Increase max upload size for AI model upload */
const long maxUploadSize = 1024L * 1024L * 1024L * 5L; // 5GB max upload limit
builder.Services.Configure<FormOptions>(options => {
    options.MultipartBodyLengthLimit = maxUploadSize; 
});
builder.WebHost.ConfigureKestrel(serverOptions => {
    serverOptions.Limits.MaxRequestBodySize = maxUploadSize;
});

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

builder.Services.AddScoped<ISAPBuilder, SAPBuilder>();
builder.Services.AddScoped<ITasksServices, TasksService>();

builder.Services.AddSingleton<IAIChat, LLamaChat>();

builder.Services.AddScoped(sp => new HttpClient { 
    BaseAddress = new Uri(builder.Configuration.GetValue<string>("APIKey"))
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();

builder.Services.AddControllersWithViews();

builder.Services.AddSignalR();

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

app.MapHub<ChatHub>("/chatHub");
app.MapHub<AIChatHub>("/aiChatHub");

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}"
);

app.Run();