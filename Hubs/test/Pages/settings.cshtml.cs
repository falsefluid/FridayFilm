using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace RolsaTechnologies.Pages;

public class SettingsModel : PageModel
{
    private readonly ILogger<SettingsModel> _logger;
    private readonly RolsaTechnologiesContext _context;

    public SettingsModel(ILogger<SettingsModel> logger, RolsaTechnologiesContext context)
    {
        _logger = logger;
        _context = context;
    }

    public List<ScheduleBooking> UpcomingConsultations { get; set; } = new();
    public List<ScheduleBooking> UpcomingInstallations { get; set; } = new();

    // Add properties for user information
    public Customer? UserInfo { get; set; }

    [BindProperty]
    public string? Forename { get; set; }
    [BindProperty]
    public string? Surname { get; set; }
    [BindProperty]
    public string? Username { get; set; }
    [BindProperty]
    public string? Email { get; set; }
    [BindProperty]
    public string? PhoneNumber { get; set; }

    public async Task OnGetAsync()
    {
        var userId = User.Identity?.Name;

        if (userId != null)
        {
            // Fetch user information
            UserInfo = await _context.Customer.FirstOrDefaultAsync(c => c.Username == userId);

            if (UserInfo != null)
            {
                Forename = UserInfo.Forename;
                Surname = UserInfo.Surname;
                Username = UserInfo.Username;
                Email = UserInfo.Email;
                PhoneNumber = UserInfo.PhoneNumber;
            }

            // Fetch upcoming consultations
            UpcomingConsultations = await _context.ScheduleBooking
                .Include(sb => sb.ScheduleType)
                .Where(sb => sb.ScheduleType.Schedule == "Consultation" && sb.Date >= DateOnly.FromDateTime(DateTime.UtcNow))
                .ToListAsync();

            // Fetch upcoming installations
            UpcomingInstallations = await _context.ScheduleBooking
                .Include(sb => sb.ScheduleType)
                .Where(sb => sb.ScheduleType.Schedule == "Installation" && sb.Date >= DateOnly.FromDateTime(DateTime.UtcNow))
                .ToListAsync();
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var userId = User.Identity?.Name;

        if (userId != null)
        {
            var user = await _context.Customer.FirstOrDefaultAsync(c => c.Username == userId);

            if (user != null)
            {
                // Update user information
                user.Forename = Forename;
                user.Surname = Surname;
                user.Email = Email;
                user.PhoneNumber = PhoneNumber;

                _context.Customer.Update(user);
                await _context.SaveChangesAsync();
            }
        }

        return RedirectToPage();
    }
}