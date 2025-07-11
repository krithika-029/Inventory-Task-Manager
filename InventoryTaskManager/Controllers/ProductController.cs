using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InventoryTaskManager.Data;
using InventoryTaskManager.Models;

namespace InventoryTaskManager.Controllers
{
    /// <summary>
    /// Controller for managing Product entities in the inventory system
    /// </summary>
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor that injects ApplicationDbContext
        /// </summary>
        /// <param name="context">Database context</param>
        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: Product
        /// Display list of all products with their category names
        /// </summary>
        /// <returns>View with list of products</returns>
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .OrderBy(p => p.Name)
                .ToListAsync();

            return View(products);
        }

        /// <summary>
        /// GET: Product/Details/5
        /// Display full product information by ID
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns>View with product details or NotFound</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        /// <summary>
        /// GET: Product/Create
        /// Display form to create a new product
        /// </summary>
        /// <returns>View with create form</returns>
        public async Task<IActionResult> Create()
        {
            await PopulateCategoriesDropDownList();
            return View();
        }

        /// <summary>
        /// POST: Product/Create
        /// Process form submission to create a new product
        /// </summary>
        /// <param name="product">Product model from form</param>
        /// <returns>Redirect to Index on success, or return view with validation errors</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Quantity,Price,CategoryId,SKU,MinStockLevel")] Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Set timestamps
                    product.CreatedDate = DateTime.Now;
                    product.UpdatedDate = DateTime.Now;

                    _context.Add(product);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Product created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    // Handle database errors (e.g., duplicate SKU)
                    if (ex.InnerException?.Message.Contains("UNIQUE constraint failed") == true)
                    {
                        ModelState.AddModelError("SKU", "A product with this SKU already exists.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "An error occurred while saving the product. Please try again.");
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            await PopulateCategoriesDropDownList(product.CategoryId);
            return View(product);
        }

        /// <summary>
        /// GET: Product/Edit/5
        /// Display form to edit an existing product
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns>View with edit form or NotFound</returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            await PopulateCategoriesDropDownList(product.CategoryId);
            return View(product);
        }

        /// <summary>
        /// POST: Product/Edit/5
        /// Process form submission to update an existing product
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <param name="product">Product model from form</param>
        /// <returns>Redirect to Index on success, or return view with validation errors</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Quantity,Price,CategoryId,SKU,MinStockLevel,CreatedDate")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Set update timestamp
                    product.UpdatedDate = DateTime.Now;

                    _context.Update(product);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Product updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
                    // Handle database errors (e.g., duplicate SKU)
                    if (ex.InnerException?.Message.Contains("UNIQUE constraint failed") == true)
                    {
                        ModelState.AddModelError("SKU", "A product with this SKU already exists.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "An error occurred while updating the product. Please try again.");
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            await PopulateCategoriesDropDownList(product.CategoryId);
            return View(product);
        }

        /// <summary>
        /// GET: Product/Delete/5
        /// Display confirmation page for deleting a product
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns>View with delete confirmation or NotFound</returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        /// <summary>
        /// POST: Product/Delete/5
        /// Process confirmation to delete a product
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns>Redirect to Index</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Product deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Product not found or already deleted.";
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// GET: Product/LowStock
        /// Display products with low stock levels
        /// </summary>
        /// <returns>View with low stock products</returns>
        public async Task<IActionResult> LowStock()
        {
            var lowStockProducts = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.Quantity < p.MinStockLevel)
                .OrderBy(p => p.Quantity)
                .ToListAsync();

            ViewBag.Title = "Low Stock Products";
            return View("Index", lowStockProducts);
        }

        /// <summary>
        /// GET: Product/Search
        /// Search products by name, description, or SKU
        /// </summary>
        /// <param name="searchTerm">Search term</param>
        /// <param name="categoryId">Optional category filter</param>
        /// <returns>View with search results</returns>
        public async Task<IActionResult> Search(string searchTerm, int? categoryId)
        {
            var products = _context.Products.Include(p => p.Category).AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                products = products.Where(p => 
                    p.Name.Contains(searchTerm) || 
                    p.Description!.Contains(searchTerm) || 
                    p.SKU!.Contains(searchTerm));
            }

            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == categoryId.Value);
            }

            var result = await products.OrderBy(p => p.Name).ToListAsync();

            ViewBag.SearchTerm = searchTerm;
            ViewBag.CategoryId = categoryId;
            ViewBag.Title = $"Search Results ({result.Count} found)";
            
            await PopulateCategoriesDropDownList(categoryId);
            return View("Index", result);
        }

        /// <summary>
        /// AJAX: Get product details for quick view
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns>JSON with product details</returns>
        public async Task<IActionResult> GetProductDetails(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return Json(new
            {
                id = product.Id,
                name = product.Name,
                description = product.Description,
                quantity = product.Quantity,
                price = product.Price,
                category = product.Category.Name,
                sku = product.SKU,
                minStockLevel = product.MinStockLevel,
                isLowStock = product.IsLowStock,
                stockValue = product.StockValue,
                createdDate = product.CreatedDate.ToString("yyyy-MM-dd"),
                updatedDate = product.UpdatedDate.ToString("yyyy-MM-dd")
            });
        }

        /// <summary>
        /// Check if a product exists
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns>True if product exists</returns>
        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        /// <summary>
        /// Populate the categories dropdown list for forms
        /// </summary>
        /// <param name="selectedCategory">Currently selected category ID</param>
        private async Task PopulateCategoriesDropDownList(int? selectedCategory = null)
        {
            var categories = await _context.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();

            ViewBag.CategoryId = new SelectList(categories, "Id", "Name", selectedCategory);
        }
    }
}
