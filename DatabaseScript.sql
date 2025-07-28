-- Employee Management System Database Script
-- This script creates the database schema for the ASP.NET Core Web API project

USE master;
GO

-- Drop database if it exists
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'EmployeeManagementDB')
BEGIN
    ALTER DATABASE EmployeeManagementDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE EmployeeManagementDB;
END
GO

-- Create the database
CREATE DATABASE EmployeeManagementDB;
GO

USE EmployeeManagementDB;
GO

-- Create Departments table
CREATE TABLE Departments (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Code NVARCHAR(10) NOT NULL UNIQUE,
    Name NVARCHAR(100) NOT NULL,
    DateOfCreation DATETIME2 DEFAULT GETDATE(),
    IsDeleted BIT DEFAULT 0,
    CreationDate DATETIME2 DEFAULT GETDATE()
);

-- Create Employees table
CREATE TABLE Employees (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Age INT,
    Address NVARCHAR(500),
    Salary DECIMAL(18,2) NOT NULL,
    IsActive BIT DEFAULT 1,
    Email NVARCHAR(100),
    PhoneNumber NVARCHAR(20),
    HiringDate DATETIME2 NOT NULL,
    IsDeleted BIT DEFAULT 0,
    CreationDate DATETIME2 DEFAULT GETDATE(),
    ImageName NVARCHAR(255),
    DepartmentId INT,
    FOREIGN KEY (DepartmentId) REFERENCES Departments(Id)
);

-- Create AspNetUsers table for Identity
CREATE TABLE AspNetUsers (
    Id NVARCHAR(450) PRIMARY KEY,
    UserName NVARCHAR(256),
    NormalizedUserName NVARCHAR(256),
    Email NVARCHAR(256),
    NormalizedEmail NVARCHAR(256),
    EmailConfirmed BIT NOT NULL,
    PasswordHash NVARCHAR(MAX),
    SecurityStamp NVARCHAR(MAX),
    ConcurrencyStamp NVARCHAR(MAX),
    PhoneNumber NVARCHAR(MAX),
    PhoneNumberConfirmed BIT NOT NULL,
    TwoFactorEnabled BIT NOT NULL,
    LockoutEnd DATETIMEOFFSET,
    LockoutEnabled BIT NOT NULL,
    AccessFailedCount INT NOT NULL
);

-- Create AspNetRoles table
CREATE TABLE AspNetRoles (
    Id NVARCHAR(450) PRIMARY KEY,
    Name NVARCHAR(256),
    NormalizedName NVARCHAR(256),
    ConcurrencyStamp NVARCHAR(MAX)
);

-- Create AspNetUserRoles table
CREATE TABLE AspNetUserRoles (
    UserId NVARCHAR(450) NOT NULL,
    RoleId NVARCHAR(450) NOT NULL,
    PRIMARY KEY (UserId, RoleId),
    FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE,
    FOREIGN KEY (RoleId) REFERENCES AspNetRoles(Id) ON DELETE CASCADE
);

-- Create AspNetUserClaims table
CREATE TABLE AspNetUserClaims (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId NVARCHAR(450) NOT NULL,
    ClaimType NVARCHAR(MAX),
    ClaimValue NVARCHAR(MAX),
    FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE
);

-- Create AspNetUserLogins table
CREATE TABLE AspNetUserLogins (
    LoginProvider NVARCHAR(450) NOT NULL,
    ProviderKey NVARCHAR(450) NOT NULL,
    ProviderDisplayName NVARCHAR(MAX),
    UserId NVARCHAR(450) NOT NULL,
    PRIMARY KEY (LoginProvider, ProviderKey),
    FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE
);

-- Create AspNetUserTokens table
CREATE TABLE AspNetUserTokens (
    UserId NVARCHAR(450) NOT NULL,
    LoginProvider NVARCHAR(450) NOT NULL,
    Name NVARCHAR(450) NOT NULL,
    Value NVARCHAR(MAX),
    PRIMARY KEY (UserId, LoginProvider, Name),
    FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE
);

