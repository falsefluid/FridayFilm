using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RolsaTechnologies;
using Microsoft.EntityFrameworkCore;

namespace RolsaTechnologies.Pages;

public class GreenEnergyModel : PageModel
{
    private readonly RolsaTechnologiesContext _context;
    public GreenEnergyModel (RolsaTechnologiesContext context)
    {
        _context = context;
    }
    public IList<GreenEnergyProduct> GreenEnergyProducts{ get; set; }

    public IList<Article> Articles{ get; set; }

    public async Task OnGetAsync()
    {
        Articles = await _context.Articles
            .OrderByDescending(m => m.CreateDate)
            .ThenByDescending(m => m.AID)
            .Take(6)
            .ToListAsync();
    }
}

