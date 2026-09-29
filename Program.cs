using System.Linq;

IExpenseRepository repository = new InMemoryExpenseRepository();

ExpenseService expenseService = new ExpenseService(repository);


string GetValidDescription()
{
    while (true)
    {
        Console.WriteLine("Enter expense description: ");
        string? description = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(description))
        {
            return description;
        }

        Console.WriteLine("Description cannot be empty.");
    }
}

decimal GetValidAmount()
{
  while (true)
    {
        Console.Write("Enter expense amount: ");
        string? amountInput = Console.ReadLine();

        if (decimal.TryParse(amountInput, out decimal amount))
        {
            if (amount > 0)
            {
                return amount;
            }

            Console.WriteLine("Amount must be greater than zero.");
        }
        else
        {
            Console.WriteLine("Please enter a valid amount.");
        }
    }
}

string GetValidCategory()
{
    while (true)
    {
        Console.Write("Enter expense category: ");
        string? category = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(category))
        {
            return category;
        }

        Console.WriteLine("Category cannot be empty.");
    }
}

void HandleAddExpense()
{
    string description = GetValidDescription();
    decimal amount = GetValidAmount();
    string category = GetValidCategory();

    expenseService.AddExpense(description, amount, category);

    Console.WriteLine("Expense added successfully.");
}

while (true)
{
    Console.WriteLine("=== EXPENSE TRACKER ===");
    Console.WriteLine();

    Console.WriteLine("1. Add Expense");
    Console.WriteLine("2. View Expenses");
    Console.WriteLine("3. View Total");
    Console.WriteLine("4. Delete Expense");
    Console.WriteLine("5. Exit");

    Console.Write("Choose an option: ");
    string? choice = Console.ReadLine();

    if (choice == "1")
    {
       HandleAddExpense();        
    }
    else if (choice == "2")
    {
        IReadOnlyList<Expense> expenses = expenseService.GetExpenses();

        foreach (Expense expense in expenses)
        {
            Console.WriteLine($"{expense.Id} - {expense.Description} - {expense.Amount:F2} - {expense.Category}");
        }
    }
    else if (choice == "3")
    {
        decimal total = expenseService.CalculateTotal();

        Console.WriteLine($"Total expenses: {total:F2}");
    }
    else if (choice == "4")
    {
    Console.Write("Enter the expense ID to delete: ");
    string? idInput = Console.ReadLine();

    if (Guid.TryParse(idInput, out Guid id))
    {
        bool deleted = expenseService.DeleteExpense(id);

        if (deleted)
        {
            Console.WriteLine("Expense deleted.");
        }
        else
        {
            Console.WriteLine("Expense not found!");
        }
    }
    else
    {
        Console.WriteLine("Please enter a valid expense ID.");
    }
    }
   
    else if (choice == "5")
    {
        Console.WriteLine("Goodbye");
        break;
    }
    else
    {
        Console.WriteLine("Invalid option");
    }
}


