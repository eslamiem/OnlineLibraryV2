using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Data;
using OnlineLibrary.Models;

namespace OnlineLibrary.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Book
        public async Task<IActionResult> Index()
        {
            TempData["PastDueDate"] = false;
            TempData["PastDueDateBookTitles"] = "";

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var pastDueDateList = await _context.BorrowTransactions
                .Include(b => b.Book)
                .Where(bt => bt.UserId == userId
                    && bt.EndDate < DateTime.Now
                    && bt.State == BorrowTransaction.BorrowingState.InProgrees)
                .ToListAsync();

            if (pastDueDateList.Any())
            {
                TempData["PastDueDate"] = true;
                // Extract book titles from pastDueDateTransactions
                var bookTitles = pastDueDateList.Select(bt => bt.Book.Title).ToList();

                // Construct the message with newline characters
                var bookTitlesNewlines = string.Join("\n\r", bookTitles);

                // Ensure that TempData["PastDueDateBookTitles"] is properly encoded before passing it to the view
                var encodedBookTitles = System.Web.HttpUtility.JavaScriptStringEncode(bookTitlesNewlines);
                TempData["PastDueDateBookTitles"] = encodedBookTitles;

            }

            return View(await _context.Books.ToListAsync());
        }

        // GET: Book/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.CodeNumber == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Book/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Book/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CodeNumber,Author,Title,YearPublished,Quantity,Available")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Book/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Book/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CodeNumber,Author,Title,YearPublished,Quantity,Available")] Book book)
        {
            if (id != book.CodeNumber)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.CodeNumber))
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
            return View(book);
        }

        // GET: Book/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.CodeNumber == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.CodeNumber == id);
        }


        // GET: Books/Borrow
        [Authorize]
        public async Task<IActionResult> Borrow(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Book? book = await _context.Books.FirstOrDefaultAsync(m => m.CodeNumber == id);

            if (book == null || userId == null || book.Available == Book.Availability.NotAvailable)
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
                CodeNumber = book!.CodeNumber,
                UserId = userId,
            };
            if (book?.Available == Book.Availability.NotAvailable)
            {
                //TODO: Add view
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                book!.Quantity--;
                if (book!.Quantity == 0)
                {
                    book!.IsNotAvailable();
                }
                _context.Update(book);
                await _context.SaveChangesAsync();

                _context.Add(borrowTransaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nameof(Index));

            // if (id == null)
            // {
            //     return NotFound();
            // }
            // return RedirectToAction("Borrow", "BorrowTransaction",
            //     new
            //     {
            //         Id = id.Value,
            //     });
        }

    }
}
