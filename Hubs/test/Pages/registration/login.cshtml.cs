using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Cryptography;
using System.Text;
using RolsaTechnologies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace RolsaTechnologies.Pages;
public class LoginModel : PageModel 
{
    // Variables for login 
    [BindProperty]
    public string? Username { get; set; }
    [BindProperty]
    public string? Password { get; set; }

    private readonly RolsaTechnologiesContext _context;
    public LoginModel(RolsaTechnologiesContext context)
    {
        _context = context;
    }


    public IActionResult OnGet()
    {
        // Check if the user is logged in
        if (User.Identity?.IsAuthenticated ?? true)
        {
            // Redirect to the index page if logged in
            return RedirectToPage("/index");
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (ValidateUser(Username, Password))
        {
            var customer = _context.Customer.SingleOrDefault(c => c.Username == Username);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, customer.CID.ToString()),
                new Claim(ClaimTypes.Name, Username ?? string.Empty)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Sign in the customer with the claims
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return RedirectToPage("/Index");
        }

        // If we got this far, something failed; redisplay form
        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return Page();

    }

    private bool ValidateUser (string? Username, string? Password)
    {
        // Making sure that the inputs are not empty 
        if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
        {
            return false;
        }

        // Checks if the user exists
        var customer = _context.Customer.SingleOrDefault(c => c.Username == Username || c.Email == Username);
        if (customer == null)
        {
            return false;
        }

        var hashedPassword = HashPassword(Password, customer.Salt);
        return hashedPassword == customer.HashedPassword;
    }

    private string HashPassword(string password, string salt)
    {
        // Applies salt to the password and then uses SHA256 to hash the password
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
    }
}