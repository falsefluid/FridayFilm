using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RolsaTechnologies;
using Microsoft.EntityFrameworkCore;

namespace RolsaTechnologies.Pages;

public class GreenTechnologyModel : PageModel
{
    private readonly RolsaTechnologiesContext _context;
    public GreenTechnologyModel(RolsaTechnologiesContext context)
    {
        _context = context;
    }

    public IList<Article> Articles { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public const int ArticlesPerPage = 3;

    [BindProperty(SupportsGet = true)]
    public string? SearchQuery { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? FilterType { get; set; }

    public async Task OnGetAsync(int pageNumber = 1)
    {
        CurrentPage = pageNumber;

        var query = _context.Articles.AsQueryable();

        // Apply search filter
        if (!string.IsNullOrEmpty(SearchQuery) && !string.IsNullOrEmpty(FilterType))
        {
            if (FilterType == "title")
            {
                query = query.Where(a => a.Title.Contains(SearchQuery));
            }
            else if (FilterType == "author")
            {
                query = query.Where(a => a.Author != null && a.Author.Contains(SearchQuery));
            }
        }

        var totalArticles = await query.CountAsync();
        TotalPages = (int)Math.Ceiling(totalArticles / (double)ArticlesPerPage);

        Articles = await query
            .OrderByDescending(a => a.CreateDate)
            .ThenByDescending(a => a.AID)
            .Skip((CurrentPage - 1) * ArticlesPerPage)
            .Take(ArticlesPerPage)
            .ToListAsync();
    }
}