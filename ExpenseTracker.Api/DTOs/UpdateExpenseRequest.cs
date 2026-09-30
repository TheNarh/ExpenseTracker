namespace ExpenseTracker.Api.DTOs;

public class UpdateExpenseRequest
{
    public string Description { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public string Category { get; set; } = string.Empty;
}