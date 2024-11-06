using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebsiteApi.Context;
using WebsiteApi.Models;

namespace WebsiteApi.Services
{
    /// <summary>
    /// Service for managing expenses.
    /// </summary>
    public class ExpenseService
    {
        private readonly DataContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpenseService"/> class.
        /// </summary>
        /// <param name="context">The data context.</param>
        public ExpenseService(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves an expense by its ID.
        /// </summary>
        /// <param name="id">The ID of the expense.</param>
        /// <returns>The expense if found; otherwise, null.</returns>
        public async Task<Expense?> GetExpenseByIdAsync(int id)
        {
            return await _context.Expenses
                                 .Include(e => e.Job)
                                 .FirstOrDefaultAsync(e => e.ExpenseId == id);
        }

        /// <summary>
        /// Retrieves all expenses associated with a specific job.
        /// </summary>
        /// <param name="jobId">The ID of the job.</param>
        /// <returns>A list of expenses.</returns>
        public async Task<IEnumerable<Expense>> GetExpensesByJobIdAsync(int jobId)
        {
            return await _context.Expenses
                                 .Where(e => e.JobId == jobId)
                                 .ToListAsync();
        }

        /// <summary>
        /// Creates a new expense.
        /// </summary>
        /// <param name="expense">The expense to create.</param>
        /// <returns>The created expense.</returns>
        /// <exception cref="ArgumentException">Thrown when the associated job does not exist.</exception>
        public async Task<Expense> CreateExpenseAsync(Expense expense)
        {
            // Validate that the associated job exists
            var jobExists = await _context.Jobs.AnyAsync(j => j.JobId == expense.JobId);
            if (!jobExists)
            {
                throw new ArgumentException($"Job with ID {expense.JobId} does not exist.");
            }

            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
            return expense;
        }

        /// <summary>
        /// Updates an existing expense.
        /// </summary>
        /// <param name="expense">The expense with updated information.</param>
        /// <returns>True if the update was successful; otherwise, false.</returns>
        /// <exception cref="ArgumentException">Thrown when the associated job does not exist.</exception>
        public async Task<bool> UpdateExpenseAsync(Expense expense)
        {
            var existingExpense = await _context.Expenses.FindAsync(expense.ExpenseId);
            if (existingExpense == null)
            {
                return false;
            }

            // Update fields
            existingExpense.Description = expense.Description;
            existingExpense.Amount = expense.Amount;
            existingExpense.Date = expense.Date;
            existingExpense.JobId = expense.JobId;

            // Optional: Validate that the new JobId exists
            var jobExists = await _context.Jobs.AnyAsync(j => j.JobId == expense.JobId);
            if (!jobExists)
            {
                throw new ArgumentException($"Job with ID {expense.JobId} does not exist.");
            }

            _context.Expenses.Update(existingExpense);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Deletes an expense by its ID.
        /// </summary>
        /// <param name="id">The ID of the expense to delete.</param>
        /// <returns>True if the deletion was successful; otherwise, false.</returns>
        public async Task<bool> DeleteExpenseAsync(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
            {
                return false;
            }

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}