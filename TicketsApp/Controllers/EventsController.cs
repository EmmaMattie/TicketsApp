using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using TicketsApp.Data;
using TicketsApp.Models;

namespace TicketsApp.Controllers
{
    public class EventsController : Controller
    {
        private readonly TicketsAppContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EventsController(TicketsAppContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            var ticketsAppContext = _context.Event.Include(e => e.Category);
            return View(await ticketsAppContext.ToListAsync());
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Event
                .Include(e => e.Category)
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_context.Category.AsNoTracking(), nameof(Category.CategoryId), nameof(Category.Title));
            return View();
        }

        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event @event)
        {
            if (ModelState.IsValid)
            {
                // Handle the image upload
                if (@event.ImageFile != null && @event.ImageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "event-images");

                    // Create folder if it doesn't exist
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Generate a unique file name
                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(@event.ImageFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Save the file to the server
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await @event.ImageFile.CopyToAsync(stream);
                    }

                    // Set the filename for the event
                    @event.ImageFilePath = uniqueFileName;
                }

                // Set creation date
                @event.CreateDate = DateTime.UtcNow;

                // Save the event to the database
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Re-populate the Category dropdown in case of validation failure
            ViewBag.CategoryId = new SelectList(_context.Category.AsNoTracking(), nameof(Category.CategoryId), nameof(Category.Title), @event.CategoryId);
            return View(@event);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Event.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            ViewBag.CategoryId = new SelectList(_context.Category.AsNoTracking(), nameof(Category.CategoryId), nameof(Category.Title), @event.CategoryId);
            return View(@event);
        }

        // POST: Events/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Event @event)
        {
            if (id != @event.EventId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Handle the image upload if a new image is uploaded
                    if (@event.ImageFile != null && @event.ImageFile.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "event-images");

                        // Create folder if it doesn't exist
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        // Generate a unique file name
                        var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(@event.ImageFile.FileName);
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        // Save the file to the server
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await @event.ImageFile.CopyToAsync(stream);
                        }

                        // Set the filename for the event
                        @event.ImageFilePath = uniqueFileName;
                    }

                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.EventId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.CategoryId = new SelectList(_context.Category.AsNoTracking(), nameof(Category.CategoryId), nameof(Category.Title), @event.CategoryId);
            return View(@event);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Event
                .Include(e => e.Category)
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Event.FindAsync(id);
            if (@event != null)
            {
                _context.Event.Remove(@event);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Event.Any(e => e.EventId == id);
        }
    }
}
