using Microsoft.EntityFrameworkCore;
using InventoryRazorApp.Models;
using Microsoft.AspNetCore.Identity; 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages(options => 
{
    // Authorization: Requires a logged-in user for all core inventory pages
    options.Conventions.AuthorizePage("/Index");
    options.Conventions.AuthorizePage("/Dashboard");
    options.Conventions.AuthorizePage("/Create");
    options.Conventions.AuthorizePage("/Edit");
    options.Conventions.AuthorizePage("/Delete");
});

// --- 1. Configure SQLite Database Connection ---
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// --- 2. Configure ASP.NET Identity ---
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<AppDbContext>();

var app = builder.Build();

// --- 3. Configure Middleware ---
app.UseStaticFiles();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

// REQUIRED: Authentication must be placed before Authorization
app.UseAuthentication(); 
app.UseAuthorization();

app.MapRazorPages();

// --- 4. Initialize Database on Startup ---
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate(); 
}

app.Run();