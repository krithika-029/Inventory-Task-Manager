using Microsoft.EntityFrameworkCore;
using InventoryTaskManager.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Configure Entity Framework with SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add developer exception page for EF
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Configure application settings
builder.Services.Configure<ApplicationSettings>(
    builder.Configuration.GetSection("ApplicationSettings"));

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Configure routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Create database if it doesn't exist
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
}

app.Run();

/// <summary>
/// Application settings configuration class
/// </summary>
public class ApplicationSettings
{
    public int LowStockThreshold { get; set; } = 5;
    public int TaskReminderDays { get; set; } = 3;
    public string ApplicationTitle { get; set; } = "Inventory & Task Management System";
    public string CompanyName { get; set; } = "Retail Store Management";
    public int ItemsPerPage { get; set; } = 10;
}
