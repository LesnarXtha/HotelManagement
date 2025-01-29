using HotelManagement.Data;
using HotelManagement.Models;
using HotelManagement.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HotelManagement.Controllers
{
    public class AccessController : Controller
    {
        private readonly HotelDbContext _context;

        public AccessController(HotelDbContext context)
        {
            _context = context;
        }

        // Display login form (GET)
        public IActionResult Login()
        {
            return View();
        }

        // Handle login logic (POST)
        [HttpPost]
        public async Task<IActionResult> Login(VMlogin model)
        {
            // Validate form data
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Retrieve user by email from database
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.email);

            // Check if user is found and if the password matches
            if (user == null || user.Password != model.password)
            {
                ViewData["ValidateMessage"] = "Invalid email or password. Please try again.";
                return View(model);
            }

            // Determine role (Admin or User) based on email
            string role = "User"; // Default role
            if (user.Email == "admin@hotel.com") // Check if the email belongs to the admin
            {
                role = "Admin";
            }

            // Create claims for the logged-in user based on email and role
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Email),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, role) // Add the role to the claims
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
            };

            // Sign in the user using cookies
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

            // Redirect to the appropriate dashboard based on the role
            if (role == "Admin")
            {
                return RedirectToAction("AdminIndex", "Home"); // Admin dashboard
            }
            else
            {
                return RedirectToAction("Index", "Home"); // User dashboard
            }
        }

        // Logout logic
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Access");
        }
    }
}
