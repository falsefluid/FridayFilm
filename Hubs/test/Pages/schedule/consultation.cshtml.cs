using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace RolsaTechnologies.Pages;

public class ConsultationModel : PageModel
{
    private readonly ILogger<ConsultationModel> _logger;
    private readonly RolsaTechnologiesContext _context;

    public ConsultationModel(ILogger<ConsultationModel> logger, RolsaTechnologiesContext context)
    {
        _logger = logger;
        _context = context;
    }

    [BindProperty]
    public string? HouseAddress { get; set; }
    [BindProperty]
    public string? Email { get; set; }

    [BindProperty]
    public DateTime? Date { get; set; }

    [BindProperty]
    public string? Message { get; set; }

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
        if (string.IsNullOrEmpty(HouseAddress) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Message) || Date == null)
        {
            ModelState.AddModelError(string.Empty, "All fields are required.");
            return Page();
        }

        if (!Email.Contains("@") || !Email.Contains("."))
        {
            ModelState.AddModelError(string.Empty, "Invalid email format.");
            return Page();
        }

        if (Date < DateTime.Today)
        {
            ModelState.AddModelError(string.Empty, "The consultation date cannot be in the past.");
            return Page();
        }

        if (Message.Length > 1000)
        {
            ModelState.AddModelError(string.Empty, "The message cannot exceed 1000 characters.");
            return Page();
        }

        // Check if the user is logged in and retrieve their ID
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdClaim, out int userId) || userId == 0)
        {
            ModelState.AddModelError(string.Empty, "User ID is invalid.");
            return Page();
        }

        // Save the consultation message to the database
        
        var scheduleBooking = new ScheduleBooking
        {
            ScheduleTypeID = 2, // Consultation
            HousePostcode = HouseAddress, // No house address required for consultations
            Date = DateOnly.FromDateTime(Date.Value),
            Hour = 12, // Default hour for consultation
            UsernameID = userId,
            EmailID = userId,
            CreateDate = DateTime.UtcNow
        };

        _context.ScheduleBooking.Add(scheduleBooking);
        await _context.SaveChangesAsync();
        
        var consultationMessage = new ConsultationMessage
        {
            EmailID = userId,
            SBID = scheduleBooking.SBID,
            Message = Message
        };

        _context.ConsultationMessages.Add(consultationMessage);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving consultation message to the database.");
            ModelState.AddModelError(string.Empty, "An error occurred while saving your consultation. Please try again.");
            return Page();
        }

        TempData["SuccessMessage"] = "Your consultation has been successfully scheduled!";
        return RedirectToPage("/Index");
    }
}