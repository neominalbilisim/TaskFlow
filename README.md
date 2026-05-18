# TaskFlow - Kurumsal Görev Yönetim Sistemi

## 🎯 Proje Hakkında

TaskFlow, **Clean Architecture** ve **SOLID** prensipleri kullanılarak geliştirilmiş, production-ready bir görev yönetim API'sidir.

### 🏗️ Mimari Katmanlar

```
TaskFlow/
├── TaskFlow.Domain          → Entities, Enums (No dependencies)
├── TaskFlow.Application     → Use Cases, DTOs, Interfaces
├── TaskFlow.Infrastructure  → EF Core, Repositories, External Services
└── TaskFlow.API            → Controllers, Middleware, Filters
```

## ✨ Özellikler

### 🔐 Güvenlik
- ✅ JWT Token-based Authentication
- ✅ Role-based Authorization (Admin, Manager, Employee)
- ✅ Resource-based Authorization
- ✅ Password Hashing (BCrypt)

### ⚡ Performans
- ✅ AsNoTracking() for read-only queries
- ✅ Eager Loading to prevent N+1 problem
- ✅ Pagination support
- ✅ Proper indexing on database

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

## 📦 Teknolojiler

- **.NET 8**
- **Entity Framework Core 8**
- **SQL Server**
- **JWT Authentication**
- **Serilog**
- **FluentValidation**
- **Swagger**
- **Docker**

## 🚀 Kurulum ve Çalıştırma

### Docker ile Çalıştırma (Önerilen)

```bash
# Projeyi klonlayın
git clone <repo-url>
cd TaskFlow

# Docker Compose ile başlatın
docker-compose up -d

# API: http://localhost:5000
# Swagger: http://localhost:5000/swagger
```

### Manuel Çalıştırma

```bash
# SQL Server bağlantısını appsettings.json'da güncelleyin

# Migration çalıştırın
cd src/TaskFlow.API
dotnet ef database update

# Uygulamayı başlatın
dotnet run

# API: https://localhost:5001
# Swagger: https://localhost:5001/swagger
```

## 📚 API Endpoints

### Authentication
```
POST   /api/auth/register    → Yeni kullanıcı kaydı
POST   /api/auth/login       → Giriş yap (JWT token al)
```

### Tasks
```
GET    /api/tasks                    → Tüm görevler (Pagination + Filters)
GET    /api/tasks/{id}               → Görev detayı
GET    /api/tasks/assigned-to-me     → Bana atanan görevler
POST   /api/tasks                    → Yeni görev oluştur (Manager+)
PUT    /api/tasks/{id}               → Görevi güncelle
PATCH  /api/tasks/{id}/status        → Görev durumunu güncelle
DELETE /api/tasks/{id}               → Görevi sil (Manager+)
```

### Projects
```
GET    /api/projects           → Tüm projeler
GET    /api/projects/{id}      → Proje detayı
GET    /api/projects/{id}/tasks → Proje görevleri
POST   /api/projects           → Yeni proje (Manager+)
PUT    /api/projects/{id}      → Projeyi güncelle (Manager+)
```

### Users (Admin Only)
```
GET    /api/users              → Tüm kullanıcılar
GET    /api/users/{id}         → Kullanıcı detayı
PUT    /api/users/{id}/role    → Kullanıcı rolünü değiştir
```

### Health
```
GET    /health                 → Uygulama sağlık durumu
```

## 🔑 Kullanıcı Rolleri

### Employee
- Sadece kendine atanan görevleri görebilir
- Kendi görevlerinin durumunu güncelleyebilir

### Manager
- Departmanındaki tüm görevleri görebilir
- Görev oluşturabilir, atayabilir
- Proje oluşturabilir

### Admin
- Tüm sistem erişimi
- Kullanıcı rollerini değiştirebilir
- Tüm CRUD işlemlerini yapabilir

## 🧪 Test Senaryosu

```bash
# 1. Kullanıcı kaydı
POST /api/auth/register
{
  "email": "john@company.com",
  "password": "password123",
  "firstName": "John",
  "lastName": "Doe"
}

# 2. Giriş yap
POST /api/auth/login
{
  "email": "john@company.com",
  "password": "password123"
}
# → JWT token alın

# 3. Swagger'da "Authorize" butonuna token ekleyin
Bearer <your-jwt-token>

# 4. Proje oluştur (Admin/Manager gerekli)
POST /api/projects
{
  "name": "Web Application",
  "description": "Customer portal project"
}

# 5. Görev oluştur
POST /api/tasks
{
  "title": "Setup database",
  "description": "Create tables and relationships",
  "projectId": 1,
  "priority": 2,
  "assignedUserIds": [1]
}

# 6. Benim görevlerim
GET /api/tasks/assigned-to-me

# 7. Görev durumunu güncelle
PATCH /api/tasks/1/status
{
  "status": 1  // InProgress
}
```

## 🏛️ Clean Architecture Katmanları

### Domain Layer
- **Entities:** User, Project, TaskItem, TaskAssignment, Comment
- **Enums:** TaskStatus, TaskPriority, UserRole
- **No Dependencies:** Tamamen bağımsız, Pure C#

### Application Layer
- **Interfaces:** Repository ve Service interface'leri
- **DTOs:** API request/response modelleri
- **Services:** Business logic
- **Validators:** FluentValidation rules
- **Depends on:** Domain

### Infrastructure Layer
- **DbContext:** Entity Framework configuration
- **Repositories:** Data access implementation
- **External Services:** TokenService
- **Depends on:** Application, Domain

### API Layer (Presentation)
- **Controllers:** HTTP endpoint'ler
- **Middleware:** Exception handling
- **Program.cs:** DI configuration
- **Depends on:** Application, Infrastructure

## 🛡️ Güvenlik Özellikleri

### JWT Token Yapısı
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
[Authorize]                        // Authenticated user gerekli
[Authorize(Roles = "Admin")]      // Admin rolü gerekli
[Authorize(Roles = "Admin,Manager")] // Admin VEYA Manager
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

Serilog ile yapılandırılmış loglama:

```csharp
// Logs klasöründe günlük dosyalar
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

## 📚 Öğrenilen Konular

Bu proje **TüM eğitim modüllerini** kapsar:

### Modül 1: Modern C#
✅ Records  
✅ Primary Constructors  
✅ Nullable Reference Types  
✅ LINQ  
✅ Dependency Injection  
✅ Options Pattern  

### Modül 2: Web API & EF Core
✅ Controller-based API  
✅ Entity Framework Core  
✅ Fluent API Configuration  
✅ Migrations  
✅ AsNoTracking (Performance)  
✅ Eager Loading  
✅ Global Exception Handling  
✅ ProblemDetails  

### Modül 3: Architecture & Security
✅ Clean Architecture  
✅ SOLID Principles  
✅ JWT Authentication  
✅ Role-based Authorization  
✅ Serilog Structured Logging  
✅ Health Checks  
✅ Docker & Docker Compose  

## 👨‍💻 Geliştirici Notları

### Migration Ekleme
```bash
cd src/TaskFlow.API
dotnet ef migrations add MigrationName
dotnet ef database update
```

### Environment Variables
Production'da şunları environment variable olarak set edin:
- `ConnectionStrings__DefaultConnection`
- `JwtSettings__SecretKey`

## 📄 License

MIT

## 🤝 Katkıda Bulunma

Bu proje eğitim amaçlıdır. Geliştirmeler için Pull Request açabilirsiniz.

---

**Developed with ❤️ for .NET Mastery Training**
