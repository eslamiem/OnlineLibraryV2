using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Data;
using OnlineLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace OnlineLibrary.Controllers
{
    public class BorrowTransactionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BorrowTransactionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BorrowTransaction
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var applicationDbContext = _context.BorrowTransactions
                                            .Include(b => b.Book)
                                            .Where(b => b.UserId == userId);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: BorrowTransaction/Return/5
        public async Task<IActionResult> Return(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var borrowTransaction = await _context.BorrowTransactions.FindAsync(id);
                if (borrowTransaction == null)
                {
                    return NotFound();
                }
                try
                {
                    borrowTransaction.EndBorrowing();
                    _context.Update(borrowTransaction);
                    await _context.SaveChangesAsync();

                    var book = await _context.Books.FindAsync(borrowTransaction.CodeNumber);
                    if (book != null)
                    {
                        book.Quantity++;
                        book.IsAvailable();
                        _context.Books.Update(book);
                    }

                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BorrowTransactionExists(borrowTransaction.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

            }
            return RedirectToAction(nameof(Index));
        }

        private bool BorrowTransactionExists(int id)
        {
            return _context.BorrowTransactions.Any(e => e.Id == id);
        }

        // GET: Borrowings/Borrow
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Borrow(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            CustomUser? customUser = _context.Users.Find(ClaimTypes.NameIdentifier);

            Book? book = await _context.Books.FirstOrDefaultAsync(m => m.CodeNumber == id);
            if (book == null || customUser == null || book.Available == Book.Availability.NotAvailable)
            {
                return NotFound();
            }
            if (book?.Quantity == 0)
            {
                return NotFound();
            }
            BorrowTransaction borrowTransaction = new BorrowTransaction
            {
                Book = book!,
                Borrower = customUser,
            };
            if (book?.Available == Book.Availability.NotAvailable)
            {
                //TODO: Add view
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Add(borrowTransaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nameof(Index));
        }
    }

}
