using Microsoft.EntityFrameworkCore;
using TicketsApp.Models;

namespace TicketsApp.Data;

public class TicketsAppContext : DbContext
{
    // Constructor to initialize the DbContext with options
    public TicketsAppContext(DbContextOptions<TicketsAppContext> options)
        : base(options)
    {
    }

    // DbSet for Category entity, represents a table of categories in the database
    public DbSet<Category> Category { get; set; } = default!;

    // DbSet for Event entity, represents a table of events in the database
    public DbSet<Event> Event { get; set; } = default!;

    // This method is used to configure the model and seed data
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // Call the base class implementation

        // Seeding initial data for the Category table
        modelBuilder.Entity<Category>()
            .HasData(
                new Category() { CategoryId = 1, Title = "Art Show", Events = [] }, // Art Show category
                new Category() { CategoryId = 2, Title = "Concert", Events = [] },   // Concert category
                new Category() { CategoryId = 3, Title = "Esports Event", Events = [] }, // Esports Event category
                new Category() { CategoryId = 4, Title = "Sports Event", Events = [] },  // Sports Event category
                new Category() { CategoryId = 5, Title = "Festival", Events = [] }  // Festival category
            );
    }
}
