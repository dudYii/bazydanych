using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using projekt_bd_wypozyczalnia_gier.Models;
using System.Threading.Tasks;

namespace projekt_bd_wypozyczalnia_gier.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    var customer = new Customer
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        Address = model.Address
                    };

                    _context.Customers.Add(customer);
                    user.EmailConfirmed = true;
                    await _context.SaveChangesAsync();

                    await _signInManager.SignInAsync(user, isPersistent: true);

                    TempData["SuccessMessage"] = "Rejestracja zakończona pomyślnie!";

                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                    if (result.Succeeded)
                    {
                        HttpContext.Session.SetString("UserId", user.Id);
                        var roles = await _userManager.GetRolesAsync(user);
                        HttpContext.Session.SetString("UserRole", roles.FirstOrDefault());

                        if (roles.Contains("Admin"))
                        {
                            return RedirectToAction("Index", "Game");
                        }
                        returnUrl ??= Url.Content("~/");
                        return Redirect(returnUrl);
                    }
                }
                ModelState.AddModelError(string.Empty, "Nieprawidłowy email lub hasło.");
            }
            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}