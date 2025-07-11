# Inventory & Task Management System

A comprehensive full-stack ASP.NET Core MVC application for managing inventory and tasks with a modern, responsive dashboard.

## Features

### 📦 Inventory Management
- **Product Management**: Complete CRUD operations for products
- **Category Management**: Organize products into categories
- **Stock Tracking**: Monitor stock levels and get low stock alerts
- **Inventory Valuation**: Calculate total inventory value and average prices
- **Search & Filter**: Find products by name, category, or stock status

### ✅ Task Management
- **Task CRUD Operations**: Create, read, update, and delete tasks
- **Status Tracking**: Track tasks through Pending, In Progress, and Completed states
- **Priority Management**: Set task priorities (Low, Medium, High, Critical)
- **Due Date Monitoring**: Track overdue tasks and upcoming deadlines
- **Task Filtering**: Filter tasks by status, priority, and due date

### 📊 Dashboard & Analytics
- **Real-time Metrics**: View key performance indicators at a glance
- **Visual Indicators**: Color-coded alerts and progress bars
- **Quick Actions**: Fast access to common operations
- **Recent Activity**: Track recently added products and tasks
- **Alerts System**: Immediate notifications for low stock and overdue tasks

## Technology Stack

- **Framework**: ASP.NET Core MVC 8.0
- **Database**: Entity Framework Core with SQLite
- **Frontend**: Bootstrap 5, Font Awesome, jQuery
- **Validation**: Client-side and server-side validation
- **Architecture**: Model-View-Controller (MVC) pattern

## Project Structure

```
InventoryTaskManager/
├── Controllers/
│   ├── HomeController.cs       # Dashboard and main navigation
│   ├── ProductController.cs    # Product management
│   ├── CategoryController.cs   # Category management
│   └── TaskController.cs       # Task management
├── Models/
│   ├── Product.cs              # Product entity
│   ├── Category.cs             # Category entity
│   ├── TaskItem.cs             # Task entity
│   └── DashboardViewModel.cs   # Dashboard data model
├── Data/
│   └── ApplicationDbContext.cs # Database context
├── Views/
│   ├── Home/                   # Dashboard views
│   ├── Product/                # Product management views
│   ├── Category/               # Category management views
│   ├── Task/                   # Task management views
│   └── Shared/                 # Layout and shared views
├── wwwroot/
│   ├── css/                    # Custom stylesheets
│   ├── js/                     # JavaScript files
│   └── lib/                    # Client-side libraries
└── Program.cs                  # Application entry point
```

## Getting Started

### Prerequisites
- .NET 8.0 SDK
- Visual Studio 2022 or VS Code
- SQL Server or SQLite (configured for SQLite by default)

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd InventoryTaskManager
   ```

2. **Restore packages**
   ```bash
   dotnet restore
   ```

3. **Update database**
   ```bash
   dotnet ef database update
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

5. **Access the application**
   - Navigate to `https://localhost:5001` or `http://localhost:5000`

## Database Schema

### Products Table
- `Id` (Primary Key)
- `Name` (Required, Max 100 characters)
- `Description` (Optional, Max 500 characters)
- `Price` (Required, Range 0.01-999999.99)
- `Quantity` (Required, Range 0-999999)
- `LowStockThreshold` (Required, Range 1-1000)
- `CategoryId` (Foreign Key)
- `CreatedDate`, `UpdatedDate` (Auto-managed)

### Categories Table
- `Id` (Primary Key)
- `Name` (Required, Max 100 characters)
- `Description` (Optional, Max 500 characters)
- `CreatedDate`, `UpdatedDate` (Auto-managed)

### TaskItems Table
- `Id` (Primary Key)
- `Title` (Required, Max 100 characters)
- `Description` (Optional, Max 1000 characters)
- `Status` (Enum: Pending, InProgress, Completed)
- `Priority` (Enum: Low, Medium, High, Critical)
- `DueDate` (Optional)
- `CreatedDate`, `UpdatedDate` (Auto-managed)

## Key Features Detailed

### Low Stock Alerts
- Automatically calculated based on `LowStockThreshold`
- Visual indicators on dashboard and product listings
- Dedicated low stock filter and view

### Task Priority System
- Color-coded priority levels
- CSS classes for consistent styling
- Priority-based sorting and filtering

### Overdue Task Detection
- Automatic calculation of overdue status
- "Due Soon" alerts for tasks within 3 days
- Dashboard widgets for urgent tasks

### Dashboard Metrics
- Total counts for products, categories, and tasks
- Task completion rate with progress bar
- Inventory value calculations
- Recent activity tracking

## Validation Rules

### Product Validation
- Name: Required, 1-100 characters
- Price: Required, $0.01-$999,999.99
- Quantity: Required, 0-999,999 units
- Low Stock Threshold: Required, 1-1,000 units

### Category Validation
- Name: Required, 1-100 characters
- Description: Optional, max 500 characters
- Cannot delete category with associated products

### Task Validation
- Title: Required, 1-100 characters
- Description: Optional, max 1,000 characters
- Due Date: Optional, must be future date for new tasks

## API Endpoints

### Product Management
- `GET /Product` - List all products
- `GET /Product/Details/{id}` - View product details
- `GET /Product/Create` - Create product form
- `POST /Product/Create` - Submit new product
- `GET /Product/Edit/{id}` - Edit product form
- `POST /Product/Edit/{id}` - Update product
- `GET /Product/Delete/{id}` - Delete confirmation
- `POST /Product/Delete/{id}` - Delete product

### Category Management
- `GET /Category` - List all categories
- `GET /Category/Details/{id}` - View category details
- `GET /Category/Create` - Create category form
- `POST /Category/Create` - Submit new category
- `GET /Category/Edit/{id}` - Edit category form
- `POST /Category/Edit/{id}` - Update category
- `GET /Category/Delete/{id}` - Delete confirmation
- `POST /Category/Delete/{id}` - Delete category

### Task Management
- `GET /Task` - List all tasks
- `GET /Task/Details/{id}` - View task details
- `GET /Task/Create` - Create task form
- `POST /Task/Create` - Submit new task
- `GET /Task/Edit/{id}` - Edit task form
- `POST /Task/Edit/{id}` - Update task
- `GET /Task/Delete/{id}` - Delete confirmation
- `POST /Task/Delete/{id}` - Delete task
- `POST /Task/MarkComplete/{id}` - Quick complete task

## Customization

### Adding New Fields
1. Update the corresponding model class
2. Add validation attributes
3. Create and run migration
4. Update views to include new fields

### Styling Customization
- Modify `wwwroot/css/site.css` for custom styles
- Update Bootstrap classes in views
- Add custom CSS classes for specific components

### Adding New Features
1. Create new controller actions
2. Add corresponding views
3. Update navigation and routing
4. Add validation and error handling

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Support

For support and questions:
- Create an issue in the repository
- Check the documentation
- Review the code comments for implementation details

## Version History

- **v1.0.0** - Initial release with core functionality
  - Product, Category, and Task management
  - Dashboard with key metrics
  - Responsive design with Bootstrap 5
  - Complete CRUD operations for all entities
