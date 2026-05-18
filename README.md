# TaskFlow - Enterprise Task Management System

## 🎯 About The Project

TaskFlow is a production-ready task management API developed using **Clean Architecture** and **SOLID** principles.

### 🏗️ Architecture Layers

```
TaskFlow/
├── TaskFlow.Domain          → Entities, Enums (No dependencies)
├── TaskFlow.Application     → Use Cases, DTOs, Interfaces
├── TaskFlow.Infrastructure  → EF Core, Repositories, External Services
└── TaskFlow.API             → Controllers, Middleware, Filters
```

## ✨ Features

### 🔐 Security

- ✅ JWT Token-based Authentication
- ✅ Role-based Authorization (Admin, Manager, Employee)
- ✅ Resource-based Authorization
- ✅ Password Hashing (BCrypt)

### ⚡ Performance

- ✅ AsNoTracking() for read-only queries
- ✅ Eager Loading to prevent the N+1 problem
- ✅ Pagination support
- ✅ Proper database indexing

### 🎨 Best Practices

- ✅ Clean Architecture
- ✅ SOLID Principles
- ✅ Repository Pattern
- ✅ Dependency Injection
- ✅ FluentValidation
- ✅ Global Exception Handling
- ✅ Structured Logging (Serilog)
- ✅ Health Checks
- ✅ Swagger/OpenAPI Documentation

## 📦 Technologies

- **.NET 8**
- **Entity Framework Core 8**
- **SQL Server**
- **JWT Authentication**
- **Serilog**
- **FluentValidation**
- **Swagger**
- **Docker**

## 🚀 Setup and Run

### Run with Docker (Recommended)

```bash
# Clone the project
git clone <repo-url>
cd TaskFlow

# Start with Docker Compose
docker-compose up -d

# API: http://localhost:5000
# Swagger: http://localhost:5000/swagger
```

### Run Manually

```bash
# Update SQL Server connection in appsettings.json

# Run migration
cd src/TaskFlow.API
dotnet ef database update

# Start the application
dotnet run

# API: https://localhost:5001
# Swagger: https://localhost:5001/swagger
```

## 📚 API Endpoints

### Authentication

```
POST   /api/auth/register    → New user registration
POST   /api/auth/login       → Sign in (get JWT token)
```

### Tasks

```
GET    /api/tasks                    → All tasks (Pagination + Filters)
GET    /api/tasks/{id}               → Task details
GET    /api/tasks/assigned-to-me     → Tasks assigned to me
POST   /api/tasks                    → Create new task (Manager+)
PUT    /api/tasks/{id}               → Update task
PATCH  /api/tasks/{id}/status        → Update task status
DELETE /api/tasks/{id}               → Delete task (Manager+)
```

### Projects

```
GET    /api/projects            → All projects
GET    /api/projects/{id}       → Project details
GET    /api/projects/{id}/tasks → Project tasks
POST   /api/projects            → Create new project (Manager+)
PUT    /api/projects/{id}       → Update project (Manager+)
```

### Users (Admin Only)

```
GET    /api/users              → All users
GET    /api/users/{id}         → User details
PUT    /api/users/{id}/role    → Change user role
```

### Health

```
GET    /health                 → Application health status
```

## 🔑 User Roles

### Employee

- Can only view tasks assigned to themselves
- Can update status of their own tasks

### Manager

- Can view all tasks in their department
- Can create and assign tasks
- Can create projects

### Admin

- Full system access
- Can change user roles
- Can perform all CRUD operations

## 🧪 Test Scenario

