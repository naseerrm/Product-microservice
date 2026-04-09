🏗️ Project Overview

This project is an enterprise-grade Product Microservice built using modern .NET 10 backend architecture principles. It demonstrates scalable design patterns used in real-world distributed systems.

The application is designed to handle high-volume data operations, support microservices architecture, and integrate with cloud platforms like Microsoft Azure.

🚀 Key Features
✅ Clean Architecture (Domain, Application, Infrastructure)
✅ CQRS Pattern using MediatR
✅ Minimal APIs with Endpoint Grouping
✅ Full CRUD Operations
✅ Advanced APIs (Search, Bulk Upload, Inventory, Pricing)
✅ Dapper for High-Performance Reads
✅ Entity Framework Core for Write Operations
✅ Redis Cache (optional integration)
✅ Pagination for handling millions of records
✅ Swagger API Documentation
✅ Azure Deployment Ready
✅ CI/CD Pipeline Ready
🧱 Architecture

This project follows Clean Architecture principles:

Domain → Core business logic
Application → CQRS (Commands & Queries)
Infrastructure → Database, Dapper, External services
API Layer → Minimal APIs (Endpoints)


⚙️ Tech Stack
.NET 10 Web API
MediatR
Entity Framework Core
Dapper
SQL Server
Redis (Caching Layer)
Swagger (OpenAPI)
Azure App Service
Azure DevOps (CI/CD)

📌 API Endpoints
Product APIs
Method	Endpoint	Description
POST	/products	Create product
GET	/products/{id}	Get product by ID
PUT	/products	Update product
DELETE	/products/{id}	Delete product
Advanced APIs
Method	Endpoint	Description
GET	/products/search/{keyword}	Search products
GET	/products/paged-search	Pagination support
POST	/products/bulk-upload	Bulk upload products
PUT	/products/inventory	Update stock
PUT	/products/pricing	Update price

📊 Scalability Features
Efficient data fetching using Dapper
Pagination to handle millions of records
Stateless API design
Ready for horizontal scaling in cloud
