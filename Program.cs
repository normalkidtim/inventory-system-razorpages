using Microsoft.EntityFrameworkCore;
using InventoryRazorApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// --- 1. Configure SQLite Database Connection ---
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

var app = builder.Build();

// --- 2. Configure File Uploads (wwwroot/images) ---
// This allows the application to serve files from the 'wwwroot' folder, 
// which is where we will save our item images.
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

// --- 3. Initialize Database on Startup ---
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate(); // Ensures the database file and table are created
}

app.Run();