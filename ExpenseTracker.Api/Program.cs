using ExpenseTracker.Api.DTOs;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IExpenseRepository, InMemoryExpenseRepository>();
builder.Services.AddSingleton<ExpenseService>();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
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

