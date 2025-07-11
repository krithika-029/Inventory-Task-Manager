using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryTaskManager.Data;
using InventoryTaskManager.Models;

namespace InventoryTaskManager.Controllers
{
    /// <summary>
    /// Home controller for the main dashboard and navigation
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor that injects ApplicationDbContext
        /// </summary>
        /// <param name="context">Database context</param>
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: Home/Index
        /// Display the main dashboard with key metrics and summaries
        /// </summary>
        /// <returns>Dashboard view with metrics</returns>
        public async Task<IActionResult> Index()
        {
            var dashboard = new DashboardViewModel();

            // Get all data needed for dashboard
            var products = await _context.Products.Include(p => p.Category).ToListAsync();
            var categories = await _context.Categories.ToListAsync();
            var tasks = await _context.TaskItems.ToListAsync();

            // Calculate product metrics
            dashboard.TotalProducts = products.Count;
            dashboard.TotalCategories = categories.Count;
            dashboard.LowStockProducts = products.Count(p => p.IsLowStock);
            dashboard.LowStockItems = products.Where(p => p.IsLowStock).OrderBy(p => p.Quantity).Take(5).ToList();
            dashboard.TotalInventoryValue = products.Sum(p => p.StockValue);
            dashboard.AverageProductPrice = products.Any() ? products.Average(p => p.Price) : 0;

            // Calculate task metrics
            dashboard.TotalTasks = tasks.Count;
            dashboard.PendingTasks = tasks.Count(t => t.Status == InventoryTaskManager.Models.TaskStatus.Pending);
            dashboard.InProgressTasks = tasks.Count(t => t.Status == InventoryTaskManager.Models.TaskStatus.InProgress);
            dashboard.CompletedTasks = tasks.Count(t => t.Status == InventoryTaskManager.Models.TaskStatus.Completed);
            dashboard.OverdueTasks = tasks.Count(t => t.IsOverdue);
            dashboard.UpcomingTasks = tasks.Count(t => t.IsDueSoon && !t.IsOverdue);

            // Calculate task completion rate
            dashboard.TaskCompletionRate = dashboard.TotalTasks > 0 ? 
                (double)dashboard.CompletedTasks / dashboard.TotalTasks * 100 : 0;

            // Get urgent tasks (overdue or due soon with high priority)
            dashboard.UrgentTasks = tasks
                .Where(t => (t.IsOverdue || t.IsDueSoon) && t.Status != InventoryTaskManager.Models.TaskStatus.Completed)
                .OrderBy(t => t.DueDate)
                .Take(5)
                .ToList();

            // Get critical tasks (high priority and due soon)
            dashboard.CriticalTasks = tasks
                .Where(t => t.Priority == InventoryTaskManager.Models.TaskPriority.Critical && t.Status != InventoryTaskManager.Models.TaskStatus.Completed)
                .OrderBy(t => t.DueDate)
                .Take(5)
                .ToList();

            // Get recent products (last 5 added)
            dashboard.RecentProducts = products
                .OrderByDescending(p => p.CreatedDate)
                .Take(5)
                .ToList();

            // Get recent tasks (last 5 added)
            dashboard.RecentTasks = tasks
                .OrderByDescending(t => t.CreatedDate)
                .Take(5)
                .ToList();

            // Get products needing attention (low stock)
            dashboard.ProductsNeedingAttention = products
                .Where(p => p.IsLowStock)
                .OrderBy(p => p.Quantity)
                .Take(5)
                .ToList();

            // Find most and least stocked categories
            var categoryStats = categories
                .Select(c => new { Category = c, ProductCount = c.Products.Count })
                .OrderByDescending(x => x.ProductCount)
                .ToList();

            dashboard.MostStockedCategory = categoryStats.FirstOrDefault()?.Category?.Name;
            dashboard.LeastStockedCategory = categoryStats.LastOrDefault()?.Category?.Name;

            return View(dashboard);
        }

        /// <summary>
        /// GET: Home/Privacy
        /// Privacy policy page
        /// </summary>
        /// <returns>Privacy view</returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Error page
        /// </summary>
        /// <returns>Error view</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
