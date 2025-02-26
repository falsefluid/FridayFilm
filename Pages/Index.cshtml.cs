using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FridayFilmClub;

namespace FridayFilmClub.Pages;

public class IndexModel : PageModel
{
    private readonly FilmClubContext _context;

    public IndexModel(FilmClubContext context)
    {
        _context = context;
    }

    public IList<Movies> Movie { get; set; }
    public IList<Movies> TopRatedMovies { get; set; }

    public async Task OnGetAsync()
    {
        Movie = await _context.Movies
            .OrderByDescending(m => m.ReleaseDate)
            .ThenByDescending(m => m.MovieID)
            .Take(9)
            .ToListAsync();

        TopRatedMovies = await _context.Movies
            .OrderByDescending(m => m.AverageRating)
            .ThenByDescending(m => m.NumberOfReviews)
            .ThenByDescending(m => m.MovieID)
            .Take(9)
            .ToListAsync();
    }
}