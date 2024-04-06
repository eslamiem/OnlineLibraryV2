using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Data;
using OnlineLibrary.Models;

namespace OnlineLibrary.Services;
public class BookService
{
    private ApplicationDbContext _context;
    public BookService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<List<BorrowTransaction>> GetPastDueBookListAsync()
    {
        var pastDueDateBooks = _context.BorrowTransactions
                                    .Include(b => b.Book)
                                    .Include(b => b.Borrower)
                                    .Where(bt => bt.EndDate < DateTime.Now.Date);
        return await pastDueDateBooks.ToListAsync();
    }
    public async Task<BorrowTransaction?> GetTransactionByIdAsync(int id)
    {
        return await _context.BorrowTransactions.FindAsync(id) ?? null;
    }

    public async Task<BorrowTransaction> UpdateDueDateAsync(int id, BorrowTransaction b)
    {
        var borrowTransaction = await _context.BorrowTransactions!.FindAsync(id);

        if (borrowTransaction == null)
            return null!;

        borrowTransaction.EndDate = b.EndDate;

        _context.BorrowTransactions.Update(borrowTransaction);
        await _context.SaveChangesAsync();

        return borrowTransaction!;
    }
}
