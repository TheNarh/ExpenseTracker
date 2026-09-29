using System.Linq;
public class ExpenseService
{
    private readonly IExpenseRepository repository;

    public ExpenseService(IExpenseRepository repository)
    {
        this.repository = repository;
    }

    public void AddExpense(string description, decimal amount, string category)
    {
        Expense expense = new Expense(description, amount, category);
        repository.Add(expense);
    }

    public IReadOnlyList<Expense> GetExpenses()
    {
        return repository.GetAll();
    }

    public decimal CalculateTotal()
    {
        decimal total = 0;

        foreach (Expense expense in repository.GetAll())
        {
            total += expense.Amount;
        }

        return total;
    }

    public bool DeleteExpense(Guid id)
{
    Expense? expenseToDelete = repository.GetById(id);

    if (expenseToDelete != null)
    {
        repository.Delete(expenseToDelete);
        return true;
    }

    return false;
}
}