```bash
# 1. User registration
POST /api/auth/register
{
  "email": "john@company.com",
  "password": "password123",
  "firstName": "John",
  "lastName": "Doe"
}

# 2. Sign in
POST /api/auth/login
{
  "email": "john@company.com",
  "password": "password123"
}
# → Get JWT token

# 3. Add token to "Authorize" in Swagger
Bearer <your-jwt-token>

# 4. Create project (Admin/Manager required)
POST /api/projects
{
  "name": "Web Application",
  "description": "Customer portal project"
}

# 5. Create task
POST /api/tasks
{
  "title": "Setup database",
  "description": "Create tables and relationships",
  "projectId": 1,
  "priority": 2,
  "assignedUserIds": [1]
}

# 6. My tasks
GET /api/tasks/assigned-to-me

# 7. Update task status
PATCH /api/tasks/1/status
{
  "status": 1  // InProgress
}
```

## 🏛️ Clean Architecture Layers

### Domain Layer

- **Entities:** User, Project, TaskItem, TaskAssignment, Comment
- **Enums:** TaskStatus, TaskPriority, UserRole
- **No Dependencies:** Completely independent, Pure C#

### Application Layer

- **Interfaces:** Repository and service interfaces
- **DTOs:** API request/response models
- **Services:** Business logic
- **Validators:** FluentValidation rules
- **Depends on:** Domain

### Infrastructure Layer

- **DbContext:** Entity Framework configuration
- **Repositories:** Data access implementation
- **External Services:** TokenService
- **Depends on:** Application, Domain

### API Layer (Presentation)

- **Controllers:** HTTP endpoints
- **Middleware:** Exception handling
- **Program.cs:** DI configuration
- **Depends on:** Application, Infrastructure

## 🛡️ Security Features

### JWT Token Structure

```json
{
	"nameid": "1",
	"email": "john@company.com",
	"role": "Employee",
	"exp": 1234567890
}
```

### Authorization Examples

```csharp
[Authorize]                           // Authenticated user required
[Authorize(Roles = "Admin")]         // Admin role required
[Authorize(Roles = "Admin,Manager")] // Admin OR Manager
```

## 📊 Database Schema

```
Users (1) ──────┐
                ├──→ TaskAssignments (N) ←──┤
Tasks (1) ──────┘                           Projects (1)

Tasks (1) ──→ Comments (N)
```

### Indexes

- `User.Email` (Unique)
- `Task.Status + Task.Priority` (Composite)
- `Task.Deadline`
- `TaskAssignment.TaskId + UserId` (Unique)

## 📝 Logging

Structured logging with Serilog:

```csharp
// Daily files in logs folder
logs/taskflow-20250518.txt

// Console output
[10:30:45 INF] Creating task "Setup database"
[10:30:46 ERR] Task not found: Id=999
```

## 🏥 Health Checks

```bash
GET /health

Response:
{
  "status": "Healthy",
  "entries": {
    "ApplicationDbContext": {
      "status": "Healthy"
    }
  }
}
```

## 📚 Topics Covered

This project covers **ALL training modules**:

### Module 1: Modern C#

✅ Records  
✅ Primary Constructors  
✅ Nullable Reference Types  
✅ LINQ  
✅ Dependency Injection  
✅ Options Pattern

### Module 2: Web API & EF Core

✅ Controller-based API  
✅ Entity Framework Core  
✅ Fluent API Configuration  
✅ Migrations  
✅ AsNoTracking (Performance)  
✅ Eager Loading  
✅ Global Exception Handling  
✅ ProblemDetails

### Module 3: Architecture & Security

✅ Clean Architecture  
✅ SOLID Principles  
✅ JWT Authentication  
✅ Role-based Authorization  
✅ Serilog Structured Logging  
✅ Health Checks  
✅ Docker & Docker Compose

## 👨‍💻 Developer Notes

### Add Migration

```bash
cd src/TaskFlow.API
dotnet ef migrations add MigrationName
dotnet ef database update
```

### Environment Variables

Set these as environment variables in production:

- `ConnectionStrings__DefaultConnection`
- `JwtSettings__SecretKey`

## 📄 License

MIT

## 🤝 Contributing

This project is for educational purposes. Feel free to open a Pull Request for improvements.

---

**Developed with ❤️ for .NET Mastery Training**
