# StackBuld API - Product Catalog and Order Processing

A production-grade C# Web API for managing a product catalog and processing orders with proper stock management.

## Tech Stack

- **.NET 9.0** - Latest long-term support version
- **ASP.NET Core** - For building the RESTful API
- **Entity Framework Core** - ORM for database operations
- **SQLite** - Lightweight relational database
- **AutoMapper** - For object-to-object mapping
- **Swagger/OpenAPI** - API documentation and testing
- **API Versioning** - For future API version management
- **xUnit & Moq** - For unit testing

## Architecture

This solution follows the Clean Architecture principles:

- **Domain Layer**: Contains entities, interfaces, and domain logic
- **Application Layer**: Contains services, DTOs, and application logic
- **Infrastructure Layer**: Contains database, repositories, and external concerns
- **Presentation Layer**: Contains controllers and API endpoints
- **Tests Layer**: Contains unit tests for the application

## Features

- **Product Management**:
  - CRUD operations for product catalog
  - Each product has Id, Name, Description, Price, StockQuantity

- **Order Processing**:
  - Place orders with multiple products
  - Validation for sufficient stock
  - Concurrent stock updates with optimistic concurrency control (using RowVersion)

- **Data Integrity**:
  - Prevents overselling through concurrency control
  - Transactional order processing

## Setup Instructions

### Prerequisites

- .NET 9.0 SDK or later
- Visual Studio 2022 or JetBrains Rider (recommended)

### Running the Application

1. Clone the repository
   ```
   git clone https://github.com/yourusername/stackbuld.git
   cd stackbuld
   ```

2. Restore dependencies
   ```
   dotnet restore
   ```

3. Build the solution
   ```
   dotnet build
   ```

4. Run the application
   ```
   dotnet run --project stackbuld.csproj
   ```

5. Open your browser and navigate to:
   ```
   https://localhost:5001/swagger/index.html
   ```

### Running Tests

```
dotnet test
```

## API Endpoints

### Products

- `GET /api/v1/products` - Get all products
- `GET /api/v1/products/{id}` - Get product by ID
- `POST /api/v1/products` - Create a new product
- `PUT /api/v1/products/{id}` - Update a product
- `DELETE /api/v1/products/{id}` - Delete a product

### Orders

- `POST /api/v1/orders/place` - Place a new order
- `GET /api/v1/orders` - Get all orders
- `GET /api/v1/orders/{id}` - Get order by ID

## Design Decisions and Assumptions

- **SQLite Database**: Chosen for simplicity and ease of setup. In a production environment, this could be replaced with PostgreSQL or SQL Server.
- **Optimistic Concurrency**: Used RowVersion to prevent concurrent updates to stock.
- **No Authentication/Authorization**: For simplicity, this project does not include authentication. In a production application, JWT or OAuth 2.0 would be implemented.
- **Minimal Order Entity**: For simplicity, the Order entity only contains essential information. In a production environment, it would include shipping details, payment information, etc.
- **Error Handling**: Used a global error handling middleware to ensure consistent error responses across the API.

## Improvements for Production

- Add authentication and authorization
- Implement more comprehensive logging
- Add database migrations strategy
- Add integration tests
- Implement caching strategies
- Implement rate limiting
- Add health checks
- Containerize the application using Docker