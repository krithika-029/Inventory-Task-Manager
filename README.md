# Inventory & Task Management System

A full-stack ASP.NET Core MVC application for managing inventory and tasks in Indian retail stores. Built with modern web technologies and designed for small to medium-sized businesses.

## 🚀 Features

### Inventory Management
- **Product Management**: Add, edit, delete, and view products with detailed information
- **Category Organization**: Organize products into categories (Electronics, Grocery, Office Supplies, etc.)
- **Stock Tracking**: Monitor stock levels with low stock alerts
- **SKU Management**: Unique product identification with SKU codes
- **Price Management**: Product pricing with INR (₹) currency formatting
- **Stock Valuation**: Calculate total inventory value and stock worth

### Task Management
- **Task Creation**: Create and assign tasks to team members
- **Priority Levels**: Set task priorities (Low, Medium, High, Critical)
- **Status Tracking**: Track task status (Pending, In Progress, Completed)
- **Due Date Management**: Set and monitor task deadlines
- **Assignment**: Assign tasks to specific team members

### Dashboard & Analytics
- **Real-time Dashboard**: Overview of inventory and task metrics
- **Summary Cards**: Quick view of total products, categories, low stock items, and tasks
- **Inventory Analytics**: Total inventory value, average product price
- **Task Overview**: Task status distribution and priority analysis
- **Low Stock Alerts**: Identify products that need restocking

### Search & Filtering
- **Advanced Search**: Search products by name, description, or SKU
- **Category Filtering**: Filter products by category
- **Sorting Options**: Sort by various criteria (name, price, stock, etc.)
- **Task Filtering**: Filter tasks by status, priority, or due date

## 🛠️ Technologies Used

- **Backend**: ASP.NET Core MVC 8.0
- **Database**: SQLite with Entity Framework Core
- **Frontend**: Bootstrap 5, HTML5, CSS3, JavaScript
- **Icons**: Font Awesome
- **Validation**: jQuery Validation, DataAnnotations
- **Testing**: xUnit, Entity Framework InMemory

## 📦 Installation & Setup

### Prerequisites
- .NET 8.0 SDK
- Visual Studio 2022 or Visual Studio Code
- Git

### Setup Instructions

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/InventoryTaskManager.git
   cd InventoryTaskManager
   ```

2. **Restore NuGet packages**
   ```bash
   dotnet restore
   ```

3. **Update the database**
   ```bash
   dotnet ef database update
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

5. **Open in browser**
   - Navigate to `http://localhost:5000` or `https://localhost:5001`

## 🗄️ Database Schema

The application uses Entity Framework Core with SQLite and includes the following entities:

### Product
- Id (Primary Key)
- Name (Required)
- Description
- Quantity
- Price (Decimal)
- CategoryId (Foreign Key)
- SKU (Unique)
- MinStockLevel
- CreatedDate
- UpdatedDate

### Category
- Id (Primary Key)
- Name (Required, Unique)
- Description
- CreatedDate
- UpdatedDate

### TaskItem
- Id (Primary Key)
- Title (Required)
- Description
- Status (Enum: Pending, InProgress, Completed)
- Priority (Enum: Low, Medium, High, Critical)
- DueDate
- AssignedTo
- CreatedDate
- UpdatedDate
- CompletedDate

## 📊 Sample Data

The application comes with realistic Indian retail store data including:

### Categories
- **Electronics**: Laptops, mobiles, printers, accessories
- **Grocery**: Daily essentials, food items, beverages
- **Office Supplies**: Stationery, paper, office equipment

### Products
- HP Pavilion Laptop (₹45,000)
- Samsung Galaxy A54 (₹25,000)
- Amul Milk 1L (₹60)
- Basmati Rice 5kg (₹450)
- A4 Paper Bundle (₹350)

### Tasks
- Restock inventory items
- Update pricing
- Audit stock levels
- Organize store sections

## 🧪 Testing

The project includes comprehensive unit and integration tests:

```bash
# Run all tests
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"
```

### Test Coverage
- **Unit Tests**: Model validation, business logic
- **Integration Tests**: Database operations, CRUD functionality
- **Controller Tests**: HTTP responses, view rendering

## 🎨 UI/UX Features

- **Responsive Design**: Mobile-first approach with Bootstrap 5
- **Modern Interface**: Clean, professional design
- **Interactive Elements**: Modal dialogs, alerts, tooltips
- **Data Visualization**: Cards, badges, progress indicators
- **Search & Filter**: Real-time search with filtering options
- **Pagination**: Efficient data display for large datasets

## 🔧 Configuration

### Database Configuration
The application uses SQLite by default. Connection string is configured in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=inventory.db"
  }
}
```

### Seeding Data
Initial data is seeded through Entity Framework migrations. Modify `ApplicationDbContext.cs` to customize seed data.

## 🚀 Deployment

### Local Development
```bash
dotnet run --environment Development
```

### Production Build
```bash
dotnet publish -c Release -o ./publish
```

### Docker Deployment
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY ./publish .
ENTRYPOINT ["dotnet", "InventoryTaskManager.dll"]
```

## 📝 API Endpoints

### Products
- `GET /Product` - List all products
- `GET /Product/Details/{id}` - Product details
- `POST /Product/Create` - Create new product
- `PUT /Product/Edit/{id}` - Update product
- `DELETE /Product/Delete/{id}` - Delete product

### Categories
- `GET /Category` - List all categories
- `POST /Category/Create` - Create new category
- `PUT /Category/Edit/{id}` - Update category
- `DELETE /Category/Delete/{id}` - Delete category

### Tasks
- `GET /Task` - List all tasks
- `POST /Task/Create` - Create new task
- `PUT /Task/Edit/{id}` - Update task
- `DELETE /Task/Delete/{id}` - Delete task

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- Built with ASP.NET Core and Entity Framework
- UI components from Bootstrap 5
- Icons from Font Awesome
- Indian retail context and pricing


