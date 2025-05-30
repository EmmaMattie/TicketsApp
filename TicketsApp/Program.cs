using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TicketsApp.Data;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add database
builder.Services.AddDbContext<TicketsAppContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TicketsAppContext")
        ?? throw new InvalidOperationException("Connection string 'TicketsAppContext' not found.")));

// Add MVC + Session + Authentication
builder.Services.AddControllersWithViews();
builder.Services.AddSession(); // ✅ Add this line
builder.Services.AddAuthentication("MyCookieAuth").AddCookie("MyCookieAuth", options =>
{
    options.LoginPath = "/Home/Unauthorized";
    options.AccessDeniedPath = "/Home/Unauthorized";
});

var app = builder.Build();

// Configure request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Apply migrations on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<TicketsAppContext>();
    await context.Database.MigrateAsync();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); 
app.UseAuthentication();
app.UseAuthorization();
app.UseStatusCodePagesWithReExecute("/Home/Unauthorized");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
