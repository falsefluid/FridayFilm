using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FridayFilmClub.Pages;

public class UserModel : PageModel
{
    private readonly FilmClubContext _context;
    private readonly ILogger<UserModel> _logger;

    public UserModel(FilmClubContext context, ILogger<UserModel> logger)
    {
        _context = context;
        _logger = logger;
    }

    public Users user { get; set; }
    public List<Reviews> UserReviews { get; set; }
    public Reviews HighestRatedReview { get; set; }
    public Reviews LatestRatedReview { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        user = await _context.Users.FirstOrDefaultAsync(u => u.UserID == id);

        if (user == null)
        {
            return NotFound();
        }

        UserReviews = await _context.Reviews
            .Where(r => r.UserID == id)
            .Include(r => r.Movie)
            .OrderByDescending(r => r.Date)
            .ThenByDescending(r => r.MovieID)
            .ToListAsync();

        HighestRatedReview = await _context.Reviews
            .Where(r => r.UserID == id)
            .Include(r => r.Movie)
            .OrderByDescending(r => r.Rating)
            .FirstOrDefaultAsync();

        LatestRatedReview = await _context.Reviews
            .Where(r => r.UserID == id)
            .Include(r => r.Movie)
            .OrderByDescending(r => r.Date)
            .FirstOrDefaultAsync();

        return Page();
    }
}