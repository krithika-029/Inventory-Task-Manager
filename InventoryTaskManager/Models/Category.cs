using System.ComponentModel.DataAnnotations;

namespace InventoryTaskManager.Models
{
    /// <summary>
    /// Represents a product category in the inventory system
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Primary key for the category
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the category
        /// </summary>
        [Required(ErrorMessage = "Category name is required")]
        [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters")]
        [Display(Name = "Category Name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Description of the category
        /// </summary>
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        /// <summary>
        /// Navigation property for products in this category
        /// </summary>
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();

        /// <summary>
        /// Date when the category was created
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Date when the category was last updated
        /// </summary>
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
    }
}
