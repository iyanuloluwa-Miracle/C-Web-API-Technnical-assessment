# Product Catalog and Order Processing API

## Overview
A production-grade ASP.NET Core Web API for managing products and processing orders, preventing overselling via concurrency control.

## Setup
1. Clone repo.
2. `dotnet restore`
3. `dotnet ef database update --project Infrastructure --startup-project Presentation`
4. `dotnet run --project Presentation`

## Assumptions
- See above.

## Tech Stack
- .NET 8
- EF Core with SQLite
- Clean Architecture
- AutoMapper, Serilog, Swagger

## Best Practices
- Concurrency handling with row versions.
- Transactions via UnitOfWork.
- API versioning.
- Logging and error handling.
- Unit tests included.

For Docker: Add Dockerfile to Presentation.