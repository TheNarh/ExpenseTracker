using ExpenseTracker.Api.DTOs;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Render provides the PORT environment variable.
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

// CORS middleware
app.UseCors("ReactApp");

// OpenAPI / Scalar only in development
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// Get all expenses
app.MapGet("/api/expenses", (ExpenseService expenseService) =>
{
    return expenseService.GetExpenses();
});

// Add expense
app.MapPost("/api/expenses", (
    CreateExpenseRequest request,
    ExpenseService expenseService) =>
{
    var expense = expenseService.AddExpense(
        request.Description,
        request.Amount,
        request.Category
    );

    return Results.Created($"/api/expenses/{expense.Id}", expense);
});

// Update expense
app.MapPut("/api/expenses/{id}", (
    int id,
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

// Delete expense
app.MapDelete("/api/expenses/{id}", (
    int id,
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