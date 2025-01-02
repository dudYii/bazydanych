using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using projekt_bd_wypozyczalnia_gier.Models;

namespace projekt_bd_wypozyczalnia_gier.Controllers
{
    public class RentalController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RentalController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var rentals = _context.Rentals.Include(r => r.Customer).Include(r => r.Game);
            return View(await rentals.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rental = await _context.Rentals
                .Include(r => r.Customer)
                .Include(r => r.Game)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rental == null)
            {
                return NotFound();
            }

            return View(rental);
        }
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name");
            ViewData["GameId"] = new SelectList(_context.Games, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,GameId,CustomerId,RentalDate,ReturnDate")] Rental rental)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rental);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", rental.CustomerId);
            ViewData["GameId"] = new SelectList(_context.Games, "Id", "Name", rental.GameId);
            return View(rental);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rental = await _context.Rentals.FindAsync(id);
            if (rental == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", rental.CustomerId);
            ViewData["GameId"] = new SelectList(_context.Games, "Id", "Name", rental.GameId);
            return View(rental);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GameId,CustomerId,RentalDate,ReturnDate")] Rental rental)
        {
            if (id != rental.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rental);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentalExists(rental.Id))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", rental.CustomerId);
            ViewData["GameId"] = new SelectList(_context.Games, "Id", "Name", rental.GameId);
            return View(rental);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rental = await _context.Rentals
                .Include(r => r.Customer)
                .Include(r => r.Game)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rental == null)
            {
                return NotFound();
            }

            return View(rental);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rental = await _context.Rentals.FindAsync(id);
            _context.Rentals.Remove(rental);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RentalExists(int id)
        {
            return _context.Rentals.Any(e => e.Id == id);
        }
    }
}
