# Expense Tracker

A full-stack personal expense management application built with **C#, ASP.NET Core Web API, React, and Vite**.

The project demonstrates practical software engineering concepts including object-oriented programming, RESTful API development, layered architecture, repository and service patterns, dependency injection, DTOs, validation, CRUD operations, unit testing, frontend state management, API integration, and responsive UI design.

## Overview

Expense Tracker allows users to record, manage, search, filter, edit, and delete personal expenses through a responsive web interface.

The application consists of a React frontend that communicates with an ASP.NET Core Web API over HTTP/JSON.

```text
React Frontend
      |
      | HTTP / JSON
      v
ASP.NET Core Web API
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

## Features

### Expense Management

* Add expenses
* View expenses
* Edit expenses
* Delete expenses
* Delete confirmation modal
* Automatic transaction dates
* Input validation
* Expense categories

### Dashboard

* Total spending
* Number of transactions
* Average expense
* Spending breakdown by category
* Percentage-based category analysis

### Search & Filtering

* Search expenses by description
* Filter expenses by category
* Display matching transaction count
* Empty states for searches with no results

### Backend

* RESTful ASP.NET Core Web API
* CRUD operations
* Service layer
* Repository pattern
* Dependency injection
* DTO-based requests
* Business validation
* Unit testing
* Swagger/OpenAPI support
* Logging repository implementation

### Frontend

* React
* Vite
* React Hooks
* REST API integration
* Loading and error states
* Responsive design
* Form validation
* Interactive confirmation modal
* Responsive dashboard UI

## Technology Stack

### Backend

* C#
* .NET 10
* ASP.NET Core Web API
* xUnit
* Swagger/OpenAPI

### Frontend

* React
* Vite
* JavaScript
* HTML
* CSS

### Development Tools

* Git
* GitHub
* Visual Studio Code
* npm

## Project Structure

```text
ExpenseTracker/
│
├── ExpenseTracker.Api/
│   ├── Controllers/
│   ├── DTOs/
│   ├── Program.cs
│   └── ExpenseTracker.Api.csproj
│
├── expense-tracker-client/
│   ├── public/
│   ├── src/
│   │   ├── App.jsx
│   │   ├── App.css
│   │   └── ...
│   ├── package.json
│   └── vite.config.js
│
├── ExpenseTracker.Tests/
│
├── Expense.cs
├── ExpenseService.cs
├── IExpenseRepository.cs
├── InMemoryExpenseRepository.cs
├── LoggingExpenseRepository.cs
├── ExpenseTracker.csproj
├── Program.cs
└── README.md
```

## Architecture

The application separates responsibilities between the API, business logic, and data access layers.

```text
                    React Frontend
                         |
                         | HTTP / JSON
                         v
                ASP.NET Core Web API
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
* CreatedAt

Business validation is performed when an expense is created or updated.

### ExpenseService

`ExpenseService` contains the application's business logic.

It is responsible for operations such as:

* Creating expenses
* Retrieving expenses
* Updating expenses
* Calculating expense totals
* Deleting expenses
* Applying business validation

### Repository

The repository abstraction separates business logic from data storage.

`IExpenseRepository` defines the data operations while `InMemoryExpenseRepository` provides the current implementation.

This allows the storage mechanism to be changed later without rewriting the business logic.

### Dependency Injection

ASP.NET Core's built-in dependency injection container provides the required services and repositories to the API.

This keeps components loosely coupled and makes the application easier to test and maintain.

## API Endpoints

| Method | Endpoint             | Description           |
| ------ | -------------------- | --------------------- |
| GET    | `/api/expenses`      | Retrieve all expenses |
| POST   | `/api/expenses`      | Create an expense     |
| PUT    | `/api/expenses/{id}` | Update an expense     |
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
    "category": "Food",
    "createdAt": "2026-09-30T10:30:00Z"
  }
]
```

### Update an Expense

```http
PUT /api/expenses/{id}
Content-Type: application/json
```

Example request:

```json
{
  "description": "Business Lunch",
  "amount": 75,
  "category": "Food"
}
```

### Delete an Expense

```http
DELETE /api/expenses/{id}
```

Returns `204 No Content` when the expense is successfully deleted and `404 Not Found` when the expense does not exist.

## Running the Application Locally

### Prerequisites

Make sure you have installed:

* .NET 10 SDK
* Node.js
* npm
* Git

### 1. Clone the repository

```bash
git clone https://github.com/TheNarh/ExpenseTracker.git
cd ExpenseTracker
```

### 2. Start the ASP.NET Core API

```bash
dotnet run --project ExpenseTracker.Api
```

The API will start on the local development URL provided by ASP.NET Core.

### 3. Start the React frontend

Open a second terminal:

```bash
cd expense-tracker-client
npm install
npm run dev
```

Vite will provide the local frontend URL, normally:

```text
http://localhost:5173
```

### 4. Open the application

Open the Vite URL in your browser.

The React application communicates with the ASP.NET Core API to create, retrieve, update, and delete expenses.

## Testing

Run the backend test suite with:

```bash
dotnet test
```

The tests cover business logic and repository operations including:

* Expense total calculation
* Empty expense collections
* Expense deletion
* Description validation
* Category validation
* Repository operations

## Current Data Storage

The application currently uses an **in-memory repository**.

This keeps the project simple to run locally without requiring an external database while demonstrating the separation between business logic and data access.

The repository abstraction makes it possible to replace the in-memory implementation with a persistent database implementation without significantly changing the service layer.

## Future Improvements

Potential production enhancements include:

* PostgreSQL or SQL Server persistence
* Entity Framework Core
* Authentication and authorization
* Global exception handling
* Structured logging
* Integration testing
* Docker containerization
* CI/CD pipeline
* API versioning
* Production monitoring
* Persistent user accounts
* Financial reporting and visualization

## Project Status

The current version demonstrates a complete full-stack CRUD workflow:

```text
Create
  ↓
React Form
  ↓
ASP.NET Core API
  ↓
Business Logic
  ↓
Repository
  ↓
Expense Data
```

The same architecture supports:

```text
Create → POST
Read   → GET
Update → PUT
Delete → DELETE
```

## Author

**Ludwig Sackey Narh**

GitHub: [TheNarh](https://github.com/TheNarh)

---

Built with **C#, ASP.NET Core, React, and Vite**.
