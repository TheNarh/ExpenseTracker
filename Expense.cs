public class Expense
{
    public Guid Id { get; private set; }

    public string? Description { get; private set; }

    public decimal Amount { get; private set; }

    public string? Category { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public Expense(
        string description,
        decimal amount,
        string category)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException(
                "Expense description cannot be empty."
            );
        }

        if (amount <= 0)
        {
            throw new ArgumentException(
                "Expense amount must be greater than zero."
            );
        }

        if (string.IsNullOrWhiteSpace(category))
        {
            throw new ArgumentException(
                "Expense category cannot be empty."
            );
        }

        Id = Guid.NewGuid();

        Description = description;

        Amount = amount;

        Category = category;

        CreatedAt = DateTime.UtcNow;
    }

    public void Update(
        string description,
        decimal amount,
        string category)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException(
                "Expense description cannot be empty."
            );
        }

        if (amount <= 0)
        {
            throw new ArgumentException(
                "Expense amount must be greater than zero."
            );
        }

        if (string.IsNullOrWhiteSpace(category))
        {
            throw new ArgumentException(
                "Expense category cannot be empty."
            );
        }

        Description = description;

        Amount = amount;

        Category = category;
    }
}