using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

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

    public List<ScheduleBooking> UpcomingConsultations{get;set;} = new();
    public List<ScheduleBooking> UpcomingInstallations{get;set;} = new();

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

    public async Task<IActionResult> OnGetAsync()
    {
        // Check if the user is logged in
        if (!User.Identity?.IsAuthenticated ?? true)
        {
            // Redirect to the sign-up page if not logged in
            return RedirectToPage("/registration/sign-up");
        }

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
            

            // Fetch upcoming installations
            UpcomingInstallations = await _context.ScheduleBooking
                .Include(sb => sb.ScheduleType)
                .Include(sb => sb.SolarPanelInstallation) // Include SolarPanelInstallation for price
                .Where(sb => sb.ScheduleType.Schedule == "Installation" && sb.Date >= DateOnly.FromDateTime(DateTime.UtcNow) && sb.UsernameID == UserInfo.CID)
                .OrderBy(sb => sb.Date)
                .ToListAsync();

            // Fetch upcoming consultations
            UpcomingConsultations = await _context.ScheduleBooking
                .Include(sb => sb.ScheduleType)
                .Where(sb => sb.ScheduleType.Schedule == "Consultation" && sb.Date >= DateOnly.FromDateTime(DateTime.UtcNow) && sb.UsernameID == UserInfo.CID)
                .OrderBy(sb => sb.Date)
                .ToListAsync();
            }
        }

        return Page();
    }



    public async Task<IActionResult> OnPostAsync()
    {
        var userId = User.Identity?.Name;
        UserInfo = await _context.Customer.FirstOrDefaultAsync(c => c.Username == userId);

        if (UserInfo != null)
        {
            // Input Validation
            // Email validation
            if (Email != null) 
            {
                if (!Email.Contains("@") || !Email.Contains("."))
                {
                    ModelState.AddModelError(string.Empty,"Invalid email format.");
                    return Page();
                }

                var existingEmail = await _context.Customer
                        .Where(c => c.Email == Email && c.CID != UserInfo.CID) // Exclude current user
                        .FirstOrDefaultAsync();

                if (existingEmail != null)
                {
                    ModelState.AddModelError(string.Empty, "Email is already being used.");
                    return Page();
                }
            }

            // Capitalises forename and surname 
            if (Forename != null) 
            {
                Forename = char.ToUpper(Forename.First()) + Forename.Substring(1).ToLower();
            }

            if (Surname != null) 
            {
                Surname = char.ToUpper(Surname.First()) + Surname.Substring(1).ToLower();
            }

            // Phone validation
            if (PhoneNumber != null)
            {
                if (ValidatePhoneNumber(PhoneNumber))
                {
                    var existingPhoneNumber = await _context.Customer
                        .Where(c => c.PhoneNumber == PhoneNumber && c.CID != UserInfo.CID) // Exclude current user
                        .FirstOrDefaultAsync();

                    if (existingPhoneNumber != null)
                    {
                        ModelState.AddModelError(string.Empty, "Phone number is already being used.");
                        return Page();
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Phone number is formatted incorrectly.");
                    return Page();
                }
            }
        }

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

    // Regex validation for UK phone numbers (assuming the company will stay locally based)
    public static bool ValidatePhoneNumber(string PhoneNumber)
    {
        return Regex.Match(PhoneNumber, @"^0\d{9,10}$").Success;
    }

    public async Task<IActionResult> OnPostCancelAsync(int id)
    {
        // Find the ScheduleBooking by ID
        var scheduleBooking = await _context.ScheduleBooking
            .Include(sb => sb.SolarPanelInstallation) // Include related installations
            .Include(sb => sb.ConsultationMessage)   // Include related consultations
            .FirstOrDefaultAsync(sb => sb.SBID == id);

        if (scheduleBooking == null)
        {
            ModelState.AddModelError(string.Empty, "The booking could not be found.");
            return Page();
        }

        // Remove related SolarPanelInstallation records if they exist
        if (scheduleBooking.SolarPanelInstallation.Any())
        {
            _context.SolarPanelInstallations.RemoveRange(scheduleBooking.SolarPanelInstallation);
        }

        // Remove related ConsultationMessage records if they exist
        if (scheduleBooking.ConsultationMessage.Any())
        {
            _context.ConsultationMessages.RemoveRange(scheduleBooking.ConsultationMessage);
        }

        // Remove the ScheduleBooking itself
        _context.ScheduleBooking.Remove(scheduleBooking);

        // Save changes to the database
        await _context.SaveChangesAsync();

        // Redirect to the same page to refresh the list
        return RedirectToPage();
    }
}