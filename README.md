\# CRN Product API



A production-style RESTful Web API built with ASP.NET Core 8, Entity Framework Core, SQL Server, JWT authentication, refresh tokens, Docker, and xUnit testing.



\## Features



\- JWT-based authentication

\- Refresh token authentication

\- Product CRUD operations

\- Item CRUD operations

\- Product-item relationship

\- Pagination

\- Request validation

\- Global exception handling middleware

\- Entity Framework Core migrations

\- SQL Server database

\- Docker and Docker Compose support

\- Unit tests using xUnit

\- Swagger/OpenAPI documentation



\## Technology Stack



\- C#

\- .NET 8

\- ASP.NET Core Web API

\- Entity Framework Core

\- SQL Server

\- JWT

\- xUnit

\- Docker

\- Docker Compose

\- Swagger / OpenAPI



\## Project Architecture



The project follows a layered architecture:



```text

CRN.ProductAPI

│

├── Application

│   ├── DTOs

│   ├── Interfaces

│   ├── Mapping

│   ├── Services

│   └── Validators

│

├── Controllers

│

├── Domain

│   └── Entities

│

├── Infrastructure

│   ├── Data

│   └── Repositories

│

├── Middleware

│

└── Migrations

