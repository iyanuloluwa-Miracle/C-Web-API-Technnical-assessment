# StackBuld API - Product Catalog and Order Processing

A production-grade C# Web API for managing a product catalog and processing orders with proper stock management.

## Tech Stack

- **.NET 9.0** - Latest long-term support version
- **ASP.NET Core** - For building the RESTful API
- **Entity Framework Core** - ORM for database operations
- **PostgreSQL** - Production-ready relational database
- **AutoMapper** - For object-to-object mapping
- **Swagger/OpenAPI** - API documentation and testing
- **API Versioning** - For future API version management

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
  - Concurrent stock updates with optimistic concurrency control (using Version field)

- **Data Integrity**:
  - Prevents overselling through concurrency control
  - Transactional order processing

## Setup Instructions

### Prerequisites

- .NET 9.0 SDK or later
- PostgreSQL 12 or later
- Visual Studio 2022 or JetBrains Rider (recommended)

### Database Setup

1. Install PostgreSQL if not already installed
2. Create a database named `studybd_db`
3. Update the connection string in `appsettings.json` if needed:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Port=5432;Database=studybd_db;Username=postgres;Password=your_password"
     }
   }
   ```

### Running the Application

1. Clone the repository

   ```bash
   git clone https://github.com/iyanuloluwa-Miracle/TechAssessment-ProductCatalogApi.git
   cd TechAssessment-ProductCatalogApi
   ```

2. Restore dependencies

   ```bash
   dotnet restore
   ```

3. Build the solution

   ```bash
   dotnet build
   ```

4. Run the application

   ```bash
   dotnet run --project stackbuld.csproj
   ```

5. Open your browser and navigate to:

   ```url
   https://localhost:7189/swagger/index.html
   ```

### Running Tests

Currently, no tests are implemented. This would be a good area for future development.

```bash
# When tests are implemented:
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

- **PostgreSQL Database**: Chosen for production-grade reliability and performance. The database is configured to run on localhost with standard PostgreSQL settings.
- **Optimistic Concurrency**: Used a Version field with ConcurrencyCheck attribute to prevent concurrent updates to stock.
- **No Authentication/Authorization**: For simplicity, this project does not include authentication. In a production application, JWT or OAuth 2.0 would be implemented.
- **Minimal Order Entity**: For simplicity, the Order entity only contains essential information. In a production environment, it would include shipping details, payment information, etc.
- **Error Handling**: Used a global error handling middleware to ensure consistent error responses across the API.

## Improvements for Production

- Add authentication and authorization
- Implement comprehensive unit and integration tests
- Implement more comprehensive logging
- Add database migrations strategy
- Implement caching strategies
- Implement rate limiting
- Add health checks
- Containerize the application using Docker