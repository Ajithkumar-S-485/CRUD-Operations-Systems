-- Employee Management Database Schema
-- This script creates the database structure for the Demo Employee Management API

USE master;
GO

-- Create the database
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'DemoEmployeeManagement')
BEGIN
    CREATE DATABASE DemoEmployeeManagement;
END
GO

USE DemoEmployeeManagement;
GO

-- Create Departments table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Departments')
BEGIN
    CREATE TABLE Departments (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Code NVARCHAR(50) NOT NULL UNIQUE,
        Name NVARCHAR(100) NOT NULL,
        DateOfCreation DATETIME2 NOT NULL DEFAULT GETDATE()
    );
END
GO

-- Create Employees table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Employees')
BEGIN
    CREATE TABLE Employees (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL,
        Age INT NULL,
        Address NVARCHAR(200) NULL,
        Salary DECIMAL(18,2) NOT NULL,
        IsActive BIT NOT NULL DEFAULT 1,
        Email NVARCHAR(100) NULL,
        PhoneNumber NVARCHAR(20) NULL,
        HiringDate DATETIME2 NOT NULL DEFAULT GETDATE(),
        IsDeleted BIT NOT NULL DEFAULT 0,
        CreationDate DATETIME2 NOT NULL DEFAULT GETDATE(),
        ImageName NVARCHAR(100) NULL,
        DepartmentId INT NULL,
        CONSTRAINT FK_Employees_Departments FOREIGN KEY (DepartmentId) REFERENCES Departments(Id) ON DELETE SET NULL
    );
END
GO

-- Create indexes for better performance
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Departments_Code')
BEGIN
    CREATE UNIQUE INDEX IX_Departments_Code ON Departments(Code);
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Employees_DepartmentId')
BEGIN
    CREATE INDEX IX_Employees_DepartmentId ON Employees(DepartmentId);
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Employees_Email')
BEGIN
    CREATE INDEX IX_Employees_Email ON Employees(Email);
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Employees_IsDeleted')
BEGIN
    CREATE INDEX IX_Employees_IsDeleted ON Employees(IsDeleted);
END
GO

-- Insert sample data
INSERT INTO Departments (Code, Name, DateOfCreation) VALUES
('IT', 'Information Technology', GETDATE()),
('HR', 'Human Resources', GETDATE()),
('FIN', 'Finance', GETDATE()),
('MKT', 'Marketing', GETDATE()),
('OPS', 'Operations', GETDATE());

INSERT INTO Employees (Name, Age, Address, Salary, IsActive, Email, PhoneNumber, HiringDate, DepartmentId, CreationDate) VALUES
('John Doe', 30, '123 Main St, New York, NY', 75000.00, 1, 'john.doe@company.com', '555-0123', DATEADD(YEAR, -2, GETDATE()), 1, GETDATE()),
('Jane Smith', 28, '456 Oak Ave, Los Angeles, CA', 65000.00, 1, 'jane.smith@company.com', '555-0124', DATEADD(YEAR, -1, GETDATE()), 2, GETDATE()),
('Mike Johnson', 35, '789 Pine St, Chicago, IL', 80000.00, 1, 'mike.johnson@company.com', '555-0125', DATEADD(YEAR, -3, GETDATE()), 1, GETDATE()),
('Sarah Wilson', 32, '321 Elm St, Houston, TX', 70000.00, 1, 'sarah.wilson@company.com', '555-0126', DATEADD(YEAR, -2, GETDATE()), 3, GETDATE()),
('David Brown', 29, '654 Maple Ave, Phoenix, AZ', 68000.00, 1, 'david.brown@company.com', '555-0127', DATEADD(MONTH, -8, GETDATE()), 4, GETDATE()),
('Lisa Davis', 31, '987 Cedar Blvd, Philadelphia, PA', 72000.00, 1, 'lisa.davis@company.com', '555-0128', DATEADD(YEAR, -1, GETDATE()), 5, GETDATE()),
('Robert Miller', 38, '147 Birch Lane, San Antonio, TX', 85000.00, 1, 'robert.miller@company.com', '555-0129', DATEADD(YEAR, -4, GETDATE()), 1, GETDATE()),
('Jennifer Garcia', 27, '258 Spruce Way, San Diego, CA', 63000.00, 1, 'jennifer.garcia@company.com', '555-0130', DATEADD(MONTH, -6, GETDATE()), 2, GETDATE());

-- Create views for common queries
GO
CREATE OR ALTER VIEW vw_ActiveEmployees AS
SELECT 
    e.Id,
    e.Name,
    e.Age,
    e.Address,
    e.Salary,
    e.Email,
    e.PhoneNumber,
    e.HiringDate,
    e.CreationDate,
    d.Name AS DepartmentName,
    d.Code AS DepartmentCode
FROM Employees e
LEFT JOIN Departments d ON e.DepartmentId = d.Id
WHERE e.IsActive = 1 AND e.IsDeleted = 0;
GO

CREATE OR ALTER VIEW vw_DepartmentSummary AS
SELECT 
    d.Id,
    d.Code,
    d.Name,
    d.DateOfCreation,
    COUNT(e.Id) AS TotalEmployees,
    COUNT(CASE WHEN e.IsActive = 1 THEN 1 END) AS ActiveEmployees,
    AVG(CASE WHEN e.IsActive = 1 AND e.IsDeleted = 0 THEN e.Salary END) AS AverageSalary
FROM Departments d
LEFT JOIN Employees e ON d.Id = e.DepartmentId AND e.IsDeleted = 0
GROUP BY d.Id, d.Code, d.Name, d.DateOfCreation;
GO

PRINT 'Database schema created successfully!';
PRINT 'Tables created: Departments, Employees';
PRINT 'Sample data inserted for testing';
PRINT 'Views created: vw_ActiveEmployees, vw_DepartmentSummary';