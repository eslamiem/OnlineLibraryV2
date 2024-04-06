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
    public class CustomUsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomUsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CustomUsers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        // GET: CustomUsers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customUser = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customUser == null)
            {
                return NotFound();
            }

            return View(customUser);
        }

        // GET: CustomUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CustomUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Country,State,Street,PostalCode,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] CustomUser customUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customUser);
        }

        // GET: CustomUsers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customUser = await _context.Users.FindAsync(id);
            if (customUser == null)
            {
                return NotFound();
            }
            return View(customUser);
        }

        // POST: CustomUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("FirstName,LastName,Country,State,Street,PostalCode,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] CustomUser customUser)
        {
            if (id != customUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomUserExists(customUser.Id))
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
            return View(customUser);
        }

        // GET: CustomUsers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customUser = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customUser == null)
            {
                return NotFound();
            }

            return View(customUser);
        }

        // POST: CustomUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var customUser = await _context.Users.FindAsync(id);
            if (customUser != null)
            {
                _context.Users.Remove(customUser);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomUserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
