using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebsiteApi.Context;
using WebsiteApi.Models;

namespace WebsiteApi.Services
{
    public interface IExpenseService
    {
        Task<Expense?> GetExpenseByIdAsync(int id);
        Task<IEnumerable<Expense>> GetExpensesByJobIdAsync(int jobId);
        Task<Expense> CreateExpenseAsync(Expense expense);
        Task<bool> UpdateExpenseAsync(Expense expense);
        Task<bool> DeleteExpenseAsync(int id);
    }

    public class ExpenseService : IExpenseService
    {
        private readonly DataContext _context;

        public ExpenseService(DataContext context)
        {
            _context = context;
        }

        public async Task<Expense?> GetExpenseByIdAsync(int id)
        {
            return await _context.Expenses
                                 .Include(e => e.Job)
                                 .FirstOrDefaultAsync(e => e.ExpenseId == id);
        }


        public async Task<IEnumerable<Expense>> GetExpensesByJobIdAsync(int jobId)
        {
            return await _context.Expenses
                                 .Where(e => e.JobId == jobId)
                                 .ToListAsync();
        }

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