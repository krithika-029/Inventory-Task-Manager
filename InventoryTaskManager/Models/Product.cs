using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryTaskManager.Models
{
    /// <summary>
    /// Represents a product in the inventory system
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Primary key for the product
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the product
        /// </summary>
        [Required(ErrorMessage = "Product name is required")]
        [StringLength(200, ErrorMessage = "Product name cannot exceed 200 characters")]
        [Display(Name = "Product Name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Description of the product
        /// </summary>
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string? Description { get; set; }

        /// <summary>
        /// Current quantity in stock
        /// </summary>
        [Required(ErrorMessage = "Quantity is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a positive number")]
        public int Quantity { get; set; }

        /// <summary>
        /// Price per unit
        /// </summary>
        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Unit Price")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal Price { get; set; }

        /// <summary>
        /// Foreign key to Category
        /// </summary>
        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        /// <summary>
        /// Navigation property to Category
        /// </summary>
        public virtual Category Category { get; set; } = null!;

        /// <summary>
        /// SKU (Stock Keeping Unit) - unique identifier for the product
        /// </summary>
        [StringLength(50, ErrorMessage = "SKU cannot exceed 50 characters")]
        public string? SKU { get; set; }

        /// <summary>
        /// Minimum stock level before low stock alert
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Minimum stock level must be a positive number")]
        [Display(Name = "Minimum Stock Level")]
        public int MinStockLevel { get; set; } = 5;

        /// <summary>
        /// Indicates if the product is currently in low stock
        /// </summary>
        [Display(Name = "Low Stock")]
        public bool IsLowStock => Quantity < MinStockLevel;

        /// <summary>
        /// Date when the product was created
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Date when the product was last updated
        /// </summary>
        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Total value of current stock
        /// </summary>
        [Display(Name = "Stock Value")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal StockValue => Quantity * Price;
    }
}
