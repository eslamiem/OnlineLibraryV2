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
            var applicationDbContext = _context.BorrowTransactions.Include(b => b.Book).Include(b => b.Borrower);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: BorrowTransaction/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrowTransaction = await _context.BorrowTransactions
                .Include(b => b.Book)
                .Include(b => b.Borrower)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (borrowTransaction == null)
            {
                return NotFound();
            }

            return View(borrowTransaction);
        }

        // GET: BorrowTransaction/Create
        public IActionResult Create()
        {
            ViewData["CodeNumber"] = new SelectList(_context.Books, "CodeNumber", "Author");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: BorrowTransaction/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RentalDate,EndDate,State,CodeNumber,UserId")] BorrowTransaction borrowTransaction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(borrowTransaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CodeNumber"] = new SelectList(_context.Books, "CodeNumber", "Author", borrowTransaction.CodeNumber);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", borrowTransaction.UserId);
            return View(borrowTransaction);
        }

        // GET: BorrowTransaction/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrowTransaction = await _context.BorrowTransactions.FindAsync(id);
            if (borrowTransaction == null)
            {
                return NotFound();
            }
            ViewData["CodeNumber"] = new SelectList(_context.Books, "CodeNumber", "Author", borrowTransaction.CodeNumber);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", borrowTransaction.UserId);
            return View(borrowTransaction);
        }

        // POST: BorrowTransaction/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RentalDate,EndDate,State,CodeNumber,UserId")] BorrowTransaction borrowTransaction)
        {
            if (id != borrowTransaction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(borrowTransaction);
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["CodeNumber"] = new SelectList(_context.Books, "CodeNumber", "Author", borrowTransaction.CodeNumber);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", borrowTransaction.UserId);
            return View(borrowTransaction);
        }

        // GET: BorrowTransaction/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrowTransaction = await _context.BorrowTransactions
                .Include(b => b.Book)
                .Include(b => b.Borrower)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (borrowTransaction == null)
            {
                return NotFound();
            }

            return View(borrowTransaction);
        }

        // POST: BorrowTransaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var borrowTransaction = await _context.BorrowTransactions.FindAsync(id);
            if (borrowTransaction != null)
            {
                _context.BorrowTransactions.Remove(borrowTransaction);
            }

            await _context.SaveChangesAsync();
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
            if(book == null || customUser == null || book.Available == Book.Availability.NotAvailable)
            {
                return NotFound();
            }
            if(book?.Quantity == 0)
            {
                return NotFound();
            }
            BorrowTransaction borrowTransaction = new BorrowTransaction     
            {
                Book = book!,
                Borrower = customUser,
            };
            if(book?.Available == Book.Availability.NotAvailable)
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
