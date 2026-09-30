using ExpenseTracker.Api.DTOs;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactApp", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

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

app.UseHttpsRedirection();

app.MapGet("/api/expenses", (ExpenseService expenseService) =>
{
    return expenseService.GetExpenses();
});

app.MapPost("/api/expenses", (AddExpenseRequest request, ExpenseService expenseService) =>
{
    expenseService.AddExpense(
        request.Description,
        request.Amount,
        request.Category
    );

    return Results.Ok();
});

app.MapPut("/api/expenses/{id:guid}", (
    Guid id,
    UpdateExpenseRequest request,
    ExpenseService expenseService) =>
{
    bool updated = expenseService.UpdateExpense(
        id,
        request.Description,
        request.Amount,
        request.Category
    );

    if (!updated)
    {
        return Results.NotFound();
    }

    return Results.NoContent();
});

app.MapDelete("/api/expenses/{id:guid}", (Guid id, ExpenseService expenseService) =>
{
    bool deleted = expenseService.DeleteExpense(id);

    if (!deleted)
    {
        return Results.NotFound();
    }

    return Results.NoContent();
});

app.Run();
