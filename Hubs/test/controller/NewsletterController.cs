using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RolsaTechnologies;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

[Route("Newsletter")]
public class NewsletterController : Controller
{
    private readonly RolsaTechnologiesContext _context;

    public NewsletterController(RolsaTechnologiesContext context)
    {
        _context = context;
    }

    [HttpPost("Subscribe")]
    public async Task<IActionResult> Subscribe(string Email)
    {
        // Validate email format
        if (string.IsNullOrWhiteSpace(Email) || !new EmailAddressAttribute().IsValid(Email))
        {
            TempData["NewsletterMessage"] = "Invalid email format. Please enter a valid email.";
            return RedirectToAction("Index", "Home"); // Adjust redirection as needed
        }

        // Check for duplicate email
        bool existingEmail = await _context.EmailNews.AnyAsync(e => e.Email == Email);
        if (existingEmail)
        {
            TempData["NewsletterMessage"] = "This email is already subscribed!";
            return RedirectToAction("Index", "Home");
        }

        // Save email to the database
        var emailEntry = new EmailNews { Email = Email };
        _context.EmailNews.Add(emailEntry);
        await _context.SaveChangesAsync();

        TempData["NewsletterMessage"] = "You have successfully subscribed!";
        return RedirectToAction("Index", "Home");
    }
}