
public class ExpenseServiceTests
{
    [Fact]
    public void CalculateTotal_ReturnsSumOfExpenses()
    {
        // Arrange
        IExpenseRepository repository = new InMemoryExpenseRepository();
        ExpenseService expenseService = new ExpenseService(repository);

        expenseService.AddExpense("Milk", 25, "Food");
        expenseService.AddExpense("Bread", 15, "Food");

        // Act
        decimal total = expenseService.CalculateTotal();

        // Assert
        Assert.Equal(40, total);
    }

    [Fact]
    public void CalculateTotal_WhenThereAreNoExpenses_ReturnsZero()
    {
        // Arrange
        IExpenseRepository repository = new InMemoryExpenseRepository();
        ExpenseService expenseService = new ExpenseService(repository);

        // Act
        decimal total = expenseService.CalculateTotal();

        // Assert
        Assert.Equal(0, total);
    }

    [Fact]
    public void DeleteExpense_WhenExpenseExists_RemovesExpense()
    {
        // Arrange
        IExpenseRepository repository = new InMemoryExpenseRepository();
        ExpenseService expenseService = new ExpenseService(repository);

        expenseService.AddExpense("Milk", 25, "Food");

        IReadOnlyList<Expense> expenses = expenseService.GetExpenses();
        Expense expense = expenses[0];

        // Act
        bool deleted = expenseService.DeleteExpense(expense.Id);

        // Assert
        Assert.True(deleted);

        IReadOnlyList<Expense> remainingExpenses = expenseService.GetExpenses();

        Assert.Empty(remainingExpenses);
    }

    [Fact]
    public void AddExpense_WhenDescriptionIsEmpty_ThrowsException()
    {
        // Arrange
        IExpenseRepository repository = new InMemoryExpenseRepository();
        ExpenseService expenseService = new ExpenseService(repository);

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            expenseService.AddExpense("", 25, "Food")
        );
    }

    [Fact]
    public void AddExpense_WhenCategoryIsEmpty_ThrowsException()
    {
        // Arrange
        IExpenseRepository repository = new InMemoryExpenseRepository();
        ExpenseService expenseService = new ExpenseService(repository);

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            expenseService.AddExpense("Milk", 25, "")
        );
    }
}

public class InMemoryExpenseRepositoryTests
{
    [Fact]
    public void Add_WhenExpenseIsAdded_ReturnsExpense()
    {
        // Arrange
        IExpenseRepository repository = new InMemoryExpenseRepository();

        Expense expense = new Expense("Milk", 25, "Food");

        // Act
        repository.Add(expense);

        // Assert
        IReadOnlyList<Expense> expenses = repository.GetAll();

        Assert.Single(expenses);
        Assert.Equal("Milk", expenses[0].Description);
    }
}
