using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryTaskManager.Data;
using InventoryTaskManager.Models;

namespace InventoryTaskManager.Controllers
{
    /// <summary>
    /// Controller for managing TaskItem entities in the task management system
    /// </summary>
    public class TaskController : Controller
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor that injects ApplicationDbContext
        /// </summary>
        /// <param name="context">Database context</param>
        public TaskController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: Task
        /// Display list of all tasks with filtering and sorting options
        /// </summary>
        /// <param name="status">Filter by task status</param>
        /// <param name="sortBy">Sort by field (DueDate, Priority, Status)</param>
        /// <param name="sortOrder">Sort order (asc, desc)</param>
        /// <returns>View with list of tasks</returns>
        public async Task<IActionResult> Index(InventoryTaskManager.Models.TaskStatus? status, string sortBy = "DueDate", string sortOrder = "asc")
        {
            var tasks = _context.TaskItems.AsQueryable();

            // Apply status filter
            if (status.HasValue)
            {
                tasks = tasks.Where(t => t.Status == status.Value);
            }

            // Apply sorting
            tasks = sortBy.ToLower() switch
            {
                "priority" => sortOrder == "desc" ? tasks.OrderByDescending(t => t.Priority) : tasks.OrderBy(t => t.Priority),
                "status" => sortOrder == "desc" ? tasks.OrderByDescending(t => t.Status) : tasks.OrderBy(t => t.Status),
                "title" => sortOrder == "desc" ? tasks.OrderByDescending(t => t.Title) : tasks.OrderBy(t => t.Title),
                "duedate" => sortOrder == "desc" ? tasks.OrderByDescending(t => t.DueDate) : tasks.OrderBy(t => t.DueDate),
                _ => tasks.OrderBy(t => t.DueDate)
            };

            var taskList = await tasks.ToListAsync();

            // Pass filter and sort parameters to view
            ViewBag.CurrentStatus = status;
            ViewBag.CurrentSortBy = sortBy;
            ViewBag.CurrentSortOrder = sortOrder;
            ViewBag.NextSortOrder = sortOrder == "asc" ? "desc" : "asc";

            return View(taskList);
        }

        /// <summary>
        /// GET: Task/Details/5
        /// Display task details by ID
        /// </summary>
        /// <param name="id">Task ID</param>
        /// <returns>View with task details or NotFound</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskItem = await _context.TaskItems.FirstOrDefaultAsync(m => m.Id == id);

            if (taskItem == null)
            {
                return NotFound();
            }

