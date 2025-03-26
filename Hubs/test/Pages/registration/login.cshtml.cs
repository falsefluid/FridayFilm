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
    [BindProperty]
    public string? Username { get; set; }

    [BindProperty]
    public string? Password { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? ReturnUrl { get; set; }
    
    private readonly RolsaTechnologiesContext _context;

    public LoginModel(RolsaTechnologiesContext context)
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
            // Get the customer from the database
            var customer = _context.Customer.SingleOrDefault(c => c.Username == Username);
            
            if (customer == null)
            {
                // If customer doesn't exist, return a failed login attempt
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }

            // Create a list of claims including NameIdentifier and Name
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, customer.CID.ToString()),
                new Claim(ClaimTypes.Name, Username ?? string.Empty)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Sign in the customer with the claims
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

        var customer = _context.Customer.SingleOrDefault(c => c.Username == username);
        if (customer == null)
        {
            return false;
        }

        var hashedPassword = HashPassword(password, customer.Salt);
        return hashedPassword == customer.HashedPassword;
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
