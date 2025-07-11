using InventoryTaskManager.Models;

namespace InventoryTaskManager.Models
{
    /// <summary>
    /// View model for the dashboard displaying key metrics and summaries
    /// </summary>
    public class DashboardViewModel
    {
        /// <summary>
        /// Total count of products in inventory
        /// </summary>
        public int TotalProducts { get; set; }

        /// <summary>
        /// Total count of categories
        /// </summary>
        public int TotalCategories { get; set; }

        /// <summary>
        /// Total count of tasks
        /// </summary>
        public int TotalTasks { get; set; }

        /// <summary>
        /// Count of pending tasks
        /// </summary>
        public int PendingTasks { get; set; }

        /// <summary>
        /// Count of in-progress tasks
        /// </summary>
        public int InProgressTasks { get; set; }

        /// <summary>
        /// Count of completed tasks
        /// </summary>
        public int CompletedTasks { get; set; }

        /// <summary>
        /// Count of overdue tasks
        /// </summary>
        public int OverdueTasks { get; set; }

        /// <summary>
        /// Count of tasks due within the next 3 days
        /// </summary>
        public int UpcomingTasks { get; set; }

        /// <summary>
        /// Count of products with low stock
        /// </summary>
        public int LowStockProducts { get; set; }

        /// <summary>
        /// List of products with low stock levels
        /// </summary>
        public List<Product> LowStockItems { get; set; } = new List<Product>();

        /// <summary>
        /// List of urgent tasks (due within 3 days or overdue)
        /// </summary>
        public List<TaskItem> UrgentTasks { get; set; } = new List<TaskItem>();

        /// <summary>
        /// List of recent products added
        /// </summary>
        public List<Product> RecentProducts { get; set; } = new List<Product>();

        /// <summary>
        /// List of recent tasks added
        /// </summary>
        public List<TaskItem> RecentTasks { get; set; } = new List<TaskItem>();

        /// <summary>
        /// Total value of inventory
        /// </summary>
        public decimal TotalInventoryValue { get; set; }

        /// <summary>
        /// Average product price
        /// </summary>
        public decimal AverageProductPrice { get; set; }

        /// <summary>
        /// Task completion rate as percentage
        /// </summary>
        public double TaskCompletionRate { get; set; }

        /// <summary>
        /// Most stocked category
        /// </summary>
        public string? MostStockedCategory { get; set; }

        /// <summary>
        /// Least stocked category
        /// </summary>
        public string? LeastStockedCategory { get; set; }

        /// <summary>
        /// Critical tasks (high priority and due soon)
        /// </summary>
        public List<TaskItem> CriticalTasks { get; set; } = new List<TaskItem>();

        /// <summary>
        /// Products that need immediate attention
        /// </summary>
        public List<Product> ProductsNeedingAttention { get; set; } = new List<Product>();
    }
}
