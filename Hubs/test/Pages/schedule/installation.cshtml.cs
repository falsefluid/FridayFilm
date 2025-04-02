using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace RolsaTechnologies.Pages;

public class InstallationModel : PageModel
{
    private readonly ILogger<InstallationModel> _logger;
    private readonly RolsaTechnologiesContext _context;
    
    public InstallationModel(ILogger<InstallationModel> logger, RolsaTechnologiesContext context)
    {
        _logger = logger;
        _context = context;
    }

    [BindProperty]
    public string? HouseAddress{get;set;}
    [BindProperty]
    public string? HouseType{get;set;}
    [BindProperty]
    public int? Bedrooms {get;set;}
    [BindProperty]
    public DateTime? InstallationDate {get;set;}
    [BindProperty]
    public bool IncludeBatteries {get;set;}

    public IActionResult OnGet()
    {
        // Check if the user is logged in
        if (!User.Identity?.IsAuthenticated ?? true)
        {
            // Redirect to the sign-up page if not logged in
            return RedirectToPage("/registration/sign-up");
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        // Input validation
        if (string.IsNullOrEmpty(HouseAddress) || string.IsNullOrEmpty(HouseType) || Bedrooms == null || InstallationDate == null)
        {
            ModelState.AddModelError(string.Empty, "All fields are required.");
            return Page();
        }

        if (Bedrooms <= 0)
        {
            ModelState.AddModelError(string.Empty, "Number of bedrooms must be greater than 0.");
            return Page();
        }

        if (InstallationDate < DateTime.Today)
        {
            ModelState.AddModelError(string.Empty, "Installation date cannot be in the past.");
            return Page();
        }

        // Check for existing installations on the selected date
        var existingInstallation = await _context.ScheduleBooking
            .AnyAsync(sb => sb.Date == DateOnly.FromDateTime(InstallationDate.Value) && sb.ScheduleTypeID == 1);

        if (existingInstallation)
        {
            ModelState.AddModelError(string.Empty, "An installation is already scheduled on this date. Please choose another date.");
            return Page();
        }

        // Save the installation to the database
        var userIDClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIDClaim, out int userId) || userId == 0)
        {
            ModelState.AddModelError(string.Empty, "User ID is invalid.");
            return Page();
        }

        var scheduleBooking = new ScheduleBooking
        {
            ScheduleTypeID = 1, // Installation
            HousePostcode = HouseAddress,
            Date = DateOnly.FromDateTime(InstallationDate.Value),
            Hour = 9, // Default hour for installation (change if need be)
            UsernameID = userId,
            EmailID = userId,
            CreateDate = DateTime.UtcNow
        };

        var solarPanelInstallation = new SolarPanelInstallation 
        {
            HouseType = HouseType,
            NumOfBedrooms = Bedrooms.Value,
            IncludeBattery = IncludeBatteries,
            ScheduleBooking = scheduleBooking
        };

        _context.ScheduleBooking.Add(scheduleBooking);
        _context.SolarPanelInstallations.Add(solarPanelInstallation);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An Error occurred while saving the installation date.");
            ModelState.AddModelError(string.Empty, "An error occurred while saving your data. Please try again.");
            return Page();
        }

        TempData["SuccessMessage"] = "Your installation has been successfully scheduled!";
        return RedirectToPage("/Index");

    }
}