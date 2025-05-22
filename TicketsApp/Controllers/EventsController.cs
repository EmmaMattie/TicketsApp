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
using Microsoft.AspNetCore.Authorization;

namespace TicketsApp.Controllers
{
    [Authorize]
    public class EventsController : Controller
    {
        private readonly TicketsAppContext _context; // Database context for accessing events
        private readonly IWebHostEnvironment _webHostEnvironment; // To manage file storage on the server

        // Constructor to initialize context and web host environment
        public EventsController(TicketsAppContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Events - Displays the list of events
        public async Task<IActionResult> Index()
        {
            var ticketsAppContext = _context.Event.Include(e => e.Category); // Fetch events with their category
            return View(await ticketsAppContext.ToListAsync()); // Returns the events to the view
        }

        // GET: Events/Details/5 - Displays the details of a single event
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) // If no event id is provided, return not found
            {
                return NotFound();
            }

            var @event = await _context.Event
                .Include(e => e.Category) // Includes category information
                .FirstOrDefaultAsync(m => m.EventId == id); // Fetch event by id
            if (@event == null) // If event doesn't exist, return not found
            {
                return NotFound();
            }

            return View(@event); // Return the event details to the view
        }

        // GET: Events/Create - Displays the form to create a new event
        public IActionResult Create()
        {
            // Populates the category dropdown list with category data
            ViewBag.CategoryId = new SelectList(_context.Category.AsNoTracking(), nameof(Category.CategoryId), nameof(Category.Title));
            return View(); // Returns the create form view
        }

        // POST: Events/Create - Creates a new event based on the form submission
        [HttpPost]
        [ValidateAntiForgeryToken] // Protects against CSRF attacks
        public async Task<IActionResult> Create(Event @event)
        {
            if (ModelState.IsValid) // Checks if the model state is valid
            {
                // Handle the image upload for the event
                if (@event.ImageFile != null && @event.ImageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "event-images"); // Path for storing images

                    // Create the folder if it doesn't exist
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Generate a unique file name and save the image
                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(@event.ImageFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create)) // Save the file to the server
                    {
                        await @event.ImageFile.CopyToAsync(stream);
                    }

                    // Store the filename for the event
                    @event.ImageFileName = uniqueFileName;
                }

                // Set the event's creation date
                @event.CreateDate = DateTime.UtcNow;

                // Save the event to the database
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // Redirect to the event list after creating
            }

            // Re-populate the category dropdown if the form submission is invalid
            ViewBag.CategoryId = new SelectList(_context.Category.AsNoTracking(), nameof(Category.CategoryId), nameof(Category.Title), @event.CategoryId);
            return View(@event); // Return the create form with errors
        }

        // GET: Events/Edit/5 - Displays the form to edit an event
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) // If no event id is provided, return not found
            {
                return NotFound();
            }

            var @event = await _context.Event.FindAsync(id); // Find event by id
            if (@event == null) // If event doesn't exist, return not found
            {
                return NotFound();
            }

            // Populates the category dropdown list with the selected category
            ViewBag.CategoryId = new SelectList(_context.Category.AsNoTracking(), nameof(Category.CategoryId), nameof(Category.Title), @event.CategoryId);
            return View(@event); // Return the edit form with event data
        }

        // POST: Events/Edit/5 - Edits an existing event
        [HttpPost]
        [ValidateAntiForgeryToken] // Protects against CSRF attacks
        public async Task<IActionResult> Edit(int id, Event @event)
        {
            if (id != @event.EventId) // If event id doesn't match, return not found
            {
                return NotFound();
            }

            if (ModelState.IsValid) // Check if the model is valid
            {
                try
                {
                    // Get the original event from the DB
                    var existingEvent = await _context.Event.AsNoTracking().FirstOrDefaultAsync(e => e.EventId == id);

                    // Handle the image upload if a new image is uploaded
                    if (@event.ImageFile != null && @event.ImageFile.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "event-images");

                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(@event.ImageFile.FileName);
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await @event.ImageFile.CopyToAsync(stream);
                        }

                        @event.ImageFileName = uniqueFileName;
                    }
                    else
                    {
                        // If no new image is uploaded, preserve the original filename
                        @event.ImageFileName = existingEvent?.ImageFileName ?? "";
                    }

                    _context.Update(@event); // Update the event in the database
                    await _context.SaveChangesAsync(); // Save changes to the database
                }
                catch (DbUpdateConcurrencyException) // Handle concurrency issues
                {
                    if (!EventExists(@event.EventId)) // Check if event still exists
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw; // Re-throw exception if another error occurs
                    }
                }
                return RedirectToAction(nameof(Index)); // Redirect to the event list after editing
            }

            // Re-populate the category dropdown in case of validation failure
            ViewBag.CategoryId = new SelectList(_context.Category.AsNoTracking(), nameof(Category.CategoryId), nameof(Category.Title), @event.CategoryId);
            return View(@event); // Return the edit form with errors
        }

        // GET: Events/Delete/5 - Displays the confirmation form to delete an event
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) // If no event id is provided, return not found
            {
                return NotFound();
            }

            var @event = await _context.Event
                .Include(e => e.Category) // Include category info
                .FirstOrDefaultAsync(m => m.EventId == id); // Find event by id
            if (@event == null) // If event doesn't exist, return not found
            {
                return NotFound();
            }

            return View(@event); // Return the delete confirmation view
        }

        // POST: Events/Delete/5 - Deletes the event from the database
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken] // Protects against CSRF attacks
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Event.FindAsync(id); // Find event by id
            if (@event != null) // If event exists, remove it
            {
                _context.Event.Remove(@event);
            }

            await _context.SaveChangesAsync(); // Save changes to the database
            return RedirectToAction(nameof(Index)); // Redirect to event list after deletion
        }

        // Checks if an event exists by id
        private bool EventExists(int id)
        {
            return _context.Event.Any(e => e.EventId == id); // Returns true if event exists
        }
    }
}
