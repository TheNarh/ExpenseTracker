# Expense Tracker

A C# and ASP.NET Core application for managing personal expenses.

The project demonstrates practical backend development concepts including object-oriented programming, layered architecture, repository pattern, dependency injection, DTOs, RESTful APIs, validation, and unit testing.

## Features

* Add expenses
* View all expenses
* Calculate total expenses
* Delete expenses
* Input validation
* RESTful Web API
* Dependency injection
* Repository pattern
* Service layer
* DTO-based API requests
* Unit tests

## Technology Stack

* C#
* .NET 10
* ASP.NET Core Web API
* xUnit
* Swagger/OpenAPI
* Git/GitHub

## Architecture

The application separates responsibilities into different layers:

```text
Client
   |
   v
ASP.NET Core Web API
   |
   v
DTO
   |
   v
ExpenseService
   |
   v
IExpenseRepository
   |
   v
InMemoryExpenseRepository
```

### Expense

The `Expense` class represents the core business entity.

It contains:

* ID
* Description
* Amount
* Category

Business validation is performed when an expense is created.

### ExpenseService

`ExpenseService` contains the application's business logic.

It is responsible for:

* Creating expenses
* Retrieving expenses
* Calculating expense totals
* Deleting expenses

### Repository

The repository abstraction separates business logic from data storage.

`IExpenseRepository` defines the data operations while `InMemoryExpenseRepository` provides the current implementation.

This allows the storage mechanism to be changed later without rewriting the business logic.

### Dependency Injection

ASP.NET Core's built-in dependency injection container provides the repository and service to the API.

This keeps components loosely coupled and makes the application easier to test and maintain.

## API Endpoints

| Method | Endpoint             | Description           |
| ------ | -------------------- | --------------------- |
| GET    | `/api/expenses`      | Retrieve all expenses |
| POST   | `/api/expenses`      | Create an expense     |
| DELETE | `/api/expenses/{id}` | Delete an expense     |

### Create an Expense

```http
POST /api/expenses
Content-Type: application/json
```

Example request:

```json
{
  "description": "Lunch",
  "amount": 50,
  "category": "Food"
}
```

### Get Expenses

```http
GET /api/expenses
```

Example response:

```json
[
  {
    "id": "744a3ab0-f4b4-4bde-ad90-aa9214e694ec",
    "description": "Lunch",
    "amount": 50,
    "category": "Food"
  }
]
```

### Delete an Expense

```http
DELETE /api/expenses/{id}
```

Returns `204 No Content` when the expense is successfully deleted and `404 Not Found` when the expense does not exist.

## Running the Application

Clone the repository:

```bash
git clone https://github.com/TheNarh/ExpenseTracker.git
cd ExpenseTracker
```

Build the project:

```bash
dotnet build
```

Run the API:

```bash
dotnet run --project ExpenseTracker.Api
```

The API can then be accessed through the local development URL shown by ASP.NET Core.

## Testing

Run the test suite with:

```bash
dotnet test
```

The tests cover business logic including:

* Expense total calculation
* Empty expense collections
* Expense deletion
* Validation of descriptions
* Validation of categories
* Repository operations

## Current Data Storage

The current API uses an in-memory repository.

This keeps the project focused on demonstrating backend architecture and allows the application to run without requiring an external database.

For a production implementation, the repository could be replaced with a persistent database implementation using Entity Framework Core and a relational database such as PostgreSQL or SQL Server.

The repository abstraction allows this change to be made with minimal impact on the service layer.

## Future Improvements

Potential production enhancements include:

* PostgreSQL or SQL Server persistence
* Entity Framework Core
* Authentication and authorization
* Global exception handling
* Structured logging
* API validation
* Integration tests
* Docker containerization
* CI/CD pipeline
* API versioning
* Production monitoring

## Author

**Ludwig Sackey Narh**

GitHub: [TheNarh](https://github.com/TheNarh)
