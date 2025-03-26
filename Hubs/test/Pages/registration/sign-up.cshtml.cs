using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using RolsaTechnologies;
using System.Threading.Tasks;

namespace RolsaTechnologies.Pages;

public class SignUpModel : PageModel
{
    [BindProperty]
    public string? Username { get; set; }

    [BindProperty]
    public string? Password { get; set; }

    [BindProperty]
    public string? ReEnterPassword { get; set; }

    private readonly RolsaTechnologiesContext _context;

    public SignUpModel(RolsaTechnologiesContext context)
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

        if (Username.Length > 15 || Username.Length < 3)
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

        // Check if username already exists
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