            return View(taskItem);
        }

        /// <summary>
        /// GET: Task/Create
        /// Display form to create a new task
        /// </summary>
        /// <returns>View with create form</returns>
        public IActionResult Create()
        {
            var task = new TaskItem
            {
                DueDate = DateTime.Now.AddDays(7), // Default due date 7 days from now
                Priority = InventoryTaskManager.Models.TaskPriority.Medium,
                Status = InventoryTaskManager.Models.TaskStatus.Pending
            };

            return View(task);
        }

        /// <summary>
        /// POST: Task/Create
        /// Process form submission to create a new task
        /// </summary>
        /// <param name="taskItem">TaskItem model from form</param>
        /// <returns>Redirect to Index on success, or return view with validation errors</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,Status,Priority,DueDate,AssignedTo")] TaskItem taskItem)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Set timestamps
                    taskItem.CreatedDate = DateTime.Now;
                    taskItem.UpdatedDate = DateTime.Now;

                    // Set completion date if status is completed
                    if (taskItem.Status == InventoryTaskManager.Models.TaskStatus.Completed)
                    {
                        taskItem.CompletedDate = DateTime.Now;
                    }

                    _context.Add(taskItem);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Task created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "An error occurred while saving the task. Please try again.");
                }
            }

            return View(taskItem);
        }

        /// <summary>
        /// GET: Task/Edit/5
        /// Display form to edit an existing task
        /// </summary>
        /// <param name="id">Task ID</param>
        /// <returns>View with edit form or NotFound</returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskItem = await _context.TaskItems.FindAsync(id);
            if (taskItem == null)
            {
                return NotFound();
            }

            return View(taskItem);
        }

        /// <summary>
        /// POST: Task/Edit/5
        /// Process form submission to update an existing task
        /// </summary>
        /// <param name="id">Task ID</param>
        /// <param name="taskItem">TaskItem model from form</param>
        /// <returns>Redirect to Index on success, or return view with validation errors</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Status,Priority,DueDate,AssignedTo,CreatedDate,CompletedDate")] TaskItem taskItem)
        {
            if (id != taskItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Set update timestamp
                    taskItem.UpdatedDate = DateTime.Now;

                    // Set completion date if status changed to completed
                    if (taskItem.Status == InventoryTaskManager.Models.TaskStatus.Completed && taskItem.CompletedDate == null)
                    {
                        taskItem.CompletedDate = DateTime.Now;
                    }
                    // Clear completion date if status changed from completed
                    else if (taskItem.Status != InventoryTaskManager.Models.TaskStatus.Completed && taskItem.CompletedDate != null)
                    {
                        taskItem.CompletedDate = null;
                    }

                    _context.Update(taskItem);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Task updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskItemExists(taskItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "An error occurred while updating the task. Please try again.");
                }
            }

            return View(taskItem);
        }

        /// <summary>
        /// GET: Task/Delete/5
        /// Display confirmation page for deleting a task
        /// </summary>
        /// <param name="id">Task ID</param>
        /// <returns>View with delete confirmation or NotFound</returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskItem = await _context.TaskItems.FirstOrDefaultAsync(m => m.Id == id);

            if (taskItem == null)
            {
                return NotFound();
            }

            return View(taskItem);
        }

        /// <summary>
        /// POST: Task/Delete/5
        /// Process confirmation to delete a task
        /// </summary>
        /// <param name="id">Task ID</param>
        /// <returns>Redirect to Index</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taskItem = await _context.TaskItems.FindAsync(id);
            if (taskItem != null)
            {
                _context.TaskItems.Remove(taskItem);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Task deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Task not found or already deleted.";
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// GET: Task/Overdue
        /// Display overdue tasks
        /// </summary>
        /// <returns>View with overdue tasks</returns>
        public async Task<IActionResult> Overdue()
        {
            var overdueTasks = await _context.TaskItems
                .Where(t => t.DueDate < DateTime.Now && t.Status != InventoryTaskManager.Models.TaskStatus.Completed)
                .OrderBy(t => t.DueDate)
                .ToListAsync();

            ViewBag.Title = "Overdue Tasks";
            return View("Index", overdueTasks);
        }

        /// <summary>
        /// GET: Task/DueSoon
        /// Display tasks due within the next 3 days
        /// </summary>
        /// <returns>View with tasks due soon</returns>
        public async Task<IActionResult> DueSoon()
        {
            var dueSoonTasks = await _context.TaskItems
                .Where(t => t.DueDate <= DateTime.Now.AddDays(3) && t.Status != InventoryTaskManager.Models.TaskStatus.Completed)
                .OrderBy(t => t.DueDate)
                .ToListAsync();

            ViewBag.Title = "Tasks Due Soon";
            return View("Index", dueSoonTasks);
        }

        /// <summary>
        /// POST: Task/MarkComplete/5
        /// Mark a task as completed
        /// </summary>
        /// <param name="id">Task ID</param>
        /// <returns>Redirect to Index</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkComplete(int id)
        {
            var taskItem = await _context.TaskItems.FindAsync(id);
            if (taskItem != null)
            {
                taskItem.Status = InventoryTaskManager.Models.TaskStatus.Completed;
                taskItem.CompletedDate = DateTime.Now;
                taskItem.UpdatedDate = DateTime.Now;

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Task marked as completed!";
            }
            else
            {
                TempData["ErrorMessage"] = "Task not found.";
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Check if a task item exists
        /// </summary>
        /// <param name="id">Task ID</param>
        /// <returns>True if task exists</returns>
        private bool TaskItemExists(int id)
        {
            return _context.TaskItems.Any(e => e.Id == id);
        }
    }
}
