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
public class LoginModel : PageModel
{
    [BindProperty]
    public string? Username { get; set; }

    [BindProperty]
    public string? Password { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? ReturnUrl { get; set; }
    
    private readonly FilmClubContext _context;

    public LoginModel(FilmClubContext context)
    {
        _context = context;
    }
    

    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (ValidateUser(Username, Password))
        {
            // Get the user from the database
            var user = _context.Users.SingleOrDefault(u => u.Username == Username);
            
            if (user == null)
            {
                // If user doesn't exist, return a failed login attempt
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }

            // Create a list of claims including NameIdentifier and Name
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                new Claim(ClaimTypes.Name, Username ?? string.Empty)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Sign in the user with the claims
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
            {
                return Redirect(ReturnUrl);
            }

            return RedirectToPage("/Index");
        }

        // If we got this far, something failed; redisplay form
        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return Page();
    }
    private bool ValidateUser(string? username, string? password)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            return false;
        }

        var user = _context.Users.SingleOrDefault(u => u.Username == username);
        if (user == null)
        {
            return false;
        }

        var hashedPassword = HashPassword(password, user.Salt);
        return hashedPassword == user.PasswordHash;
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