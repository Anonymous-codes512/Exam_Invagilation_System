using Exam_Invagilation_System.Entities;
using Exam_Invagilation_System.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Exam_Invagilation_System.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _db;
        public AuthController(AppDbContext db)
        {
            _db = db;
        }
        // GET: Login Page
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Login Form
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _db.UserAccounts.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null || user.Password != password) // Insecure plain check
            {
                TempData["error"] = "Invalid email or password.";
                return View();
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Name),
                new(ClaimTypes.Email, user.Email),
                new("UserId", user.Id.ToString())
            };

            var identity = new ClaimsIdentity(claims, "MyCookieAuth");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("MyCookieAuth", principal);
            TempData["success"] = "Login Successfully.";

            return RedirectToAction("Dashboard", "Home"); // or any protected page
        }


        // GET: Signup Page
        //[HttpGet]
        public IActionResult Signup()
        {
            return View();
        }

        // POST: Signup Form
        [HttpPost]
        public async Task<IActionResult> Signup(string email, string password, string name)
        {
            var existing = await _db.UserAccounts.FirstOrDefaultAsync(u => u.Email == email);
            if (existing != null)
            {
                TempData["error"] = "Email already registered.";

                return View();
            }

            Console.WriteLine(email);

            var user = new UserAccount
            {
                Email = email,
                Password = password, // ❌ plain password — replace with hashing later
                Name = name
            };

            _db.UserAccounts.Add(user);
            await _db.SaveChangesAsync();
            TempData["success"] = "Signup Successfully.";

            return RedirectToAction("Login");
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            return RedirectToAction("Login");
        }



        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult NotFound()
        {
            return View();
        }
    }
}
