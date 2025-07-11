using Xunit;
using InventoryTaskManager.Data;
using InventoryTaskManager.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

public class ProductCrudTests
{
    [Fact]
    public void CanUpdateProduct()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "UpdateTestDb")
            .Options;

        using (var context = new ApplicationDbContext(options))
        {
            var category = new Category { Id = 200, Name = "UpdateCat", Description = "Test", CreatedDate = System.DateTime.Now, UpdatedDate = System.DateTime.Now };
            context.Categories.Add(category);
            context.SaveChanges();

            var product = new Product
            {
                Name = "OldName",
                Description = "Old Desc",
                Price = 50,
                Quantity = 2,
                SKU = "UPDTSKU",
                CategoryId = category.Id,
                MinStockLevel = 1,
                CreatedDate = System.DateTime.Now,
                UpdatedDate = System.DateTime.Now
            };
            context.Products.Add(product);
            context.SaveChanges();

            // Update
            product.Name = "NewName";
            product.Price = 99;
            context.Products.Update(product);
            context.SaveChanges();

            var updated = context.Products.FirstOrDefault(p => p.SKU == "UPDTSKU");
            Assert.NotNull(updated);
            Assert.Equal("NewName", updated.Name);
            Assert.Equal(99, updated.Price);
        }
    }

    [Fact]
    public void CanDeleteProduct()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "DeleteTestDb")
            .Options;

        using (var context = new ApplicationDbContext(options))
        {
            var category = new Category { Id = 300, Name = "DeleteCat", Description = "Test", CreatedDate = System.DateTime.Now, UpdatedDate = System.DateTime.Now };
            context.Categories.Add(category);
            context.SaveChanges();

            var product = new Product
            {
                Name = "DeleteMe",
                Description = "To be deleted",
                Price = 10,
                Quantity = 1,
                SKU = "DELTSKU",
                CategoryId = category.Id,
                MinStockLevel = 1,
                CreatedDate = System.DateTime.Now,
                UpdatedDate = System.DateTime.Now
            };
            context.Products.Add(product);
            context.SaveChanges();

            context.Products.Remove(product);
            context.SaveChanges();

            Assert.False(context.Products.Any(p => p.SKU == "DELTSKU"));
        }
    }


    [Fact]
    public void ProductModelValidationFailsIfNameOrSKUIsMissing()
    {
        var product = new Product
        {
            // Name is missing
            Description = "No Name",
            Price = 10,
            Quantity = 1,
            SKU = null, // SKU is missing
            CategoryId = 1,
            MinStockLevel = 1,
            CreatedDate = System.DateTime.Now,
            UpdatedDate = System.DateTime.Now
        };
        var context = new System.ComponentModel.DataAnnotations.ValidationContext(product, null, null);
        var results = new System.Collections.Generic.List<System.ComponentModel.DataAnnotations.ValidationResult>();
        bool isValid = System.ComponentModel.DataAnnotations.Validator.TryValidateObject(product, context, results, true);
        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains("Name") || r.MemberNames.Contains("SKU"));
    }
}
