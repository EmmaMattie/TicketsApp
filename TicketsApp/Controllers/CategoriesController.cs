using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TicketsApp.Data;
using TicketsApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace TicketsApp.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
        // Context for accessing the database
        private readonly TicketsAppContext _context;

        // Constructor that initializes the context
        public CategoriesController(TicketsAppContext context)
        {
            _context = context;
        }

        // GET: Categories - Displays the list of categories
        public async Task<IActionResult> Index()
        {
            return View(await _context.Category.ToListAsync()); // Fetches categories from the database
        }

        // GET: Categories/Details/5 - Displays details of a category
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) // If no id is provided, return not found
            {
                return NotFound();
            }

            // Fetch category based on provided id
            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null) // If category doesn't exist, return not found
            {
                return NotFound();
            }

            return View(category); // Returns the view with category details
        }

        // GET: Categories/Create - Displays the form to create a new category
        public IActionResult Create()
        {
            return View(); // Displays the create form
        }

        // POST: Categories/Create - Creates a new category from the form submission
        [HttpPost]
        [ValidateAntiForgeryToken] // Protects against CSRF attacks
        public async Task<IActionResult> Create([Bind("CategoryId,Title")] Category category)
        {
            if (ModelState.IsValid) // Check if the model is valid
            {
                _context.Add(category); // Add the new category to the context
                await _context.SaveChangesAsync(); // Save changes to the database
                return RedirectToAction(nameof(Index)); // Redirect to Index view after successful creation
            }
            return View(category); // Return the create view if model is invalid
        }

        // GET: Categories/Edit/5 - Displays the form to edit a category
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) // If no id is provided, return not found
            {
                return NotFound();
            }

            var category = await _context.Category.FindAsync(id); // Find category by id
            if (category == null) // If category doesn't exist, return not found
            {
                return NotFound();
            }
            return View(category); // Display the edit form with category data
        }

        // POST: Categories/Edit/5 - Edits an existing category
        [HttpPost]
        [ValidateAntiForgeryToken] // Protects against CSRF attacks
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,Title")] Category category)
        {
            if (id != category.CategoryId) // If category id doesn't match, return not found
            {
                return NotFound();
            }

            if (ModelState.IsValid) // Check if the model is valid
            {
                try
                {
                    _context.Update(category); // Update the category in the context
                    await _context.SaveChangesAsync(); // Save changes to the database
                }
                catch (DbUpdateConcurrencyException) // Handle concurrency issues
                {
                    if (!CategoryExists(category.CategoryId)) // If category doesn't exist, return not found
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw; // If another error occurs, rethrow the exception
                    }
                }
                return RedirectToAction(nameof(Index)); // Redirect to Index view after successful edit
            }
            return View(category); // Return the edit form if model is invalid
        }

        // GET: Categories/Delete/5 - Displays the confirmation form to delete a category
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) // If no id is provided, return not found
            {
                return NotFound();
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.CategoryId == id); // Find category by id
            if (category == null) // If category doesn't exist, return not found
            {
                return NotFound();
            }

            return View(category); // Display the delete confirmation view
        }

        // POST: Categories/Delete/5 - Deletes the category from the database
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken] // Protects against CSRF attacks
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Category.FindAsync(id); // Find the category by id
            if (category != null) // If category exists, remove it
            {
                _context.Category.Remove(category); // Remove category from context
            }

            await _context.SaveChangesAsync(); // Save changes to the database
            return RedirectToAction(nameof(Index)); // Redirect to Index view after deletion
        }

        // Checks if a category exists by id
        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.CategoryId == id); // Returns true if category exists
        }
    }
}
