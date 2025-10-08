# Library Management API

A .NET 8 Web API for managing books, members, and borrowing records with JWT authentication.

## Features

- Add / List Books
- Add / Get Members
- Borrow / Return Books
- Top 5 Borrowed Books
- Overdue Books
- JWT Authentication & Authorization
- Swagger API Documentation
- Error handling with proper HTTP status codes

## Tech Stack

- .NET 8
- Entity Framework Core (Database First)
- SQL Server
- JWT Authentication
- Swagger

## NuGet Packages Used

- `Microsoft.EntityFrameworkCore.SqlServer` → EF Core provider for SQL Server
- `Microsoft.EntityFrameworkCore.Tools` → EF Core scaffolding and migrations
- `Microsoft.AspNetCore.Authentication.JwtBearer` → JWT authentication
- `Swashbuckle.AspNetCore` → Swagger API documentation

## Database Setup

- SQL Server database: `LibraryDb`
- Tables: `Books`, `Members`, `BorrowRecords`
- Stored procedure: `GetOverdueBooks` (to fetch overdue books)
- Sample data added for testing

## Project Structure / Flow

```
LibraryAPI/
│
├─ Controllers/
│   ├─ BooksController.cs       → Add/List/Get Books
│   ├─ MembersController.cs     → Add/Get Members
│   ├─ BorrowController.cs      → Borrow/Return Books
│   ├─ ReportsController.cs     → Top Borrowed / Overdue Books
│   └─ AuthController.cs        → JWT Authentication (login)
│
├─ Models/                     → EF Core models generated via Database First
│   ├─ Book.cs
│   ├─ Member.cs
│   ├─ BorrowRecord.cs
│   └─ LibraryContext.cs
│
├─ DTOs/                       → Data Transfer Objects
│   ├─ BookCreateDto.cs
│   ├─ MemberCreateDto.cs
│   └─ BorrowDto.cs
│
├─ Program.cs                  → Main project setup
│                               - DB Context registration
│                               - JWT Authentication & Authorization
│                               - Swagger configuration
│                               - Middleware: HTTPS, Auth, Controllers
│
└─ appsettings.json            → Connection strings and configuration
```

### Flow:

1. User logs in via `/api/Auth/login` → receives JWT token.
2. User sends JWT in **Authorization header** (Bearer token) for protected endpoints.
3. Users can **add books**, **add members**, **borrow and return books**.
4. Reports endpoints return **top borrowed books** and **overdue books**.
5. Swagger UI allows testing and provides **Authorize button** for JWT.
6. All controller methods have **try/catch error handling** for robust responses.

## Setup Instructions

1. Clone the repository:
   ```
   git clone <your-repo-url>
   ```
2. Update the connection string in `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "LibraryDb": "Server=YOUR_SERVER;Database=LibraryDb;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```
3. Build the project in Visual Studio 2022.
4. Run the project (Swagger UI will open automatically).
5. Use `/api/Auth/login` to get JWT token.
6. Click **Authorize** in Swagger and paste the token to access protected endpoints.
7. Test all endpoints: Books, Members, Borrow/Return, Reports.

## Notes

- Database created using **SQL Server** and EF Core **Database First** approach.
- **JWT** secures API endpoints. Only authorized users can add books, members, or borrow/return.
- All controller methods have **try/catch error handling**.
- Swagger provides **interactive API documentation**.

