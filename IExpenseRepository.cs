public interface IExpenseRepository
{
    void Add(Expense expense);
    IReadOnlyList<Expense> GetAll();
    Expense? GetById(Guid id);
    void Delete(Expense expense);
}