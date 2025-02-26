using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FridayFilmClub.Pages;

public class MovieModel : PageModel
{
    private readonly FilmClubContext _context;
    private readonly ILogger<MovieModel> _logger;

    public MovieModel(FilmClubContext context, ILogger<MovieModel> logger)
    {
        _context = context;
        _logger = logger;
    }

    public Movies Movie { get; set; }
    public double? AverageRating { get; set; }
    public int NumberOfReviews { get; set; }
    public bool HasSubmittedReview { get; set; }
    public List<Reviews> UserReviews { get; set; }
    public string Genres { get; set; }

    [BindProperty]
    public Reviews NewReview { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Movie = await _context.Movies
            .Include(m => m.CelebritiesInMovies)
                .ThenInclude(cim => cim.Celebrity)
            .Include(m => m.CelebritiesInMovies)
                .ThenInclude(cim => cim.Credit)
            .Include(m => m.MovieGenres)
                .ThenInclude(mg => mg.Genre)
            .FirstOrDefaultAsync(m => m.MovieID == id);

        if (Movie == null)
        {
            return NotFound();
        }

        // Calculate number of reviews
        NumberOfReviews = await _context.Reviews
            .Where(r => r.MovieID == id)
            .CountAsync();

        // Calculate average rating
        AverageRating = await _context.Reviews
            .Where(r => r.MovieID == id)
            .AverageAsync(r => (double?)r.Rating);

        // Sort CelebritiesInMovies so that the director appears first
        Movie.CelebritiesInMovies = Movie.CelebritiesInMovies
            .OrderByDescending(cim => cim.Credit.Role == "Director")
            .ToList();

        // Fetch genres and format them as a string
        Genres = string.Join(" • ", Movie.MovieGenres.Select(mg => mg.Genre.GenreName));

        // Check if the user has already submitted a review for this movie
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId != null)
        {
            HasSubmittedReview = await _context.Reviews
                .AnyAsync(r => r.MovieID == id && r.UserID == int.Parse(userId));
        }

        // Fetch user reviews
        UserReviews = await _context.Reviews
            .Where(r => r.MovieID == id)
            .Include(r => r.User)
            .ToListAsync();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        _logger.LogInformation($"Is User Authenticated: {User.Identity.IsAuthenticated}");
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        _logger.LogInformation($"User ID: {userId}");
        if (userId == null)
        {
            _logger.LogWarning("User is not authenticated.");
            return Unauthorized();
        }

        // Check if the user has already submitted a review for this movie
        var existingReview = await _context.Reviews
            .FirstOrDefaultAsync(r => r.MovieID == id && r.UserID == int.Parse(userId));

        if (existingReview != null)
        {
            _logger.LogWarning("User has already submitted a review for this movie.");
            ModelState.AddModelError(string.Empty, "You have already submitted a review for this movie.");
            return Page();
        }

        NewReview.MovieID = id;
        NewReview.UserID = int.Parse(userId);
        NewReview.Date = DateTime.UtcNow;
        NewReview.Rating *= 2; // Multiply the rating by 2

        _context.Reviews.Add(NewReview);
        await _context.SaveChangesAsync();

        // Update the movie's average rating and number of reviews
        var movie = await _context.Movies.FindAsync(id);
        if (movie != null)
        {
            movie.NumberOfReviews = await _context.Reviews.CountAsync(r => r.MovieID == id);
            movie.AverageRating = (float)await _context.Reviews.Where(r => r.MovieID == id).AverageAsync(r => r.Rating);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage(new { id });
    }

    public async Task<IActionResult> OnPostDeleteReviewAsync(int reviewId, int movieId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var isAdmin = User.IsInRole("Admin");

        if (userId == null || !isAdmin)
        {
            return Unauthorized();
        }

        var review = await _context.Reviews.FindAsync(reviewId);
        if (review == null)
        {
            return NotFound();
        }

        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();

        // Update the movie's average rating and number of reviews
        var movie = await _context.Movies.FindAsync(movieId);
        if (movie != null)
        {
            movie.NumberOfReviews = await _context.Reviews.CountAsync(r => r.MovieID == movieId);
            movie.AverageRating = (float)await _context.Reviews.Where(r => r.MovieID == movieId).AverageAsync(r => r.Rating);
            await _context.SaveChangesAsync();
        }

        return new JsonResult(new { success = true });
    }
}