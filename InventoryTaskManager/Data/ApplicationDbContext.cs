using Microsoft.EntityFrameworkCore;
using InventoryTaskManager.Models;

namespace InventoryTaskManager.Data
{
    /// <summary>
    /// Application database context for Entity Framework Core
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Constructor that accepts DbContextOptions
        /// </summary>
        /// <param name="options">Database context options</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Categories DbSet
        /// </summary>
        public DbSet<Category> Categories { get; set; } = null!;

        /// <summary>
        /// Products DbSet
        /// </summary>
        public DbSet<Product> Products { get; set; } = null!;

        /// <summary>
        /// Task Items DbSet
        /// </summary>
        public DbSet<TaskItem> TaskItems { get; set; } = null!;

        /// <summary>
        /// Configure the model relationships and constraints
        /// </summary>
        /// <param name="modelBuilder">The model builder</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Category entity
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.HasIndex(e => e.Name).IsUnique();
            });

            // Configure Product entity
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                entity.Property(e => e.SKU).HasMaxLength(50);
                
                // Configure relationship with Category
                entity.HasOne(e => e.Category)
                      .WithMany(c => c.Products)
                      .HasForeignKey(e => e.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Create index on SKU for uniqueness if provided
                entity.HasIndex(e => e.SKU).IsUnique();
            });

            // Configure TaskItem entity
            modelBuilder.Entity<TaskItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(2000);
                entity.Property(e => e.AssignedTo).HasMaxLength(100);
                
                // Configure enum properties
                entity.Property(e => e.Status)
                      .HasConversion<string>()
                      .IsRequired();
                      
                entity.Property(e => e.Priority)
                      .HasConversion<string>()
                      .IsRequired();
            });

            // Seed some initial data
            SeedData(modelBuilder);
        }

        /// <summary>
        /// Seed initial data for the database - Indian Retail Store
        /// 
        /// Categories:
        /// - Electronics: Laptops, mobiles, printers, and electronic accessories
        /// - Grocery: Daily essentials like milk, rice, oil, and food items
        /// - Office Supplies: Stationery, paper, and office equipment
        /// 
        /// Products with realistic Indian pricing (INR):
        /// - HP Laptop: ₹45,000 (Electronics)
        /// - Samsung Mobile: ₹25,000 (Electronics)
        /// - Canon Printer: ₹8,500 (Electronics)
        /// - Canon Ink Cartridge: ₹2,500 (Electronics)
        /// - Amul Milk 1L: ₹60 (Grocery)
        /// - Basmati Rice 5kg: ₹450 (Grocery)
        /// - Fortune Sunflower Oil 1L: ₹140 (Grocery)
        /// - Maggi Noodles: ₹15 (Grocery)
        /// - A4 Paper Bundle (500 sheets): ₹350 (Office Supplies)
        /// - Parker Pen: ₹150 (Office Supplies)
        /// 
        /// Tasks reflect typical Indian retail operations
        /// </summary>
        /// <param name="modelBuilder">The model builder</param>
        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Categories - Indian Retail Store
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Electronics", Description = "Electronic devices, computers, mobiles, and accessories", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
                new Category { Id = 2, Name = "Grocery", Description = "Daily essentials, food items, beverages, and household products", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
                new Category { Id = 3, Name = "Office Supplies", Description = "Stationery, paper, pens, and office equipment", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now }
            );

            // Seed Products - Indian Retail Store with INR pricing
            modelBuilder.Entity<Product>().HasData(
                // Electronics Category
                new Product { Id = 1, Name = "HP Pavilion Laptop", Description = "HP Pavilion 15-inch laptop with Intel Core i5, 8GB RAM, 512GB SSD", Quantity = 12, Price = 45000.00m, CategoryId = 1, SKU = "ELE001", MinStockLevel = 3, CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
                new Product { Id = 2, Name = "Samsung Galaxy A54", Description = "Samsung Galaxy A54 5G smartphone with 128GB storage", Quantity = 8, Price = 25000.00m, CategoryId = 1, SKU = "ELE002", MinStockLevel = 5, CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
                new Product { Id = 3, Name = "Canon PIXMA Printer", Description = "Canon PIXMA G3010 All-in-One wireless printer", Quantity = 5, Price = 8500.00m, CategoryId = 1, SKU = "ELE003", MinStockLevel = 2, CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
                new Product { Id = 4, Name = "Canon Ink Cartridge", Description = "Canon GI-790 PGBK Black Ink Bottle", Quantity = 2, Price = 2500.00m, CategoryId = 1, SKU = "ELE004", MinStockLevel = 5, CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
                
                // Grocery Category
                new Product { Id = 5, Name = "Amul Milk 1L", Description = "Amul Taaza Homogenised Toned Milk 1 Litre", Quantity = 25, Price = 60.00m, CategoryId = 2, SKU = "GRO001", MinStockLevel = 20, CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
                new Product { Id = 6, Name = "Basmati Rice 5kg", Description = "India Gate Basmati Rice 5kg premium quality", Quantity = 15, Price = 450.00m, CategoryId = 2, SKU = "GRO002", MinStockLevel = 8, CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
                new Product { Id = 7, Name = "Fortune Sunflower Oil 1L", Description = "Fortune Sunflower Refined Oil 1 Litre", Quantity = 20, Price = 140.00m, CategoryId = 2, SKU = "GRO003", MinStockLevel = 10, CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
                new Product { Id = 8, Name = "Maggi Noodles", Description = "Maggi 2-Minute Masala Noodles 70g", Quantity = 3, Price = 15.00m, CategoryId = 2, SKU = "GRO004", MinStockLevel = 12, CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
                
                // Office Supplies Category
                new Product { Id = 9, Name = "A4 Paper Bundle", Description = "JK Copier A4 Paper 500 sheets 70 GSM", Quantity = 30, Price = 350.00m, CategoryId = 3, SKU = "OFF001", MinStockLevel = 10, CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
                new Product { Id = 10, Name = "Parker Pen", Description = "Parker Vector Standard Ball Pen Blue", Quantity = 18, Price = 150.00m, CategoryId = 3, SKU = "OFF002", MinStockLevel = 15, CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now }
            );

            // Seed TaskItems - Indian Retail Store Operations
            modelBuilder.Entity<TaskItem>().HasData(
                new TaskItem { Id = 1, Title = "Restock Canon Ink Cartridges", Description = "Order Canon ink cartridges from authorized dealer - current stock very low", Status = InventoryTaskManager.Models.TaskStatus.Pending, Priority = InventoryTaskManager.Models.TaskPriority.Critical, DueDate = DateTime.Now.AddDays(1), AssignedTo = "Store Manager", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
                new TaskItem { Id = 2, Title = "Update Grocery Prices", Description = "Review and update grocery item prices based on latest wholesale rates", Status = InventoryTaskManager.Models.TaskStatus.InProgress, Priority = InventoryTaskManager.Models.TaskPriority.Medium, DueDate = DateTime.Now.AddDays(3), AssignedTo = "Pricing Team", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
                new TaskItem { Id = 3, Title = "Organize Electronics Section", Description = "Rearrange electronics display area for better customer experience", Status = InventoryTaskManager.Models.TaskStatus.Pending, Priority = InventoryTaskManager.Models.TaskPriority.Low, DueDate = DateTime.Now.AddDays(7), AssignedTo = "Sales Associate", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
                new TaskItem { Id = 4, Title = "Monthly Stock Audit", Description = "Complete monthly physical inventory count for all categories", Status = InventoryTaskManager.Models.TaskStatus.Pending, Priority = InventoryTaskManager.Models.TaskPriority.High, DueDate = DateTime.Now.AddDays(2), AssignedTo = "Inventory Team", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
                new TaskItem { Id = 5, Title = "Restock Maggi Noodles", Description = "Order Maggi noodles from distributor - popular item running low", Status = InventoryTaskManager.Models.TaskStatus.Pending, Priority = InventoryTaskManager.Models.TaskPriority.High, DueDate = DateTime.Now.AddDays(1), AssignedTo = "Purchase Executive", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now }
            );
        }

        /// <summary>
        /// Override SaveChanges to automatically update timestamps
        /// </summary>
        /// <returns>Number of entities updated</returns>
        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        /// <summary>
        /// Override SaveChangesAsync to automatically update timestamps
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Number of entities updated</returns>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Update timestamps for entities being modified
        /// </summary>
        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is Category || e.Entity is Product || e.Entity is TaskItem);

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Modified)
                {
                    switch (entry.Entity)
                    {
                        case Category category:
                            category.UpdatedDate = DateTime.Now;
                            break;
                        case Product product:
                            product.UpdatedDate = DateTime.Now;
                            break;
                        case TaskItem taskItem:
                            taskItem.UpdatedDate = DateTime.Now;
                            if (taskItem.Status == InventoryTaskManager.Models.TaskStatus.Completed && taskItem.CompletedDate == null)
                            {
                                taskItem.CompletedDate = DateTime.Now;
                            }
                            break;
                    }
                }
            }
        }
    }
}
