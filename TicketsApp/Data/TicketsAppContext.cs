using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketsApp.Models;

namespace TicketsApp.Data
{
    public class TicketsAppContext : DbContext
    {
        public TicketsAppContext (DbContextOptions<TicketsAppContext> options)
            : base(options)
        {
        }

        public DbSet<TicketsApp.Models.Category> Category { get; set; } = default!;
        public DbSet<TicketsApp.Models.Event> Event { get; set; } = default!;
    }
}