-- Create AspNetRoleClaims table
CREATE TABLE AspNetRoleClaims (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    RoleId NVARCHAR(450) NOT NULL,
    ClaimType NVARCHAR(MAX),
    ClaimValue NVARCHAR(MAX),
    FOREIGN KEY (RoleId) REFERENCES AspNetRoles(Id) ON DELETE CASCADE
);

-- Insert sample data for Departments
INSERT INTO Departments (Code, Name, DateOfCreation) VALUES
('IT', 'Information Technology', '2023-01-15'),
('HR', 'Human Resources', '2023-01-10'),
('FIN', 'Finance', '2023-01-20'),
('MKT', 'Marketing', '2023-01-25'),
('OPS', 'Operations', '2023-02-01');

-- Insert sample data for Employees
INSERT INTO Employees (Name, Age, Address, Salary, IsActive, Email, PhoneNumber, HiringDate, DepartmentId) VALUES
('John Doe', 30, '123 Main St, City', 75000.00, 1, 'john.doe@company.com', '555-0101', '2023-01-15', 1),
('Jane Smith', 28, '456 Oak Ave, Town', 65000.00, 1, 'jane.smith@company.com', '555-0102', '2023-01-20', 2),
('Mike Johnson', 35, '789 Pine Rd, Village', 80000.00, 1, 'mike.johnson@company.com', '555-0103', '2023-02-01', 1),
('Sarah Wilson', 32, '321 Elm St, Borough', 70000.00, 1, 'sarah.wilson@company.com', '555-0104', '2023-02-15', 3),
('David Brown', 29, '654 Maple Dr, County', 60000.00, 1, 'david.brown@company.com', '555-0105', '2023-03-01', 4);

-- Create indexes for better performance
CREATE INDEX IX_Employees_DepartmentId ON Employees(DepartmentId);
CREATE INDEX IX_Employees_Email ON Employees(Email);
CREATE INDEX IX_Departments_Code ON Departments(Code);

-- Create a view for Employee details with Department information
CREATE VIEW vw_EmployeeDetails AS
SELECT 
    e.Id,
    e.Name,
    e.Age,
    e.Address,
    e.Salary,
    e.IsActive,
    e.Email,
    e.PhoneNumber,
    e.HiringDate,
    e.CreationDate,
    e.ImageName,
    d.Id AS DepartmentId,
    d.Code AS DepartmentCode,
    d.Name AS DepartmentName
FROM Employees e
LEFT JOIN Departments d ON e.DepartmentId = d.Id
WHERE e.IsDeleted = 0 AND d.IsDeleted = 0;

-- Create stored procedure for getting employees by department
CREATE PROCEDURE sp_GetEmployeesByDepartment
    @DepartmentId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        e.Id,
        e.Name,
        e.Age,
        e.Address,
        e.Salary,
        e.IsActive,
        e.Email,
        e.PhoneNumber,
        e.HiringDate,
        e.CreationDate,
        e.ImageName,
        d.Id AS DepartmentId,
        d.Code AS DepartmentCode,
        d.Name AS DepartmentName
    FROM Employees e
    LEFT JOIN Departments d ON e.DepartmentId = d.Id
    WHERE e.IsDeleted = 0 
        AND (@DepartmentId IS NULL OR e.DepartmentId = @DepartmentId)
    ORDER BY e.Name;
END

-- Create stored procedure for employee statistics
CREATE PROCEDURE sp_GetEmployeeStatistics
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        COUNT(*) AS TotalEmployees,
        COUNT(CASE WHEN IsActive = 1 THEN 1 END) AS ActiveEmployees,
        AVG(Salary) AS AverageSalary,
        MAX(Salary) AS MaxSalary,
        MIN(Salary) AS MinSalary,
        AVG(Age) AS AverageAge
    FROM Employees
    WHERE IsDeleted = 0;
END

PRINT 'Database EmployeeManagementDB created successfully with sample data!';
GO