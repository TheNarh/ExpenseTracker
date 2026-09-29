class LoggingExpenseRepository : IExpenseRepository
{
    public void Add(Expense expense)
    {
        Console.WriteLine("Logging repository: Add called.");
    }

    public IReadOnlyList<Expense> GetAll()
    {
        return new List<Expense>();
    }

    public Expense? GetById(Guid id)
    {
        Console.WriteLine("Logging repository: GetById called.");
        return null;
    }

    public void Delete(Expense expense)
    {
        Console.WriteLine("Logging repository: Delete called.");
    }
}