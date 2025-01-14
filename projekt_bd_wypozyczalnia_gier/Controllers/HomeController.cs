using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projekt_bd_wypozyczalnia_gier.Models;

namespace projekt_bd_wypozyczalnia_gier.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetString("UserId");
            var userRole = HttpContext.Session.GetString("UserRole");

            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(userRole))
            {
                ApplicationUser user = await _userManager.FindByIdAsync(userId);

                if (userRole == "Admin")
                {
                    return RedirectToAction("Index", "Game");
                }
            }
            var gamesWithoutUser = await _context.Games.ToListAsync();
            return View(gamesWithoutUser);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
