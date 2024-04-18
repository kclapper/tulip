using Tulip.Data;
using Tulip.Hubs;
using Tulip.Services.Implementations;
using Tulip.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using LLama.Common;
using LLama;
using LLama.Abstractions;
using LLama.Native;
using Microsoft.Extensions.Logging.Abstractions;
using System.Reflection;

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

builder.Services.AddScoped<ISAPBuilder, SAPBuilder>();
builder.Services.AddScoped<ITasksServices, TasksService>();

var assemblyPath = Assembly.GetExecutingAssembly().Location;
var assemblyDirectory = Path.GetDirectoryName(assemblyPath);
var modelPath = assemblyDirectory + "/Hubs/llama-2-7b-chat.gguf";

NativeLibraryConfig.Instance.WithLogs(false);
var modelParameters = new ModelParams(modelPath);
var model = LLamaWeights.LoadFromFile(modelParameters);
var context = model.CreateContext(modelParameters, NullLogger.Instance);
var executor = new InteractiveExecutor(context, NullLogger.Instance);
builder.Services.AddScoped<ILLamaExecutor, InteractiveExecutor>((serviceProvider) => executor);

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