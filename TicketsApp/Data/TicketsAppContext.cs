using Microsoft.EntityFrameworkCore;
using TicketsApp.Models;

namespace TicketsApp.Data;

public class TicketsAppContext : DbContext
{
    public TicketsAppContext (DbContextOptions<TicketsAppContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Category { get; set; } = default!;
    public DbSet<Event> Event { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>()
            .HasData(
                new Category() { CategoryId = 1, Title = "Art Show", Events = [] },
                new Category() { CategoryId = 2, Title = "Concert", Events = [] },
                new Category() { CategoryId = 3, Title = "Esports Event", Events = [] },
                new Category() { CategoryId = 4, Title = "Sports Event", Events = [] },
                new Category() { CategoryId = 5, Title = "Festival", Events = [] });
    }
}
