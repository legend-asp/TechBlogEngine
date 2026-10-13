# 📰 TechBlogEngine — Enterprise Content Platform & RESTful API
[![Framework](https://img.shields.io/badge/ASP.NET_Core-Web_API_%26_MVC-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)]()
[![Documentation](https://img.shields.io/badge/API_Docs-Swagger_/_OpenAPI-85EA2D?style=for-the-badge&logo=swagger&logoColor=black)]()
[![Logging](https://img.shields.io/badge/Logging-NLog_Structured-red?style=for-the-badge)]()
[![ORM](https://img.shields.io/badge/ORM-Entity_Framework_Core-blue?style=for-the-badge)]()
> A full-featured publishing platform and RESTful API demonstrating clean architecture, cookie-based authentication, structured logging, and automated OpenAPI documentation.
---
## 📌 Architecture & Features
- **📐 Clean Architecture & Service Layer:** Strict separation between Controller, Service (`IArticleService`), and Data Persistence layers.
- **📄 Interactive Swagger / OpenAPI:** Custom schema generation handling JSON reference loops (`ReferenceLoopHandling.Ignore`) for circular relationship models (Article ↔ User).
- **🛡️ Authentication & Authorization:** Cookie-based security workflow with protected route policies and login redirections.
- **📝 Structured Logging:** Centralized diagnostic logging configured via `NLog.Web` with dedicated file targets and exception handling.
---
## 📡 API Endpoints Overview
| Method | Endpoint | Description | Auth Required |
| :--- | :--- | :--- | :--- |
| `GET` | `/api/articles` | Retrieve paginated articles | No |
| `GET` | `/api/articles/{id}` | Get article details by ID | No |
| `POST` | `/api/articles` | Create a new blog publication | Yes |
| `PUT` | `/api/articles/{id}` | Update existing publication | Yes |
| `DELETE` | `/api/articles/{id}` | Delete article | Yes |
---
## 🚀 Quickstart
```bash
git clone https://github.com/yandereye1/TechBlogEngine.git
cd TechBlogEngine/MyTechnicalBlog
dotnet ef database update
dotnet run

---

## 👨‍💻 Author
GitHub: [@yandereye1](https://github.com/yandereye1)
