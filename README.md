# ThothDeskCore

Educational management backend built with **.NET 8**, designed to showcase a clean architecture approach.

## 🚀 Tech Stack
- ASP.NET Core 8 (Web API with Controllers)
- Entity Framework Core 8 + SQL Server (Dockerized)
- ASP.NET Core Identity + JWT authentication & role-based authorization
- FluentValidation
- Swagger / OpenAPI
- (soon) xUnit, GitHub Actions CI
- (soon) Serilog + Seq, Redis cache, Hangfire jobs

## ✨ Features
- **Authentication & Authorization**  
  - Register/login with JWT  
  - Role-based authorisation (Admin / Instructor / Student)

- **Courses**  
  - CRUD operations  
  - Filtering & pagination  

- **Assignments**  
  - CRUD linked to Courses  
  - Validation (title, due date, points)  
  - Pagination & filtering  

- **Extensible design** for Enrollments, Submissions, and Grading

---

## 🏗 Architecture
- Client (Swagger / Next.js)
- HTTP JSON
- API Layer (Controllers, DTOs, Validation, Mapping)
- Infrastructure Layer (EF Core, Identity, DbContext)
- Domain Layer (Entities, Business Logic)
- SQL Server (Docker)

## 🔐 Authentication Flow
1. POST /auth/register → create user
2. POST /auth/login → get JWT token
3. Authorize in Swagger (Authorize → Bearer <token>)
4. Access protected endpoints (e.g. POST /courses, POST /assignments)

## 📚 API Highlights
- GET /api/thothdesk/v1/courses?page=1&pageSize=20
- POST /api/thothdesk/v1/courses → create a course
- GET /api/thothdesk/v1/assignments?courseId=<GUID>
- POST /api/thothdesk/v1/assignments (Instructor/Admin only)

## 🧪 Testing (soon)
- Unit tests: xUnit for domain logic
- Integration tests: WebApplicationFactory for controllers
- CI pipeline: GitHub Actions (restore, build, test)

## 🗺 Roadmap
- Enrollment requests & approvals
- Submissions & grading
- Logging with Serilog + Seq
- Redis caching
- File uploads (MinIO, presigned URLs)
- Background jobs with Hangfire
