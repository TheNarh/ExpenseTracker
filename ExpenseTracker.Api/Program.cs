using ExpenseTracker.Api.DTOs;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddOpenApi();

var app = builder.Build();

app.UseCors("ReactApp");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// GET expenses
app.MapGet("/api/expenses", (ExpenseService expenseService) =>
{
    return expenseService.GetExpenses();
});

// POST expense
app.MapPost("/api/expenses", (
    Expense expense,
    ExpenseService expenseService) =>
{
    expenseService.AddExpense(
        expense.Description,
        expense.Amount,
        expense.Category
    );

    return Results.Created("/api/expenses", expense);
});

// PUT expense
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

    return Results.Ok(updatedExpense);
});

// DELETE expense
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