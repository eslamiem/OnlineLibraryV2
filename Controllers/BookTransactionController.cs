using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Data;
using OnlineLibrary.Models;

namespace OnlineLibrary.Controllers
{
    public class BookTransactionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookTransactionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BookTransaction
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BookTransactions.Include(b => b.Book).Include(b => b.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: BookTransaction/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookTransaction = await _context.BookTransactions
                .Include(b => b.Book)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookTransaction == null)
            {
                return NotFound();
            }

            return View(bookTransaction);
        }

        // GET: BookTransaction/Create
        public IActionResult Create()
        {
            ViewData["BookCodeNumber"] = new SelectList(_context.Books, "CodeNumber", "Author");
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id");
            return View();
        }

        // POST: BookTransaction/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BookCodeNumber,UserId,DueDate,Returned")] BookTransaction bookTransaction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bookTransaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookCodeNumber"] = new SelectList(_context.Books, "CodeNumber", "Author", bookTransaction.BookCodeNumber);
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", bookTransaction.UserId);
            return View(bookTransaction);
        }

        // GET: BookTransaction/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookTransaction = await _context.BookTransactions.FindAsync(id);
            if (bookTransaction == null)
            {
                return NotFound();
            }
            ViewData["BookCodeNumber"] = new SelectList(_context.Books, "CodeNumber", "Author", bookTransaction.BookCodeNumber);
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", bookTransaction.UserId);
            return View(bookTransaction);
        }

        // POST: BookTransaction/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BookCodeNumber,UserId,DueDate,Returned")] BookTransaction bookTransaction)
        {
            if (id != bookTransaction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookTransaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookTransactionExists(bookTransaction.Id))
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
            ViewData["BookCodeNumber"] = new SelectList(_context.Books, "CodeNumber", "Author", bookTransaction.BookCodeNumber);
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", bookTransaction.UserId);
            return View(bookTransaction);
        }

        // GET: BookTransaction/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookTransaction = await _context.BookTransactions
                .Include(b => b.Book)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookTransaction == null)
            {
                return NotFound();
            }

            return View(bookTransaction);
        }

        // POST: BookTransaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bookTransaction = await _context.BookTransactions.FindAsync(id);
            if (bookTransaction != null)
            {
                _context.BookTransactions.Remove(bookTransaction);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookTransactionExists(int id)
        {
            return _context.BookTransactions.Any(e => e.Id == id);
        }
    }
}
