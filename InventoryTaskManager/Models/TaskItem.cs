using System.ComponentModel.DataAnnotations;

namespace InventoryTaskManager.Models
{
    /// <summary>
    /// Represents the status of a task item
    /// </summary>
    public enum TaskStatus
    {
        Pending,
        [Display(Name = "In Progress")]
        InProgress,
        Completed
    }

    /// <summary>
    /// Represents the priority level of a task
    /// </summary>
    public enum TaskPriority
    {
        Low,
        Medium,
        High,
        Critical
    }

    /// <summary>
    /// Represents a task item in the task management system
    /// </summary>
    public class TaskItem
    {
        /// <summary>
        /// Primary key for the task item
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Title of the task
        /// </summary>
        [Required(ErrorMessage = "Task title is required")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        [Display(Name = "Task Title")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Detailed description of the task
        /// </summary>
        [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
        [Display(Name = "Task Description")]
        public string? Description { get; set; }

        /// <summary>
        /// Current status of the task
        /// </summary>
        [Required(ErrorMessage = "Status is required")]
        [Display(Name = "Task Status")]
        public TaskStatus Status { get; set; } = TaskStatus.Pending;

        /// <summary>
        /// Priority level of the task
        /// </summary>
        [Required(ErrorMessage = "Priority is required")]
        [Display(Name = "Task Priority")]
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;

        /// <summary>
        /// Due date for the task
        /// </summary>
        [Required(ErrorMessage = "Due date is required")]
        [Display(Name = "Due Date")]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        /// <summary>
        /// Date when the task was created
        /// </summary>
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Date when the task was last updated
        /// </summary>
        [Display(Name = "Updated Date")]
        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Date when the task was completed (if applicable)
        /// </summary>
        [Display(Name = "Completed Date")]
        public DateTime? CompletedDate { get; set; }

        /// <summary>
        /// Person or department assigned to the task
        /// </summary>
        [StringLength(100, ErrorMessage = "Assigned to cannot exceed 100 characters")]
        [Display(Name = "Assigned To")]
        public string? AssignedTo { get; set; }

        /// <summary>
        /// Indicates if the task is overdue
        /// </summary>
        [Display(Name = "Overdue")]
        public bool IsOverdue => DueDate < DateTime.Now && Status != TaskStatus.Completed;

        /// <summary>
        /// Indicates if the task is due within the next 3 days
        /// </summary>
        [Display(Name = "Due Soon")]
        public bool IsDueSoon => DueDate <= DateTime.Now.AddDays(3) && Status != TaskStatus.Completed;

        /// <summary>
        /// Number of days until due date (negative if overdue)
        /// </summary>
        [Display(Name = "Days Until Due")]
        public int DaysUntilDue => (int)(DueDate - DateTime.Now).TotalDays;

        /// <summary>
        /// CSS class for priority color coding
        /// </summary>
        public string PriorityClass => Priority switch
        {
            TaskPriority.Low => "badge-success",
            TaskPriority.Medium => "badge-info",
            TaskPriority.High => "badge-warning",
            TaskPriority.Critical => "badge-danger",
            _ => "badge-secondary"
        };

        /// <summary>
        /// CSS class for status color coding
        /// </summary>
        public string StatusClass => Status switch
        {
            TaskStatus.Pending => "badge-warning",
            TaskStatus.InProgress => "badge-info",
            TaskStatus.Completed => "badge-success",
            _ => "badge-secondary"
        };
    }
}
