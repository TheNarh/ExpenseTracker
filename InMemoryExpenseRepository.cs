public class InMemoryExpenseRepository : IExpenseRepository
{
    private List<Expense> expenses = new List<Expense>();
    public void Add(Expense expense)
    {
        expenses.Add(expense);
    }

    public IReadOnlyList<Expense> GetAll()
    {
        return expenses.AsReadOnly();
    }

    public Expense? GetById(Guid id)
    {
        return expenses.FirstOrDefault(
            expense => expense.Id == id
        );
    }

    public void Update(Expense expense)
{
    int index = expenses.FindIndex(
        existingExpense => existingExpense.Id == expense.Id
    );

    if (index != -1)
    {
        expenses[index] = expense;
    }
}
    public void Delete(Expense expense)
    {
        expenses.Remove(expense);
    }
}