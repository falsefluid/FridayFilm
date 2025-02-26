using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FridayFilmClub.Pages;

public class FindModel : PageModel
{
    private readonly FilmClubContext _context;
    private readonly ILogger<FindModel> _logger;

    public FindModel(FilmClubContext context, ILogger<FindModel> logger)
    {
        _context = context;
        _logger = logger;
    }

    public string Query { get; set; }
    public List<Movies> Movies { get; set; }
    public List<Celebrities> Celebrities { get; set; }
    public List<Users> Users { get; set; }
    public Dictionary<int, Movies> LatestReviewedMovies { get; set; } = new Dictionary<int, Movies>();

    public async Task OnGetAsync(string query)
    {
        Query = query;

        if (!string.IsNullOrEmpty(query))
        {
            // Search for movies by title or genre
            Movies = await _context.Movies
                .Include(m => m.CelebritiesInMovies)
                    .ThenInclude(cim => cim.Celebrity)
                .Include(m => m.CelebritiesInMovies)
                    .ThenInclude(cim => cim.Credit)
                .Include(m => m.MovieGenres)
                    .ThenInclude(mg => mg.Genre)
                .Where(m => EF.Functions.Like(m.MovieTitle, $"{query}%") || EF.Functions.Like(m.MovieTitle, $"%{query}%") ||
                            m.MovieGenres.Any(mg => EF.Functions.Like(mg.Genre.GenreName, $"{query}%") || EF.Functions.Like(mg.Genre.GenreName, $"%{query}%")))
                .OrderByDescending(m => EF.Functions.Like(m.MovieTitle, $"{query}%"))
                .ThenBy(m => m.MovieTitle)
                .ToListAsync();

            // Search for celebrities by name
            Celebrities = await _context.Celebrities
                .Include(c => c.CelebritiesInMovies)
                    .ThenInclude(cim => cim.Movie)
                .Include(c => c.CelebritiesInMovies)
                    .ThenInclude(cim => cim.Credit)
                .Where(c => EF.Functions.Like(c.Forename, $"{query}%") || EF.Functions.Like(c.Surname, $"{query}%") || 
                            EF.Functions.Like(c.Forename, $"%{query}%") || EF.Functions.Like(c.Surname, $"%{query}%") ||
                            EF.Functions.Like(c.Forename + " " + c.Surname, $"{query}%") || EF.Functions.Like(c.Forename + " " + c.Surname, $"%{query}%"))
                .OrderByDescending(c => EF.Functions.Like(c.Forename, $"{query}%") || EF.Functions.Like(c.Surname, $"{query}%"))
                .ThenBy(c => c.Forename)
                .ThenBy(c => c.Surname)
                .ToListAsync();

            // Search for users by username
            Users = await _context.Users
                .Where(u => EF.Functions.Like(u.Username, $"{query}%") || EF.Functions.Like(u.Username, $"%{query}%"))
                .OrderByDescending(u => EF.Functions.Like(u.Username, $"{query}%"))
                .ThenBy(u => u.Username)
                .ToListAsync();

            // Fetch the latest reviewed movie for each user
            foreach (var user in Users)
            {
                var latestReview = await _context.Reviews
                    .Where(r => r.UserID == user.UserID)
                    .OrderByDescending(r => r.Date)
                    .FirstOrDefaultAsync();

                if (latestReview != null)
                {
                    var latestMovie = await _context.Movies
                        .FirstOrDefaultAsync(m => m.MovieID == latestReview.MovieID);

                    if (latestMovie != null)
                    {
                        LatestReviewedMovies[user.UserID] = latestMovie;
                    }
                }
            }
        }
    }
}