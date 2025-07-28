# Employee Management Web API

A comprehensive ASP.NET Core Web API project built with the Repository Pattern for managing employees and departments. This API provides full CRUD operations with Entity Framework Core and SQL Server.

## Features

- **Repository Pattern Implementation**: Clean separation of concerns with generic and specific repositories
- **Unit of Work Pattern**: Manages transactions and coordinates multiple repositories
- **Entity Framework Core**: Modern ORM with code-first approach
- **SQL Server Database**: Robust relational database with proper relationships
- **DTOs (Data Transfer Objects)**: Clean API contracts separating internal models from API responses
- **Swagger Documentation**: Interactive API documentation and testing interface
- **Validation**: Comprehensive input validation with model validation attributes
- **Soft Delete**: Employees are soft-deleted (marked as deleted) instead of permanent deletion
- **CORS Support**: Cross-Origin Resource Sharing enabled for frontend integration

## API Endpoints

### Departments

- `GET /api/departments` - Get all departments with employee count
- `GET /api/departments/{id}` - Get department by ID
- `GET /api/departments/by-code/{code}` - Get department by code
- `POST /api/departments` - Create new department
- `PUT /api/departments/{id}` - Update department
- `DELETE /api/departments/{id}` - Delete department (if no employees)
- `GET /api/departments/{id}/employees` - Get all employees in a department

### Employees

- `GET /api/employees` - Get all employees with department information
- `GET /api/employees/{id}` - Get employee by ID with department
- `GET /api/employees/active` - Get only active employees
- `GET /api/employees/department/{departmentId}` - Get employees by department
- `POST /api/employees` - Create new employee
- `PUT /api/employees/{id}` - Update employee
- `DELETE /api/employees/{id}` - Soft delete employee

## Project Structure

```
Demo.WebApi/
├── Controllers/           # API Controllers
│   ├── DepartmentsController.cs
│   └── EmployeesController.cs
├── Data/
│   └── Context/
│       └── AppDbContext.cs     # Entity Framework DbContext
├── DTOs/                  # Data Transfer Objects
│   ├── DepartmentDto.cs
│   └── EmployeeDto.cs
├── Interfaces/            # Repository Interfaces
│   ├── IGenericRepository.cs
│   ├── IDepartmentRepository.cs
│   ├── IEmployeeRepository.cs
│   └── IUnitOfWork.cs
├── Models/                # Entity Models
│   ├── ModelBase.cs
│   ├── Department.cs
│   └── Employee.cs
├── Repositories/          # Repository Implementations
│   ├── GenericRepository.cs
│   ├── DepartmentRepository.cs
│   ├── EmployeeRepository.cs
│   └── UnitOfWork.cs
└── Database_Schema.sql    # Database creation script
```

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- SQL Server (LocalDB, SQL Server Express, or full SQL Server)
- Visual Studio Code or Visual Studio

### Installation

1. **Restore packages:**
   ```bash
   dotnet restore
   ```

2. **Update database connection:**
   - Open `appsettings.json`
   - Update the `DefaultConnection` string to match your SQL Server instance

3. **Create database:**
   - The application will automatically create the database on first run
   - Alternatively, run the `Database_Schema.sql` script manually

4. **Run the application:**
   ```bash
   dotnet run
   ```

5. **Access Swagger UI:**
   - Open browser and navigate to `https://localhost:5001` or `http://localhost:5000`
   - Swagger UI will be displayed at the root URL for API testing

### Database Configuration

The default connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=DemoEmployeeManagement;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

For other environments, update accordingly:
- **SQL Server Express**: `Server=.\\SQLEXPRESS;Database=DemoEmployeeManagement;Trusted_Connection=True;TrustServerCertificate=True`
- **Azure SQL**: `Server=tcp:yourserver.database.windows.net,1433;Database=DemoEmployeeManagement;User ID=yourusername;Password=yourpassword;Encrypt=True;`

## Sample Data

The database comes pre-populated with:
- 5 Departments (IT, HR, Finance, Marketing, Operations)
- 8 Sample Employees across different departments

## API Usage Examples

### Create a Department
```bash
POST /api/departments
{
  "code": "ENG",
  "name": "Engineering"
}
```

### Create an Employee
```bash
POST /api/employees
{
  "name": "John Smith",
  "age": 30,
  "address": "123 Main St",
  "salary": 75000,
  "email": "john.smith@company.com",
  "phoneNumber": "555-0123",
  "isActive": true,
  "departmentId": 1
}
```

### Update an Employee
```bash
PUT /api/employees/1
{
  "name": "John Smith Updated",
  "age": 31,
  "address": "456 New St",
  "salary": 80000,
  "email": "john.smith.updated@company.com",
  "phoneNumber": "555-0124",
  "isActive": true,
  "departmentId": 2
}
```

## Validation Rules

### Department
- Code: Required, unique, max 50 characters
- Name: Required, max 100 characters

### Employee
- Name: Required, max 100 characters
- Email: Valid email format, unique
- Salary: Required, decimal value
- Phone: Valid phone number format
- Department: Must exist if provided

## Error Handling

The API returns appropriate HTTP status codes:
- `200 OK` - Successful GET requests
- `201 Created` - Successful POST requests
- `204 No Content` - Successful PUT/DELETE requests
- `400 Bad Request` - Validation errors
- `404 Not Found` - Resource not found
- `409 Conflict` - Duplicate data (email, department code)

## Technology Stack

- **ASP.NET Core 8.0** - Web API framework
- **Entity Framework Core 8.0** - ORM
- **SQL Server** - Database
- **Swagger/OpenAPI** - API documentation
- **Repository Pattern** - Data access pattern
- **Unit of Work Pattern** - Transaction management

## Future Enhancements

- Authentication and Authorization (JWT)
- Logging with Serilog
- API versioning
- Response caching
- Rate limiting
- Health checks
- Docker containerization
- Integration tests

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

This project is for educational purposes and demonstration of ASP.NET Core Web API best practices with Repository Pattern.