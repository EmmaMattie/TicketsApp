using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TicketsApp.Data;

// Create a new WebApplicationBuilder instance, which helps in setting up the application's services and configurations
var builder = WebApplication.CreateBuilder(args);

// Add the TicketsAppContext to the services container using SQL Server as the database provider
builder.Services.AddDbContext<TicketsAppContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TicketsAppContext")
        ?? throw new InvalidOperationException("Connection string 'TicketsAppContext' not found.")));

// Add MVC controllers and views to the DI container
builder.Services.AddControllersWithViews();

// Build the application from the builder
var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    // In non-development environments, use a custom error handler
    app.UseExceptionHandler("/Home/Error");
    // Enable HTTP Strict Transport Security (HSTS) for added security in production
    app.UseHsts();
}

// Apply any pending database migrations at application startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<TicketsAppContext>(); // Get the TicketsAppContext from the service provider
    await context.Database.MigrateAsync(); // Apply any migrations to the database asynchronously
}

// Configure middlewares for HTTPS redirection, static files, routing, and authorization
app.UseHttpsRedirection();  // Force all traffic to be redirected to HTTPS
app.UseStaticFiles();       // Enable static file serving for assets like images, CSS, and JS files

app.UseRouting();           // Enable routing middleware to handle incoming requests

app.UseAuthorization();     // Enable authorization middleware to manage user access

// Define a default route for the application
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");  // Route pattern with controller, action, and optional id

// Run the application
app.Run();
