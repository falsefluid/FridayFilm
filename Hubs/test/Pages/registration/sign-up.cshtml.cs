using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using RolsaTechnologies;
using System.Threading.Tasks;

namespace RolsaTechnologies.Pages;

public class SignUpModel : PageModel
{
    // Variables for sign up
    [BindProperty]
    public string? Username {get;set;}
    [BindProperty]
    public string? Password {get;set;}
    [BindProperty]
    public string? ReEnterPassword {get;set;}



    private readonly RolsaTechnologiesContext _context;

    public SignUpModel(RolsaTechnologiesContext context)
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
        // Validation for sign up
        if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(ReEnterPassword))
        {
            ModelState.AddModelError(string.Empty, "All fields are required");
            return Page();
        }

        if (Username.Length < 3 || Username.Length > 15)
        {
            ModelState.AddModelError(string.Empty, "Username must be between 3 and 15 characters long.");
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

        var existingUser = await _context.Customer.SingleOrDefaultAsync(c => c.Username == Username);
        if (existingUser != null)
        {
            ModelState.AddModelError(string.Empty, "Username already exists.");
            return Page();
        }

        var salt = GenerateSalt();
        var hashedPassword = HashPassword(Password, salt);

        var newCustomer = new Customer
        {
            Username = Username,
            HashedPassword = hashedPassword,
            Salt = salt,
            CreateDate = DateTime.UtcNow
        };

        _context.Customer.Add(newCustomer);
        await _context.SaveChangesAsync();

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

    public string GenerateSalt()
    {
        // Creating a salt to apply to user's password
        var saltBytes = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(saltBytes);
        }
        return Convert.ToBase64String(saltBytes);
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