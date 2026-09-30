using ExpenseTracker.Api.DTOs;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Render provides the PORT environment variable
var port = Environment.GetEnvironmentVariable("PORT");

if (!string.IsNullOrEmpty(port))
{
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
}

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactApp", policy =>
    {
        policy
            .SetIsOriginAllowed(origin =>
            {
                if (string.IsNullOrEmpty(origin))
                    return false;

                var uri = new Uri(origin);

                return uri.Host.EndsWith(".vercel.app");
            })
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Dependency Injection
builder.Services.AddSingleton<IExpenseRepository, InMemoryExpenseRepository>();
builder.Services.AddSingleton<ExpenseService>();

// OpenAPI
builder.Services.AddOpenApi();

var app = builder.Build();

// Enable CORS
app.UseCors("ReactApp");

// OpenAPI / Scalar only in development
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// GET all expenses
app.MapGet("/api/expenses", (ExpenseService expenseService) =>
{
    return expenseService.GetExpenses();
});

// POST - add expense
app.MapPost("/api/expenses", (
    Expense expense,
    ExpenseService expenseService) =>
{
    var createdExpense = expenseService.AddExpense(
        expense.Description,
        expense.Amount,
        expense.Category
    );

    return Results.Created(
        $"/api/expenses/{createdExpense.Id}",
        createdExpense
    );
});

// PUT - update expense
app.MapPut("/api/expenses/{id:guid}", (
    Guid id,
    UpdateExpenseRequest request,
    ExpenseService expenseService) =>
{
    var updatedExpense = expenseService.UpdateExpense(
        id,
        request.Description,
        request.Amount,
        request.Category
    );

    if (updatedExpense == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(updatedExpense);
});

// DELETE - delete expense
app.MapDelete("/api/expenses/{id:guid}", (
    Guid id,
    ExpenseService expenseService) =>
{
    var deleted = expenseService.DeleteExpense(id);

    if (!deleted)
    {
        return Results.NotFound();
    }

    return Results.NoContent();
});

app.Run();