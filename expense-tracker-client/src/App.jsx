import { useEffect, useState } from "react";
import "./App.css";

const API_URL = "https://expense-tracker-api-shz5.onrender.com";

function App() {
  const [expenses, setExpenses] = useState([]);
  const [description, setDescription] = useState("");
  const [amount, setAmount] = useState("");
  const [category, setCategory] = useState("");
  const [searchTerm, setSearchTerm] = useState("");
  const [categoryFilter, setCategoryFilter] = useState("All");
  const [editingId, setEditingId] = useState(null);
  const [deleteId, setDeleteId] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [formError, setFormError] = useState("");

  const total = expenses.reduce((sum, expense) => {
    return sum + expense.amount;
  }, 0);

  const transactionCount = expenses.length;

  const averageExpense = transactionCount === 0 ? 0 : total / transactionCount;

  // Filter expenses
  const filteredExpenses = expenses.filter((expense) => {
    const matchesSearch = expense.description
      .toLowerCase()
      .includes(searchTerm.toLowerCase());

    const matchesCategory =
      categoryFilter === "All" || expense.category === categoryFilter;

    return matchesSearch && matchesCategory;
  });

  // Calculate spending by category
  const categoryTotals = expenses.reduce((totals, expense) => {
    if (!totals[expense.category]) {
      totals[expense.category] = 0;
    }

    totals[expense.category] += expense.amount;

    return totals;
  }, {});

  // Load expenses when the application starts
  useEffect(() => {
    loadExpenses();
  }, []);

  function loadExpenses() {
    fetch(`${API_URL}/api/expenses`)
      .then((response) => {
        if (!response.ok) {
          throw new Error("Failed to load expenses");
        }

        return response.json();
      })
      .then((data) => {
        setExpenses(data);
        setLoading(false);
      })
      .catch((error) => {
        console.error(error);
        setError("Unable to load expenses.");
        setLoading(false);
      });
  }

  // Add or update expense
  function handleSubmit(event) {
    event.preventDefault();
    setFormError("");

    if (!description.trim()) {
      setFormError("Please enter an expense description.");
      return;
    }

    if (!amount || Number(amount) <= 0) {
      setFormError("Please enter an amount greater than zero.");
      return;
    }

    if (!category) {
      setFormError("Please select an expense category.");
      return;
    }

    const expenseData = {
      description: description.trim(),
      amount: Number(amount),
      category: category,
    };

    const url = editingId
      ? `${API_URL}/api/expenses/${editingId}`
      : `${API_URL}/api/expenses`;

    const method = editingId ? "PUT" : "POST";

    fetch(url, {
      method: method,
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(expenseData),
    })
      .then((response) => {
        if (!response.ok) {
          throw new Error(
            editingId ? "Failed to update expense" : "Failed to add expense",
          );
        }

        return fetch(`${API_URL}/api/expenses`);
      })
      .then((response) => response.json())
      .then((data) => {
        setExpenses(data);
        setDescription("");
        setAmount("");
        setCategory("");
        setEditingId(null);
        setFormError("");
      })
      .catch((error) => {
        console.error(error);

        setFormError(
          editingId ? "Unable to update expense." : "Unable to add expense.",
        );
      });
  }

  // Start editing an expense
  function handleEdit(expense) {
    setEditingId(expense.id);
    setDescription(expense.description);
    setAmount(expense.amount);
    setCategory(expense.category);
    setFormError("");
  }

  // Cancel editing
  function handleCancelEdit() {
    setEditingId(null);
    setDescription("");
    setAmount("");
    setCategory("");
    setFormError("");
  }

  // Open delete confirmation modal
  function handleDelete(id) {
    setDeleteId(id);
  }

  // Confirm and delete expense
  function confirmDelete() {
    fetch(`${API_URL}/api/expenses/${deleteId}`, {
      method: "DELETE",
    })
      .then((response) => {
        if (!response.ok) {
          throw new Error("Failed to delete expense");
        }

        return fetch(`${API_URL}/api/expenses`);
      })
      .then((response) => response.json())
      .then((data) => {
        setExpenses(data);
        setDeleteId(null);
      })
      .catch((error) => {
        console.error(error);
        setError("Unable to delete expense.");
        setDeleteId(null);
      });
  }

  return (
    <div className="app">
      {/* HEADER */}
      <header className="header">
        <div className="brand">
          <div className="brand-mark">A</div>

          <div>
            <h1>Expense Tracker</h1>
            <p>Personal financial management</p>
          </div>
        </div>

        <div className="header-status">
          <span className="status-dot"></span>
          <span>Secure Dashboard</span>
        </div>
      </header>

      <main className="container">
        {/* WELCOME */}
        <section className="welcome">
          <div>
            <p className="eyebrow">FINANCIAL OVERVIEW</p>

            <h2>Manage your expenses with confidence.</h2>

            <p className="welcome-text">
              Keep track of your spending and maintain better control over your
              finances.
            </p>
          </div>
        </section>

        {/* METRICS */}
        <section className="metrics-grid">
          {/* TOTAL */}
          <div className="metric-card primary-metric">
            <div>
              <p className="metric-label">TOTAL SPENDING</p>

              <h2>GH₵ {total.toFixed(2)}</h2>

              <p className="metric-description">Total recorded spending</p>
            </div>

            <div className="metric-icon">₵</div>
          </div>

          {/* TRANSACTIONS */}
          <div className="metric-card">
            <div>
              <p className="metric-label">TRANSACTIONS</p>

              <h2>{transactionCount}</h2>

              <p className="metric-description">Recorded expenses</p>
            </div>

            <div className="metric-icon light-icon">#</div>
          </div>

          {/* AVERAGE */}
          <div className="metric-card">
            <div>
              <p className="metric-label">AVERAGE EXPENSE</p>

              <h2>GH₵ {averageExpense.toFixed(2)}</h2>

              <p className="metric-description">Average per transaction</p>
            </div>

            <div className="metric-icon light-icon">≈</div>
          </div>
        </section>

        {/* CATEGORY BREAKDOWN */}
        <section className="category-card">
          <div className="card-header">
            <div>
              <p className="eyebrow">SPENDING ANALYSIS</p>
              <h2>Spending by Category</h2>
            </div>
          </div>

          <div className="category-list">
            {Object.entries(categoryTotals).map(([category, amount]) => {
              const percentage = total === 0 ? 0 : (amount / total) * 100;

              return (
                <div className="category-item" key={category}>
                  <div className="category-header">
                    <span>{category}</span>

                    <strong>GH₵ {amount.toFixed(2)}</strong>
                  </div>

                  <div className="category-bar">
                    <div
                      className="category-progress"
                      style={{
                        width: `${percentage}%`,
                      }}
                    ></div>
                  </div>

                  <p>{percentage.toFixed(1)}% of total spending</p>
                </div>
              );
            })}
          </div>
        </section>

        {/* ERROR MESSAGE */}
        {error && <div className="error-message">{error}</div>}

        {/* MAIN DASHBOARD */}
        <section className="dashboard">
          {/* ADD / EDIT EXPENSE */}
          <div className="card add-expense-card">
            <div className="card-header">
              <div>
                <p className="eyebrow">TRANSACTION</p>

                <h2>{editingId ? "Edit Expense" : "Add Expense"}</h2>
              </div>

              <div className="card-icon">{editingId ? "✎" : "+"}</div>
            </div>

            <form onSubmit={handleSubmit}>
              {/* DESCRIPTION */}
              <div className="form-group">
                <label>Description</label>

                <input
                  type="text"
                  placeholder="e.g. Lunch"
                  value={description}
                  onChange={(event) => setDescription(event.target.value)}
                  required
                />
              </div>

              {/* AMOUNT */}
              <div className="form-group">
                <label>Amount</label>

                <div className="amount-input">
                  <span>GH₵</span>

                  <input
                    type="number"
                    placeholder="0.00"
                    value={amount}
                    onChange={(event) => setAmount(event.target.value)}
                    min="0"
                    step="0.01"
                    required
                  />
                </div>
              </div>

              {/* CATEGORY */}
              <div className="form-group">
                <label>Category</label>

                <select
                  value={category}
                  onChange={(event) => setCategory(event.target.value)}
                  required
                >
                  <option value="">Select category</option>
                  <option value="Food">Food</option>
                  <option value="Transport">Transport</option>
                  <option value="Shopping">Shopping</option>
                  <option value="Bills">Bills</option>
                  <option value="Entertainment">Entertainment</option>
                  <option value="Health">Health</option>
                  <option value="Other">Other</option>
                </select>
              </div>

              {/* FORM ERROR */}
              {formError && <div className="form-error">{formError}</div>}

              {/* SUBMIT */}
              <button className="primary-button" type="submit">
                <span>{editingId ? "✓" : "+"}</span>

                {editingId ? "Update Expense" : "Add Expense"}
              </button>

              {/* CANCEL EDIT */}
              {editingId && (
                <button
                  type="button"
                  className="cancel-button"
                  onClick={handleCancelEdit}
                >
                  Cancel
                </button>
              )}
            </form>
          </div>

          {/* RECENT EXPENSES */}
          <div className="card expenses-card">
            <div className="card-header">
              <div>
                <p className="eyebrow">ACTIVITY</p>
                <h2>Recent Expenses</h2>
              </div>

              <div className="expense-count">{filteredExpenses.length}</div>
            </div>

            {/* SEARCH + FILTER */}
            <div className="filters">
              <input
                type="text"
                placeholder="Search expenses..."
                value={searchTerm}
                onChange={(event) => setSearchTerm(event.target.value)}
              />

              <select
                value={categoryFilter}
                onChange={(event) => setCategoryFilter(event.target.value)}
              >
                <option value="All">All Categories</option>
                <option value="Food">Food</option>
                <option value="Transport">Transport</option>
                <option value="Shopping">Shopping</option>
                <option value="Bills">Bills</option>
                <option value="Entertainment">Entertainment</option>
                <option value="Health">Health</option>
                <option value="Other">Other</option>
              </select>
            </div>

            {/* LOADING */}
            {loading ? (
              <div className="empty-state">
                <p>Loading expenses...</p>
              </div>
            ) : expenses.length === 0 ? (
              /* NO EXPENSES */
              <div className="empty-state">
                <div className="empty-icon">₵</div>

                <h3>No expenses yet</h3>

                <p>Add your first expense to start tracking your spending.</p>
              </div>
            ) : filteredExpenses.length === 0 ? (
              /* NO SEARCH RESULTS */
              <div className="empty-state">
                <div className="empty-icon">🔍</div>

                <h3>No matching expenses</h3>

                <p>Try a different search or category.</p>
              </div>
            ) : (
              /* EXPENSE LIST */
              <div className="expense-list">
                {filteredExpenses.map((expense) => (
                  <div className="expense-item" key={expense.id}>
                    <div className="expense-icon">₵</div>

                    <div className="expense-details">
                      <h3>{expense.description}</h3>

                      <p>
                        {expense.category} •{" "}
                        {new Date(expense.createdAt).toLocaleDateString()}
                      </p>
                    </div>

                    <div className="expense-right">
                      <strong>GH₵ {expense.amount.toFixed(2)}</strong>

                      <div className="expense-actions">
                        <button
                          className="edit-button"
                          onClick={() => handleEdit(expense)}
                        >
                          Edit
                        </button>

                        <button
                          className="delete-button"
                          onClick={() => handleDelete(expense.id)}
                        >
                          Delete
                        </button>
                      </div>
                    </div>
                  </div>
                ))}
              </div>
            )}
          </div>
        </section>
      </main>

      {/* DELETE CONFIRMATION MODAL */}
      {deleteId && (
        <div className="modal-overlay">
          <div className="delete-modal">
            <div className="delete-modal-icon">!</div>

            <h2>Delete Expense?</h2>

            <p>
              Are you sure you want to delete this expense? This action cannot
              be undone.
            </p>

            <div className="modal-actions">
              <button
                className="cancel-delete-button"
                onClick={() => setDeleteId(null)}
              >
                Cancel
              </button>

              <button className="confirm-delete-button" onClick={confirmDelete}>
                Delete Expense
              </button>
            </div>
          </div>
        </div>
      )}

      {/* FOOTER */}
      <footer>
        <p>Expense Tracker • Built with React & ASP.NET Core</p>
      </footer>
    </div>
  );
}

export default App;
