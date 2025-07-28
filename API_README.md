# Demo Web API

This is a RESTful Web API built with ASP.NET Core 5.0 using the Repository Pattern and Entity Framework Core.

## Features

- **Repository Pattern**: Clean separation of concerns with repository interfaces and implementations
- **Unit of Work Pattern**: Transaction management and data consistency
- **AutoMapper**: Object-to-object mapping for DTOs
- **Entity Framework Core**: ORM for data access
- **Swagger/OpenAPI**: API documentation and testing interface
- **CORS Support**: Cross-origin resource sharing enabled
- **Validation**: Data validation using Data Annotations

## API Endpoints

### Health Check
- `GET /api/home` - API status check
- `GET /api/home/health` - Detailed health information

### Departments

#### Get All Departments
```
GET /api/department
```

#### Get Department by ID
```
GET /api/department/{id}
```

#### Create Department
```
POST /api/department
Content-Type: application/json

{
  "code": "IT",
  "name": "Information Technology"
}
```

#### Update Department
```
PUT /api/department/{id}
Content-Type: application/json

{
  "code": "IT",
  "name": "Information Technology"
}
```

#### Delete Department
```
DELETE /api/department/{id}
```

### Employees

#### Get All Employees
```
GET /api/employee
```

#### Get Employees with Search
```
GET /api/employee?searchInp=john
```

#### Get Employee by ID
```
GET /api/employee/{id}
```

#### Create Employee
```
POST /api/employee
Content-Type: application/json

{
  "name": "John Doe",
  "age": 25,
  "address": "123-Main-Street-City",
  "salary": 50000.00,
  "isActive": true,
  "email": "john.doe@example.com",
  "phoneNumber": "+1234567890",
  "hiringDate": "2023-01-15T00:00:00",
  "departmentId": 1
}
```

#### Update Employee
```
PUT /api/employee/{id}
Content-Type: application/json

{
  "name": "John Doe",
  "age": 26,
  "address": "123-Main-Street-City",
  "salary": 55000.00,
  "isActive": true,
  "email": "john.doe@example.com",
  "phoneNumber": "+1234567890",
  "hiringDate": "2023-01-15T00:00:00",
  "departmentId": 1
}
```

#### Delete Employee
```
DELETE /api/employee/{id}
```

#### Get Employees by Department
```
GET /api/employee/department/{departmentId}
```

## Data Models

### Department
- `Id` (int) - Primary key
- `Code` (string) - Department code (required)
- `Name` (string) - Department name (required)
- `DateOfCreation` (DateTime) - Creation timestamp

### Employee
- `Id` (int) - Primary key
- `Name` (string) - Employee name (required, 5-50 characters)
- `Age` (int?) - Employee age (22-30 range)
- `Address` (string) - Address in format "123-Street-City-Country"
- `Salary` (decimal) - Employee salary
- `IsActive` (bool) - Active status
- `Email` (string) - Email address
- `PhoneNumber` (string) - Phone number
- `HiringDate` (DateTime) - Date of hiring
- `ImageName` (string) - Profile image filename
- `DepartmentId` (int?) - Foreign key to Department
- `Department` (Department) - Navigation property

## Validation Rules

### Department
- Code and Name are required fields

### Employee
- Name: Required, 5-50 characters
- Age: Range 22-30
- Address: Must follow pattern "123-Street-City-Country"
- Email: Must be valid email format
- Phone Number: Must be valid phone format

## Running the Application

1. **Prerequisites**
   - .NET 5.0 SDK
   - SQL Server (LocalDB or full SQL Server)

2. **Database Setup**
   - Update connection string in `appsettings.json`
   - Run Entity Framework migrations:
     ```bash
     dotnet ef database update
     ```

3. **Run the Application**
   ```bash
   dotnet run
   ```

4. **Access Swagger UI**
   - Navigate to `https://localhost:5001/swagger` in development mode

## Project Structure

```
Demo.PL/                 # Presentation Layer (Web API)
├── Controllers/         # API Controllers
├── ViewModels/         # DTOs and ViewModels
├── Helpers/            # AutoMapper profiles
└── Extensions/         # Service extensions

Demo.BLL/               # Business Logic Layer
├── Interfaces/         # Repository interfaces
└── Repositories/       # Repository implementations

Demo.DAL/               # Data Access Layer
├── Data/              # Entity Framework context
├── Models/            # Entity models
└── Migrations/        # Database migrations
```

## Repository Pattern Implementation

The application uses a clean repository pattern with:

- **IGenericRepository<T>**: Generic CRUD operations
- **IDepartmentRepository**: Department-specific operations
- **IEmployeeRepository**: Employee-specific operations
- **IUnitOfWork**: Transaction management
- **UnitOfWork**: Implementation of unit of work pattern

## Error Handling

The API returns appropriate HTTP status codes:
- `200 OK`: Successful GET requests
- `201 Created`: Successful POST requests
- `204 No Content`: Successful PUT/DELETE requests
- `400 Bad Request`: Validation errors or invalid data
- `404 Not Found`: Resource not found
- `500 Internal Server Error`: Server errors

## CORS Configuration

CORS is configured to allow all origins, methods, and headers for development. In production, you should configure more restrictive CORS policies.