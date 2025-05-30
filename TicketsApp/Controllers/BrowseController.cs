using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketsApp.Data;
using TicketsApp.Models;

namespace TicketsApp.Controllers
{
    public class BrowseController : Controller
    {
        private readonly TicketsAppContext _context;

        public BrowseController(TicketsAppContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var events = await _context.Event.Include(e => e.Category).ToListAsync();
            return View(events);
        }
    }
}
