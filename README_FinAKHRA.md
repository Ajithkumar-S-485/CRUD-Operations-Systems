# FinAKHRA Financial Management System

## Overview
FinAKHRA is a comprehensive financial management system built for membership-based organizations. It provides complete functionality for managing members, fees, events, medicaid requests, and full accounting operations with journal entries, ledger accounts, and financial reporting.

## Features

### 🏢 Member Management
- **Complete Member Registry**: Add, edit, view, and manage organization members
- **Member Search**: Quick search functionality by name, email, or mobile
- **Active/Inactive Status**: Track member status and manage membership lifecycle
- **Member Details**: Complete member profiles with contact information and join dates

### 💰 Fee Management
- **Monthly Fee Tracking**: Record and track member fees by month and year
- **Payment Processing**: Mark fees as paid with receipt generation
- **Pending Payments**: Quick view of all outstanding fees
- **Fee Reports**: Generate comprehensive fee collection reports
- **Duplicate Prevention**: Automatic validation to prevent duplicate fee entries

### 🎉 Event Management
- **Event Planning**: Create and manage organizational events with dates and descriptions
- **Budget Management**: Set budgets and track actual expenses for events
- **Event Calendar**: View events by month/year with calendar interface
- **Budget Analysis**: Identify events that are over or under budget
- **Financial Tracking**: Complete expense tracking for all events

### 🏥 Medicaid Request Management
- **Request Processing**: Submit and manage medicaid assistance requests
- **Approval Workflow**: Approve, reject, or keep requests pending
- **Amount Tracking**: Track requested vs approved amounts
- **Member Integration**: Link requests directly to member profiles
- **Status Reporting**: Generate reports on medicaid fund utilization

### 📊 Accounting System
- **Double-Entry Bookkeeping**: Full journal entry system with debits and credits
- **Chart of Accounts**: Comprehensive ledger account management
- **Cash Book**: Track all cash transactions with running balances
- **Voucher System**: Receipt and payment voucher management
- **Trial Balance**: Generate trial balance reports
- **Income Statement**: Profit and loss reporting
- **Financial Reports**: Complete financial reporting suite

### 📈 Dashboard & Analytics
- **Real-time Statistics**: Live dashboard with key performance indicators
- **Financial Summary**: Monthly income, expenses, and net income tracking
- **Member Statistics**: Active member counts and growth tracking
- **Quick Actions**: Fast access to common operations
- **Visual Reports**: Charts and graphs for financial data

## Technology Stack

### Backend
- **Framework**: ASP.NET Core 5.0 MVC
- **Database**: SQL Server with Entity Framework Core
- **Architecture**: 3-Layer Architecture (DAL, BLL, PL)
- **Patterns**: Repository Pattern, Unit of Work, Dependency Injection

### Frontend
- **UI Framework**: Bootstrap 4/5
- **Icons**: Font Awesome
- **JavaScript**: jQuery for dynamic interactions
- **Responsive Design**: Mobile-friendly interface

### Database
- **Database**: SQL Server (FinAKHRA)
- **ORM**: Entity Framework Core with Code-First approach
- **Relationships**: Full foreign key relationships and constraints
- **Seeding**: Initial data seeding for accounts and permissions

## Database Schema

### Core Tables
1. **Members** - Member information and contact details
2. **Users** - System users with role-based access
3. **LedgerAccounts** - Chart of accounts for financial tracking
4. **JournalEntries** - Double-entry bookkeeping journal entries
5. **Transactions** - Individual debit/credit transactions
6. **Fees** - Member fee tracking and payments
7. **Events** - Event management with budget tracking
8. **MedicaidRequests** - Medicaid assistance requests
9. **CashBook** - Cash transaction tracking
10. **Vouchers** - Receipt and payment vouchers
11. **Permissions** - Role-based access control

### Key Relationships
- Members have multiple Fees, MedicaidRequests, and Vouchers
- JournalEntries contain multiple Transactions
- Transactions link to LedgerAccounts
- Users create JournalEntries
- All financial data maintains referential integrity

## Installation & Setup

### Prerequisites
- .NET 5.0 SDK or later
- SQL Server (Express or full version)
- Visual Studio 2019/2022 or VS Code

### Database Setup
1. **Create Database**: The system will create the `FinAKHRA` database automatically
2. **Connection String**: Update the connection string in `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "FinAkhraConnection": "Server=.;Database=FinAKHRA;Trusted_Connection=true;"
     }
   }
   ```

### Running the Application
1. **Clone the repository**
2. **Navigate to the project directory**
3. **Restore packages**: `dotnet restore`
4. **Build the solution**: `dotnet build`
5. **Run migrations**: `dotnet ef database update` (if using migrations)
6. **Start the application**: `dotnet run --project Demo.PL`

### Initial Setup
1. **Access the application** at `https://localhost:5001`
2. **Login** with default admin credentials (if seeded)
3. **Configure** initial chart of accounts
4. **Add members** and start using the system

## Key Controllers & Features

### MemberController
- Member CRUD operations
- Search and filter functionality
- Member status management
- Fee history viewing

### FeeController
- Fee recording and payment processing
- Monthly and yearly fee reports
- Pending payment management
- Receipt generation

### EventController
- Event creation and management
- Budget vs actual expense tracking
- Event calendar view
- Financial analysis

### MedicaidController
- Request submission and processing
- Approval workflow management
- Amount tracking and reporting
- Member-specific request history

### AccountingController
- Journal entry creation
- Ledger account management
- Cash book maintenance
- Financial report generation

### DashboardController
- Real-time statistics
- Financial summaries
- Quick access to key functions
- System overview

## Security Features
- **Role-based Access Control**: Users have different permission levels
- **Data Validation**: Comprehensive server-side validation
- **SQL Injection Prevention**: Entity Framework parameterized queries
- **CSRF Protection**: Anti-forgery tokens on forms

## Reporting Capabilities
- **Fee Collection Reports**: Monthly, yearly, and member-specific
- **Event Budget Reports**: Budget vs actual analysis
- **Medicaid Reports**: Request status and fund utilization
- **Financial Reports**: Trial balance, income statement, cash flow
- **Member Reports**: Active members, join dates, payment history

## API Endpoints
The system provides RESTful endpoints for:
- Member management
- Fee processing
- Event management
- Medicaid requests
- Financial transactions
- Reporting data

## Contributing
1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## License
This project is open source and available under the MIT License.

## Support
For support and questions, please create an issue in the repository or contact the development team.

## Version History
- **v1.0.0** - Initial release with core functionality
- **v1.1.0** - Added dashboard and enhanced reporting
- **v1.2.0** - Improved UI/UX and additional validation

---

**FinAKHRA** - Making financial management simple and efficient for membership organizations.