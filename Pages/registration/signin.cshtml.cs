using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Cryptography;
using System.Text;
using FridayFilmClub;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FridayFilmClub.Pages;

public class SignInModel : PageModel
{
    [BindProperty]
    public string? Username { get; set; }

    [BindProperty]
    public string? Password { get; set; }

    [BindProperty]
    public string? ReEnterPassword { get; set; }

    private readonly FilmClubContext _context;

    public SignInModel(FilmClubContext context)
    {
        _context = context;
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(ReEnterPassword))
        {
            ModelState.AddModelError(string.Empty, "All fields are required.");
            return Page();
        }

        if (Username.Length > 15)
        {
            ModelState.AddModelError(string.Empty, "Username cannot be more than 15 characters long.");
            return Page();
        }

        if (Password.Length < 8)
        {
            ModelState.AddModelError(string.Empty, "Password must be at least 8 characters long.");
            return Page();
        }

        if (Password != ReEnterPassword)
        {
            ModelState.AddModelError(string.Empty, "Passwords do not match.");
            return Page();
        }

        // Check if the username already exists
        var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.Username == Username);
        if (existingUser != null)
        {
            ModelState.AddModelError(string.Empty, "Username already exists.");
            return Page();
        }

        var salt = GenerateSalt();
        var hashedPassword = HashPassword(Password, salt);

        var newUser = new Users
        {
            Username = Username,
            PasswordHash = hashedPassword,
            Salt = salt,
            Bio = string.Empty,
            Picture = "/images/default-profile.png"
        };

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        return RedirectToPage("/registration/login");
    }

    private string GenerateSalt()
    {
        var saltBytes = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(saltBytes);
        }
        return Convert.ToBase64String(saltBytes);
    }

    private string HashPassword(string password, string salt)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
    }
}