using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RolsaTechnologies.Pages;

public class ArticleModel : PageModel
{
    private readonly RolsaTechnologiesContext _context;
    private readonly ILogger<ArticleModel> _logger;
    
    public ArticleModel(RolsaTechnologiesContext context, ILogger<ArticleModel> logger)
    {
        _context = context;
        _logger = logger;
    }

    public Article Article { get; set; }

    public void OnGet(int id) // Adding an id parameter to identify the specific article
    {
        // Fetching the article from the database based on the provided id
        Article = _context.Articles.FirstOrDefault(a => a.AID == id);
        
        if (Article == null)
        {
            // Handle case where the article is not found
            _logger.LogWarning($"Article with id {id} not found.");
            // Optionally, you can redirect to an error page or show a friendly message
        }
    }
}
