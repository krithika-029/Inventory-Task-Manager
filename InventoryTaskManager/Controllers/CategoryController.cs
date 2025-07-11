using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryTaskManager.Data;
using InventoryTaskManager.Models;

namespace InventoryTaskManager.Controllers
{
    /// <summary>
    /// Controller for managing Category entities in the inventory system
    /// </summary>
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor that injects ApplicationDbContext
        /// </summary>
        /// <param name="context">Database context</param>
        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: Category
        /// Display list of all categories with product count
        /// </summary>
        /// <returns>View with list of categories</returns>
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories
                .Include(c => c.Products)
                .OrderBy(c => c.Name)
                .ToListAsync();

            return View(categories);
        }

        /// <summary>
        /// GET: Category/Details/5
        /// Display category details with associated products
        /// </summary>
        /// <param name="id">Category ID</param>
        /// <returns>View with category details or NotFound</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        /// <summary>
        /// GET: Category/Create
        /// Display form to create a new category
        /// </summary>
        /// <returns>View with create form</returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// POST: Category/Create
        /// Process form submission to create a new category
        /// </summary>
        /// <param name="category">Category model from form</param>
        /// <returns>Redirect to Index on success, or return view with validation errors</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description")] Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Set timestamps
                    category.CreatedDate = DateTime.Now;
                    category.UpdatedDate = DateTime.Now;

                    _context.Add(category);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Category created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    // Handle database errors (e.g., duplicate category name)
                    if (ex.InnerException?.Message.Contains("UNIQUE constraint failed") == true)
                    {
                        ModelState.AddModelError("Name", "A category with this name already exists.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "An error occurred while saving the category. Please try again.");
                    }
                }
            }

            return View(category);
        }

        /// <summary>
        /// GET: Category/Edit/5
        /// Display form to edit an existing category
        /// </summary>
        /// <param name="id">Category ID</param>
        /// <returns>View with edit form or NotFound</returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        /// <summary>
        /// POST: Category/Edit/5
        /// Process form submission to update an existing category
        /// </summary>
        /// <param name="id">Category ID</param>
        /// <param name="category">Category model from form</param>
        /// <returns>Redirect to Index on success, or return view with validation errors</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,CreatedDate")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Set update timestamp
                    category.UpdatedDate = DateTime.Now;

                    _context.Update(category);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Category updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (DbUpdateException ex)
                {
                    // Handle database errors (e.g., duplicate category name)
                    if (ex.InnerException?.Message.Contains("UNIQUE constraint failed") == true)
                    {
                        ModelState.AddModelError("Name", "A category with this name already exists.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "An error occurred while updating the category. Please try again.");
                    }
                }
            }

            return View(category);
        }

        /// <summary>
        /// GET: Category/Delete/5
        /// Display confirmation page for deleting a category
        /// </summary>
        /// <param name="id">Category ID</param>
        /// <returns>View with delete confirmation or NotFound</returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        /// <summary>
        /// POST: Category/Delete/5
        /// Process confirmation to delete a category
        /// </summary>
        /// <param name="id">Category ID</param>
        /// <returns>Redirect to Index</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category != null)
            {
                // Check if category has products
                if (category.Products.Any())
                {
                    TempData["ErrorMessage"] = "Cannot delete category because it contains products. Please remove all products from this category first.";
                    return RedirectToAction(nameof(Delete), new { id = id });
                }

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Category deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Category not found or already deleted.";
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// GET: Category/Products/5
        /// Display all products in a specific category
        /// </summary>
        /// <param name="id">Category ID</param>
        /// <returns>View with products in category</returns>
        public async Task<IActionResult> Products(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            ViewBag.CategoryName = category.Name;
            ViewBag.CategoryId = category.Id;
            return View(category.Products.OrderBy(p => p.Name));
        }

        /// <summary>
        /// Check if a category exists
        /// </summary>
        /// <param name="id">Category ID</param>
        /// <returns>True if category exists</returns>
        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
