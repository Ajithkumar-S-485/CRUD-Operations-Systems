# Employee Management System - ASP.NET Core Web API

This project is an ASP.NET Core Web API built with a repository pattern for managing employees and departments.

## 🚀 Features

- **Repository Pattern**: Clean separation of concerns with repository and unit of work patterns
- **Entity Framework Core**: ORM for database operations
- **AutoMapper**: Object-to-object mapping
- **Identity**: User authentication and authorization
- **CORS Support**: Cross-origin resource sharing enabled
- **Soft Delete**: Records are marked as deleted instead of being physically removed
- **Comprehensive API**: Full CRUD operations for employees and departments
- **Statistics Endpoints**: Built-in analytics and reporting

## 📋 Prerequisites

- .NET 6.0 or later
- SQL Server (LocalDB, Express, or Full)
- Visual Studio 2022 or VS Code

## 🛠️ Setup Instructions

### 1. Database Setup

1. **Run the SQL Script**:
   ```sql
   -- Execute the DatabaseScript.sql file in SQL Server Management Studio
   -- or run it via command line:
   sqlcmd -S . -i DatabaseScript.sql
   ```

2. **Update Connection String** (if needed):
   - Open `Demo.PL/appsettings.json`
   - Modify the `DefaultConnection` string to match your SQL Server instance

### 2. Build and Run

1. **Restore NuGet Packages**:
   ```bash
   dotnet restore
   ```

2. **Build the Solution**:
   ```bash
   dotnet build
   ```

3. **Run the Application**:
   ```bash
   cd Demo.PL
   dotnet run
   ```

4. **Access the API**:
   - API Base URL: `https://localhost:7001` or `http://localhost:5001`
   - API Documentation: `GET /api`
   - Health Check: `GET /api/health`

## 📚 API Endpoints

### Employees API (`/api/EmployeesApi`)

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/EmployeesApi` | Get all employees (with optional search) |
| GET | `/api/EmployeesApi/{id}` | Get employee by ID |
| GET | `/api/EmployeesApi/department/{departmentId}` | Get employees by department |
| POST | `/api/EmployeesApi` | Create new employee |
| PUT | `/api/EmployeesApi/{id}` | Update employee |
| DELETE | `/api/EmployeesApi/{id}` | Soft delete employee |
| GET | `/api/EmployeesApi/statistics` | Get employee statistics |

### Departments API (`/api/DepartmentsApi`)

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/DepartmentsApi` | Get all departments |
| GET | `/api/DepartmentsApi/{id}` | Get department by ID |
| GET | `/api/DepartmentsApi/{id}/employees` | Get employees in department |
| POST | `/api/DepartmentsApi` | Create new department |
| PUT | `/api/DepartmentsApi/{id}` | Update department |
| DELETE | `/api/DepartmentsApi/{id}` | Soft delete department |
| GET | `/api/DepartmentsApi/statistics` | Get department statistics |

## 🔧 Sample Requests

### Create Employee
```http
POST /api/EmployeesApi
Content-Type: application/json

{
  "name": "John Doe",
  "age": 30,
  "address": "123 Main St, City",
  "salary": 75000.00,
  "isActive": true,
  "email": "john.doe@company.com",
  "phoneNumber": "555-0101",
  "hiringDate": "2023-01-15T00:00:00",
  "departmentId": 1
}
```

### Create Department
```http
POST /api/DepartmentsApi
Content-Type: application/json

{
  "code": "IT",
  "name": "Information Technology"
}
```

### Get Employees with Search
```http
GET /api/EmployeesApi?search=john
```

### Get Department Statistics
```http
GET /api/DepartmentsApi/statistics
```

## 🏗️ Project Structure

```
Demo.PL/                    # Presentation Layer (Web API)
├── Controllers/
│   ├── Api/               # API Controllers
│   │   ├── EmployeesApiController.cs
│   │   ├── DepartmentsApiController.cs
│   │   └── ApiInfoController.cs
│   └── ...                # MVC Controllers (legacy)
├── ViewModels/            # Data Transfer Objects
├── Helpers/              # Utility classes
└── Extensions/           # Service extensions

Demo.BLL/                  # Business Logic Layer
├── Interfaces/           # Repository interfaces
├── Repositories/         # Repository implementations
└── UnitOfWork/          # Unit of Work pattern

Demo.DAL/                  # Data Access Layer
├── Data/
│   ├── Context/         # DbContext
│   ├── Configurations/  # Entity configurations
│   └── Migrations/      # EF Core migrations
└── Models/              # Entity models
```

## 🔍 Key Features Explained

### Repository Pattern
- **IGenericRepository<T>**: Generic interface for basic CRUD operations
- **IEmployeeRepository**: Specific interface for employee operations
- **IDepartmentRepository**: Specific interface for department operations
- **IUnitOfWork**: Manages transactions and coordinates repositories

### Soft Delete
- Records are marked with `IsDeleted = true` instead of being physically removed
- Queries automatically filter out deleted records
- Maintains data integrity and audit trail

### AutoMapper
- Maps between Entity models and ViewModels
- Reduces boilerplate code
- Ensures clean separation between layers

### Error Handling
- Comprehensive try-catch blocks in API controllers
- Consistent error response format
- Proper HTTP status codes

## 🧪 Testing the API

### Using curl
```bash
# Get all employees
curl -X GET "https://localhost:7001/api/EmployeesApi"

# Create an employee
curl -X POST "https://localhost:7001/api/EmployeesApi" \
  -H "Content-Type: application/json" \
  -d '{"name":"Jane Smith","age":28,"salary":65000,"isActive":true,"email":"jane@company.com","hiringDate":"2023-01-20T00:00:00","departmentId":1}'

# Get employee statistics
curl -X GET "https://localhost:7001/api/EmployeesApi/statistics"
```

### Using Postman
1. Import the API endpoints
2. Set base URL to `https://localhost:7001`
3. Test each endpoint with appropriate HTTP methods and data

## 🔐 Security Considerations

- **CORS**: Configured to allow all origins (customize for production)
- **Identity**: ASP.NET Core Identity for authentication
- **Validation**: Model validation on all inputs
- **Soft Delete**: Prevents accidental data loss

## 🚀 Deployment

### Development
```bash
dotnet run --environment Development
```

### Production
```bash
dotnet publish -c Release
dotnet run --environment Production
```

## 📝 Notes

- The API supports both JSON and XML responses
- CORS is enabled for cross-origin requests
- All endpoints return consistent JSON responses
- Error responses include meaningful messages
- The database script includes sample data for testing

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## 📄 License

This project is licensed under the MIT License.